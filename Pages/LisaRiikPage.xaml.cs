using System.Collections.ObjectModel;

namespace MobiileApp.Pages;

public partial class LisaRiikPage : ContentPage
{
    private ObservableCollection<EuroopaRiik> riigid;

    public LisaRiikPage(ObservableCollection<EuroopaRiik> olemasolevad)
    {
        InitializeComponent();
        riigid = olemasolevad;
    }

    private async void OnLisaClicked(object sender, EventArgs e)
    {
        string nimi = nimiEntry.Text?.Trim();
        string pealinn = pealinnEntry.Text?.Trim();
        string lipp = lippEntry.Text?.Trim();
        bool success = int.TryParse(rahvaarvEntry.Text, out int rahvaarv);

        if (string.IsNullOrEmpty(nimi) || string.IsNullOrEmpty(pealinn) || !success || string.IsNullOrEmpty(lipp))
        {
            await DisplayAlert("Viga", "Täida kõik väljad korrektselt.", "OK");
            return;
        }

        if (riigid.Any(r => r.Nimi.Equals(nimi, StringComparison.OrdinalIgnoreCase)))
        {
            await DisplayAlert("Teade", "Riik on juba olemas!", "OK");
            return;
        }

        riigid.Add(new EuroopaRiik { Nimi = nimi, Pealinn = pealinn, Rahvaarv = rahvaarv, Lipp = lipp });
        await Navigation.PopAsync();
    }
}
