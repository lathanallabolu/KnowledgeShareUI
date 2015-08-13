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
using KnowledgeShare.Models;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using KnowledgeShare.ViewModels;
using System.Windows.Navigation;

using System.Text;

namespace KnowledgeShare
{
    public partial class Page2 : PhoneApplicationPage
    {
        string selectedId;
        private ViewSolutionsViewModel ViewModelSolution;
        string userId;
        public Page2()
        {
            InitializeComponent();
            ViewModelSolution = new ViewSolutionsViewModel() { Items = new List<ViewSolution>() };
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("userId", out userId);
            
            if (NavigationContext.QueryString.TryGetValue("selectedId", out selectedId))
            {
                GetSolutionData();


            }
        }
        private void GetSolutionData()
        {

            var api = "http://knowledgeshare.azurewebsites.net/api/solution/" + "test" + "/" + selectedId;
            WebClient webClient, webClientSolution;
            webClientSolution = new WebClient();
            webClientSolution.OpenReadCompleted +=
        new OpenReadCompletedEventHandler(webClientSolution_OpenReadCompleted);
            webClientSolution.OpenReadAsync(new Uri(api));
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBlock11.Text = "Please provide solution";
                return;
            }
            var solution = new ViewSolution { Id = Convert.ToInt32(selectedId), Solution = textBox4.Text, SolutionBy = userId };
           
            DataContractJsonSerializer jsonData =
              new DataContractJsonSerializer(typeof(ViewSolution));
            MemoryStream memStream = new MemoryStream();
            //S2 : Write data into Memory Stream
            jsonData.WriteObject(memStream, solution);
            //S3 : Read the bytes from Stream do that it can then beconverted to JSON String 
            byte[] jsonDataToPost = memStream.ToArray();
            memStream.Close();
            //S4: Ehencode the stream into string format
            var data = Encoding.UTF8.GetString(jsonDataToPost, 0, jsonDataToPost.Length);

            var api = "http://knowledgeshare.azurewebsites.net/api/solution";
            WebClient webClient, webClientSolution;
            webClientSolution = new WebClient();
            webClientSolution.UploadStringCompleted +=
        new UploadStringCompletedEventHandler(webClientSolution_OpenWriteCompleted);
            webClientSolution.Headers["content-type"] = "application/json";
            webClientSolution.UploadStringAsync(new Uri(api), "POST", data);
        }

        void webClientSolution_OpenWriteCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                //S1: Read the Received Stream
                //Stream jsonDataStream = e.Result;
                ////S2: JSON Serializer Container
                //DataContractJsonSerializer jsonData =
                //    new DataContractJsonSerializer(typeof(string));
                //////S3: Read JSON Data and DeSerialize into object MedicineMaster[]
                //var solutions = (string)jsonData.ReadObject(jsonDataStream);
                textBlock11.Text = e.Result;
                DataContext = new ViewSolutionsViewModel() {Items=new List<ViewSolution>()  };
              
            }
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
            NavigationService.Navigate(new Uri("/Menu.xaml?userid=" + userId, UriKind.Relative));
        }
    }
}