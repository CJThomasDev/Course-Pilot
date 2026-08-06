using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Course_Pilot.Models;

namespace Course_Pilot.Services
{
    public static class CourseParserService
    {


        //organizing course 
         public static async Task<Course> ParseCourseAsync(string pdfText)
        {
            //ask OpenAI for JSON
            string json = await ApiService.AskAIAsync(pdfText);

            //turn JSON into a Course object
            Course? course = JsonSerializer.Deserialize<Course>(json);

            if (course == null)
                throw new Exception("Failed to parse course.");

            //keep the original syllabus text locally for the course chat
            course.SyllabusText = pdfText;

            return course;
        }

    }
}
