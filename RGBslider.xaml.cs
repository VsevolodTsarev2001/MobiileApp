namespace MobileApp
{
    public partial class RGBSlider : ContentPage
    {
        private Slider redSlider, greenSlider, blueSlider;
        private BoxView colorDisplay;
        private Label hexLabel, lblRed, lblGreen, lblBlue, header, hexCodeLabel;
        private Button saveBtn, randomColorBtn, applyHexCodeBtn;
        private Entry hexCodeEntry;
        private Stepper sizeStepper, cornerRadiusStepper, opacityStepper;
        private Frame mainFrame, redFrame, greenFrame, blueFrame;

        public RGBSlider(int k)
        {
            // Заголовок страницы
            Title = "RGB Model";

            // Инициализация элементов
            header = new Label
            {
                Text = "RGB Model",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromRgb(0, 0, 0)  // Используем Color.FromRgb для черного
            };

            lblRed = new Label { Text = "Punane: 0", TextColor = Color.FromRgb(0, 0, 0) };
            lblGreen = new Label { Text = "Roheline: 0", TextColor = Color.FromRgb(0, 0, 0) };
            lblBlue = new Label { Text = "Sinine: 0", TextColor = Color.FromRgb(0, 0, 0) };

            redSlider = CreateSlider();
            greenSlider = CreateSlider();
            blueSlider = CreateSlider();

            hexLabel = new Label
            {
                Text = "#000000",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 24,
            };

            hexCodeLabel = new Label
            {
                Text = "Praegune Hex Code: #000000",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 18,
                TextColor = Color.FromRgb(0, 0, 0),
            };

            hexCodeEntry = new Entry
            {
                Placeholder = "Sisestage Hex Code (e.g. #FF5733)",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 18,
            };

            applyHexCodeBtn = new Button
            {
                Text = "Rakenda Hex Code",
                WidthRequest = 200,
                HeightRequest = 40
            };

            applyHexCodeBtn.Clicked += ApplyHexCodeBtn_Clicked;

            saveBtn = new Button
            {
                Text = "Salvesta Hex Code",
                WidthRequest = 200,
                HeightRequest = 40
            };

            saveBtn.Clicked += async (s, e) =>
            {
                await Clipboard.SetTextAsync(hexLabel.Text);
                await DisplayAlert("Salvesta", "Hex Code lõikelauale kopeeritud", "OK");
            };

            randomColorBtn = new Button
            {
                Text = "Juhuslik värv",
                WidthRequest = 200,
                HeightRequest = 40
            };

            randomColorBtn.Clicked += RandomColorBtn_Clicked;

            sizeStepper = new Stepper { Minimum = 50, Maximum = 300, Increment = 10, Value = 100 };
            sizeStepper.ValueChanged += SizeStepper_ValueChanged;

            cornerRadiusStepper = new Stepper { Minimum = 0, Maximum = 50, Increment = 1, Value = 10 };
            cornerRadiusStepper.ValueChanged += CornerRadiusStepper_ValueChanged;

            opacityStepper = new Stepper { Minimum = 0, Maximum = 1, Increment = 0.1, Value = 1 };
            opacityStepper.ValueChanged += OpacityStepper_ValueChanged;

            mainFrame = new Frame { BackgroundColor = Color.FromRgb(0, 0, 0), HeightRequest = 200, WidthRequest = 200 };
            redFrame = new Frame { BackgroundColor = Color.FromRgb(255, 0, 0), HeightRequest = 100, WidthRequest = 100 };
            greenFrame = new Frame { BackgroundColor = Color.FromRgb(0, 255, 0), HeightRequest = 100, WidthRequest = 100 };
            blueFrame = new Frame { BackgroundColor = Color.FromRgb(0, 0, 255), HeightRequest = 100, WidthRequest = 100 };

            var layout = new AbsoluteLayout();

            AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 10, 360, 40));
            AbsoluteLayout.SetLayoutBounds(mainFrame, new Rect(10, 50, 340, 200));
            AbsoluteLayout.SetLayoutBounds(randomColorBtn, new Rect(10, 270, 340, 40));

            AbsoluteLayout.SetLayoutBounds(redFrame, new Rect(10, 320, 100, 100));
            AbsoluteLayout.SetLayoutBounds(greenFrame, new Rect(130, 320, 100, 100));
            AbsoluteLayout.SetLayoutBounds(blueFrame, new Rect(250, 320, 100, 100));

            AbsoluteLayout.SetLayoutBounds(lblRed, new Rect(20, 430, 100, 30));
            AbsoluteLayout.SetLayoutBounds(lblGreen, new Rect(130, 430, 100, 30));
            AbsoluteLayout.SetLayoutBounds(lblBlue, new Rect(260, 430, 100, 30));

            AbsoluteLayout.SetLayoutBounds(redSlider, new Rect(20, 470, 330, 60));
            AbsoluteLayout.SetLayoutBounds(greenSlider, new Rect(20, 500, 330, 60));
            AbsoluteLayout.SetLayoutBounds(blueSlider, new Rect(20, 530, 330, 60));

            AbsoluteLayout.SetLayoutBounds(opacityStepper, new Rect(50, 580, 300, 40));
            AbsoluteLayout.SetLayoutBounds(cornerRadiusStepper, new Rect(50, 620, 300, 40));
            AbsoluteLayout.SetLayoutBounds(sizeStepper, new Rect(50, 660, 300, 40));

            AbsoluteLayout.SetLayoutBounds(hexCodeEntry, new Rect(20, 710, 320, 40));
            AbsoluteLayout.SetLayoutBounds(applyHexCodeBtn, new Rect(50, 750, 300, 40));
            AbsoluteLayout.SetLayoutBounds(hexCodeLabel, new Rect(20, 790, 320, 30));

            layout.Children.Add(header);
            layout.Children.Add(mainFrame);
            layout.Children.Add(randomColorBtn);
            layout.Children.Add(redFrame);
            layout.Children.Add(greenFrame);
            layout.Children.Add(blueFrame);
            layout.Children.Add(lblRed);
            layout.Children.Add(lblGreen);
            layout.Children.Add(lblBlue);
            layout.Children.Add(redSlider);
            layout.Children.Add(greenSlider);
            layout.Children.Add(blueSlider);
            layout.Children.Add(opacityStepper);
            layout.Children.Add(cornerRadiusStepper);
            layout.Children.Add(sizeStepper);
            layout.Children.Add(hexCodeEntry);
            layout.Children.Add(applyHexCodeBtn);
            layout.Children.Add(hexCodeLabel);

            Content = new ScrollView
            {
                Content = layout
            };


            redSlider.ValueChanged += ColorSlider_ValueChanged;
            greenSlider.ValueChanged += ColorSlider_ValueChanged;
            blueSlider.ValueChanged += ColorSlider_ValueChanged;
        }

        private Slider CreateSlider()
        {
            return new Slider
            {
                Minimum = 0,
                Maximum = 255,
                Value = 0
            };
        }

        private void ColorSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int red = (int)redSlider.Value;
            int green = (int)greenSlider.Value;
            int blue = (int)blueSlider.Value;

            lblRed.Text = $"Punane: {red}";
            lblGreen.Text = $"Roheline: {green}";
            lblBlue.Text = $"Sinine: {blue}";

            // Устанавливаем цвета для рамок
            redFrame.BackgroundColor = Color.FromRgb(red, 0, 0);
            greenFrame.BackgroundColor = Color.FromRgb(0, green, 0);
            blueFrame.BackgroundColor = Color.FromRgb(0, 0, blue);

            // Устанавливаем основной цвет
            mainFrame.BackgroundColor = Color.FromRgb(red, green, blue);
            header.TextColor = Color.FromRgb(red, green, blue);
            lblRed.TextColor = Color.FromRgb(red, green, blue);
            lblGreen.TextColor = Color.FromRgb(red, green, blue);
            lblBlue.TextColor = Color.FromRgb(red, green, blue);

            // Создаем Hex код цвета
            string hexCode = $"#{red:X2}{green:X2}{blue:X2}";
            hexLabel.Text = hexCode;
            hexCodeLabel.Text = $"Praegune Hex Code: {hexCode}";

            // Изменяем цвет слайдеров
            redSlider.ThumbColor = Color.FromRgb(red, 0, 0);
            greenSlider.ThumbColor = Color.FromRgb(0, green, 0);
            blueSlider.ThumbColor = Color.FromRgb(0, 0, blue);

            redSlider.MinimumTrackColor = Color.FromRgb(red, 0, 0);
            greenSlider.MinimumTrackColor = Color.FromRgb(0, green, 0);
            blueSlider.MinimumTrackColor = Color.FromRgb(0, 0, blue);

            redSlider.MaximumTrackColor = Color.FromRgb(169, 169, 169);
            greenSlider.MaximumTrackColor = Color.FromRgb(169, 169, 169);
            blueSlider.MaximumTrackColor = Color.FromRgb(169, 169, 169);
        }

        private void RandomColorBtn_Clicked(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int rndRed = rnd.Next(0, 256);
            int rndGreen = rnd.Next(0, 256);
            int rndBlue = rnd.Next(0, 256);

            redSlider.Value = rndRed;
            greenSlider.Value = rndGreen;
            blueSlider.Value = rndBlue;
        }

        private void ApplyHexCodeBtn_Clicked(object sender, EventArgs e)
        {
            string hexCode = hexCodeEntry.Text?.Trim();

            if (string.IsNullOrEmpty(hexCode) || hexCode.Length != 7 || hexCode[0] != '#')
            {
                DisplayAlert("Kehtetu Hex Code", "Sisestage kehtiv hex code (e.g., #FF5733)", "OK");
                return;
            }

            try
            {
                int red = Convert.ToInt32(hexCode.Substring(1, 2), 16);
                int green = Convert.ToInt32(hexCode.Substring(3, 2), 16);
                int blue = Convert.ToInt32(hexCode.Substring(5, 2), 16);

                redSlider.Value = red;
                greenSlider.Value = green;
                blueSlider.Value = blue;

                ColorSlider_ValueChanged(null, null);
            }
            catch
            {
                DisplayAlert("Kehtetu Hex Code", "Hex code sisestatud ei kehti.", "OK");
            }
        }

        private void SizeStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double newSize = sizeStepper.Value;

            mainFrame.WidthRequest = newSize;
            mainFrame.HeightRequest = newSize;
            redFrame.WidthRequest = newSize;
            redFrame.HeightRequest = newSize;
            greenFrame.WidthRequest = newSize;
            greenFrame.HeightRequest = newSize;
            blueFrame.WidthRequest = newSize;
            blueFrame.HeightRequest = newSize;
        }

        private void CornerRadiusStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            float cornerRadius = (float)cornerRadiusStepper.Value;

            mainFrame.CornerRadius = cornerRadius;
            redFrame.CornerRadius = cornerRadius;
            greenFrame.CornerRadius = cornerRadius;
            blueFrame.CornerRadius = cornerRadius;
        }

        private void OpacityStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            float opacity = (float)opacityStepper.Value;

            mainFrame.BackgroundColor = mainFrame.BackgroundColor.WithAlpha(opacity);
            redFrame.BackgroundColor = redFrame.BackgroundColor.WithAlpha(opacity);
            greenFrame.BackgroundColor = greenFrame.BackgroundColor.WithAlpha(opacity);
            blueFrame.BackgroundColor = blueFrame.BackgroundColor.WithAlpha(opacity);
        }
    }
}
