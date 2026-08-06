using System.Net.Http;
using System.Net.Http.Json;
using Course_Pilot.Models;

namespace Course_Pilot.Services
{
    //sends requests from the WPF desktop app to the CoursePilot API
    internal static class ApiService
    {
        //reuse one HttpClient instead of creating a new one for every request
        private static readonly HttpClient client = new()
        {
            // API's current local development address
            BaseAddress = new Uri("https://coursepilot-api-ujm9.onrender.com/")
        };

        //sends syllabus text to POST /api/AI/ask
        public static async Task<string> AskAIAsync(string pdfText)
        {
            //this object becomes:
            // {
            //   "prompt": "the syllabus text..."
            // }
            var request = new
            {
                prompt = pdfText
            };

            //send the JSON request to your ASP.NET backend
            HttpResponseMessage response =
                await client.PostAsJsonAsync("api/AI/ask", request);

            //throw an exception when the server returns an error status
            response.EnsureSuccessStatusCode();

            //convert the server's JSON response into a C# object
            AskResponse? result =
                await response.Content.ReadFromJsonAsync<AskResponse>();

            if (string.IsNullOrWhiteSpace(result?.Answer))
            {
                throw new Exception("The server returned an empty AI response.");
            }

            return result.Answer;
        }


        //sends a course-specific question to the ASP.NET backend
        public static async Task<string> AskCourseQuestionAsync(
            string syllabusText,
            string question)
        {
            //create the JSON object that will be sent to the API.
            var request = new
            {
                syllabusText,
                question
            };

            //send the request to the /api/AI/course-question endpoint.
            HttpResponseMessage response =
                await client.PostAsJsonAsync("api/AI/course-question", request);

            //throw an exception if the server returned an error.
            response.EnsureSuccessStatusCode();

            //convert the JSON response into a C# object.
            AskResponse? result =
                await response.Content.ReadFromJsonAsync<AskResponse>();

            //return the AI's answer.
            return result?.Answer ?? "";
        }

        //sends a web search question to the ASP.NET backend.
        public static async Task<string> AskWithWebAsync(
            string question)
        {
            //create the JSON object that will be sent to the API.
            var request = new
            {
                question
            };

            //send the request to the /api/AI/web-question endpoint.
            HttpResponseMessage response =
                await client.PostAsJsonAsync("api/AI/web-question", request);

            //throw an exception if the server returned an error.
            response.EnsureSuccessStatusCode();

            //convert the JSON response into a C# object.
            AskResponse? result =
                await response.Content.ReadFromJsonAsync<AskResponse>();

            //return the AI's answer.
            return result?.Answer ?? "";
        }


        //matches the JSON returned by the API:
        // {
        //   "answer": "..."
        // }
        private class AskResponse
        {
            public string Answer { get; set; } = string.Empty;
        }


        //sends course information to the backend and receives generated resources.
        public static async Task<CourseResources?> GenerateCourseResourcesAsync(
            Course course)
        {
            //nothing null being sent to AI
            course.OfficeHours ??= "";
            course.Email ??= "";
            course.Location ??= "";
            course.Days ??= "";
            course.Time ??= "";
            course.CourseDescription ??= "";
            course.SyllabusText ??= "";

            //create the JSON body expected by the API.
            var request = new
            {
                course
            };

            //send the course to POST /api/AI/generate-resources.
            HttpResponseMessage response =
                await client.PostAsJsonAsync("api/AI/generate-resources", request);

            //throw an exception if the server returned an error.
            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Server returned {(int)response.StatusCode} " +
                    $"{response.StatusCode}:\n{errorBody}");
            }

            //convert the returned JSON into a CourseResources object.
            CourseResources? resources =
                await response.Content.ReadFromJsonAsync<CourseResources>();

            return resources;
        }
    }
}