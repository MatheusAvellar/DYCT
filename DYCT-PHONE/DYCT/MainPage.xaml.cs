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
using System.Text.RegularExpressions;
 
namespace DYCT
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage() {
            InitializeComponent();
            this.ErrorText.Text = "Please insert a username!";
        }
 
        private void Button_Click(object sender, RoutedEventArgs e) {
            if (InsertUsernameBox.Text != "") {
                this.InsertUsernameBox.Visibility = Visibility.Collapsed;
                this.InsertUsernameText.Visibility = Visibility.Collapsed;
                this.FindOutButton.Visibility = Visibility.Collapsed;
                this.ErrorText.Visibility = Visibility.Collapsed;

                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringCallback);
                wc.DownloadStringAsync(new Uri("http://matheus.avellar.c9.io/dyct?n=" + this.InsertUsernameBox.Text));
            } else {
                this.ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void DownloadStringCallback(object sender, DownloadStringCompletedEventArgs e) {
            string result = e.Result;
            string name = this.InsertUsernameBox.Text;
            this.InsertUsernameBox.Text = "";
            this.UsernameText.Text = name;

            this.UsernameText.Visibility = Visibility.Visible;

            if (result[0] != '!') {
                string[] strArr = result.Replace("&ndash;", "-").Split('|');

                if (strArr[strArr.Length - 1] == "!!!") {
                    this.CommitInfoText.Text = (strArr[4] != "No recent contributions") ? "Last commit: " + strArr[4] : strArr[4];
                    this.TheSubtitle.Text = "No!";
                    this.ContentPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0xf3, 0x9c, 0x12));
                } else {
                    string[] removedBreaks = Regex.Replace(result.Replace("&ndash;", "-").Replace("\r\n", "").Replace("\n", "").Replace("\r", ""), @"\r\n?|\n", "").Split('|');
                    this.CommitInfoText.Text = "Current streak: " + removedBreaks[4].Replace(System.Environment.NewLine, "").Replace("              ", "");
                    this.TheSubtitle.Text = "Yes!";
                    this.ContentPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0x23, 0x9c, 0x56));

                    this.CommitLog.Text = "Best streak: " + removedBreaks[1] + "\n";
                    this.CommitLog.Text += "(" + removedBreaks[2] + ")\n";
                    this.CommitLog.Text += "Current streak: " + removedBreaks[3] + "\n";
                    this.CommitLog.Text += "(" + removedBreaks[4] + ")\n";
                }

                this.InsertUsernameBox.Visibility = Visibility.Visible;
                this.InsertUsernameText.Visibility = Visibility.Visible;
                this.FindOutButton.Visibility = Visibility.Visible;
            } else {
                string err = result[1] == '0' ? "Invalid username." :
                             result[1] == '1' ? "Error loading page." :
                             result[1] == '2' ? "No user with given username." :
                             result[1] == '3' ? "Name belongs to organization, not user." : "Something went wrong!";
                this.TheSubtitle.Text = "Error";
                this.CommitInfoText.Text = err;
                this.ContentPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0xc0, 0x39, 0x2b));
                this.InsertUsernameBox.Visibility = Visibility.Visible;
                this.InsertUsernameText.Visibility = Visibility.Visible;
                this.FindOutButton.Visibility = Visibility.Visible;
            }
        }

        /*
         * 0xff, 0x23, 0x9c, 0x56 GREEN
         * 0xff, 0xc0, 0x39, 0x2b RED
         * 0xff, 0xf3, 0x9c, 0x12 ORANGE
         */
    }
}