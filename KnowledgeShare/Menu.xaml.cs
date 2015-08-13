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

namespace KnowledgeShare
{
    public partial class Page1 : PhoneApplicationPage
    {
        private string _userId;
        public Page1()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (!NavigationContext.QueryString.TryGetValue("userid", out _userId))
            {
                _userId = null;
            }
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/EnrollExpert.xaml?userid=" + _userId, UriKind.Relative));
        }

        private void hyperlinkButton3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ProblemSubmission.xaml?userid=" + _userId, UriKind.Relative));
        }

        private void hyperlinkButton2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ExpertCourses.xaml?userid=" + _userId, UriKind.Relative));
        }

        private void hyperlinkButton4_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ProvideSolution.xaml?userid=" + _userId, UriKind.Relative));
        }

        private void hyperlinkButton5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ViewSolutions.xaml?userid=" + _userId, UriKind.Relative));
        }

        private void hyperlinkButton6_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}