using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

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
        private const int SLEEPTIME = 750;

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
                Console.WriteLine("\nHey player {0} What's your name? ", i + 1);
                players.Add(new Player(Console.ReadLine())); // adds a player obj to list
            }
            Console.WriteLine("\nAlright, starting with {0} player(s)!", totalPlayers);
        }

        /// <summary>
        /// Starts the game loop for number of rounds
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

            PrintWinner();
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
        /// Returns the winner (highest TotalMoney) of the game
        /// </summary>
        private void PrintWinner()
        {
            Player winner = players[0];
            for (int i = 1; i < players.Count; i++)
            {
                if (players[i].TotalMoney > winner.TotalMoney)
                {
                    winner = players[i];
                }
            }
            Console.WriteLine("\n{0} is the winner with ${1}", winner.Name, winner.TotalMoney);
        }

        /// <summary>
        /// Main play loop. Asks for user's guess until incorrect or skipped.
        /// </summary>
        /// <param name="player">A player object instantiated by the Player class.</param>
        private void Play(Player player)
        {
            Console.WriteLine("Hey {0}! Now it's your turn!", player.Name);
            PrintPlayerRoundMoney(player);
            PrintPuzzle();
            bool guessCorrect = false, skipBankrupt = false;
            SpinAction(player, ref guessCorrect, ref skipBankrupt);
            Thread.Sleep(SLEEPTIME);
            Console.Clear();
            ActionLoop:
            PrintPlayerRoundMoney(player);
            if (!skipBankrupt)
            {
                PrintPuzzle();
                if (!puzzle.IsSolved())
                {
                    if (guessCorrect)
                    {
                        NextAction(player, ref guessCorrect, ref skipBankrupt);
                        Thread.Sleep(SLEEPTIME);
                        Console.Clear();
                        goto ActionLoop;
                    }
                    else
                    {
                        Console.WriteLine("\nYour guess was wrong... Let's move on to the next player.");
                    }
                }
                else
                {
                    Console.WriteLine("\nYAYYYY! You solved it!");     
                }
            }
            else
            {
                System.Console.WriteLine("\nLet's move on to the next player.");
            }
        }

        /// <summary>
        /// Perform the spin action.
        /// Updates bool parameters for Play loop to determine next action.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>bool if action was a success</returns>
        private void SpinAction(Player player, ref bool guessCorrect, ref bool skipBankrupt)
        {
            int spinValue = wheel.WheelSpin();
            if (wheel.isBankrupt)
            {
                BankruptPlayer(player);
                skipBankrupt = true;
            }
            else if (wheel.skipTurn)
            {
                SkipTurn();
                skipBankrupt = true;
            }
            else
            {
                System.Console.WriteLine("\nYou spun ${0}!", spinValue);
                char userLetter = AskForLetter(player);
                int matches = player.GuessLetter(userLetter, puzzle, spinValue);
                if (matches > 0)
                {
                    guessCorrect = true;
                }
                else
                {
                    guessCorrect = false;
                }
            }
        }

        /// <summary>
        /// Asks for user's next action (assumes previously correct)
        /// Updates bool parameters for Play loop to determine next action.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>bool if action was a success</returns>
        private void NextAction(Player player, ref bool guessCorrect, ref bool skipBankrupt)
        {
            Console.WriteLine("\nSince you guessed correctly, you can make another spin or solve!");
            int userChoice;
            bool actionValid = false;
            do
            {
                Console.WriteLine("\n1 to Spin or 2 to Solve");
                string input = Console.ReadLine();
                actionValid = int.TryParse(input, out userChoice);
            } while (!actionValid || (userChoice != 1 && userChoice != 2));

            if (userChoice == 1)
            {
                SpinAction(player, ref guessCorrect, ref skipBankrupt);
            }
            else if (userChoice == 2)
            {
                string guessString = AskForStringSolve();
                guessCorrect = player.SolvePuzzle(guessString, puzzle);
            }
        }

        /// <summary>
        /// Asks for user's letter guess
        /// Player can also purchase a letter if enough money
        /// </summary>
        /// <returns>char letter</returns>
        private char AskForLetter(Player player)
        {
            char letter = '\0';
            bool valid = false;
            do
            {
                System.Console.Write("\nGuess a single letter: ");
                valid = char.TryParse(Console.ReadLine(), out letter);
                if (guessedLetters.Contains(letter))
                {
                    System.Console.WriteLine("\n'{0}' was already guessed!", letter);
                }
                else if (!guessedLetters.Contains(letter) && IsVowel(letter))
                {
                    if (!player.CanBuyVowel())
                    {
                        System.Console.WriteLine("\nYou can't buy '{0}'. You're broke AF!!!", letter);
                        valid = false;
                    }
                    else
                    {
                        System.Console.WriteLine("\nYou bought '{0}'", letter);
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
            System.Console.Write("\nWhat is your guess? ");
            string guess = Console.ReadLine();
            return guess;
        }

        /// <summary>
        /// Prints the puzzle
        /// </summary>
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
            for (int i = 0; i < players.Count; i++)
            {
                if (player.UniqueID == players[i].UniqueID)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("{0}: ${1}", players[i].Name, players[i].RoundMoney);
                    Console.WriteLine("Total Score: ${0}", players[i].TotalMoney);
                    Console.ResetColor();
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("{0}: ${1}", players[i].Name, players[i].RoundMoney);
                    Console.WriteLine("Total Score: ${0}", players[i].TotalMoney);
                    Console.WriteLine("");
                }
            }
            
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


        /// <summary>
        /// Returns if char is a vowel
        /// </summary>
        private bool IsVowel(char c)
        {
            switch (Char.ToLower(c))
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    return true;
                default:
                    return false;
            }
        }
    }
}
