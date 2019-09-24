using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOFClassLib
{
    public class Wheel
    {
        private Random random;
        public bool skipTurn = false;
        public bool isBankrupt = false;
        public readonly int[] WOFClassLib = {
                2500,
                600,
                700,
                600,
                650,
                500,
                700,
                100,
                999,
                100,
                600,
                550,
                500,
                600,
               -999,
                650,
                700,
                0,
                800,
                500,
                650,
                500,
                900,
               -999
        };

        public Wheel()
        {
            random = new Random();
        }

        /// <summary>
        /// Randomly generates a number from the wheel
        /// </summary>
        /// <returns>The prize value of current spin</returns>
        public int WheelSpin()
        {
            isBankrupt = false;
            skipTurn = false;
            int wheelPrizeDollarIndex = random.Next(WOFClassLib.Length);
            int wheelPrizeDollarAmount = WOFClassLib[wheelPrizeDollarIndex];
            if(wheelPrizeDollarAmount == -999)
            {
                isBankrupt = true;
            }
            else if(wheelPrizeDollarAmount == 0)
            {
                skipTurn = true;
            }
            return wheelPrizeDollarAmount;
        }
    }
}
