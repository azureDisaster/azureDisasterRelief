using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WOFClassLib
{
    /// <summary>
    /// This class uses the Player and Puzzle classes to perform the functions of running a game.
    /// </summary>
    public class Game
    {
        private bool resetGame;
        private List<Player> players;
        private Player currentPlayer;
        private Phrase phrase;
        private Puzzle puzzle;
        private Wheel wheel;
        private int totalPlayers;
        private HashSet<char> guessedLetters;
        private HashSet<char> guessedVowels;

        public Game()
        {
            resetGame = false;
            players = new List<Player>();
            wheel = new Wheel();
            phrase = new Phrase();
            Console.WriteLine("Welcome to Wheel of Fortune sponsored by Azure Disaster Relief LLC.");
            InitializePlayers();
            ContinueOnKey();
        }

        /// <summary>
        /// Initializes the number of players in the game
        /// </summary>
        private void InitializePlayers()
        {
            // initialize players
            bool valid = false;
            do
            {
                Console.WriteLine("How many players would you like to begin with? ");
                string input = Console.ReadLine();
                valid = int.TryParse(input, out totalPlayers);
            } while (!valid || totalPlayers <= 0 || totalPlayers >= 4);

            for (int i = 0; i < totalPlayers; i++)
            {
                Console.WriteLine("Hey player {0} What's your name? ", i + 1);
                players.Add(new Player(Console.ReadLine())); // adds a player obj to list
            }
            Console.WriteLine("Alright, starting with {0} player(s)!", totalPlayers);
        }

        /// <summary>
        /// This method will prompt the user for details regarding the initialization of the game. 
        /// </summary>
        public void StartGame()
        {
            // play number of rounds
            const int ROUNDS = 3;
            for (int i = 0; i < ROUNDS; i++)
            {
                Console.WriteLine("ROUND {0}", i + 1);
                StartRound();
            }
            Quit();
        }

        public bool ResetStatus()
        {
            resetGame = true;
            return resetGame;
        }

        /// <summary>
        /// Starts the game round
        /// </summary>
        private void StartRound()
        {
            puzzle = new Puzzle(phrase.GetPhrase());
            System.Console.WriteLine("The category is: {0}", phrase.Category);
            guessedLetters = new HashSet<char>(26);
            guessedVowels = new HashSet<char>(5);
            int index = 0;
            currentPlayer = players[index];
            while (!puzzle.IsSolved()) // if the game is being played, loop thru the players
            {
                Play(currentPlayer); // call play on the current player object
                FinalizeTurn(currentPlayer);
                SwitchPlayer(ref index);
                ContinueOnKey();
            }
        }

        /// <summary>
        /// Checks if puzzle is solved.
        /// If the game is finished, winning solving player gets the RoundMoney
        /// All other players are then bankrupt to start a new round.
        /// </summary>
        /// <param name="solvingPlayer"></param>
        private void FinalizeTurn(Player solvingPlayer)
        {
            if (puzzle.IsSolved())
            {
                solvingPlayer.WinRound();
                foreach (Player p in players)
                {
                    p.NewRound();
                }
            }
        }

        /// <summary>
        /// Utility to switch to the next player
        /// </summary>
        /// <param name="index">The index to increment</param>
        private void SwitchPlayer(ref int index)
        {
            index = (index + 1) % totalPlayers;
            currentPlayer = players[index];
        }

        /// <summary>
        /// This method allows the player to play and perform an action
        /// </summary>
        /// <param name="player">A player object instantiated by the Player class.</param>
        private void Play(Player player)
        {
            Console.WriteLine("Hey {0}! Now it's your turn!", player.Name);
            PrintPlayerRoundMoney(player);
            PrintPuzzle();
            bool spinSuccess = FirstSpin(player); // bool is spin was successful or not

            // actionloop repeatedly loops and uses spinSuccess to determine
            // if user gets to guess again or solve
            ActionLoop:
            PrintPlayerRoundMoney(player);
            if (spinSuccess && !puzzle.IsSolved())
            {
                PrintPuzzle();            
                spinSuccess = NextAction(player);
                goto ActionLoop;
            }
            else if (spinSuccess && puzzle.IsSolved())
            {
                Console.WriteLine("YAYYYY! You solved it!");
            }
            else
            {
                Console.WriteLine("Your guess was wrong... Let's move on to the next player.");
            }
        }

        /// <summary>
        /// Perform the a spin action.
        /// Return true if correct, false if bankrupt/skipturn/incorrect
        /// </summary>
        /// <param name="player"></param>
        /// <returns>bool if action was a success</returns>
        private bool FirstSpin(Player player)
        {
            bool guessedCorrectly = false;
            int spinValue = wheel.WheelSpin();
            if (wheel.isBankrupt)
            {
                BankruptPlayer(player);
            }
            else if (wheel.skipTurn)
            {
                SkipTurn();
            }
            else
            {
                System.Console.WriteLine("You spun ${0}!", spinValue);
                char userLetter = AskForLetter(player);
                int matches = player.GuessLetter(userLetter, puzzle, spinValue);
                if (matches > 0)
                {
                    guessedCorrectly = true;
                }
            }
            // returns true if correct guess, false if bankrupt/skip
            return guessedCorrectly;
        }

        /// <summary>
        /// Asks for user's next action.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>bool if action was a success</returns>
        private bool NextAction(Player player)
        {
            Console.WriteLine("Since you guessed correctly, you can make another spin or solve!");
            int userChoice;
            bool actionValid = false;
            do
            {
                Console.WriteLine("1 to Spin or 2 to Solve");
                string input = Console.ReadLine();
                actionValid = int.TryParse(input, out userChoice);
            } while (!actionValid || (userChoice != 1 && userChoice != 2));

            bool actionSuccess = false;
            if (userChoice == 1)
            {
                // true if guessed correct, false if incorrect/skip/bankrupt
                actionSuccess = FirstSpin(player);
            }
            else if (userChoice == 2)
            {
                // true if guessed correct, false if not
                string guessString = AskForStringSolve();
                actionSuccess = player.SolvePuzzle(guessString, puzzle);
            }
            return actionSuccess;
        }

        /// <summary>
        /// Asks for user's letter guess
        /// </summary>
        /// <returns>char letter</returns>
        private char AskForLetter(Player player)
        {
            char letter = '\0';
            bool valid = false;
            do
            {
                System.Console.Write("Guess a single letter: ");
                valid = char.TryParse(Console.ReadLine(), out letter);
                if (guessedLetters.Contains(letter))
                {
                    System.Console.WriteLine("'{0}' was already guessed!", letter);
                }
                else if (!guessedLetters.Contains(letter) && IsVowel(letter))
                {
                    if (!player.CanBuyVowel())
                    {
                        System.Console.WriteLine("You can't buy '{0}'. You're broke AF!!!", letter);
                        valid = false;
                    }
                    else
                    {
                        System.Console.WriteLine("You bought '{0}'", letter);
                        guessedVowels.Add(letter);
                        player.PurchaseVowel();
                    }
                }
            } while (!valid || !Char.IsLetter(letter) || guessedLetters.Contains(letter));

            // letter was unguessed
            guessedLetters.Add(letter);
            return letter;
        }

        /// <summary>
        /// Asks for the user's guess to solve
        /// </summary>
        /// <returns>a string for user to solve</returns>
        private string AskForStringSolve()
        {
            System.Console.Write("What is your guess? ");
            string guess = Console.ReadLine();
            return guess;
        }

        private void PrintPuzzle()
        {
            System.Console.WriteLine(puzzle.GetPuzzleDisplay());
        }

        /// <summary>
        /// Prints player's current RoundMoney
        /// </summary>
        /// <param name="player">the current player</param>
        private void PrintPlayerRoundMoney(Player player)
        {
            Console.WriteLine("{0}: ${1}", player.Name, player.RoundMoney);
        }

        /// <summary>
        /// Delay to continue on keypress and clear screen
        /// </summary>
        private void ContinueOnKey()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Bankrupts the current player
        /// </summary>
        /// <param name="player">the current player</param>
        private void BankruptPlayer(Player player)
        {
            player.BankruptPlayer();
            Console.WriteLine("\n\nYou spun BANKRUPT! You lost all your $$$$$!\n\n");
        }

        /// <summary>
        /// Message to skip the current player's turn
        /// </summary>
        private void SkipTurn()
        {
            Console.WriteLine("\n\nSorry you lost your turn!!!\n\n");
        }

        /// <summary>
        /// If called, this method will exit the game once the user presses a key.
        /// </summary>
        private void Quit()
        {
            Console.WriteLine("The game is over! Press any key to exit...Byeeee~ \n");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private bool IsVowel(char c)
        {
            switch (Char.ToLower(c))
            {
                case 'a': case 'e': case 'i': case 'o': case 'u':
                    return true;
                default:
                    return false;
            }
        }
    }
}
