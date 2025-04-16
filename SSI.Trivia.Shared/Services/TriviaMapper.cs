using SSI.Trivia.Shared.Models;
using SSI.Trivia.Shared.Models.GenerationModels;

namespace SSI.Trivia.Shared.Services;

public static class TriviaMapper
{
    /// <summary>
    /// Converts a TriviaGenerationResult into a list of Question objects for use in a Sprint
    /// </summary>
    /// <param name="generationResult">The trivia generation result to convert</param>
    /// <returns>A list of Question objects ready for a Sprint</returns>
    public static List<Question> ConvertToQuestions(TriviaGenerationResult generationResult)
    {
        if (generationResult == null || !string.IsNullOrEmpty(generationResult.Error))
        {
            return new List<Question>();
        }

        var questions = new List<Question>();

        // Process regular questions
        foreach (var triviaQuestion in generationResult.Questions)
        {
            var question = new Question
            {
                IsTieBreaker = false,
                QuestionText = triviaQuestion.QuestionText,
                Answers = []
            };

            // First create all answers
            foreach (var triviaAnswer in triviaQuestion.Answers)
            {
                var answer = new Answer
                {
                    Id = Guid.NewGuid(),
                    Text = triviaAnswer.Text,
                    IsCorrect = triviaAnswer.IsCorrect
                };

                question.Answers.Add(answer);
            }

            // Then explicitly set CorrectAnswerId from the correct answer
            var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
            if (correctAnswer != null)
            {
                question.CorrectAnswerId = correctAnswer.Id;
            }

            questions.Add(question);
        }

        // Add tie-breaker question if included
        if (generationResult.TieBreaker != null)
        {
            var tieBreakerQuestion = new Question
            {
                IsTieBreaker = true,
                QuestionText = generationResult.TieBreaker.QuestionText,
                TieBreakerAnswer = new Answer
                {
                    Text = generationResult.TieBreaker.Answer
                }
            };

            questions.Add(tieBreakerQuestion);
        }

        return questions;
    }

    /// <summary>
    /// Creates a sprint name based on the category of trivia
    /// </summary>
    /// <param name="category">The category of trivia</param>
    /// <returns>A formatted sprint name</returns>
    public static string GenerateSprintName(string category)
    {
        return string.IsNullOrEmpty(category) || category.Equals("Random", StringComparison.OrdinalIgnoreCase)
            ? $"Trivia Sprint {DateTime.Now:MMM d, yyyy}"
            : $"{category} Trivia {DateTime.Now:MMM d, yyyy}";
    }
}
