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
    public partial class SolutionDetailPage : PhoneApplicationPage
    {
        string selectedId;
        private ViewSolutionsViewModel ViewModelSolution;
        public SolutionDetailPage()
        {
            InitializeComponent();
            ViewModelSolution = new ViewSolutionsViewModel() { Items = new List<ViewSolution>()};
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (NavigationContext.QueryString.TryGetValue("selectedId", out selectedId))
            {
                GetSolutionData();


            }
        }
        private void GetSolutionData()
        {

            var api = "http://knowledgeshare.azurewebsites.net/api/solution/" + "test" + "/"+selectedId;
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
                foreach (ViewSolution solution in solutions)
                {
                    ViewModelSolution.Items.Add(solution);
                }

                DataContext = ViewModelSolution.Items.FirstOrDefault();
                //lstMedicine.ItemsSource = Medicines;
                //lstMedicine.DisplayMemberPath = "MedicineName";
                //lstMedicine.SelectedValuePath = "MedicineId";
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Menu.xaml?userid=" + UserDetails.Zid, UriKind.Relative));
        }
    }
}