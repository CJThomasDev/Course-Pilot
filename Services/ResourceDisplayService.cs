using Course_Pilot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Course_Pilot.Services
{
    class ResourceDisplayService
    {
        //references to the resource display controls in the Class Details window
        private readonly StackPanel _resourcesPanel;
        private readonly ScrollViewer _resourcesScrollViewer;
        private readonly Button _generateResourcesButton;


        //initializes the service with the UI controls used to display resources
        public ResourceDisplayService(
            StackPanel resourcesPanel,
            ScrollViewer resourcesScrollViewer,
            Button generateResourcesButton)
        {
            _resourcesPanel = resourcesPanel;
            _resourcesScrollViewer = resourcesScrollViewer;
            _generateResourcesButton = generateResourcesButton;
        }


        // Displays the generated resources in the Class Details window
        public void DisplayResources(CourseResources resources)
        {
            //clear any previously displayed resources
            _resourcesPanel.Children.Clear();

            AddResourceSection("Textbooks", resources.Textbooks);
            AddResourceSection("Videos", resources.Videos);
            AddResourceSection("Websites", resources.Websites);
            AddResourceSection("Practice Resources", resources.PracticeResources);

            //replace the generate button with the generated resources
            _generateResourcesButton.Visibility = Visibility.Collapsed;
            _resourcesScrollViewer.Visibility = Visibility.Visible;
        }

        //creates and displays a resource category with its associated resources
        private void AddResourceSection(
            string sectionTitle,
            List<ResourceItem> resources)
        {
            //skip this section if no resources were returned
            if (resources == null || resources.Count == 0)
            {
                return;
            }
            //create the section heading
            TextBlock heading = new TextBlock
            {
                Text = sectionTitle,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 10, 0, 5)
            };

            //add the section heading to the resources panel
            _resourcesPanel.Children.Add(heading);

            //create a UI entry for each resource
            foreach (ResourceItem resource in resources)
            {
                //create a container for the resource information
                StackPanel itemPanel = new StackPanel
                {
                    Margin = new Thickness(0, 0, 0, 12)
                };
                //create the resource title
                TextBlock title = new TextBlock
                {
                    Text = resource.Title,
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold,
                    TextWrapping = TextWrapping.Wrap
                };
                //create the resource description
                TextBlock description = new TextBlock
                {
                    Text = resource.Description,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.DimGray,
                    Margin = new Thickness(0, 2, 0, 4)
                };
                //add the resource information to the container
                itemPanel.Children.Add(title);
                itemPanel.Children.Add(description);

                //create an "Open Resource" button if a valid URL exists
                if (!string.IsNullOrWhiteSpace(resource.Url))
                {
                    Button openButton = new Button
                    {
                        Content = "Open Resource",
                        Width = 110,
                        Height = 28,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Tag = resource.Url
                    };
                    //open the resource when the button is clicked
                    openButton.Click += OpenResourceButton_Click;

                    //add the button to the resource container
                    itemPanel.Children.Add(openButton);
                }

                //add the completed resource entry to the resources panel
                _resourcesPanel.Children.Add(itemPanel);
            }
        }

        //opens the selected resource in the user's default web browser
        private void OpenResourceButton_Click(object sender, RoutedEventArgs e)
        {
            //verify that the button contains a valid URL
            if (sender is not Button button ||
                button.Tag is not string url ||
                string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            try
            {
                //launch the resource using the default application
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                //notify the user if the resource could not be opened
                MessageBox.Show(
                    $"Could not open the resource:\n{ex.Message}",
                    "Open Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

    }
}
