using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using KnowledgeShare.ViewModels;
using System.Windows.Navigation;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using KnowledgeShare.Models;

namespace KnowledgeShare
{
    public partial class ViewSolutions : PhoneApplicationPage
    {
        private string _userId;
        private ViewSolutionsViewModel ViewModelSolution;
        public ViewSolutions()
        {
            InitializeComponent();
            ViewModelSolution = new ViewSolutionsViewModel() { Items = new List<ViewSolution>()};
            //ViewModelSolution.Items.Add(new ViewSolution { CourseId = "test" , Solutions = "this is best"});
            //ViewModelSolution.Items.Add(new ViewSolution { CourseId = "test2", Solutions = "why the hell" });
            
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!NavigationContext.QueryString.TryGetValue("userid", out _userId))
            {
                _userId = null;
            }
            GetSolutionData();
            //string selectedIndex = "";
            //if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            //{
            //    int index = int.Parse(selectedIndex);
            //    DataContext = App.ViewModel.Items[index];
            //}


        }

        private void GetSolutionData()
        {

            var api = "http://knowledgeshare.azurewebsites.net/api/solution/" + _userId;
            WebClient webClient, webClientSolution;
            webClientSolution = new WebClient();
            webClientSolution.OpenReadCompleted +=
        new OpenReadCompletedEventHandler(webClientSolution_OpenReadCompleted);
            webClientSolution.OpenReadAsync(new Uri(api));
        }   

        void webClientSolution_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                //S1: Read the Received Stream
                Stream jsonDataStream = e.Result;
                //S2: JSON Serializer Container
                DataContractJsonSerializer jsonData =
                    new DataContractJsonSerializer(typeof(ViewSolution[]));
                ////S3: Read JSON Data and DeSerialize into object MedicineMaster[]
                var solutions = (ViewSolution[])jsonData.ReadObject(jsonDataStream);
                foreach(ViewSolution solution in solutions){
                    ViewModelSolution.Items.Add(solution);
                }
                DataContext = ViewModelSolution;
                //lstMedicine.ItemsSource = Medicines;
                //lstMedicine.DisplayMemberPath = "MedicineName";
                //lstMedicine.SelectedValuePath = "MedicineId";
            }

        }
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/SolutionDetailPage.xaml?selectedId=" + ViewModelSolution.Items[MainListBox.SelectedIndex].Id, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Menu.xaml?userid=" + _userId, UriKind.Relative));
        }
    }
}