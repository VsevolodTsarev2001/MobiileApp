namespace MobiileApp;

public partial class StartPage : ContentPage
{
	public List<ContentPage> lehed = new List<ContentPage>() { new TextPage(0), new FigurePage(1), new Clicker(2), new Valgusfoor(3)};
	public List<string> tekstid = new List<string> { "Tee lahti TekstPage", "Tee lahti FigurePage", "Clicker", "Valgusfoor" };
	ScrollView sv;
	VerticalStackLayout vsl;
	public StartPage()
	{
		Title = "Avaleht";
		vsl = new VerticalStackLayout { BackgroundColor = Color.FromRgb (150,100,20) };
		for (int i = 0; i < tekstid.Count; i++)
		{
			Button nupp = new Button
			{
				Text = tekstid[i],
				BackgroundColor = Color.FromRgb (i, 100, 20),
				BorderColor = Color.FromRgb (i, 100, 10),
				TextColor = Color.FromRgb (120, 250, 250),
				BorderWidth = 10,
				ZIndex = i,
				FontFamily = "Lower Pixel Regular 400"
			};
			vsl.Add(nupp);
            nupp.Clicked += Lehte_avamine;
		}
		sv = new ScrollView { Content = vsl };
		Content = sv;
	}

    private async void Lehte_avamine(object? sender, EventArgs e)
    {
		Button btn = (Button)sender;
		await Navigation.PushAsync(lehed[btn.ZIndex]);
    }
}