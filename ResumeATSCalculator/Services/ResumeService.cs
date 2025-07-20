using ResumeATSCalculator.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ResumeATSCalculator.Services
{
    public class ResumeService
    {
        private readonly HttpClient _http;

        public ResumeService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ScoreResult> ScoreResumeAsync(string resumeText)
        {
            // 🔐 Load from environment variable or secret manager
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("OpenAI API key is missing.");

            var request = new
            {
                model = "gpt-4o",
                messages = new[]
                {
            new { role = "system", content = "You are an ATS Resume Scorer." },
            new { role = "user", content = $"Score this resume:\n{resumeText}" }
        }
            };

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var json = JsonSerializer.Serialize(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Parse error and return meaningful info
                var errorInfo = JsonDocument.Parse(responseBody).RootElement;
                var errorMessage = errorInfo.GetProperty("error").GetProperty("message").GetString();
                throw new Exception($"OpenAI API error ({response.StatusCode}): {errorMessage}");
            }

            var responseJson = JsonDocument.Parse(responseBody);

            string content;
            try
            {
                content = responseJson.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse OpenAI response.", ex);
            }

            return new ScoreResult
            {
                Score = 85, // TODO: Extract actual score from AI response (if available)
                Feedback = content
            };
        }

    }

}
