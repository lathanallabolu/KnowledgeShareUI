using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace KnowledgeShare.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProblemDescription { get; set; }
        public string Zid { get; set; }
        public string Email { get; set; }
        public string Solution { get; set; }
        public string SolutionBy { get; set; }
    }
}
