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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using KnowledgeShare.ViewModels;
using KnowledgeShare.Models;
using System.Text;
using System.Windows.Navigation;

namespace KnowledgeShare
{
    public partial class Page3 : PhoneApplicationPage
    {
        private string _userId;
        public Page3()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!NavigationContext.QueryString.TryGetValue("userid", out _userId))
            {
                _userId = null;
            }
            textBox2.Text = _userId;
            textBox3.Text = UserDetails.Email;
            textBox1.Text = UserDetails.FirstName;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (textBox2.Text == "" || textBox4.Text == "")
            {
                textBlock7.Text = "Please fill in Zid and Problem";
                return;
            }
            var expert = new ViewSolution()
            {
                Course = textBox6.Text,
                FirstName = textBox1.Text,
                Zid = textBox2.Text,
                Email = textBox3.Text,  
                LastName = UserDetails.LastName,
                ProblemDescription = textBox4.Text,               
                Id = 0
            };

            //S1: Generate the JSON Serializer Data
            DataContractJsonSerializer jsonData =
                new DataContractJsonSerializer(typeof(ViewSolution));
            MemoryStream memStream = new MemoryStream();
            //S2 : Write data into Memory Stream
            jsonData.WriteObject(memStream, expert);
            //S3 : Read the bytes from Stream do that it can then beconverted to JSON String 
            byte[] jsonDataToPost = memStream.ToArray();
            memStream.Close();
            //S4: Ehencode the stream into string format
            var data = Encoding.UTF8.GetString(jsonDataToPost, 0, jsonDataToPost.Length);

            var api = "http://knowledgeshare.azurewebsites.net/api/problem";
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
                textBlock7.Text = e.Result;
                textBox6.Text = "";              

               
            }
               
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Menu.xaml?userid=" + _userId, UriKind.Relative));
        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        }


    }
