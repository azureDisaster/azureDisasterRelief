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
        private List<Player> players;
        private Phrase phrase;
        private Puzzle puzzle;
        private Wheel wheel;
        private int totalPlayers;

        public Game()
        {
            players = new List<Player>();
            wheel = new Wheel();
        }
       
        /// <summary>
        /// This method will prompt the user for details regarding the initialization of the game. 
        /// </summary>
        public void Start()
        {
            // initialize players
            Console.WriteLine("Welcome to Wheel of Fortune sponsored by Azure Disaster Relief LLC.");
            bool valid = false;
            do
            {
                Console.WriteLine("How many players would you like to begin with?");
                string input = Console.ReadLine();
                valid = int.TryParse(input, out totalPlayers);
            } while (!valid || totalPlayers <= 0 || totalPlayers >= 4);

            for (int i = 0; i < totalPlayers; i++)
            {
                Console.WriteLine("Hey player {0} What's your name? \n", i+1);
                players.Add(new Player(Console.ReadLine())); // adds a player obj to list
            }

            // play number of rounds
            Console.WriteLine("Alright, starting with {0} player(s)! \n", totalPlayers);
            ContinueOnKey();
            const int ROUNDS = 3;
            for (int i = 0; i < ROUNDS; i++)
            {
                Console.WriteLine("ROUND {0}", i + 1);
                StartRound();
            }

            Quit();
        }

        /// <summary>
        /// Starts the game round
        /// </summary>
        private void StartRound()
        {
            phrase = new Phrase();
            puzzle = new Puzzle(phrase.GetPhrase());
            int index = 0;
            Player currentPlayer = players[index];
            while (!puzzle.IsSolved()) // if the game is being played, loop thru the players
            {
                Play(currentPlayer); // call play on the current player object
                // finalize the round
                if (puzzle.IsSolved())
                {
                    FinalizeRound(currentPlayer);
                }

                // switch player
                index = (index + 1) % totalPlayers;
                currentPlayer = players[index];
                ContinueOnKey();
            }
        }

        /// <summary>
        /// The player that solves the phrase gets the money.
        /// All other players are bankrupt.
        /// </summary>
        /// <param name="solvingPlayer"></param>
        private void FinalizeRound(Player solvingPlayer)
        {
            solvingPlayer.WinRound();
            foreach (Player p in players)
            {
                p.NewRound();
            }
        }

        /// <summary>
        /// This method will create a player for every player in the game. 
        /// </summary>
        /// <param name="player">A player object instantiated by the Player class.</param>
        private void Play(Player player)
        {
            Console.WriteLine("Hey {0}! Now it's your turn, make a guess. Remember, you can only guess a letter, no solving allowed!\n", player.Name);
            PlayerFirstSpin(player);
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
        /// The first spin for the player.
        /// An action item is determined with the spin.
        /// </summary>
        /// <param name="player">the current player</param>
        private void PlayerFirstSpin(Player player)
        {
            int spinValue = wheel.WheelSpin();
            if (wheel.isBankrupt)
            {
                BankruptPlayer(player);
                return;
            }
            else if (wheel.skipTurn)
            {
                SkipTurn();
                return;
            }
            else
            {
                PlayerCanGuess(player, spinValue);
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
        /// Allows player to guess a letter for a spinValue.
        /// If player guesses correctly, then player perform another action.
        /// </summary>
        /// <param name="player">the current player</param>
        /// <param name="spinValue">the value of the spin</param>
        private void PlayerCanGuess(Player player, int spinValue)
        {
            PrintPlayerRoundMoney(player);
            // ask for a letter
            Console.WriteLine("You spun ${0}!", spinValue);
            Console.WriteLine(puzzle.GetPuzzleDisplay());
            string guess = "";
            int numberOfCorrectLetters = 0;

            // validate letter
            bool validGuess = false;
            while (!validGuess)
            {
                Console.WriteLine("This is your first guess, please enter a single letter. \n");
                guess = Console.ReadLine();
                Console.WriteLine("You guessed: {0}! \n", guess);
                validGuess = Regex.IsMatch(guess, "^[a-zA-Z]") && guess.Length == 1;
            }

            // guess letter
            numberOfCorrectLetters = player.GuessLetter(guess, puzzle, spinValue);
            Console.WriteLine(puzzle.GetPuzzleDisplay());

            // if successful guess, player can guess, solve, or pass
            bool isSolved = puzzle.IsSolved(); // false
            while (numberOfCorrectLetters >= 1 && !isSolved)
            {
                PrintPlayerRoundMoney(player);

                //// ask for user's next action
                //Console.WriteLine("Since you guessed correctly, you can make another spin or pass!");
                //int userChoice;
                //bool actionValid = false;
                //do
                //{
                //    Console.WriteLine("1 to Spin or 2 to Pass");
                //    string input = Console.ReadLine();
                //    actionValid = int.TryParse(input, out userChoice);
                //} while (!actionValid || (userChoice != 1 && userChoice != 2));
                //if (userChoice == 2)
                //{
                //    Console.WriteLine("\n\nYou passed!\n\n");
                //    return;
                //}

                // user chose to spin again
                int nextSpinValue = wheel.WheelSpin();
                if (wheel.isBankrupt)
                {
                    BankruptPlayer(player);
                    return;
                }
                else if (wheel.skipTurn)
                {
                    SkipTurn();
                    return;
                }
                else
                {
                    Console.WriteLine("Make another guess or attempt to solve! \n");
                    Console.WriteLine("You spun ${0}!", nextSpinValue);
                    // player's next guesses
                    guess = Console.ReadLine();
                    bool stringGuess = Regex.IsMatch(guess, "^[a-zA-Z]+"); // returns true if only contains letters
                    while (!stringGuess)
                    {
                        Console.WriteLine("Please guess a letter or phrase.\n");
                        guess = Console.ReadLine();
                        Console.WriteLine("You guessed: {0}! \n", guess);
                        stringGuess = Regex.IsMatch(guess, @"^[a-zA-Z]+$");
                    }

                    if (guess.Length > 1) // trying to guess the phrase
                    {
                        isSolved = player.SolvePuzzle(guess, puzzle); // last modified
                        if (isSolved)
                        {
                            Console.WriteLine("YAYYYY! You solved it! \n");
                            Console.WriteLine(puzzle.GetPuzzleDisplay());
                            return; // early exit
                        }
                        numberOfCorrectLetters = 0;
                    }
                    else
                    {
                        numberOfCorrectLetters = player.GuessLetter(guess, puzzle, nextSpinValue);
                        isSolved = puzzle.IsSolved();
                    }
                    Console.WriteLine("You guessed: {0} \n", guess);
                    Console.WriteLine(puzzle.GetPuzzleDisplay());
                }
            }

            if (isSolved)
            {
                PrintPlayerRoundMoney(player);
                Console.WriteLine("Congrats! You solved it! \n");
            }
            else
            {
                PrintPlayerRoundMoney(player);
                if (totalPlayers == 1)
                {
                    Console.WriteLine("Your guess was wrong. It's okay, you may try again. \n");
                }
                else
                {
                    Console.WriteLine("Your guess was wrong... Let's move on to the next player. \n");
                }
            }
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
    }
}
