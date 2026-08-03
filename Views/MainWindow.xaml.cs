using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Win32;
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
using UglyToad.PdfPig;
using System.Text.Json;
using OpenAI.Chat;
using System.IO;
using Course_Pilot.Views;
using Course_Pilot.Models;
using Course_Pilot.Services;


namespace Course_Pilot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //stores every course currently loaded in the application
        private List<Course> savedCourses = new List<Course>();


        public MainWindow()
        {
            InitializeComponent();

            savedCourses = CourseStorage.LoadCourses();

            //adding all course cards to the screen that are saved
            foreach (Course course in savedCourses)
            {
                AddCourseCard(course, false);
            }
        }

      

        //upload syllabus pdf button
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //creating file explorer
            Microsoft.Win32.OpenFileDialog ofd = new();
            //creating bool to ensure user picks file
            bool? response = ofd.ShowDialog();

            if (response == true)
            {
                //copy file path to string 
                string filePath = ofd.FileName;

                //return if PdfPig fails to open a pdf
                if (!PdfService.VerifyPdf(filePath))
                {
                    MessageBox.Show(
                        "Please select a valid PDF file.",
                        "Invalid File",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                try
                {
                    //extracting pdf into string
                    string pdfText = PdfService.ExtractPdfText(filePath);


                    //sending the pdf string to OpenAI to organize and create course
                    Course course = await CourseParserService.ParseCourseAsync(pdfText);

                    //adding the course card to the main menu
                    AddCourseCard(course);
                }
                //throw error if API key runs out of uses
                catch (Exception)
                {
                    MessageBox.Show(
                        "Course Pilot couldn't organize your syllabus right now.\n\n" +
                        "This may be due to a temporary AI service limit or network issue.\n" +
                        "Please try again in a few minutes.",
                        "Course Upload Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                }
                


            }
        }

        //Adding Course Card to the main window
        private void AddCourseCard(Course course, bool saveCourse = true)
        {
            CourseCard card = new CourseCard(course);

            //adding course to the drop down in Course Pilot Chat Bot
            if (!CourseSelector.Items.Contains(course))
            {
                CourseSelector.Items.Add(course);
            }

            if (CourseSelector.SelectedIndex == -1)
            {
                CourseSelector.SelectedIndex = 0;
            }

            //card delete button
            card.DeleteRequested += (sender, e) =>
            {
                ClassesPanel.Children.Remove(card);
                savedCourses.Remove(course);
                CourseStorage.SaveCourses(savedCourses);
            };

            ClassesPanel.Children.Add(card);

            if (saveCourse)
            {
                savedCourses.Add(course);
                CourseStorage.SaveCourses(savedCourses);
            }

        }

        //Course Pilot Chat Bot button click
        private async void AskButton_Click(object sender, RoutedEventArgs e)
        {

            if (CourseSelector.SelectedItem is not Course selectedCourse)
            {
                MessageBox.Show("Please select a course.");
                return;
            }

            string question = QuestionTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(question))
            {
                MessageBox.Show("Please enter a question.");
                return;
            }

            AskButton.IsEnabled = false;
            AnswerTextBlock.Text = "Thinking...";

            try
            {
                string answer = await OpenAIService.AskCourseQuestionAsync(
                selectedCourse.SyllabusText,
                question
                );

                AnswerTextBlock.Text = answer;
            }
            catch (Exception ex)
            {
                AnswerTextBlock.Text = "Something went wrong.";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                AskButton.IsEnabled = true;
            }
        }

    }
}