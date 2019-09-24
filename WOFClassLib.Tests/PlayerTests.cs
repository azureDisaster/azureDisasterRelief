using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace WOFClassLib.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void TestConstructor()
        {
            Player p = new Player("kevin");
            Assert.Equal("kevin", p.Name);
            Assert.Equal(0, p.RoundMoney);
            Assert.Equal(0, p.TotalMoney);
        }

        [Fact]
        public void TestGuessLetterAsChar()
        {
            Player p = new Player("kevin");
            Puzzle puzzle = new Puzzle("a bb");
            Assert.Equal(1, p.GuessLetter('a', puzzle, 100));
            Assert.Equal(0, p.GuessLetter('z', puzzle, 100));
            Assert.Equal(2, p.GuessLetter('b', puzzle, 100));
        }

        [Fact]
        public void TestGuessLetterAsString()
        {
            Player p = new Player("kevin");
            Puzzle puzzle = new Puzzle("a bb");
            Assert.Equal(1, p.GuessLetter("a", puzzle, 100));
            Assert.Equal(0, p.GuessLetter("z", puzzle, 100));
            Assert.Equal(2, p.GuessLetter("b", puzzle, 100));
        }

        [Fact]
        public void TestRoundMoney()
        {
            Player p = new Player("kevin");
            Puzzle puzzle = new Puzzle("a bb");
            p.GuessLetter('a', puzzle, 100); // +100
            Assert.Equal(100, p.RoundMoney);

            p.GuessLetter('z', puzzle, 100); // should not increase
            Assert.Equal(100, p.RoundMoney);

            p.GuessLetter('b', puzzle, 100); // +200
            Assert.Equal(300, p.RoundMoney);
        }

        [Fact]
        public void TestBankrupt()
        {
            Player p = new Player("kevin");
            Puzzle puzzle = new Puzzle("a bb");
            p.GuessLetter('a', puzzle, 100); // +100
            p.BankruptPlayer();
            Assert.Equal(0, p.RoundMoney);
        }

        [Fact]
        public void TestSolvePuzzle()
        {
            Player sut = new Player();
            Puzzle puzzle = new Puzzle("DOG");
            Assert.True(sut.SolvePuzzle("DOG", puzzle));
            Assert.False(sut.SolvePuzzle("AAAA", puzzle));
        }

        [Fact]
        public void TestWinRoundandNewRound()
        {
            Player p = new Player();
            Puzzle puzzle = new Puzzle("DOG");
            p.GuessLetter('d', puzzle, 100);
            p.GuessLetter('o', puzzle, 100);
            p.GuessLetter('g', puzzle, 100);
            Assert.Equal(300, p.RoundMoney);
            p.WinRound();
            Assert.Equal(300, p.TotalMoney);

            p.NewRound();
            Assert.Equal(0, p.RoundMoney);
        }

    }
}
