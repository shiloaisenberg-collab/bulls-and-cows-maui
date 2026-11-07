using System.ComponentModel;
using System.Security.Cryptography;
using System.Windows.Input;
using BullsAndCowsApp.Models;   // your BullsAndCowsGame and GuessResult classes

namespace BullsAndCowsApp.ViewModels;

// The ViewModel: holds everything the page shows and does what the buttons ask.
// It implements INotifyPropertyChanged so the page updates when a property changes.
public class GameViewModel : INotifyPropertyChanged
{
    // The game from your Model. It knows all the rules.
    private BullsAndCowsGame _game = new BullsAndCowsGame();

    // ---------- INotifyPropertyChanged (written by hand) ----------

    public event PropertyChangedEventHandler? PropertyChanged;

    // Tells the page "this property changed, please refresh it"
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // ---------- Properties used by MainPage.xaml ----------

    // The text the player types in the guess box (the page can change it too)
    private string _guessText = string.Empty;
    private string _randomNumberText = string.Empty;
    public string GuessText
    {
        get { return _guessText; }
        set
        {
            if (_guessText == value) { return; }
            _guessText = value;
            OnPropertyChanged(nameof(GuessText));
        }
    }

    public string RandomNumber
    {
        get { return _randomNumberText; }
        set
        {
            if (_randomNumberText == value) { return; }
            _randomNumberText = value;
            OnPropertyChanged(nameof(RandomNumber));
        }
    }

    // How many guesses were made (shown in the orange box)
    private int _attemptCount = 0;
    public int AttemptCount
    {
        get { return _attemptCount; }
        private set
        {
            if (_attemptCount == value) { return; }
            _attemptCount = value;
            OnPropertyChanged(nameof(AttemptCount));
        }
    }

    // The result of the last guess, for example "1 bulls - 2 cows"
    private string _guessStatusText = string.Empty;
    public string GuessStatusText
    {
        get { return _guessStatusText; }
        private set
        {
            if (_guessStatusText == value) { return; }
            _guessStatusText = value;
            OnPropertyChanged(nameof(GuessStatusText));
        }
    }

    // True when the status box should be visible
    private bool _hasGuessStatus = false;
    public bool HasGuessStatus
    {
        get { return _hasGuessStatus; }
        private set
        {
            if (_hasGuessStatus == value) { return; }
            _hasGuessStatus = value;
            OnPropertyChanged(nameof(HasGuessStatus));
        }
    }

    // The text for the error box, for example "Please enter exactly 4 digits."
    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get { return _errorMessage; }
        private set
        {
            if (_errorMessage == value) { return; }
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    // True when the error box should be visible
    private bool _hasError = false;
    public bool HasError
    {
        get { return _hasError; }
        private set
        {
            if (_hasError == value) { return; }
            _hasError = value;
            OnPropertyChanged(nameof(HasError));
        }
    }

    // True while the player can still guess (false after a win)
    private bool _canGuess = true;
    public bool CanGuess
    {
        get { return _canGuess; }
        private set
        {
            if (_canGuess == value) { return; }
            _canGuess = value;
            OnPropertyChanged(nameof(CanGuess));
        }
    }

    // ---------- Commands used by MainPage.xaml ----------

    // Each command is a small class (at the bottom of this file) that calls one method below
    public ICommand SubmitCommand { get; }
    public ICommand NewGameCommand { get; }
    public ICommand RandomNumberCommand { get; }

    // ---------- Constructor ----------

    public GameViewModel()
    {
        // Give each command a reference to this ViewModel
        SubmitCommand = new SubmitGuessCommand(this);
        NewGameCommand = new StartNewGameCommand(this);
        RandomNumberCommand = new RandomNumberCommand(this);
    }

    // ---------- Methods the commands call ----------

    // Runs when the player presses Submit (or Enter)
    public void SubmitGuess()
    {
        // Same rule as the console app: no guesses after a win
        if (_game.IsGameOver) { return; }

        string guess = GuessText ?? string.Empty;

        // Same checks and messages as the console app (they live in the Model)
        string error;
        if (!BullsAndCowsGame.IsValidGuess(guess, out error))
        {
            ErrorMessage = error;
            HasError = true;
            return;
        }

        // The guess is valid, so hide any old error
        ErrorMessage = string.Empty;
        HasError = false;

        // Let the Model count the bulls and cows
        GuessResult result = _game.SubmitGuess(guess);
        AttemptCount = _game.AttemptCount;

        // Show the result, with a win message if the player got 4 bulls
        if (result.IsWinningGusss)
        {
            GuessStatusText = result.feedback + " - You won!";
        }
        else
        {
            GuessStatusText = result.feedback;
        }
        HasGuessStatus = true;

        // Clear the guess box and stop guesses if the game is over
        GuessText = string.Empty;
        CanGuess = !_game.IsGameOver;
    }

    // Runs when the player presses New game
    public void StartNewGame()
    {
        _game.StartNewGame();

        // Reset everything the page shows
        GuessText = string.Empty;
        AttemptCount = 0;
        GuessStatusText = string.Empty;
        HasGuessStatus = false;
        ErrorMessage = string.Empty;
        HasError = false;
        CanGuess = true;
    }

    public void GenerateRandomNumber()
    {
        _game.GenerateRandomNumber();
        RandomNumber = _game.RandomNumber.ToString();
    }
}

// Command for the Submit button: calls SubmitGuess() on the ViewModel
public class SubmitGuessCommand : ICommand
{
    private GameViewModel _viewModel;

    public SubmitGuessCommand(GameViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    // This command is always allowed. The page disables the button with CanGuess instead.
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _viewModel.SubmitGuess();
    }

    // Required by ICommand. We never change CanExecute, so it does nothing.
    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }
}

// Command for the New game button: calls StartNewGame() on the ViewModel
public class StartNewGameCommand : ICommand
{
    private GameViewModel _viewModel;

    public StartNewGameCommand(GameViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _viewModel.StartNewGame();
    }

    // Required by ICommand. We never change CanExecute, so it does nothing.
    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }
}
public class RandomNumberCommand : ICommand
{
    private GameViewModel _viewModel;

    public RandomNumberCommand(GameViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _viewModel.GenerateRandomNumber();
    }

    // Required by ICommand. We never change CanExecute, so it does nothing.
    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }
}
