using Microsoft.AspNetCore.SignalR;
using SSI.Trivia.Shared.Models;
using System.Collections.Concurrent;

namespace SSI.Trivia.Shared.Hubs;

public class TriviaHub : Hub
{
    // Add these variable to track the current game state
    private static Sprint? _currentSprint;
    private static int _currentQuestionIndex = 0;
    private static bool _sprintClosed = false;

    // Track tie-breaker eligible participants and their submission status
    private static readonly ConcurrentDictionary<int, bool> _tieBreakerEligibleParticipants = new();
    private static readonly ConcurrentDictionary<int, string> _tieBreakerAnswers = new();

    // Start a sprint and send it to all participants
    public async Task StartSprint(Sprint sprint)
    {
        _currentSprint = sprint;
        _currentQuestionIndex = 0;
        _sprintClosed = false;

        // Clear tie-breaker tracking data when a new sprint starts
        _tieBreakerEligibleParticipants.Clear();
        _tieBreakerAnswers.Clear();

        await Clients.All.SendAsync("SprintStarted", sprint);
    }

    // Navigate all participants to a specific question
    public async Task NavigateToQuestion(int questionIndex)
    {
        _currentQuestionIndex = questionIndex;
        await Clients.All.SendAsync("QuestionChanged", questionIndex);
    }

    // Close the sprint (end the game)
    public async Task CloseSprint()
    {
        _sprintClosed = true;
        await Clients.All.SendAsync("SprintClosed");
    }

    // Set tie-breaker eligibility for specific participants
    public async Task SetTieBreakerEligibility(string connectionId, bool isEligible)
    {
        await Clients.Client(connectionId).SendAsync("TieBreakerEligibilityChanged", isEligible);
    }

    // Create a group for presenters
    public async Task JoinPresenterGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Presenters");
    }

    // Create a group for participants
    public async Task JoinParticipantGroup(int participantId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Participants");
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Participant_{participantId}");
    }

    public async Task SetTieBreakerEligibilityForParticipant(int participantId, bool isEligible)
    {
        // Send to the specific participant group
        await Clients.Group($"Participant_{participantId}").SendAsync("TieBreakerEligibilityChanged", isEligible);

        // Track eligible participants
        if (isEligible)
        {
            _tieBreakerEligibleParticipants[participantId] = false; // Not submitted yet
        }
        else
        {
            _tieBreakerEligibleParticipants.TryRemove(participantId, out _);
            _tieBreakerAnswers.TryRemove(participantId, out _);
        }
    }

    // Add a new method for participants to request the current game state
    public async Task RequestCurrentGameState(int participantId)
    {
        if (_currentSprint != null)
        {
            // Send the current game state just to this client
            await Clients.Caller.SendAsync("CurrentGameState", _currentSprint, _currentQuestionIndex, _sprintClosed);
        }
    }

    // New method for participants to submit tie-breaker answers
    public async Task SubmitTieBreakerAnswer(int participantId, string answer)
    {
        // Record the answer
        _tieBreakerAnswers[participantId] = answer;

        // Mark participant as having submitted an answer
        if (_tieBreakerEligibleParticipants.ContainsKey(participantId))
        {
            _tieBreakerEligibleParticipants[participantId] = true;
        }

        // Check if all eligible participants have submitted answers
        bool allSubmitted = _tieBreakerEligibleParticipants.Count > 0 &&
                            _tieBreakerEligibleParticipants.All(p => p.Value);

        // Notify presenters if all eligible participants have submitted their answers
        if (allSubmitted)
        {
            await Clients.Group("Presenters").SendAsync("AllTieBreakerAnswersSubmitted");
        }

        // Also notify the participant that their answer was received
        await Clients.Group($"Participant_{participantId}").SendAsync("TieBreakerAnswerSubmitted");
    }

    // Method for presenter to get tie-breaker results
    public async Task<List<TieBreakerResult>> GetTieBreakerResults(int targetValue)
    {
        var results = new List<TieBreakerResult>();

        foreach (var entry in _tieBreakerAnswers)
        {
            if (int.TryParse(entry.Value, out int numericAnswer))
            {
                results.Add(new TieBreakerResult
                {
                    ParticipantId = entry.Key,
                    Answer = entry.Value,
                    NumericValue = numericAnswer,
                    Difference = numericAnswer <= targetValue ? targetValue - numericAnswer : int.MaxValue
                });
            }
        }

        // Sort by difference (smallest difference first, but values over target are placed last)
        return results.OrderBy(r => r.Difference).ToList();
    }
}