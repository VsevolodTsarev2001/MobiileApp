using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;

using System.Threading.Tasks;

namespace MobiileApp
{
    public partial class Valgusfoor : ContentPage
    {
        private bool isOn = false;
        private bool isAutoMode = false;
        private bool isDayAndNightMode = false;
        private List<Frame> circles = new();
        private Dictionary<string, Color> colors = new()
        {
            { "punane", Colors.Red },
            { "kollane", Colors.Yellow },
            { "roheline", Colors.Green }
        };

        public Valgusfoor(int k)
        {
            //InitializeComponent();
            CreateTrafficLight();


        }

        // Создание светофора
        private void CreateTrafficLight()
        {


            foreach (var key in colors.Keys)
            {
                var frame = new Frame
                {
                    WidthRequest = 100,
                    HeightRequest = 100,
                    CornerRadius = 50,
                    BackgroundColor = Colors.Gray,
                    BorderColor = Colors.Black,
                    HorizontalOptions = LayoutOptions.Center
                };

                var label = new Label
                {
                    Text = key,
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };



                var stack = new VerticalStackLayout { Children = { frame, label } };
                trafficLightContainer.Children.Add(stack);

                frame.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => ChangeText(label, key)) });
                circles.Add(frame);

            }

        }

        // Включить светофор
        private void TurnOnLight(object sender, EventArgs e)
        {
            isOn = true;
            statusLabel.Text = "Valgusfoor põleb";
            int i = 0;
            foreach (var key in colors.Keys)
            {
                circles[i].BackgroundColor = colors[key];
                i++;
            }
        }

        // Выключить светофор
        private void TurnOffLight(object sender, EventArgs e)
        {
            isOn = false;
            isAutoMode = false;
            isDayAndNightMode = false;
            DayOrNight.Text = "";
            statusLabel.Text = "Valgusfoor on välja lülitatud";
            foreach (var circle in circles)
            {
                circle.BackgroundColor = Colors.Gray;
            }
        }

        // Изменение текста при клике
        private void ChangeText(Label label, string key)
        {
            if (!isOn)
            {
                statusLabel.Text = "Lülitage esmalt valgusfoor põlema!";
                return;
            }

            switch (key)
            {
                case "punane":
                    label.Text = "Peatus";
                    break;
                case "kollane":
                    label.Text = "Oota";
                    break;
                case "roheline":
                    label.Text = "Mine";
                    break;
            }
        }

        private async void StartAutoMode(object sender, EventArgs e)
        {
            if (!isOn)
            {
                statusLabel.Text = "Lülitage esmalt valgusfoor põlema!";
                return;
            }

            isAutoMode = true;
            statusLabel.Text = "Auto Mode aktiivne!";

            string[] sequence = { "punane", "kollane", "roheline" };
            int index = 0;

            while (isAutoMode)
            {
                // Сбрасываем цвета (делаем все серыми)
                for (int i = 0; i < circles.Count; i++)
                {
                    circles[i].BackgroundColor = Colors.Gray;
                }

                // Включаем текущий цвет
                circles[index].BackgroundColor = colors[sequence[index]];

                // Если горит зеленый - мигаем перед переключением
                if (sequence[index] == "roheline")
                {
                    for (int j = 0; j < 3; j++) // Мигаем 3 раза
                    {
                        await Task.Delay(500);
                        circles[index].BackgroundColor = Colors.Gray; // Выкл
                        await Task.Delay(500);
                        circles[index].BackgroundColor = colors["roheline"]; // Вкл
                    }
                }

                // Переход к следующему цвету
                index = (index + 1) % sequence.Length;

                await Task.Delay(2000);
            }

        }

        private async void DayAndNightMode(object sender, EventArgs e)
        {
            if (!isOn)
            {
                statusLabel.Text = "Lülitage esmalt valgusfoor põlema!";
                return;
            }

            isDayAndNightMode = true;

            string[] mode = { "Päev", "Öö" };  // Переключение между режимами
            int currentModeIndex = 0;  // Индекс текущего режима

            while (isDayAndNightMode)
            {
                statusLabel.Text = "Päev ja öö mood aktiivne!";
                DayOrNight.Text = mode[currentModeIndex];  // Обновление текста с режимом

                // Переключение между "Päev" и "Öö" 
                currentModeIndex = (currentModeIndex) % mode.Length;

                // Если режим "Päev"
                if (mode[currentModeIndex] == "Päev")
                {
                    string[] sequence = { "punane", "kollane", "roheline" };
                    int index = 0;


                    for (int i = 0; i < circles.Count; i++)
                    {
                        circles[i].BackgroundColor = Colors.Gray;
                    }

                    circles[index].BackgroundColor = colors[sequence[index]];

                    if (sequence[index] == "roheline")
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            await Task.Delay(500);
                            circles[index].BackgroundColor = Colors.Gray;
                            await Task.Delay(500);
                            circles[index].BackgroundColor = colors["roheline"];
                        }
                    }

                    index = (index + 1) % sequence.Length;

                    await Task.Delay(2000);

                }
                // Если режим "Öö"- мигаем только желтым
                else if (mode[currentModeIndex] == "Öö")
                {
                    for (int j = 0; j < 5; j++)
                    {
                        circles[1].BackgroundColor = Colors.Gray;
                        await Task.Delay(500);
                        circles[1].BackgroundColor = colors["kollane"];
                        await Task.Delay(500);
                    }
                }
                await Task.Delay(5000);
            }
        }

        private void ChangeColorToGray(object sender, EventArgs e)
        {
            foreach (var circle in circles)
            {
                circle.BackgroundColor = Colors.Gray;
            }
        }
    }
}