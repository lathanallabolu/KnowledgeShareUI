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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using KnowledgeShare.ViewModels;
using KnowledgeShare.Models;
using System.Text;
using System.Windows.Navigation;
using System.IO;

namespace KnowledgeShare
{
    public partial class Page4 : PhoneApplicationPage
    {
        public Page4()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "") 
            {
                textBlock6.Text = "Please fill in all details";
                return;
            }
            var user = new User()
            {
                FirstName = textBox1.Text,
                LastName = textBox2.Text,
                Zid = textBox3.Text,                
                Password = textBox4.Text,
                Email = textBox5.Text,
               
            };

            //S1: Generate the JSON Serializer Data
            DataContractJsonSerializer jsonData =
                new DataContractJsonSerializer(typeof(User));
            MemoryStream memStream = new MemoryStream();
            //S2 : Write data into Memory Stream
            jsonData.WriteObject(memStream, user);
            //S3 : Read the bytes from Stream do that it can then beconverted to JSON String 
            byte[] jsonDataToPost = memStream.ToArray();
            memStream.Close();
            //S4: Ehencode the stream into string format
            var data = Encoding.UTF8.GetString(jsonDataToPost, 0, jsonDataToPost.Length);

            var api = "http://knowledgeshare.azurewebsites.net/api/user";
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
                textBlock6.Text = e.Result;
                textBox1.Text = "";
                textBox2.Text = "";

                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";


            }

        }
    }

}