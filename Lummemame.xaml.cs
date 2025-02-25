using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MobiileApp
{
    public partial class Lummemame : ContentPage
    {
        private Frame head, body, baseCircle, bucket;
        private Label actionLabel;
        private Random random = new Random();
        bool isMelt = false;
        VerticalStackLayout vst;

        public Lummemame(int k)
        {
            // ведро
            bucket = new Frame
            {
                WidthRequest = 60,
                HeightRequest = 40,
                BackgroundColor = Colors.Gray,
                BorderColor = Colors.Black,
                HasShadow = true,
                CornerRadius = 10
            };

            head = new Frame
            {
                WidthRequest = 80,
                HeightRequest = 80,
                CornerRadius = 40,
                BackgroundColor = Colors.White,
                BorderColor = Colors.Black,
                HasShadow = true
            };

            body = new Frame
            {
                WidthRequest = 110,
                HeightRequest = 110,
                CornerRadius = 55,
                BackgroundColor = Colors.White,
                BorderColor = Colors.Black,
                HasShadow = true
            };

            baseCircle = new Frame
            {
                WidthRequest = 140,
                HeightRequest = 140,
                CornerRadius = 70,
                BackgroundColor = Colors.White,
                BorderColor = Colors.Black,
                HasShadow = true
            };

            vst = new VerticalStackLayout
            {
                BackgroundColor = Colors.White
            };

            AbsoluteLayout snowmanLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutBounds(bucket, new Rect(110, 20, 60, 40));
            AbsoluteLayout.SetLayoutBounds(head, new Rect(100, 60, 80, 80));
            AbsoluteLayout.SetLayoutBounds(body, new Rect(85, 140, 110, 110));
            AbsoluteLayout.SetLayoutBounds(baseCircle, new Rect(70, 250, 140, 140));

            snowmanLayout.Children.Add(bucket);
            snowmanLayout.Children.Add(head);
            snowmanLayout.Children.Add(body);
            snowmanLayout.Children.Add(baseCircle);

            // Creating the action label
            actionLabel = new Label
            {
                Text = "Tegevus: ---",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 20,
                FontFamily = "Minecraft",
                TextColor = Colors.Black
            };

            // Buttons to control actions
            Button hideButton = new Button { Text = "Peida" };
            hideButton.Clicked += (s, e) => ToggleVisibility(false);

            Button showButton = new Button { Text = "Näita" };
            showButton.Clicked += (s, e) => ToggleVisibility(true);

            Button colorButton = new Button { Text = "Muuda värvi" };
            colorButton.Clicked += (s, e) => ChangeColor();

            Button meltButton = new Button { Text = "Sulata" };
            meltButton.Clicked += async (s, e) => await Melt();

            Button ilmumaButton = new Button { Text = "Ilmuma" };
            ilmumaButton.Clicked += async (s, e) => await Ilamuma();

            Button rotateButton = new Button { Text = "Pööra" };
            rotateButton.Clicked += async (s, e) => await Rotate();

            Button fadeButton = new Button { Text = "Hägu" };
            fadeButton.Clicked += async (s, e) => await Fade();

            Stepper sizeStepper = new Stepper(0.5, 2, 1, 0.1);
            sizeStepper.ValueChanged += (s, e) => Resize(e.NewValue);

            // Creating the control buttons layout
            VerticalStackLayout controlBtns = new VerticalStackLayout
            {
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Children = { hideButton, showButton, colorButton, meltButton, ilmumaButton, rotateButton, fadeButton, sizeStepper }
            };

            // Creating the ScrollView for controls
            ScrollView controlScrollView = new ScrollView
            {
                Content = controlBtns
            };

            // Creating the frames for snowman and actions
            Frame snowmanFrame = new Frame
            {
                Content = snowmanLayout,
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.Black,
                Padding = 10
            };

            Frame actionsFrame = new Frame
            {
                Content = controlScrollView,
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.Black,
                Padding = 10
            };

            // StackLayout to combine both frames (snowman and actions)
            StackLayout mainLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical, // Place snowman and actions side by side
                Spacing = 20,
                Children = { snowmanFrame, actionsFrame }
            };

            // Wrapping the main layout inside a ScrollView to ensure scrolling when needed
            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 20,
                    Children = { actionLabel, mainLayout }
                }
            };

        }

        // Function to make the snowman appear
        private async Task Ilamuma()
        {
            actionLabel.Text = "Tegevus: Lumememm ilmub...";

            if (isMelt)
            {
                double initialY = 200;
                double targetY = 30;
                double initialRotation = 45;
                double targetRotation = 0;

                for (int i = 0; i <= 10; i++)
                {
                    double currentY = initialY + (targetY - initialY) * (i / 10.0);
                    double currentRotation = initialRotation + (targetRotation - initialRotation) * (i / 10.0);

                    AbsoluteLayout.SetLayoutBounds(bucket, new Rect(110, currentY, 60, 40));
                    bucket.Rotation = currentRotation;

                    await Task.Delay(100);
                }

                isMelt = false;
            }

            for (int i = 0; i <= 10; i++)
            {
                double scale = i * 0.1;
                head.Scale = scale;
                body.Scale = scale;
                baseCircle.Scale = scale;
                await Task.Delay(200);
            }

            actionLabel.Text = "Tegevus: Lumememm on ilmunud!";
        }

        // Function to toggle visibility of the snowman
        private void ToggleVisibility(bool isVisible)
        {
            bucket.IsVisible = isVisible;
            head.IsVisible = isVisible;
            body.IsVisible = isVisible;
            baseCircle.IsVisible = isVisible;
            actionLabel.Text = isVisible ? "Tegevus: Näitan lumememme" : "Tegevus: Peidan lumememme";
        }

        // Function to change the color of the snowman
        private void ChangeColor()
        {
            Color randomColor = Color.FromRgb(random.Next(256), random.Next(256), random.Next(256));
            head.BackgroundColor = randomColor;
            body.BackgroundColor = randomColor;
            baseCircle.BackgroundColor = randomColor;
            actionLabel.Text = "Tegevus: Muutsin värvi";
        }

        // Function to melt the snowman
        private async Task Melt()
        {
            actionLabel.Text = "Tegevus: Lumememm sulab...";

            for (int i = 10; i >= 0; i--)
            {
                double scale = i / 10.0;
                head.Scale = scale;
                body.Scale = scale;
                baseCircle.Scale = scale;
                await Task.Delay(200);
            }

            isMelt = true;
            actionLabel.Text = "Tegevus: Lumememm on sulanud!";

            if (isMelt)
            {
                double initialY = 50;
                double targetY = 200;
                double initialRotation = 0;
                double targetRotation = 45;

                for (int i = 0; i <= 10; i++)
                {
                    double currentY = initialY + (targetY - initialY) * (i / 10.0);
                    double currentRotation = initialRotation + (targetRotation - initialRotation) * (i / 10.0);

                    AbsoluteLayout.SetLayoutBounds(bucket, new Rect(110, currentY, 60, 40));
                    bucket.Rotation = currentRotation;

                    await Task.Delay(100);
                }
            }
        }

        // Function to resize the snowman
        private void Resize(double scale)
        {
            head.Scale = scale;
            body.Scale = scale;
            baseCircle.Scale = scale;
            bucket.Scale = scale;
            actionLabel.Text = $"Tegevus: Suuruse muutmine {scale:0.0}x";
        }

        // Function to rotate the snowman
        private async Task Rotate()
        {
            actionLabel.Text = "Tegevus: Lumememm pöörleb...";
            for (int i = 0; i < 360; i++)
            {
                bucket.Rotation = i;
                head.Rotation = i;
                body.Rotation = i;
                baseCircle.Rotation = i;
                await Task.Delay(10);
            }
            actionLabel.Text = "Tegevus: Lumememm on pööratud!";
        }

        // Function to fade the snowman in and out
        private async Task Fade()
        {
            actionLabel.Text = "Tegevus: Lumememm hägustub...";
            for (int i = 0; i <= 10; i++)
            {
                double opacity = i / 10.0;
                head.Opacity = opacity;
                body.Opacity = opacity;
                baseCircle.Opacity = opacity;
                bucket.Opacity = opacity;
                await Task.Delay(100);
            }

            for (int i = 10; i >= 0; i--)
            {
                double opacity = i / 10.0;
                head.Opacity = opacity;
                body.Opacity = opacity;
                baseCircle.Opacity = opacity;
                bucket.Opacity = opacity;
                await Task.Delay(100);
            }

            actionLabel.Text = "Tegevus: Lumememm on nähtav!";
        }
    }
}
