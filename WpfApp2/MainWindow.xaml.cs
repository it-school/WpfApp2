using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string lastError = "";
        public static string weather = "";//"{\"coord\":{\"lon\":30.73,\"lat\":46.48},\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"ясно\",\"icon\":\"01d\"}],\"base\":\"stations\",\"main\":{\"temp\":300.15,\"pressure\":1013,\"humidity\":54,\"temp_min\":300.15,\"temp_max\":300.15},\"visibility\":10000,\"wind\":{\"speed\":10,\"deg\":190},\"clouds\":{\"all\":0},\"dt\":1565267352,\"sys\":{\"type\":1,\"id\":8915,\"message\":0.0061,\"country\":\"UA\",\"sunrise\":1565232363,\"sunset\":1565284775},\"timezone\":10800,\"id\":698740,\"name\":\"Odessa\",\"cod\":200}";

        public MainWindow()
        {
            Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exit button clicked");
            Close();
        }

        private void ShowWeather(object sender, RoutedEventArgs e)
        {

            GetWeatherDataSync();


            if (weather != "")
            {
                JObject details = JObject.Parse(weather);
                lblCityName.Content = details.GetValue("name");
                WeatherInfo currentWeather = JsonConvert.DeserializeObject<WeatherInfo>(weather);
                //MessageBox.Show(currentWeather.Clouds.All.ToString());
                imgSun.Opacity = 100;
 
                var bitmap = new BitmapImage(new Uri(@"pack://siteoforigin:,,/Resources/cloud1.png"));
                imgCloud.Source = bitmap;
                imgCloud.Opacity = currentWeather.Clouds.All;


                //Console.WriteLine($"{currentWeather.Name}({currentWeather.Coord.Lat} - {currentWeather.Coord.Lon})");
            }
            else
            {
                //Console.WriteLine("No weather data available");
            }
        }

        private static void GetWeatherDataSync()
        {
            lastError = "";
            WebClient client = new WebClient();
            try
            {
                using (Stream stream = client.OpenRead("https://api.openweathermap.org/data/2.5/weather?q=Kiev,ua&appid=dac392b2d2745b3adf08ca26054d78c4&lang=ru"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        weather = reader.ReadToEnd();
                    }
                }
                lastError = "successfuly";
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }
    }
}
