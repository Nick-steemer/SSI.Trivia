using Microsoft.AspNetCore.SignalR;
using SSI.Trivia.Shared.Models;

namespace SSI.Trivia.Shared.Hubs;

public class TriviaHub : Hub
{
    // Start a sprint and send it to all participants
    public async Task StartSprint(Sprint sprint)
    {
        await Clients.All.SendAsync("SprintStarted", sprint);
    }

    // Navigate all participants to a specific question
    public async Task NavigateToQuestion(int questionIndex)
    {
        await Clients.All.SendAsync("QuestionChanged", questionIndex);
    }

    // Close the sprint (end the game)
    public async Task CloseSprint()
    {
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
    }
}