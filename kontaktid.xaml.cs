using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace MobiileApp;

public partial class kontaktid : ContentPage
{
    TableView tabelview;
    EntryCell nameEntry, phoneEntry, emailEntry;
    TableSection peopleSection;
    List<(string Name, string Phone, string Email, ImageSource Photo)> peopleList;
    Button addPersonBtn, showAllBtn, deletePersonBtn;
    Button sendEmailBtn, callBtn, addPhotoBtn;
    Image userPhoto;

    private StackLayout mainLayout;
    public kontaktid(int k)
    {
        peopleList = new List<(string, string, string, ImageSource)>();

        nameEntry = new EntryCell { Label = "Nimi:", Placeholder = "Sisesta inimese nimi", Keyboard = Keyboard.Default };
        phoneEntry = new EntryCell { Label = "Telefon:", Placeholder = "Sisesta telefon", Keyboard = Keyboard.Telephone };
        emailEntry = new EntryCell { Label = "Email:", Placeholder = "Sisesta email", Keyboard = Keyboard.Email };
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

        peopleSection = new TableSection("Inimesed");


        tabelview = new TableView
        {
            Root = new TableRoot
            {
                new TableSection("Lisa uus inimene") { nameEntry, phoneEntry, emailEntry }
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
            peopleList.Add((nameEntry.Text, phoneEntry.Text, emailEntry.Text, userPhoto.Source));
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
                            new Label { Text = $"Email: {person.Email}"}
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
                    var index = peopleList.IndexOf(person);
                    if (index >= 0)
                    {
                        peopleList.RemoveAt(index);
                        peopleSection.RemoveAt(index);
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
}