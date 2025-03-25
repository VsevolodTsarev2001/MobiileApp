using System.Collections.ObjectModel;

namespace MobiileApp.Pages;

public partial class EuroopaRiigid : ContentPage
{
    private ObservableCollection<EuroopaRiik> riigid;

    public EuroopaRiigid(int k)
    {
        InitializeComponent();
        riigid = new ObservableCollection<EuroopaRiik>
        {
            new EuroopaRiik { Nimi = "Eesti", Pealinn = "Tallinn", Kirjaldus = "Text", Rahvaarv = 1300000, Lipp = "https://flagcdn.com/w320/ee.png" },
            new EuroopaRiik { Nimi = "Soome", Pealinn = "Helsinki", Kirjaldus = "test", Rahvaarv = 5500000, Lipp = "https://flagcdn.com/w320/fi.png" }
        };
        RiigidList.ItemsSource = riigid;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LisaRiikPage(riigid));
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is EuroopaRiik valitud)
        {
            await Navigation.PushAsync(new RiigiDetailPage(valitud));
            RiigidList.SelectedItem = null;
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.CommandParameter is EuroopaRiik riik)
        {
            riigid.Remove(riik);
        }
    }
}
