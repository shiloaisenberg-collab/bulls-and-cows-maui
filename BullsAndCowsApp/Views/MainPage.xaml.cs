using BullsAndCowsApp.ViewModels;

namespace BullsAndCowsApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Create the ViewModel directly (no dependency injection)
        BindingContext = new GameViewModel();
    }
}
