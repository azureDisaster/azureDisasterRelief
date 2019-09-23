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
        private Phrase phrase = new Phrase();
        private Puzzle puzzle;
        private int totalPlayers;
       
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

            players = new List<Player>(totalPlayers);
            for (int i = 0; i < totalPlayers; i++)
            {
                Console.WriteLine("Hey player {0} What's your name? \n", i+1);
                players.Add(new Player(Console.ReadLine())); // adds a player obj to list
            }

            // play number of rounds
            Console.WriteLine("Alright, starting with {0} player(s)! \n", totalPlayers);
            const int ROUNDS = 3;
            for (int i = 0; i < ROUNDS; i++)
            {
                Console.WriteLine("ROUND {0}", i + 1);
                StartRound();
                Console.WriteLine("Press any key to continue!");
                Console.ReadKey();
                Console.Clear();
            }

            Quit();
        }

        private void StartRound()
        {
            phrase = new Phrase();
            puzzle = new Puzzle(phrase.GetPhrase());
            int index = 0;
            Player currentPlayer = players[index];
            while (!puzzle.IsSolved()) // if the game is being played, loop thru the players
            {
                Play(currentPlayer); // call play on the current player object
                index = (index + 1) % totalPlayers;
                currentPlayer = players[index];
            }
        }

        /// <summary>
        /// This method will create a player for every player in the game. 
        /// </summary>
        /// <param name="player">A player object instantiated by the Player class.</param>
        private void Play(Player player)
        {
            Console.WriteLine("Hey {0}! Now it's your turn, make a guess. Remember, you can only guess a letter, no solving allowed!\n", player.Name);
            Console.WriteLine(puzzle.GetPuzzleDisplay());
            string guess = ""; 
            int numberOfCorrectLetters = 0; 

            bool validGuess = false;

            while (!validGuess) {
                Console.WriteLine("This is your first guess, please enter a single letter. \n");
                guess = Console.ReadLine();
                Console.WriteLine("You guessed: {0}! \n", guess);
                validGuess = Regex.IsMatch(guess, "^[a-zA-Z]") && guess.Length == 1;
            }

            numberOfCorrectLetters = player.GuessLetter(guess, puzzle);

            Console.WriteLine(puzzle.GetPuzzleDisplay());
            bool isSolved = puzzle.IsSolved(); // false
                

            while (numberOfCorrectLetters >= 1 && !isSolved)
            {
                Console.WriteLine("Since you guessed correctly, make another guess or attempt to solve! \n");
                // if the guess.length > 1 then assign as a string
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
                    numberOfCorrectLetters = player.GuessLetter(guess, puzzle);
                    isSolved = puzzle.IsSolved();
                }
                Console.WriteLine("You guessed: {0} \n", guess);
                Console.WriteLine(puzzle.GetPuzzleDisplay());
            }

            if (isSolved)
            {
                Console.WriteLine("Congrats! You solved it! \n");
            }
            else
            {
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