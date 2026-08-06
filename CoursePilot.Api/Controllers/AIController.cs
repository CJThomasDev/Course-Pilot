using CoursePilot.Api.Models;
using CoursePilot.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoursePilot.Api.Controllers
{
    //marks this class as an API controller.
    [ApiController]

    //base route for every endpoint in this controller:
    // /api/AI
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        //parses syllabus text into structured course JSON.
        //POST /api/AI/ask
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new
                {
                    error = "Prompt cannot be empty."
                });
            }

            try
            {
                string answer =
                    await OpenAIService.AskAIAsync(request.Prompt);

                return Ok(new
                {
                    answer
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "The AI request failed.",
                    details = ex.Message
                });
            }
        }

        //answers a question using an uploaded course syllabus.
        //POST /api/AI/course-question
        [HttpPost("course-question")]
        public async Task<IActionResult> AskCourseQuestion(
            [FromBody] CourseQuestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SyllabusText))
            {
                return BadRequest(new
                {
                    error = "Syllabus text cannot be empty."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest(new
                {
                    error = "Question cannot be empty."
                });
            }

            try
            {
                string answer =
                    await OpenAIService.AskCourseQuestionAsync(
                        request.SyllabusText,
                        request.Question);

                return Ok(new
                {
                    answer
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "The course question request failed.",
                    details = ex.Message
                });
            }
        }

        //answers a question using OpenAI's web-search functionality.
        //POST /api/AI/web-question
        [HttpPost("web-question")]
        public async Task<IActionResult> AskWebQuestion(
            [FromBody] WebQuestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest(new
                {
                    error = "Question cannot be empty."
                });
            }

            try
            {
                string answer =
                    await OpenAIService.AskWithWebAsync(request.Question);

                return Ok(new
                {
                    answer
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "The web question request failed.",
                    details = ex.Message
                });
            }
        }

        //generates helpful study resources for a course.
        //POST /api/AI/generate-resources
        [HttpPost("generate-resources")]
        public async Task<IActionResult> GenerateResources(
            [FromBody] GenerateResourcesRequest request)
        {
            CourseResources? resources =
                await OpenAIService.GenerateCourseResourcesAsync(request.Course);

            return Ok(resources);
        }
    }

    //JSON body for POST /api/AI/ask
    public class AskRequest
    {
        public string Prompt { get; set; } = string.Empty;
    }

    //JSON body for POST /api/AI/course-question
    public class CourseQuestionRequest
    {
        public string SyllabusText { get; set; } = string.Empty;

        public string Question { get; set; } = string.Empty;
    }

    //JSON body for POST /api/AI/web-question
    public class WebQuestionRequest
    {
        public string Question { get; set; } = string.Empty;
    }

    //JSON body for POST /api/AI/generate-resources
    public class GenerateResourcesRequest
    {
        public Course Course { get; set; } = new();
    }
}