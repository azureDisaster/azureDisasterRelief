using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOFClassLib;

namespace WheelOfFortune
{
    class Program
    {
        static void Main(string[] args)
        {
            bool resetGame;
            do
            {
                Game game = new Game();
                game.StartGame();
                resetGame = game.ResetStatus();
            } while (resetGame);
        }
    }
}
