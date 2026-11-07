namespace BullsAndCowsApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        // Decides which page opens first: your MainPage (no Shell)
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new Views.MainPage()) { Title = "Bulls and Cows" };
        }
    }
}