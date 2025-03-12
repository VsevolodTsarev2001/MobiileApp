using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System.Linq;

namespace MobiileApp;

public partial class kontaktid : ContentPage
{
    TableView tabelview;
    EntryCell nameEntry, phoneEntry, emailEntry, descriptionEntry;
    TableSection peopleSection;
    List<(string Name, string Phone, string Email, ImageSource Photo, string Description)> peopleList;
    Button addPersonBtn, showAllBtn, deletePersonBtn;
    Button sendEmailBtn, callBtn, addPhotoBtn, sendSmsBtn, sendGreetingsBtn;
    Image userPhoto;

    private StackLayout mainLayout;
    private Random random = new Random();

    public kontaktid(int k)
    {
        peopleList = new List<(string, string, string, ImageSource, string)>();

        nameEntry = new EntryCell { Label = "Nimi:", Placeholder = "Sisesta inimese nimi", Keyboard = Keyboard.Default };
        phoneEntry = new EntryCell { Label = "Telefon:", Placeholder = "Sisesta telefon", Keyboard = Keyboard.Telephone };
        emailEntry = new EntryCell { Label = "Email:", Placeholder = "Sisesta email", Keyboard = Keyboard.Email };
        descriptionEntry = new EntryCell { Label = "Kirjeldus:", Placeholder = "Sisesta kirjeldus" };
        userPhoto = new Image { HeightRequest = 100, WidthRequest = 100 };

        addPhotoBtn = new Button { Text = "Lisa foto" };
        addPhotoBtn.Clicked += AddPhotoBtn_Clicked;

        addPersonBtn = new Button { Text = "Lisa inimene" };
        addPersonBtn.Clicked += AddPersonBtn_Clicked;

        showAllBtn = new Button { Text = "Näita kõiki kasutajaid" };
        showAllBtn.Clicked += ShowAllBtn_Clicked;

        deletePersonBtn = new Button { Text = "Kustuta inimene" };
        deletePersonBtn.Clicked += DeletePersonBtn_Clicked;

        sendEmailBtn = new Button { Text = "Saada email" };
        sendEmailBtn.Clicked += SendEmailBtn_Clicked;

        callBtn = new Button { Text = "Helista" };
        callBtn.Clicked += CallBtn_Clicked;

        sendSmsBtn = new Button { Text = "Saada SMS" };
        sendSmsBtn.Clicked += SendSmsBtn_Clicked;

        sendGreetingsBtn = new Button { Text = "Saada tervitus" };
        sendGreetingsBtn.Clicked += SendGreetingsBtn_Clicked;

        peopleSection = new TableSection("Inimesed");

        tabelview = new TableView
        {
            Root = new TableRoot
            {
                new TableSection("Lisa uus inimene") { nameEntry, phoneEntry, emailEntry, descriptionEntry }
            },
            HeightRequest = 250
        };

        mainLayout = new StackLayout
        {
            Children =
            {
                userPhoto,
                addPhotoBtn,
                addPersonBtn,
                showAllBtn,
                deletePersonBtn,
                sendEmailBtn,
                callBtn,
                sendSmsBtn,
                sendGreetingsBtn,
                tabelview
            }
        };

        Content = new ScrollView { Content = mainLayout };
    }

    private async void AddPhotoBtn_Clicked(object sender, EventArgs e)
    {
        var photo = await MediaPicker.PickPhotoAsync();
        if (photo != null)
        {
            userPhoto.Source = ImageSource.FromFile(photo.FullPath);
        }
    }

