using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DYCT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TimeToday.Content = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DateTime.Now.ToString("MMMM"));
        }

        private void UsernameButton(object sender, RoutedEventArgs e)
        {
            string _U = UsernameCheck();
            Potato.Content = _U;
            Console.WriteLine(_U);
            // #ff27ae60 GREEN
            // #ffc0392b RED
            if (_U == "Error loading page.") {
                ChangeBg("#ffc0392b");
            } else {
                ChangeBg("#ff27ae60");
                Submit.Visibility = Visibility.Collapsed;

                /*
                 * ... Check if today's the day and whatnot :l
                 * I'm just really sad and tired due to the phone app not working.
                 */
            }
        }

        public void ChangeBg(string str)
        {
            Master.Background = (Brush)new BrushConverter().ConvertFromString(str);
        }

        public string UsernameCheck()
        {
            try {
                string str = "";
                WebRequest request = WebRequest.Create("http://matheus.avellar.c9.io/dyct?n=" + this.UsernameInput.Text);
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream())) {
                    str = stream.ReadToEnd();
                }
                return str;
            } catch {
                return "Error loading page.";
            }
        }
    }
}
