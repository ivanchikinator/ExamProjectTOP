
using System.IO;
using System.Transactions;

namespace ExamProjectTOP
{
    public class WordsList
    {
        private List<WordDictionary> _dictionaries;
        private string dictsDirectory = "C:\\Users\\Ivan\\Desktop\\Lesson39";
        public WordsList()
        {
            _dictionaries = new List<WordDictionary>();
            LoadDictionaries();
            //AddDictionary("Russian-English");
            //AddWord("Russian-English", "Как дела", new List<string>() { "How are you", "What's up" });
            //AddWord("Russian-English", "Хорошо", new List<string>() { "Good", "Well" });
            AddWord("Russian.txt", "пРивеТ", new List<string>() {"hELlo", "hI"});
            //RenameTranslation("Russian-English", "привет", "hello", "HELP");
            //RenameTranslations("Russian-English", "привет", new List<string>() { "Good", "Well" });
            //SortDictionary("Russian-English");
            ShowValues("English.txt");
            Console.WriteLine();
            ShowValues("Russian.txt");
            SaveDictionaries();
        }
        public List<string> LoadDictionaries()
        {
            var languageDicts = Directory.GetFiles(dictsDirectory,
                "*.*", SearchOption.AllDirectories);
            var dictionaryTypes = new List<string>();
            foreach (var language in languageDicts)
            {
                FileInfo dictionaryType = new FileInfo(language);
                _dictionaries.Add(new WordDictionary(dictionaryType.Name));
                dictionaryTypes.Add(dictionaryType.Name);
                _dictionaries.Last().LoadDictionary(language);
            }
            return dictionaryTypes;
        }
        public void SaveDictionaries()
        {
            var languageDicts = Directory.GetFiles(dictsDirectory,
               "*.*", SearchOption.AllDirectories);
            foreach (var language in languageDicts)
            {
                FileInfo fileInfo = new FileInfo(language);
                _dictionaries.Where(x => x.DictionaryType == fileInfo.Name).First().SaveDictionary(language);
            }
        }
        public void SortDictionary(string dictionaryType)
        {
            _dictionaries.Where(x => x.DictionaryType == dictionaryType).First().SortDictionary();
        }
        public void ShowValues(string dictionaryType)
        {
            _dictionaries.Where(x => x.DictionaryType == dictionaryType).First().ShowValues();
        }
        public void AddDictionary(string dictionaryType)
        {
            if (_dictionaries.Where(x => x.DictionaryType == dictionaryType).FirstOrDefault() != null)
            {
                Console.WriteLine("Error, there already is such dictionary");
                return;
            }

            _dictionaries.Add(new WordDictionary(dictionaryType));
            using (File.Create(dictsDirectory + $"\\{dictionaryType}"))
            {

            }
        }
        public void RemoveDictionary(string dictionaryType)
        {
            var searchedDictionary = _dictionaries
                .Where(x => x.DictionaryType == dictionaryType).FirstOrDefault();

            if (searchedDictionary == null)
            {
                Console.WriteLine("Error, there isn't such dictionary");
                return;
            }

            _dictionaries.Remove(searchedDictionary);
        }
        public void AddWord(string dictionaryType, string word, List<string> translations)
        {
            _dictionaries
                .Where(x => x.DictionaryType == dictionaryType).First()
                .Add(word, translations);
        }
        public void RenameWord(string dictionaryType, string oldWord, string newWord)
        {
            _dictionaries
                .Where(x => x.DictionaryType == dictionaryType).First()
                .Rename(oldWord, newWord);
        }
        public void AddTranslation(string dictionaryType, string word, string translation)
        {
            _dictionaries
               .Where(x => x.DictionaryType == dictionaryType).First()
               .AddTranslation(word, translation);
        }
        public void RemoveWord(string dictionaryType, string word)
        {
            _dictionaries
               .Where(x => x.DictionaryType == dictionaryType).First()
               .Remove(word);
        }
        public void RemoveTranslation(string dictionaryType, string word, string translation)
        {
            _dictionaries
               .Where(x => x.DictionaryType == dictionaryType).First()
               .RemoveTranslation(word, translation);
        }
        public void RenameTranslation(string dictionaryType, string word,
            string oldTranslation, string newTranslation)
        {
            _dictionaries
               .Where(x => x.DictionaryType == dictionaryType).First()
               .RenameTranslation(word, oldTranslation, newTranslation);
        }
        public void RenameTranslations(string dictionaryType, string word,
            List<string> newTranslations)
        {
            _dictionaries
               .Where(x => x.DictionaryType == dictionaryType).First()
               .RenameTranslations(word, newTranslations);
        }
    }
}
