namespace MobiileApp.Pages;

public partial class RiigiDetailPage : ContentPage
{
    private EuroopaRiik riik;

    public RiigiDetailPage(EuroopaRiik valitud)
    {
        InitializeComponent();
        riik = valitud;

        nimiEntry.Text = riik.Nimi;
        pealinnEntry.Text = riik.Pealinn;
        rahvaarvEntry.Text = riik.Rahvaarv.ToString();
        lippEntry.Text = riik.Lipp;
        lippImage.Source = riik.Lipp;
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nimiEntry.Text) ||
            string.IsNullOrWhiteSpace(pealinnEntry.Text) ||
            string.IsNullOrWhiteSpace(lippEntry.Text) ||
            !int.TryParse(rahvaarvEntry.Text, out int rahvaarv))
        {
            await DisplayAlert("Viga", "Palun täida kõik väljad korrektselt.", "OK");
            return;
        }

        riik.Nimi = nimiEntry.Text;
        riik.Pealinn = pealinnEntry.Text;
        riik.Rahvaarv = rahvaarv;
        riik.Lipp = lippEntry.Text;
        lippImage.Source = riik.Lipp;

        await DisplayAlert("OK", "Riik uuendatud!", "OK");
    }
}
