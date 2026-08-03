using Course_Pilot.Models;
using OpenAI.Responses;
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Course_Pilot
{
    class OpenAIService
    {

        public static async Task<string> AskAIAsync(string pdfText)
        {
            //grab OpenAI API key
            string? apiKey =
                System.Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            //make sure the key was found
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("OpenAI API key not found.");
            }

            //create the OpenAI client
            OpenAI.Chat.ChatClient client =
                new OpenAI.Chat.ChatClient(
                    model: "gpt-5.4-mini",
                    apiKey: apiKey
                );
            //prompt for OpenAI
            string prompt = $$"""
                    You are reading a university course syllabus.

                    Extract the following information:

                    - Course Code
                    - Course Title
                    - 2 sentence Course Description
                    - Professor
                    - Email
                    - OfficeHours
                    - Days 
                    - Time
                    - Location
                    - Exams (include every exam, midterm, and final)
                    - Grade Weights (every grading category and its percentage)

                    Return ONLY valid JSON using exactly this format:

                    {
                        "CourseCode": "",
                        "CourseTitle": "",
                        "CourseDescription": "",
                        "Professor": "",
                        "Email": "",
                        "OfficeHours: "",
                        "Days": "",
                        "Time": "",
                        "Location": "",
                        "Exams":
                        [
                            {
                                "Name": "",
                                "Date": "",
                                "Time": "",
                                "Coverage": ""
                            }
                        ],
                        "GradeWeights":
                        [
                            {
                                "Category": "",
                                "Weight": ""
                            }
                        ]

                        }

                                        Rules:

                    - Return every exam listed in the syllabus.
                    - Include quizzes only if they are explicitly called exams.
                    - Include the final exam.
                    - If any exam information is missing, leave that field as an empty string.
                    - If no exams exist, return:
                      "Exams": []
                    - Do not invent times.
                    - If the syllabus does not explicitly provide an exam's date, time, or coverage, return "TBD (Check Canvas or Instructor)" for that field. Do not guess or invent information.
                    - Do not include exam locations.
                    - Return every grading category listed in the syllabus.
                    - The weights should exactly match the syllabus percentages.
                    - If no grading breakdown is provided, return:
                      "GradeWeights": []
                    - Do not invent grading categories or percentages.
                   

                    Syllabus text:

                    {{pdfText}}
                    """;

            OpenAI.Chat.ChatCompletion response = await client.CompleteChatAsync(prompt);

            return response.Content[0].Text;
        }

        public static async Task<string> AskCourseQuestionAsync(
            string syllabusText,
            string question)
        {
            //retrieve OpenAI key and throw an error if its not found
            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? throw new InvalidOperationException("OpenAI API key was not found.");

            //create a new OpenAI client
            OpenAI.Chat.ChatClient client =
                new OpenAI.Chat.ChatClient(
                    model: "gpt-5.4-mini",
                    apiKey: apiKey
                );

            //prompt for OpenAI
            string prompt = $"""
            You are Course Pilot, an assistant helping a student understand one university course.

            You MUST answer using ONLY the syllabus below.

            Read the syllabus carefully before answering.

            If the answer exists anywhere in the syllabus,
            you MUST use that information.

            If the answer cannot be found in the structured course information
            or anywhere in the syllabus, return exactly:

            WEB_SEARCH_NEEDED

            Do not include any other text.

            Student question:
            {question}

            ========== COURSE SYLLABUS ==========
            {syllabusText}
            ========== END SYLLABUS ==========
            """;

            //grab response
            OpenAI.Chat.ChatCompletion response = await client.CompleteChatAsync(prompt);

            //store the AI's response
            string answer = response.Content[0].Text;

            //if the AI couldn't answer from the syllabus,
            //automatically perform a web search.
            if (answer.Trim() == "WEB_SEARCH_NEEDED")
            {
                return await AskWithWebAsync(question);
            }

            return answer;
        }

        //supressing ResponsesClient warning
        #pragma warning disable OPENAI001
        public static async Task<string> AskWithWebAsync(string question)
        {
            string apiKey =
                Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? throw new InvalidOperationException(
                    "OpenAI API key was not found.");
            //making a new client
            ResponsesClient client = new(apiKey);

            CreateResponseOptions options = new()
            {
                Model = "gpt-5.1",
                Tools =
        {
            ResponseTool.CreateWebSearchTool()
        }
            };
            //add the user's question to the request.
            options.InputItems.Add(
                ResponseItem.CreateUserMessageItem(question));

            //send the request and wait for the result
            ResponseResult response =
                await client.CreateResponseAsync(options);

            //find the assistant's message and return its text.
            foreach (ResponseItem item in response.OutputItems)
            {
                if (item is MessageResponseItem message)
                {
                    return message.Content.FirstOrDefault()?.Text
                           ?? "The web search returned no answer.";
                }
            }

            return "The web search returned no answer.";
;
        }
        //unsupressing ResponsesClient warning
        #pragma warning restore OPENAI001


        //method to generate useful course resources
        public async Task<CourseResources?> GenerateCourseResourcesAsync(Course course)
        {
            string prompt = $$"""
        Search the web for the highest quality study resources for this college course.

        Prioritize:
        - Official textbook
        - Official documentation
        - Well-known YouTube videos
        - Trusted educational websites
        - Practice websites

        Do not include any explanation before or after the JSON.

        Use official URLs whenever possible.
        If an official URL cannot be found, leave the Url field empty.

        Course code: {{course.CourseCode}}
        Course title: {{course.CourseTitle}}
        Course description: {{course.CourseDescription}}

        Return ONLY valid JSON using exactly this structure:

        {
        
                  "Textbooks": [
            {
        
                      "Title": "Resource title",
              "Description": "Brief explanation of why it is useful",
              "Url": "https://example.com"
            }
          ],
          "Videos": [
            {
        
                      "Title": "Video or channel title",
              "Description": "Brief explanation of why it is useful",
              "Url": "https://example.com"
            }
          ],
          "Websites": [
            {
        
                      "Title": "Website title",
              "Description": "Brief explanation of why it is useful",
              "Url": "https://example.com"
            }
          ],
          "PracticeResources": [
            {
        
                      "Title": "Practice resource title",
              "Description": "Brief explanation of why it is useful",
              "Url": "https://example.com"
            }
          ]
        }

        Provide approximately 2 resources in each category.
        Do not include markdown or code fences.
        """;
            //awaiting response
            string response = await AskWithWebAsync(prompt);

            //returning the resources
            try
            {
                return JsonSerializer.Deserialize<CourseResources>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
