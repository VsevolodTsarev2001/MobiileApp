namespace MobiileApp;

public partial class Clicker : ContentPage
{
    int score = 0;
    int upgradeCost = 20;
    bool upgradeAvailable = false;
    int lvl = 0;

    public Clicker(int k)
    {
        InitializeComponent();

        // Set initial values for the labels
        scoreLabel.Text = $"Score: {score}";
        upgradeLabel.Text = $"Upgrade: {upgradeCost} score";
    }

    // Handle main click button
    private void OnClickerbtnClicked(object sender, EventArgs e)
    {
        score++;
        scoreLabel.Text = $"Score: {score}";
        UpdateButtonIcon();

        if (score >= upgradeCost && !upgradeAvailable)
        {
            upgradeLabel.IsVisible = true;
            Upgradebtn.IsVisible = true;
            upgradeAvailable = true;
        }
    }

    // Handle upgrade button click
    private void OnUpgradebtnClicked(object sender, EventArgs e)
    {
        if (score >= upgradeCost)
        {
            score -= upgradeCost;
            lvl++;
            Upgradebtn.Text = $"Upgrade. LVL: {lvl}";
            scoreLabel.Text = $"Score: {score}";
            upgradeCost = (int)(upgradeCost * 2.5);
            upgradeLabel.Text = $"Upgrade: {upgradeCost} score";
            Clickerbtn.Clicked -= DefaultClick;
            Clickerbtn.Clicked += DoubleClick;
        }
    }

    // Update the button icon based on the score
    private void UpdateButtonIcon()
    {
        if (score >= 1000)
        {
            Clickerbtn.ImageSource = "clicker_icon5.png";
        }
        else if (score >= 500)
        {
            Clickerbtn.ImageSource = "clicker_icon4.png";
        }
        else if (score >= 100)
        {
            Clickerbtn.ImageSource = "clicker_icon3.png";
        }
        else if (score >= 10)
        {
            Clickerbtn.ImageSource = "clicker_icon2.png";
        }
        else
        {
            Clickerbtn.ImageSource = "clicker_icon.png";
        }
    }

    // Default click behavior
    private void DefaultClick(object sender, EventArgs e)
    {
        score++;
        scoreLabel.Text = $"Score: {score}";
        UpdateButtonIcon();
    }

    // Double-click behavior after upgrade
    private void DoubleClick(object sender, EventArgs e)
    {
        score += 2;
        scoreLabel.Text = $"Score: {score}";
        UpdateButtonIcon();
    }
}
