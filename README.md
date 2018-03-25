# AnagramFinder
Simple C# console application that reads files from a directory to show anagrams found in each file.

## Execution
Run the application. When prompted, enter a valid path where the txt files are located.

## Files
- Each file has to be a valid txt file with one word per line.
- All non alphanumerical values including spaces will be removed from each line when the file is being processed.
- Empty lines will be skipped.
- If a word has no anagrams in the file, they will not be shown in the results.
- Repeated words will not be taken into account as anagrams.

## Specification
When processing each file, a Dictionary will be created where all the words will be stored with their respective anagrams. The key for this dictionary is the word with the sorted characters. For a word to be an anagram of another, the sorted version of the word should match this key.

Each word will be cleaned (remove all non alphanumerical characters and spaces) and converted to lower case. If the sorted version of the word doesn't exist in the dictionary, it will be added with the sorted word as key and a list (HashSet) with the original word in it. If the key exists the word will be added to the list. Using a HashSet makes the list contain only unique values, non sorted (sorting is not necesary in this case). 

Every time a word is processed, the program validates if the word is the longest yet (only if there is an anagram present). If it is, the new word gets chosen as the longest word. After this validation, the amount of anagrams are also counted to set the longest set of anagrams.