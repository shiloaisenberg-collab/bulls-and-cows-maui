namespace BullsAndCowsApp.Models
{
    class BullsAndCowsGame
    {
        private const int CodeLength = 4;
        private Random? _random = null;
        private string _secretNumber = string.Empty;
        public int AttemptCount {  get; private set; }
        public bool IsGameOver {  get; private set; }
        public int RandomNumber { get; private set; }
        

        public void GenerateRandomNumber()
        {
            Random rnd = new Random();
            RandomNumber = rnd.Next(1000, 10000);
        }

        public BullsAndCowsGame()
        {
            StartNewGame();
        }
        public void StartNewGame()
        {
            AttemptCount = 0;
            IsGameOver = false;
            _random = new Random((int)DateTime.Now.Ticks);
            _secretNumber = GenerateSecretNumber();
        }
        private string GenerateSecretNumber()
        {
            string res = string.Empty;
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            while(res.Length < CodeLength)
            {
                int pos = _random.Next(0, 10);
                if (digits[pos] == '-') { continue; } 
                if (res.Length == 0 && digits[pos] == '0') { continue; }
                res += digits[pos].ToString();
                digits[pos] = '-';
            }
            return res;
        }
        public static bool IsValidGuess(string guess, out string errorMessage)
        {
            if(guess.Length != CodeLength) 
            {
                errorMessage = "Please enter exactly 4 digits.";
                return false;
            }
            int res = 0;
            if(!int.TryParse(guess, out res))
            {
                errorMessage = "Please enter exactly 4 digits.";
                return false;
            }
            if(res < 1000) 
            {
                errorMessage = "Please enter a 4 digit number that does not start with 0.";
                return false;
            }
            if(guess.Distinct().ToList().Count != CodeLength)
            {
                errorMessage = "Please enter a 4 digit number that does not repeat the same digit.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        public GuessResult SubmitGuess(string guess)
        {
            if (IsGameOver) { throw new Exception("cannot submit a guess after the game is already over"); }
            GuessResult res = new GuessResult();
            for(int i = 0; i < CodeLength; i++)
            {
                if (guess[i] == _secretNumber[i])
                {
                    res.bulls++;
                }
            }
            res.cows =guess.Intersect(_secretNumber).ToList().Count - res.bulls;
            AttemptCount++;
            if (res.IsWinningGusss) { IsGameOver = true; }
            return res;
        }
        public string Debug()
        {
            return $"AttemptCount = {AttemptCount}, IsGameOver = {IsGameOver} , _secretNumber = {_secretNumber}";
        }
    }
}
