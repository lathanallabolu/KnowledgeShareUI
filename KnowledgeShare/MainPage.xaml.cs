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
using System.IO;
using System.Text;
using KnowledgeShare.Models;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using KnowledgeShare.ViewModels;
using KnowledgeShare.Models;
using System.Text;
using System.Windows.Navigation;

namespace KnowledgeShare
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Handle selection changed on ListBox
        //private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // If selected index is -1 (no selection) do nothing
        //    if (MainListBox.SelectedIndex == -1)
        //        return;

        //    // Navigate to the new page
        //    NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

        //    // Reset selected index to -1 (no selection)
        //    MainListBox.SelectedIndex = -1;
        //}

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                textBlock4.Text = "Please enter Zid and Password";
                return;
            }
            var zid = textBox1.Text;
            var password = textBox2.Text;
            var api = "http://knowledgeshare.azurewebsites.net/api/user/" + zid;
            WebClient webClient, webClientUser;
            webClientUser = new WebClient();
            webClientUser.OpenReadCompleted +=
        new OpenReadCompletedEventHandler(webClientMedicine_OpenReadCompleted);
            webClientUser.OpenReadAsync(new Uri(api));
         //   NavigationService.Navigate(new Uri("/Menu.xaml", UriKind.Relative));
        }

        void webClientMedicine_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                //S1: Read the Received Stream
                Stream jsonDataStream = e.Result;
                //S2: JSON Serializer Container
                DataContractJsonSerializer jsonData =
                    new DataContractJsonSerializer(typeof(User));
                ////S3: Read JSON Data and DeSerialize into object MedicineMaster[]
                var user = (User)jsonData.ReadObject(jsonDataStream);
                if (user == null)
                {
                    usrError.Text = "Invalid Zid";
                }

                else if (user.Password != textBox2.Text)
                {
                    usrError.Text = "";
                    pswError.Text = "Invalid Password";
                }

                else
                {
                    usrError.Text = "";
                    pswError.Text = "";
                    UserDetails.Zid = user.Zid;
                    UserDetails.Email = user.Email;
                    UserDetails.FirstName = user.FirstName;
                    UserDetails.Email = user.Email;
                    UserDetails.LastName = user.LastName;
                    NavigationService.Navigate(new Uri("/Menu.xaml?userid=" + user.Zid, UriKind.Relative));
                }
                //lstMedicine.ItemsSource = Medicines;
                //lstMedicine.DisplayMemberPath = "MedicineName";
                //lstMedicine.SelectedValuePath = "MedicineId";
            }
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SignUp.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            textBox1.Text = "";
            textBox2.Text = "";
            UserDetails.Zid = "";
            UserDetails.Email = "";
            UserDetails.FirstName = "";
            UserDetails.Email = "";
            UserDetails.LastName = "";
        }
    }
}