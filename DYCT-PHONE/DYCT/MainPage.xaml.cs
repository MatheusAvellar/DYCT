using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DYCT.Resources;
using System.Threading.Tasks;
 
namespace DYCT
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage() {
            InitializeComponent();
        }
 
        private void Button_Click(object sender, RoutedEventArgs e) {
            if (InsertUsernameBox.Text != "") {
                this.InsertUsernameBox.Visibility = Visibility.Collapsed;
                this.InsertUsernameText.Visibility = Visibility.Collapsed;
                this.FindOutButton.Visibility = Visibility.Collapsed;
                this.ErrorText.Visibility = Visibility.Collapsed;
                this.UsernameText.Text = CheckCommit(InsertUsernameBox.Text).Result.ToString();
            } else {
                this.ErrorText.Text = "Please insert a username!";
                this.ErrorText.Visibility = Visibility.Visible;
            }
        }

        public async Task<string> CheckCommit(string str)
        {
            var webClient = new WebClient();
            Console.WriteLine("1");
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var result = webClient.DownloadStringTaskAsync(new Uri("http://matheus.avellar.c9.io/dyct?n=" + str);
            Console.WriteLine("2");
            if (result[0] != '!') {
                this.TheSubtitle.Text = "Yes!";
                this.UsernameText.Text = str + " did commit today!";
                this.ContentPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0x23, 0x9c, 0x56));
            } else {
                string err = result[1] == '0' ? "Invalid username." :
                             result[1] == '1' ? "Error loading page." :
                             result[1] == '2' ? "No user with given username." :
                             result[1] == '3' ? "Name belongs to organization, not user." : "Potatoes";
                this.TheSubtitle.Text = "Error";
                this.UsernameText.Text = "Something went wrong!";
                this.ContentPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0x23, 0x9c, 0x56));
                this.InsertUsernameBox.Visibility = Visibility.Visible;
                this.InsertUsernameText.Visibility = Visibility.Visible;
                this.FindOutButton.Visibility = Visibility.Visible;
            }
            return result;
        }
    }
}