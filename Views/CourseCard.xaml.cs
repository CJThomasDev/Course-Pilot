using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Course_Pilot.Views
{
    /// <summary>
    /// Interaction logic for CourseCard.xaml
    /// </summary>
    public partial class CourseCard : UserControl
    {
        public Course Course { get; }

        public event EventHandler? DeleteRequested;

        public CourseCard(Course course)
        {
            //initialize the course card
            InitializeComponent();

            Course = course;

            //display all course information
            TitleText.Text = $"{course.CourseCode} - {course.CourseTitle}";
            DescriptionText.Text = $"Description: {course.CourseDescription}";
            ProfessorText.Text = $"Professor: {course.Professor}";
            EmailText.Text = $"Email: {course.Email}";
            OfficeHoursText.Text = $"Office Hours: {course.OfficeHours}";
            ScheduleText.Text = $"{course.Days} | {course.Time}";
            LocationText.Text = $"Location: {course.Location}";
        }

        //ask the user to confirm deletion
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete this course?",
                "Delete Course",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                DeleteRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        //opens extra details about the class
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            //creating new detailsWindow window
            ClassDetailsWindow detailsWindow = new ClassDetailsWindow(Course);
            //makes it open as a modal window
            detailsWindow.ShowDialog();

        }
    }
}
