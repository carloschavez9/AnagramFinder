using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnagramFinder
{
    class Program
    {
        private const string FILES_PATH = @"data\";

        static void Main(string[] args)
        {
            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine("-- Anagram finder");
            Console.WriteLine("----------------------------------------------------------------------------------");
            bool validPath = false;
            string pathToRead = String.Empty;
            while (!validPath)
            {
                Console.WriteLine("-- Enter a valid folder to process txt files");
                pathToRead = Console.ReadLine();
                if (Directory.Exists(pathToRead))
                {
                    validPath = true;
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Path {0} is invalid", pathToRead);
                }
            }
            // Process files
            string[] fileEntries = Directory.GetFiles(pathToRead, "*.txt");
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);


            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine("-- Done processing files, press any key to exit.");
            Console.Read();
        }

        private static void ProcessFile(string filePath)
        {
            StreamReader file = null;
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Path given is not a valid file");
                    return;
                }

                Console.WriteLine("==============================================================");
                Console.WriteLine("== Processing file: {0}", filePath);
                Console.WriteLine("==============================================================");

                // Dictionary with the main word (sorted characters) as Key, and a list of anagrams for that word (inlcuding the main word) as Value
                Dictionary<string, HashSet<string>> anagramsDictionary = new Dictionary<string, HashSet<string>>();
                // File to read
                file = new StreamReader(filePath);
                // Length of the longest words that are anagrams
                int longestAnagrams = 0;
                // List containing the keys of the longest words that are anagrams
                HashSet<string> longestAnagramsKeyList = new HashSet<string>();
                // Length of the longest set of anagrams for one word
                int longestSetOfAnagrams = 0;
                // List containing the keys of the longest sets of anagrams for one word
                HashSet<string> longestSetOfAnagramsKeyList = new HashSet<string>();

                string word;
                while ((word = file.ReadLine()) != null)
                {
                    // Convert the word to lower case
                    word = word.ToLower().Trim();
                    // Remove all non alphanumerical values
                    Regex regex = new Regex(@"[^a-z]*");
                    string cleanWord = regex.Replace(word, "");

                    // Only process words with at least 1 character
                    if (cleanWord.Length > 0)
                    {
                        HashSet<string> anagramWords;
                        // Sort characters in the word to find anagrams
                        string sortedWord = String.Concat(cleanWord.OrderBy(p => p));
                        int sortedWordLength = sortedWord.Length;
                        // If the sorted word is not a key in the list of anagrams, create a new list of anagrams with the unsorted word.
                        if (!anagramsDictionary.ContainsKey(sortedWord))
                        {
                            anagramWords = new HashSet<string> { cleanWord };
                            anagramsDictionary.Add(sortedWord, anagramWords);

                        }
                        // If the sorted word is a key in the list of anagrams, add the word to the list of anagrams
                        else
                        {
                            anagramWords = anagramsDictionary[sortedWord];
                            anagramWords.Add(cleanWord);
                        }

                        int anagramWordsCount = anagramWords.Count;

                        // Calculate longest words that are anagrams
                        if (anagramWordsCount > 1 && sortedWordLength == longestAnagrams)
                        {
                            longestAnagramsKeyList.UnionWith(anagramWords);
                            longestAnagrams = sortedWordLength;
                        }
                        else if (anagramWordsCount > 1 && sortedWordLength > longestAnagrams)
                        {
                            longestAnagramsKeyList = new HashSet<string>(anagramWords);
                            longestAnagrams = sortedWordLength;
                        }

                        // Calculate longest set of anagrams
                        if (anagramWordsCount > 1 && anagramWordsCount > longestSetOfAnagrams)
                        {
                            longestSetOfAnagramsKeyList = new HashSet<string>(anagramWords);
                            longestSetOfAnagrams = anagramWordsCount;
                        }
                    }
                }

                // Print results
                Console.WriteLine();
                Console.WriteLine("===== Anagrams =====");
                bool anagramsFound = false;
                foreach (var anagrams in anagramsDictionary.Values)
                {
                    if (anagrams.Count > 1)
                    {
                        anagramsFound = true;
                        Console.WriteLine(string.Join(" ", anagrams));
                    }
                }
                // If no anagrams were found, show message, otherwise print results
                if (!anagramsFound)
                {
                    Console.WriteLine("No words with anagrams were found.");
                }
                else
                {
                    Console.WriteLine("===== Longest words that are anagrams =====");
                    Console.WriteLine("Length: {0} chars", longestAnagrams);
                    Console.WriteLine(string.Join(" ", longestAnagramsKeyList));

                    Console.WriteLine("===== Set of anagrams containing the most words =====");
                    Console.WriteLine("Length: {0} words", longestSetOfAnagrams);
                    Console.WriteLine(string.Join(" ", longestSetOfAnagramsKeyList));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error processing the file.");
                Console.WriteLine(ex);
            }
            finally
            {
                if (file != null)
                    file.Close();


                Console.WriteLine();
                Console.WriteLine("== Finished");
                Console.WriteLine();
            }
        }
    }
}
