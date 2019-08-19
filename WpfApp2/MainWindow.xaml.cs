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
        public double x = 0;
        public double y = 0;
        public double deg= 0;
        public MainWindow()
        {
            InitializeComponent();
            textBoxWeatherInfo.Document.Blocks.Clear();
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
                textBoxWeatherInfo.Document.Blocks.Clear();
                JObject details = JObject.Parse(weather);
                lblCityName.Content = details.GetValue("name");
                WeatherInfo currentWeather = JsonConvert.DeserializeObject<WeatherInfo>(weather);
                textBoxWeatherInfo.AppendText($"{Properties.Resources.temperature}: {(currentWeather.Main.Temp - 273.15).ToString()} {Properties.Resources.degT}");
                textBoxWeatherInfo.AppendText($"\nHumidity: {currentWeather.Main.Humidity.ToString()} %");
                textBoxWeatherInfo.AppendText($"\nPressure: {currentWeather.Main.Pressure.ToString()}");
                textBoxWeatherInfo.AppendText($"\n{Properties.Resources.wind}: {currentWeather.Wind.Deg}, {currentWeather.Wind.Speed.ToString()} {Properties.Resources.windSpeed}");

                windSock.Source = new BitmapImage(new Uri(@"pack://siteoforigin:,,/Resources/sock.png"));
                windRose.Source = new BitmapImage(new Uri(@"pack://siteoforigin:,,/Resources/compass.png"));


                if (currentWeather.Wind.Speed > 0)
                    DrawWind(currentWeather.Wind.Deg, currentWeather.Wind.Speed);
                textBoxWeatherInfo.AppendText($"\nClouds: {currentWeather.Clouds.All.ToString()} %");
                textBoxWeatherInfo.AppendText($"\nVisibility: {currentWeather.Visibility.ToString()}");

                if (currentWeather.Clouds.All > 0)
                {
                    imgCloud.Source = new BitmapImage(new Uri(@"pack://siteoforigin:,,/Resources/cloud1.png"));
                    imgCloud.Opacity = currentWeather.Clouds.All / 100.0;
                }


                //Console.WriteLine($"{currentWeather.Name}({currentWeather.Coord.Lat} - {currentWeather.Coord.Lon})");
            }
            else
            {
                //Console.WriteLine("No weather data available");
            }
        }

        private void DrawWind(double deg, double speed)
        {


            ScaleTransform scaleTransform = new ScaleTransform(1, 1*(speed/30));
            RotateTransform rotateTransform = new RotateTransform(deg, 25* speed / 30, 100* speed / 30);

            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(rotateTransform);
            myTransformGroup.Children.Add(scaleTransform);

            windSock.RenderTransform = myTransformGroup;
        }

        private static void GetWeatherDataSync()
        {
            lastError = "";
            WebClient client = new WebClient();
            try
            {
                Stream stream = client.OpenRead("https://api.openweathermap.org/data/2.5/weather?q=Norwich,uk&appid=dac392b2d2745b3adf08ca26054d78c4&lang=ru");
                weather = new StreamReader(stream).ReadToEnd();
                lastError = "successfuly";
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            switch (comboLang.SelectedIndex)
            {
                case 0 :
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
                    break;
                default:
                    break;
            }
            */
        }

    }
}
