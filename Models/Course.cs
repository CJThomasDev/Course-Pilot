using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course_Pilot.Models;

namespace Course_Pilot
{
    public class Course
    {
        public string CourseCode { get; set; }
        
        public string CourseTitle { get; set; }

        public string CourseDescription { get; set; }

        public string Professor { get; set; }

        public string Email { get; set; }

        public string OfficeHours { get; set; }

        public string Days { get; set; } 

        public string Time { get; set; }

        public string Location { get; set; }

        public List<Exam> Exams { get; set; } = new();

        public List<GradeCategory> GradeWeights { get; set; } = new();

        public string SyllabusText { get; set; } = "";

        public CourseResources? Resources { get; set; }
    }
}
