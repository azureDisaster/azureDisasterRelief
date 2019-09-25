using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace WOFClassLib
{
    public class Puzzle
    {
        private string puzzlePhrase;
        private int phraseLength;
        private char[] display;
        private bool solved = false;

        /// <summary>
        /// A constructor that creates an instance of a Puzzle.
        /// </summary>
        /// <param name="phrase">The solution phrase for the puzzle.</param>
        public Puzzle(string phrase)
        {
            puzzlePhrase = phrase.ToUpper();
            phraseLength = phrase.Length;
            display = InitializePuzzle(phrase.ToUpper());
        } 
        
        /// <summary>
        /// Creates the initial state of the puzzle's display. It validates each character of the phrase, 
        /// and substitutes dashes for the letters.
        /// </summary>
        /// <param name="phrase">The phrase for the puzzle</param>
        /// <returns>The current state of the puzzle as an array of chars</returns>
        private char[] InitializePuzzle(string phrase)
        {
            List<char> puz = new List<char>(phrase.Length);

            for (int i = 0; i < phraseLength; i++)
            {
                char c = puzzlePhrase[i];

                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    throw new ArgumentException("The puzzle phrase should only contain characters that are letters or spaces.");
                }

                if (i != phraseLength - 1)
                {
                    char nextChar = puzzlePhrase[i + 1];
                    if (char.IsWhiteSpace(c) && char.IsWhiteSpace(nextChar))
                    {
                        throw new ArgumentException("The puzzle phrase should not contain consecutive spaces.");
                    }
                }

                if(char.IsWhiteSpace(c))
                {
                    puz.Add(' ');
                }
                else
                {
                    puz.Add('-');
                }
            }
            return puz.ToArray();
        }

        /// <summary>
        /// Gets the current state of the puzzle display in array form
        /// </summary>
        /// <returns>An array of characters representing the current state of the puzzle display</returns>
        private char[] GetPuzzleDisplayAsArray()
        {
            return display;
        }

        /// <summary>
        /// Gets the current state of the puzzle display 
        /// </summary>
        /// <returns>A string representing the current state of the puzzle</returns>
        public string GetPuzzleDisplay()
        {
            return new string(display);
        }

        /// <summary>
        /// Checks if a letter guessed is in the puzzle, and updates the puzzle display accordingly.
        /// </summary>
        /// <param name="guess"></param>
        /// <returns>The number of times the letter guessed appears in the puzzle phrase</returns>
        public int Guess(char guess)
        {
            if (!char.IsLetter(guess))
            {
                throw new ArgumentException("The guessed character must be a valid letter");
            }

            guess = char.ToUpper(guess);
            int numberOfMatches = 0;
            char[] currentDisplayArray = GetPuzzleDisplayAsArray();

            for (int i = 0; i < phraseLength; i++)
            {
                if (puzzlePhrase[i].Equals(guess))
                {
                    numberOfMatches++;

                    currentDisplayArray[i] = guess;

                    if (!display.Contains('-'))
                    {
                        solved = true;
                    }
                }
            }
            return numberOfMatches;
        }

        /// <summary>
        /// Checks if the guessed solution to a puzzle is correct.
        /// </summary>
        /// <param name="guess">The guessed solution</param>
        /// <returns>A boolean value that is true if the solution is correct, and false otherwise.</returns>
        public bool Solve(string guess)
        {
            if(guess.ToUpper().Equals(puzzlePhrase))
            {
                solved = true;
                display = puzzlePhrase.ToCharArray();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets whether the puzzle has been solved or not.
        /// </summary>
        /// <returns>A boolean value that is true if the state of puzzle is solved, and false otherwise.</returns>
        public bool IsSolved()
        {
            return solved;
        }
    }
}
