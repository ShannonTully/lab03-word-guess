using System;
using System.IO;

namespace word_game
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Menu();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Exit();
            }
        }

        /// <summary>
        /// Runs the main menu for starting a game, going to admin, and exiting.
        /// </summary>
        static void Menu()
        {
            while (true)
            {
                Console.WriteLine("Press '1' to play");
                Console.WriteLine("Press '2' to use the admin console");
                Console.WriteLine("Press '3' to exit");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    GameLoop();
                }
                else if (input == "2")
                {
                    Admin();
                }
                else
                {
                    Exit();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static void GameLoop()
        {
            char[] word = GetRandomWord().ToCharArray();
            char[] guesses = new char[word.Length];
            char[] failures = new char[27];
            int failCounter = 0;

            for (int i = 0; i < guesses.Length; i++)
            {
                guesses[i] = '_';
            }

            while (true)
            {
                Console.WriteLine($"{JoinCharArray(guesses)}");
                Console.WriteLine($"Failures: {JoinCharArray(failures)}");
                Console.WriteLine("Make a guess");

                char guess = Convert.ToChar(Console.ReadLine());

                int position = IsLetterInWord(word, guess);

                if (position == -1)
                {
                    Console.WriteLine("That isn't in the word");
                    try
                    {
                        failures[failCounter] = guess;
                        failCounter++;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new IndexOutOfRangeException("You've ran out of guesses");
                    }
                }
                else
                {
                    guesses[position] = guess;
                }

                if (JoinCharArray(guesses).Contains("_") == false)
                {
                    Console.WriteLine(JoinCharArray(word));
                    Console.WriteLine("Great job");
                    break;
                }
            }
        }

        /// <summary>
        /// Runs the admin menu
        /// </summary>
        static void Admin()
        {
            while (true)
            {
                Console.WriteLine("Press '1' to list words");
                Console.WriteLine("Press '2' to add a word");
                Console.WriteLine("Press '3' to remove a word");
                Console.WriteLine("Press '4' to reset the words");
                Console.WriteLine("Press '5' to return to the menu");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    string[] words = ListAvailableWords();

                    foreach (string word in words)
                    {
                        Console.WriteLine(word);
                    }
                }
                else if (input == "2")
                {
                    Console.WriteLine("Input word to add");
                    string word = Console.ReadLine();
                    int pass = AddWordToWords(word);

                    if (pass == 1)
                    {
                        Console.WriteLine("Word already in list");
                    }
                }
                else if (input == "3")
                {
                    Console.WriteLine("Input word to remove");
                    string word = Console.ReadLine();
                    int pass = RemoveWordFromWords(word);

                    if (pass == 1)
                    {
                        Console.WriteLine("Word not in list");
                    }
                }
                else if (input == "4")
                {
                    ResetWords();
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Exit the program
        /// </summary>
        static void Exit()
        {
            Console.WriteLine("Goodbye");
            Console.ReadKey();
            Environment.Exit(0);
        }

        /// <summary>
        /// Adds a word to the text file for guessing
        /// </summary>
        /// <param name="wordToAdd">Word to add</param>
        /// /// <returns>0: worked
        /// 1: Didn't</returns>
        public static int AddWordToWords(string wordToAdd)
        {
            string path = "../../../../words.txt";
            wordToAdd = wordToAdd.ToLower();
            string[] words = File.ReadAllLines(path);
            string[] output = new string[words.Length + 1];

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == wordToAdd)
                {
                    return 1;
                }

                output[i] = words[i];
            }

            output[output.Length - 1] = wordToAdd;

            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (string word in output)
                {
                    sw.WriteLine(word);
                }
            }

            return 0;
        }

        /// <summary>
        /// Remove a word from the text file for guessing
        /// </summary>
        /// <param name="wordToRemove">Word to remove</param>
        /// <returns>0: worked
        /// 1: Didn't</returns>
        public static int RemoveWordFromWords(string wordToRemove)
        {
            string path = "../../../../words.txt";
            wordToRemove = wordToRemove.ToLower();
            string[] words = File.ReadAllLines(path);
            string[] output = new string[words.Length - 1];
            bool flag = false;
            try
            {
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i] != wordToRemove && flag == false)
                    {
                        output[i] = words[i];
                    }
                    else if (words[i] == wordToRemove && flag == false)
                    {
                        flag = true;
                    }
                    else
                    {
                        output[i - 1] = words[i];
                    }
                }

                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (string word in output)
                    {
                        sw.WriteLine(word);
                    }
                }

                return 0;
            }
            catch (IndexOutOfRangeException)
            {
                return 1;
            }
            
        }

        /// <summary>
        /// Reset all of the words in the word list 
        /// Used for testing
        /// </summary>
        public static void ResetWords()
        {
            string path = "../../../../words.txt";
            string[] words = { "cat", "goat", "human" };

            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (string word in words)
                {
                    sw.WriteLine(word);
                }
            }
        }

        /// <summary>
        /// List all of the words in the text file for guessing
        /// </summary>
        /// <returns>List of all words</returns>
        public static string[] ListAvailableWords()
        {
            string path = "../../../../words.txt";
            string[] words = File.ReadAllLines(path);
            return words;
        }

        /// <summary>
        /// Get a random word from the text file for guessing
        /// </summary>
        /// <returns>Word</returns>
        public static string GetRandomWord()
        {
            string path = "../../../../words.txt";
            string[] words = File.ReadAllLines(path);
            Random rand = new Random();
            return words[rand.Next(words.Length)];
        }

        /// <summary>
        /// Check to see if a letter is in a char array
        /// </summary>
        /// <param name="word">Char array of the word</param>
        /// <param name="letter">Letter to check</param>
        /// <returns>Index of location or -1</returns>
        public static int IsLetterInWord(char[] word, char letter)
        {
            letter = Convert.ToChar(letter.ToString().ToLower());

            for (int i = 0; i < word.Length; i++)
            {
                if (letter == word[i])
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Joins a char array with spaces
        /// </summary>
        /// <param name="input">A char array</param>
        /// <returns>String with spaces</returns>
        public static string JoinCharArray(char[] input)
        {
            var sb = new System.Text.StringBuilder();

            foreach (char letter in input)
            {
                sb.Append(letter);
                sb.Append(' ');
            }

            return sb.ToString();
        }
    }
}
