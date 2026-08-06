using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursePilot.Api.Models;

namespace CoursePilot.Api.Models
{
    public class Course
    {
        public string CourseCode { get; set; } = string.Empty;

        public string CourseTitle { get; set; } = string.Empty;

        public string CourseDescription { get; set; } = string.Empty;

        public string Professor { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string OfficeHours { get; set; } = string.Empty;

        public string Days { get; set; } = string.Empty;

        public string Time { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public List<Exam> Exams { get; set; } = new();

        public List<GradeCategory> GradeWeights { get; set; } = new();

        public string SyllabusText { get; set; } = "";

        public CourseResources? Resources { get; set; }
    }
}
