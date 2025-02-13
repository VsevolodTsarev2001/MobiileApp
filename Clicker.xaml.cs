namespace MobiileApp;

public partial class Clicker : ContentPage
{

    private int score = 0;
    private Label scoreLabel;
    private Label upgradeLabel;
    private int upgradeCost = 20;
    private bool upgradeAvailable = false;
    private int lvl = 0;
    private int upgradeLvl;

    public Clicker(int k)
    {
        Title = "Clicker";

        // Initialize UI elements
        scoreLabel = CreateLabel("Score: 0", 24);
        upgradeLabel = CreateLabel($"Upgrade: {upgradeCost} score", 18, false);

        Clickerbtn = CreateButton("clicker_icon.png", 350, 350, () =>
        {
            score++;
            UpdateScore();
            HandleUpgradeVisibility();
        });

        Upgradebtn = new Button
        {
            Text = $"Upgrade. LVL: {lvl}",
            WidthRequest = 200,
            HeightRequest = 50,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            IsVisible = false
        };

        Upgradebtn.Clicked += (sender, e) =>
        {
            if (score >= upgradeCost)
            {
                score -= upgradeCost;
                lvl++;
                upgradeCost = (int)(upgradeCost * 2.5);
                UpdateScore();
                upgradeLabel.Text = $"Upgrade: {upgradeCost} score";
                Clickerbtn.Clicked -= DefaultClick;
                Clickerbtn.Clicked += DoubleClick;
                Upgradebtn.Text = $"Upgrade. LVL: {lvl}";
            }
        };

        Content = new Grid
        {
            BackgroundColor = Colors.Black,
            Children = {
                new VerticalStackLayout
                {
                    Children = { scoreLabel, upgradeLabel },
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start
                },
                Clickerbtn,
                Upgradebtn
            }
        };
    }

    private Label CreateLabel(string text, int fontSize, bool isVisible = true)
    {
        return new Label
        {
            Text = text,
            FontSize = fontSize,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start,
            TextColor = Colors.White,
            IsVisible = isVisible
        };
    }

    private Button CreateButton(string icon, int width, int height, Action onClickAction)
    {
        var button = new Button
        {
            ImageSource = icon,
            BackgroundColor = Color.FromArgb("#00000000"),
            BorderColor = Color.FromArgb("#00000000"),
            WidthRequest = width,
            HeightRequest = height,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };

        button.Clicked += (sender, e) => onClickAction();
        return button;
    }

    private void DefaultClick(object sender, EventArgs e)
    {
        score++;
        UpdateScore();
    }

    private void DoubleClick(object sender, EventArgs e)
    {
        score += 2;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreLabel.Text = $"Score: {score}";
        UpdateButtonIcon();
    }

    private void HandleUpgradeVisibility()
    {
        if (score >= upgradeCost && !upgradeAvailable)
        {
            upgradeLabel.IsVisible = true;
            Upgradebtn.IsVisible = true;
            upgradeAvailable = true;
        }
    }

    private void UpdateButtonIcon()
    {
        string icon = score switch
        {
            >= 1000 => "clicker_icon5.png",
            >= 500 => "clicker_icon4.png",
            >= 100 => "clicker_3.png",
            >= 10 => "clicker_2.png",
            _ => "clicker.png"
        };

        Clickerbtn.ImageSource = icon;
    }
}
