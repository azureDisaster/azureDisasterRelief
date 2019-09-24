using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WOFClassLib
{
    /// <summary>
    /// This class performs the functions related to a player of the game.
    /// </summary>
    public class Player
    {
        private static int ID = 0;
        private int UniqueID;

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
            UniqueID = ID++;
        }

        public override int GetHashCode()
        {
            return UniqueID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Player o = (Player) obj;
            return this.UniqueID == o.UniqueID;
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
            char ch = guess[0];
            return GuessLetter(ch, puzzle, spinAmount);
        }

        /// <summary>
        /// Returns if guess is the correct solved phrase
        /// </summary>
        /// <param name="guess"></param>
        /// <param name="puzzle"></param>
        /// <returns></returns>
        public bool SolvePuzzle(string guess, Puzzle puzzle)
        {
            return puzzle.Solve(guess);
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
        /// The current player wins the round.
        /// RoundMoney is added to TotalMoney
        /// </summary>
        public void WinRound()
        {
            TotalMoney += RoundMoney;
        }
    }
}
