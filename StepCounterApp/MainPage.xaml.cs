namespace StepCounterApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
        MessagingCenter.Subscribe<object, int>(this, "StepUpdated", (sender, stepCount) =>
        {
            StepCountLabel.Text = $"Steps: {stepCount}";
        });
    }
}