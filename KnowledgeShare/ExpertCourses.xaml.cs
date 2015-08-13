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
using System.Windows.Navigation;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using KnowledgeShare.ViewModels;
using KnowledgeShare.Models;

namespace KnowledgeShare
{
    public partial class ExpertCourses : PhoneApplicationPage
    {
        private string _userId;
        private CoursesVm _coursesVm;
        public ExpertCourses()
        {
            InitializeComponent();
            _coursesVm = new CoursesVm() { Course = new List<Courses>()};
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!NavigationContext.QueryString.TryGetValue("userid", out _userId))
            {
                _userId = null;
            }
            GetExpertData();          

        }

        public void GetExpertData() {
            var api = "http://knowledgeshare.azurewebsites.net/api/expert/" + _userId;
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
                    new DataContractJsonSerializer(typeof(Courses[]));
                ////S3: Read JSON Data and DeSerialize into object MedicineMaster[]
                var solutions = (Courses[])jsonData.ReadObject(jsonDataStream);
                foreach (Courses solution in solutions)
                {
                    _coursesVm.Course.Add(solution);
                }
                DataContext = _coursesVm;
                //lstMedicine.ItemsSource = Medicines;
                //lstMedicine.DisplayMemberPath = "MedicineName";
                //lstMedicine.SelectedValuePath = "MedicineId";
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Menu.xaml?userid=" + _userId, UriKind.Relative));
        }
    }
}