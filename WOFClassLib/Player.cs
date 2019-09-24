using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace WOFClassLib
{
    /// <summary>
    /// This class performs the functions related to a player of the game.
    /// </summary>
    public class Player
    {
        public string Name { get; set; }
        public int RoundMoney { get; set; }
        public int TotalMoney { get; set; }

        /// <summary>
        /// Creates a new instance of Player.
        /// </summary>
        /// <param name="name">The value for the Name property</param>
        public Player(string name = "Player")
        {
            Name = name;
            RoundMoney = 0;
            TotalMoney = 0;
        }

        /// <summary>
        ///  Guesses the letter. If the guess was correct, add spinAmount*letters to the player's Round money.
        /// </summary>
        /// <param name="guess">The character for the letter is being guessed</param>
        /// <param name="puzzle">the puzzle we are </param>
        /// <param name="spinAmount"></param>
        /// <returns>The number of letters matched</returns>
        public int GuessLetter(char guess, Puzzle puzzle, int spinAmount = 0)
        {
            //Throw argument errors if input parametes are not valid
            if (puzzle == null)
            {
                throw new ArgumentNullException(nameof(puzzle));
            }
            if (spinAmount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(spinAmount), "Argument out of range, should be greater than or equal to zero");
            }
            // Try the guess and return the number of letters matched
            int numLetters = puzzle.Guess(guess);
            if (numLetters > 0)
            {
                Console.WriteLine("Correct! You won ${0}!", numLetters * spinAmount);
                RoundMoney += numLetters * spinAmount;
            }
            return numLetters;
        }

        /// <summary>
        ///  Guesses the letter. If the guess was correct, add spinAmount*letters to the player's Round money.
        /// </summary>
        /// <param name="guess">The string for the letter is being guessed</param>
        /// <param name="puzzle">the puzzle we are </param>
        /// <param name="spinAmount"></param>
        /// <returns>The number of letters matched</returns>
        public int GuessLetter(string guess, Puzzle puzzle, int spinAmount = 0)
        {
            if (guess.Length != 1)
            {
                throw new ArgumentException("The guessed string must have a length of one.", nameof(guess));
            }
            char ch = guess[0];
            return GuessLetter(ch, puzzle, spinAmount);
        }


        /// <summary>
        /// Attempt to solve the puzzle. If the guess was correct, add the player's Round money to their TotalMoney.
        /// </summary>
        /// <param name="guess"></param>
        /// <param name="puzzle"></param>
        /// <returns>true if the guess was correct.</returns>
        public bool SolvePuzzle(string guess, Puzzle puzzle)
        {

            //Throw argument errors if input parametes are not valid
            if (puzzle == null)
            {
                throw new ArgumentNullException(nameof(puzzle));
            }

            // Try to solve the puzzle, if correct update the TotalMoney
            bool isSolved = puzzle.Solve(guess);
            if (isSolved)
            {
                TotalMoney += RoundMoney;
            }
            return isSolved;
        }

        /// <summary>
        /// Bankrupts player and makes RoundMoney go to 0
        /// </summary>
        public void BankruptPlayer()
        {
            RoundMoney = 0;
        }

        /// <summary>
        /// Initialzes the player state at the start of a new round
        /// </summary>
        public void NewRound()
        {
            RoundMoney = 0;
        }

        /// <summary>
        /// Initializes the player state at the start of a new game.
        /// </summary>
        public void NewGame()
        {
            RoundMoney = 0;
            TotalMoney = 0;
        }

    }
}
