using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ExamProjectTOP
{
    public class WordDictionary
    {
        private Dictionary<string, List<string>> _dictionary;
        public string DictionaryType { get; private set; }
        public WordDictionary(string type)
        {
            _dictionary = new Dictionary<string, List<string>>();
            DictionaryType = type;
        }
        public void Add(string word, List<string> translations)
        {
            string normalizedWord = char.ToUpper(word[0]) + word[1..].ToLower();
            if (_dictionary.ContainsKey(normalizedWord))
            {
                Console.WriteLine("Error, there already exist such word " + normalizedWord);
                return;
            }
            _dictionary.Add(normalizedWord,
                translations.Select(x => char.ToUpper(x[0]) + x[1..].ToLower()).ToList());
        }
        public void Remove(string word)
        {
            string normalizedWord = char.ToUpper(word[0]) + word[1..].ToLower();
            if (!_dictionary.ContainsKey(normalizedWord))
            {
                Console.WriteLine("Error, the word doesn't exist " + normalizedWord);
                return;
            }
            _dictionary.Remove(normalizedWord);
        }
        public void RemoveTranslation(string word, string translation)
        {
            string normalizedWord = char.ToUpper(word[0]) + word[1..].ToLower();

            if (!_dictionary.ContainsKey(normalizedWord))
            {
                Console.WriteLine("Error, the word doesn't exist " + normalizedWord);
                return;
            }

            string normalizedTranslation = char.ToUpper(translation[0]) + translation[1..].ToLower();

            if (!_dictionary[normalizedWord].Contains(normalizedTranslation))
            {
                Console.WriteLine("Error, the translation doesn't exist " + normalizedTranslation);
                return;
            }

            if (_dictionary[normalizedWord].Count == 1)
            {
                Console.WriteLine($"Error, you cannot delete the translation {normalizedTranslation}" +
                    $" cause it is the only translation to the word " + normalizedWord);
                return;
            }

            _dictionary[normalizedWord].Remove(normalizedTranslation);
        }
        public void AddTranslation(string word, string translation)
        {
            string normalizedWord = char.ToUpper(word[0]) + word[1..].ToLower();

            if (!_dictionary.ContainsKey(normalizedWord))
            {
                Console.WriteLine("Error, the word doesn't exist " + normalizedWord);
                return;
            }

            string normalizedTranslation = char.ToUpper(translation[0]) + translation[1..].ToLower();

            if (_dictionary[normalizedWord].Contains(normalizedTranslation))
            {
                Console.WriteLine("Error, the translation already exists " + normalizedTranslation);
                return;
            }

            _dictionary[normalizedWord].Add(normalizedTranslation);
        }
        public void Rename(string oldWord, string newWord)
        {
            string normalizedOldWord = char.ToUpper(oldWord[0]) + oldWord[1..].ToLower();

            string normalizedNewWord = char.ToUpper(newWord[0]) + newWord[1..].ToLower();

            if (!_dictionary.ContainsKey(normalizedOldWord))
            {
                Console.WriteLine("Error, the word doesn't exist " + normalizedOldWord);
                return;
            }
            if (_dictionary.ContainsKey(normalizedNewWord))
            {
                Console.WriteLine("Error, the word already exists " + normalizedNewWord);
                return;
            }
            KeyValuePair<string, List<string>> WordNTranslation =
                _dictionary.Where(x => x.Key == oldWord).First();
            _dictionary.Remove(WordNTranslation.Key);
            _dictionary.Add(newWord, WordNTranslation.Value);
        }
        public void RenameTranslation(string word, string oldTranslation, string newTranslation)
        {
            string normalizedOldTranslation = char.ToUpper(oldTranslation[0]) + oldTranslation[1..].ToLower();

            string normalizedNewTranslation = char.ToUpper(newTranslation[0]) + newTranslation[1..].ToLower();

            string normalizedWord = char.ToUpper(word[0]) + word[1..].ToLower();

            if (!_dictionary.ContainsKey(normalizedWord))
            {
                Console.WriteLine("Error, the word doesnt exist " + normalizedWord);
                return;
            }

            if (!_dictionary[normalizedWord].Contains(normalizedOldTranslation))
            {
                Console.WriteLine("Error, the translation doesn't exist " + normalizedOldTranslation);
                return;
            }
            if (_dictionary[normalizedWord].Contains(normalizedNewTranslation))
            {
                Console.WriteLine("Error, the translation already exists " + normalizedNewTranslation);
                return;
            }
            _dictionary[normalizedWord].Remove(normalizedOldTranslation);
            _dictionary[normalizedWord].Add(normalizedNewTranslation);
        }
        public void RenameTranslations(string word,
            List<string> newTranslations)
        {
            List<string> normalizedNewTranslations = newTranslations
                .Select(x => char.ToUpper(x[0]) + x[1..].ToLower()).ToList();

            string normalizedWord = char.ToUpper(word[0]) + word[1..].ToLower();

            if (!_dictionary.ContainsKey(normalizedWord))
            {
                Console.WriteLine("Error, the word doesnt exist " + normalizedWord);
                return;
            }

            _dictionary[normalizedWord] = newTranslations;
        }
        public void SortDictionary()
        {
            _dictionary = new SortedDictionary<string, List<string>>(_dictionary).ToDictionary();
        }
        public void ShowValues()
        {
            foreach (KeyValuePair<string, List<string>> wordNTranslations in _dictionary)
            {
                Console.WriteLine(wordNTranslations.Key + " - "
                    + string.Join(" ", wordNTranslations.Value));
            }
        }
        public void LoadDictionary(string path)
        {
            var dictionary = File.ReadAllLines(path);
            foreach (var word in dictionary)
            {
                string[] keyvaluepair = word.Split();
                _dictionary.Add(keyvaluepair[0], keyvaluepair[1..].ToList());
            }
        }
        public void SaveDictionary(string path)
        {
            File.WriteAllText(path, "");
            foreach (KeyValuePair<string, List<string>> wordNTranslations in _dictionary)
            {
                File.AppendAllText(path, wordNTranslations.Key + " " + string.Join(" ", wordNTranslations.Value) + "\n");
            }
        }
    }
}