    private void AddPersonBtn_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(nameEntry.Text))
        {
            peopleList.Add((nameEntry.Text, phoneEntry.Text, emailEntry.Text, userPhoto.Source, descriptionEntry.Text));
        }
    }

    private bool isShowingAll = false;
    private void ShowAllBtn_Clicked(object sender, EventArgs e)
    {
        if (isShowingAll)
        {
            peopleSection.Clear();
            showAllBtn.Text = "Näita kõiki kasutajaid andmed";

            (Content as ScrollView).Content = mainLayout;
        }
        else
        {
            var peopleStack = new StackLayout { Spacing = 10 };

            foreach (var person in peopleList)
            {
                var stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(10),
                    Spacing = 10,
                    Children =
                    {
                        new Image { Source = person.Photo, HeightRequest = 50, WidthRequest = 50 },
                        new StackLayout
                        {
                            Children =
                            {
                                new Label { Text = person.Name, FontAttributes = FontAttributes.Bold, FontSize = 16 },
                                new Label { Text = $"Tel: {person.Phone}" },
                                new Label { Text = $"Email: {person.Email}" },
                                new Label { Text = $"Kirjeldus: {person.Description}" }
                            }
                        }
                    }
                };

                var deleteButton = new Button
                {
                    Text = "❌",
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };

                deleteButton.Clicked += (s, args) =>
                {
                    // Получаем индекс текущего человека из списка
                    var index = peopleList.IndexOf(person);
                    if (index >= 0)
                    {
                        // Удаляем элемент из peopleList
                        peopleList.RemoveAt(index);

                        // Удаляем элемент из peopleSection
                        peopleSection.RemoveAt(index);

                        // Обновление интерфейса (это важно, чтобы изменения были видны)
                        tabelview.Root = new TableRoot
                        {
                            new TableSection("Lisa uus inimene") { nameEntry, phoneEntry, emailEntry },
                            peopleSection
                        };
                    }
                };

                var frame = new Frame
                {
                    Content = new StackLayout { Children = { stackLayout, deleteButton } },
                    Padding = new Thickness(10),
                    Margin = new Thickness(10, 5),
                    BorderColor = Colors.Gray,
                    CornerRadius = 10,
                    HasShadow = true,
                    HeightRequest = 150
                };

                peopleStack.Children.Add(frame);
            }

            mainLayout.Children.Add(peopleStack);
            showAllBtn.Text = "Peida kõik kasutajad andmed";
        }
        isShowingAll = !isShowingAll;
    }

    private void DeletePersonBtn_Clicked(object sender, EventArgs e)
    {
        if (peopleList.Count > 0)
        {
            peopleList.RemoveAt(peopleList.Count - 1);
            peopleSection.RemoveAt(peopleSection.Count - 1);
        }
    }

    private void SendEmailBtn_Clicked(object sender, EventArgs e)
    {
        if (peopleList.Count > 0)
        {
            var email = peopleList[^1].Email;
            Launcher.OpenAsync($"mailto:{email}");
        }
    }

    private void CallBtn_Clicked(object sender, EventArgs e)
    {
        if (peopleList.Count > 0)
        {
            var phone = peopleList[^1].Phone;
            PhoneDialer.Open(phone);
        }
    }

    private async void SendSmsBtn_Clicked(object sender, EventArgs e)
    {
        if (peopleList.Count > 0)
        {
            var phone = peopleList[^1].Phone;
            var message = "Тестовое сообщение!";

            // Создаём объект SmsMessage и передаем номер телефона и сообщение
            var smsMessage = new SmsMessage(message, phone);

            // Отправляем SMS
            await Sms.ComposeAsync(smsMessage);
        }
    }


    private async void SendGreetingsBtn_Clicked(object sender, EventArgs e)
    {
        string[] greetings = new string[]
        {
        "Häid jõule ja head uut aastat!",
        "Palju õnne sünnipäevaks!",
        "Soovin sulle edu ja õnne!",
        "Olgu sul alati päikesepaiste südames!",
        "Kõike head uuel aastal!"
        };

        var randomGreeting = greetings[random.Next(greetings.Length)];

        if (peopleList.Count > 0)
        {
            var phone = peopleList[^1].Phone;
            var email = peopleList[^1].Email;

            // Сначала отображаем диалог с выбором
            var choice = await DisplayActionSheet("Vali saadetav tervitus", "Cancel", null, "E-mail", "SMS");

            if (choice == "E-mail")
            {
                Launcher.OpenAsync($"mailto:{email}?subject=Tervitus&body={randomGreeting}");
            }
            else if (choice == "SMS")
            {
                // Создаём объект SmsMessage с текстом и номером телефона
                var smsMessage = new SmsMessage(randomGreeting, phone);

                // Отправляем SMS
                await Sms.ComposeAsync(smsMessage);
            }
        }
    }
}
