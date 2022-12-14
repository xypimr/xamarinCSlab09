#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace xamarinCSlab09
{
    
    public partial class MainPage : ContentPage
    {
        public static string? ApiKey = "9f74a9ce6cc82482fabb284c2894a2f2";
        private static List<City> _arrayOfCities = new List<City>();
        public MainPage()
        {
            InitializeComponent();
            //
            // try
            // {
            // var filename = "../../../API_KEY.txt";
            // ApiKey = File.ReadAllText("/Users/oldmash/RiderProjects/labsC#/xamarinCSlab09/xamarinCSlab09/xamarinCSlab09/API_KEY.txt");
            // }
            // catch (FileNotFoundException)
            // {
            //     Console.WriteLine("\n\nCreate API_KEY.txt file in project's directory\n\n");
            // }
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            using (Stream input = assembly.GetManifestResourceStream("xamarinCSlab09.city.txt"))
            {    
                StreamReader reader = new StreamReader(input);
                City entry = new();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine()?.Split('\t');
                    entry.Name = line[0];
                    var coord = line[1].Replace(",", "").Split(' ');
                    
                    entry.Latitude = coord[0];
                    entry.Longitude = coord[1];
                    _arrayOfCities.Add(entry);
                    var button = new Button()
                    {
                        Text = $"{entry.Name}", 
                        TextColor = Color.DimGray, 
                        BackgroundColor = Color.Beige,
                        FontSize = 20
                    };
                    button.Clicked += ChooseCity;
                    Frame frame = new Frame
                    {
                        BackgroundColor = Color.Beige,
                        Padding = new Thickness(10),
                        Margin = new Thickness(10),
                        CornerRadius = 10,
                        Content = button
                    };
                    if (list != null) list.Children.Add(frame);
                }
            
                reader.Close();
            }
            
        }
        private void ChooseCity(object sender, EventArgs e)
        {
            Console.WriteLine(2);
            Button button = (Button)sender;
            var city = _arrayOfCities.Find(p => p.Name == button.Text);
            Info.Text = $"Звоним в {button.Text} узнавать погоду";
            GetWeather(city).GetAwaiter();
            // button.BackgroundColor = Color.Red;
        }

        public async Task<WeatherOBJ?> GetWeather(City city)
        {
            Console.WriteLine(3);
            var url = $"https://api.openweathermap.org/data/2.5/weather";
            var parameters = $"?lat={city.Latitude}&lon={city.Longitude}&appid={ApiKey}";
        
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
        
            HttpResponseMessage? response = new();
            try
            {
                response = await client.GetAsync(parameters);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                Info.Text = $"{city.Name} : Connection troubles, try again";
            }
        
            WeatherOBJ result = new WeatherOBJ();
            if (response.IsSuccessStatusCode)
            {
                var textRes = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<WeatherOBJ>(textRes);
                Info.Text = $"In {city.Name} now {result.Temp:F1} degrees : {result.Description}";
            }
            else
            {
                Info.Text = "Не дозвонились...";
            }
            
            return result;
        }
    }
}