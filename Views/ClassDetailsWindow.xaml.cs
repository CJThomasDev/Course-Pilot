using Course_Pilot.Models;
using Course_Pilot.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Course_Pilot.Views
{
    /// <summary>
    /// Interaction logic for ClassDetailsWindow.xaml
    /// </summary>
    public partial class ClassDetailsWindow : Window
    {
        public Course Course { get; }

        public ClassDetailsWindow(Course course)
        {
            InitializeComponent();

            Course = course;
            
            DataContext = Course;
        }

        //button to generate helpful resources for the user
        private async void GenerateResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GenerateResourcesButton.IsEnabled = false;
                GenerateResourcesButton.Content = "Generating...";
                

                //generate course resources
                CourseResources? resources =
                    await ApiService.GenerateCourseResourcesAsync(Course);
                //if there were none returned then throw an error
                if (resources == null)
                {
                    MessageBox.Show(
                        "The resources could not be generated.",
                        "Generation Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }
                //create the resource display service
                ResourceDisplayService resourceDisplayService =
                new ResourceDisplayService(
                    ResourcesPanel,
                    ResourcesScrollViewer,
                    GenerateResourcesButton);
                //display the generated resources
                resourceDisplayService.DisplayResources(resources);

                //make the clear button appear
                ClearResourcesButton.Visibility = Visibility.Visible;

                //notify the user that generation succeeded
                MessageBox.Show(
                    "Resources generated successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                //throw error if something went wrong
                MessageBox.Show(
                    $"Something went wrong:\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                //restore the button state
                GenerateResourcesButton.IsEnabled = true;
                GenerateResourcesButton.Content = "Generate Helpful Resources";
            }
        }

        private void ClearResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            //remove the generated resources from the course
            Course.Resources = null;

            //remove the displayed resources
            ResourcesPanel.Children.Clear();

            //restore the original UI
            ResourcesScrollViewer.Visibility = Visibility.Collapsed;
            GenerateResourcesButton.Visibility = Visibility.Visible;
            ClearResourcesButton.Visibility = Visibility.Collapsed;
        }
    }
}
