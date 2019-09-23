using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WOFClassLib.Tests
{
    public class XUnitWheelTest
    {
        [Fact]
        public void TestWheel()
        {
            Wheel wheel = new Wheel();
            for (int i = 0; i < 100; i++)
            {
                int spin = wheel.WheelSpin();
                if (spin == -999)
                {
                    Assert.True(wheel.isBankrupt);
                }
                else if (spin == 0)
                {
                    Assert.True(wheel.skipTurn);
                }
                else
                {
                    Assert.True(spin > 0);
                }
            }
        }
    }
}
