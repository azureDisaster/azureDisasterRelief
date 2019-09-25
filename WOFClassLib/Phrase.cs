using System;
using System.Collections.Generic;
using System.IO;
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
        private static Random random = new Random();
        private static string rootPath = Directory.GetCurrentDirectory() + "../../../../";
        private static string bankPath = rootPath + "/WOFWordBank/";
        private static int numFiles = Directory.GetFiles(bankPath).Count();
        private HashSet<string> usedPhrases;

        public string Category { get; private set; }
        private string randomWord;

        public Phrase()
        {
            usedPhrases = new HashSet<string>();
        }

        /// <summary>
        /// Randomly picks a file and returns a random phrase each time it is called.
        /// </summary>
        /// <returns></returns>
        public string GetPhrase()
        {
            //return "abcd";
            string path = bankPath;
            int cat = random.Next(numFiles);
            switch (cat)
            {
                case 0:
                    path += "food.txt";
                    Category = "FOOD";
                    break;
                case 1:
                    path += "people.txt";
                    Category = "PEOPLE";
                    break;
                case 2:
                    path += "places.txt";
                    Category = "PLACES";
                    break;
                default:
                    throw new Exception("There is a file not accounted for!");
            }

            try
            {
                IEnumerable<string> phrases = File.ReadLines(path);
                do
                {
                    int countTo = random.Next(phrases.Count());
                    int i = 0;
                    foreach (string s in phrases)
                    {
                        if (i == countTo)
                        {
                            randomWord = s;
                            break;
                        }
                        i++;
                    }
                } while (usedPhrases.Contains(randomWord));
                usedPhrases.Add(randomWord);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                System.Console.WriteLine(e.StackTrace);                
                throw new Exception("There was an error");
            }

            // System.Console.WriteLine(randomWord);
            return randomWord;
        }

    }
}
