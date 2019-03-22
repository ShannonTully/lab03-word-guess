using System;
using word_game;
using Xunit;
using System.IO;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void TestReset()
        {
            string path = "../../../../words.txt";
            string[] words = { "cat", "goat", "human" };
            Program.ResetWords();
            Assert.Equal(words, File.ReadAllLines(path));
        }

        [Fact]
        public void TestThatAddWordWorksValid()
        {
            Program.ResetWords();
            string path = "../../../../words.txt";
            Assert.Equal(0, Program.AddWordToWords("dog"));
            string[] words = File.ReadAllLines(path);
            Assert.Equal("dog", words[words.Length - 1]);
            Program.ResetWords();
        }

        [Fact]
        public void TestThatAddWordWorksInvalid()
        {
            Program.ResetWords();
            Assert.Equal(1, Program.AddWordToWords("cat"));
            Program.ResetWords();
        }

        [Fact]
        public void TestThatRemoveWordsWorksValid()
        {
            Program.ResetWords();
            string path = "../../../../words.txt";
            Assert.Equal(0, Program.RemoveWordFromWords("cat"));
            Assert.Equal(2, File.ReadAllLines(path).Length);
            Program.ResetWords();
        }

        [Fact]
        public void TestThatRemoveWordsWorksInalid()
        {
            Program.ResetWords();
            string path = "../../../../words.txt";
            Assert.Equal(1, Program.RemoveWordFromWords("dog"));
            Assert.Equal(3, File.ReadAllLines(path).Length);
            Program.ResetWords();
        }

        [Fact]
        public void TestListWords()
        {
            Program.ResetWords();
            string path = "../../../../words.txt";
            Assert.Equal(File.ReadAllLines(path), Program.ListAvailableWords());
        }

        [Fact]
        public void TestGetRandomWord()
        {
            Program.ResetWords();
            Program.RemoveWordFromWords("goat");
            Program.RemoveWordFromWords("human");
            string path = "../../../../words.txt";
            Assert.Equal("cat", Program.GetRandomWord());
            Program.ResetWords();
        }

        [Fact]
        public void TestLetterInWordValid()
        {
            char[] word = { 'a', 'b', 'c' };
            Assert.Equal(1, Program.IsLetterInWord(word, 'b'));
        }

        [Fact]
        public void TestLetterInWordInvalid()
        {
            char[] word = { 'a', 'b', 'c' };
            Assert.Equal(-1, Program.IsLetterInWord(word, 'd'));
        }

        [Fact]
        public void TestJoinCharArray()
        {
            char[] word = { 'a', 'b', 'c' };
            Assert.Equal("a b c ", Program.JoinCharArray(word));
        }
    }
}
