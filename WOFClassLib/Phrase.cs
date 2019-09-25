using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOFClassLib
{
    /// <summary>
    /// This class is responsible for generating phrases for the Wheel Of Fortune puzzles.
    /// </summary>
    public class Phrase
    {
        // simulate a database with a hard coded list of strings
        private List<string> phrases = new List<string> {
            "HEY JUDE",
            "I AM THE WALRUS",
            "HELLO GOODBYE",
            "YESTERDAY",
            "PENNY LANE",
            "STRAWBERRY FIELDS FOREVER",
            "LUCY IN THE SKY WITH DIAMONDS",
            "ROCKY RACOON",
            "WHILE MY GUITAR GENTLY WEEPS",
            "SOMETHING",
            "HAPPINESS IS A WARM GUN",
            "SHE LOVES YOU",
            "I WANT TO HOLD YOUR HAND",
            "A DAY IN THE LIFE",
            "LET IT BE",
            "REVOLUTION",
            "ELEANOR RIGBY",
            "WE CAN WORK IT OUT",
            "COME TOGETHER",
            "HERE COMES THE SUN",
            "IN MY LIFE",
            "HELP",
            "LOVE ME DO",
            "YELLOW SUBMARINE",
            "A HARD DAYS NIGHT",
            "ALL YOU NEED IS LOVE",
            "PAPERBACK WRITER",
            "WITH A LITTLE HELP FROM MY FRIENDS"
        };

        private List<string> usedPhrases = new List<string>();

        /// <summary>
        /// Returns a random phrase each time it is called.
        /// </summary>
        /// <returns></returns>
        public string GetPhrase()
        {
            //return "abc"; // for testing
            //check to see if there we still have phrases left in the list. If not, recycle the list.
            if (phrases.Count == 0)
            {
                phrases = usedPhrases;

                // Move the last phrase used to the usedPhrases, so we don't
                // get the same phrase twice in a row

                string lastPhase = phrases[phrases.Count - 1];
                phrases.RemoveAt(phrases.Count - 1);
                usedPhrases = new List<string> { lastPhase };

            }

            //grab a random phrase from the list
            Random rnd = new Random();
            int i = rnd.Next(phrases.Count);
            string result = phrases[i];

            //move the phrase to the usedPhrases list
            phrases.RemoveAt(i);
            usedPhrases.Add(result);

            //return the phrase
            return result;
        }
    }
}
