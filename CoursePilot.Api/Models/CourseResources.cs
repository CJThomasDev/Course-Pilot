using System.Collections.Generic;


namespace CoursePilot.Api.Models
{
    public class CourseResources
    {
        public List<ResourceItem> Textbooks { get; set; } = new();
        public List<ResourceItem> Videos { get; set; } = new();
        public List<ResourceItem> Websites { get; set; } = new();
        public List<ResourceItem> PracticeResources { get; set; } = new();
    }


    public class ResourceItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}