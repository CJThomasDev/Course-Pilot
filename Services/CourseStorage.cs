using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Documents;
using Course_Pilot.Models;

namespace Course_Pilot.Services
{
    public static class CourseStorage
    {
        //location on the user's computer where course data is saved
        private static readonly string saveFilePath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CoursePilot",
                "courses.json");

        //saves all courses to a JSON file
        public static void SaveCourses(List<Course> courses)
        {
            //get folder path where file is stored
            string? folderPath = Path.GetDirectoryName(saveFilePath);

            //create the folder if it doesn't already exist.
            if (!string.IsNullOrEmpty(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //convert the list of Course objects into readable JSON.
            string json = JsonSerializer.Serialize(courses, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            //write the JSON string to the save file.
            File.WriteAllText(saveFilePath, json);
        }

        //loads all saved courses from the JSON file
        public static List<Course> LoadCourses()
        {

            //if no save file exists yet, return an empty course list.
            if (!File.Exists(saveFilePath))
            {
                return new List<Course>();
            }

            //read all the data from JSON file then save data
            string json = File.ReadAllText(saveFilePath);

            //return back as a list of course objects
            return JsonSerializer.Deserialize<List<Course>>(json)
                   ?? new List<Course>();
        }
    }
}