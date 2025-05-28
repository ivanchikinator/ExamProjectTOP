
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
            //AddWord("Russian.txt", "пРивеТ", new List<string>() {"hELlo", "hI"});
            //RenameTranslation("Russian-English", "привет", "hello", "HELP");
            //RenameTranslations("Russian-English", "привет", new List<string>() { "Good", "Well" });
            //FindTranslation("Russian.txt", "привет");
            //ExportTranslations("Russian.txt", "привет", "C:\\Users\\Ivan\\Desktop\\Goyda.txt");

            //ShowValues("English.txt");
            //Console.WriteLine();
            //ShowValues("Russian.txt");
            ShowMenu();
            SaveDictionaries();
        }
        public void ShowMenu()
        {
            Console.WriteLine("=====Show dictionaries=====");
            Console.WriteLine("=====  Add dictionary =====");
            Console.WriteLine("=====Remove dictionary=====");
            Console.WriteLine("=====       Exit      =====");
            int optionIndex = 0;
            string dictionaryType;
            while (true)
            {
                Console.WriteLine("Write index 1-4");
                try
                {
                    optionIndex = int.Parse(Console.ReadKey().KeyChar + "");
                    Console.WriteLine();
                    if (optionIndex < 1 || optionIndex > 4) throw new Exception("");
                    break;
                }
                catch 
                {
                    Console.WriteLine("\nError");
                    continue;
                }
            }
            switch (optionIndex)
            {
                case 1:
                    string[] dictionaries = ShowDictionaryTypes();
                    if (dictionaries.Length == 0)
                    {
                        Console.WriteLine("There are no dictionaries");
                    }
                    Console.WriteLine("Write index 1-" + dictionaries.Length + " or quit(q)");
                    try
                    {
                        char answer = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                        if(answer == 'q') ShowMenu();
                        int dictionaryIndex = int.Parse(answer + "");
                        if (dictionaryIndex < 1 || dictionaryIndex > dictionaries.Length)
                            throw new Exception("");
                        FileInfo dictionaryName = new FileInfo(dictionaries[dictionaryIndex-1]);
                        ShowValues(dictionaryName.Name);

                        ShowDictionaryOptions();
                        
                    }
                    catch
                    {
                        Console.WriteLine("Error");
                        ShowMenu();
                    }
                    break;
                case 2:
                    Console.WriteLine("Enter dictionary type or quit(q)");
                    dictionaryType = Console.ReadLine() + ".txt";
                    if (dictionaryType == "q.txt")
                    {
                        ShowMenu();
                    } 
                    else if (!string.IsNullOrEmpty(dictionaryType))
                    {
                        AddDictionary(dictionaryType);
                    }
                    break;
                case 3:
                    Console.WriteLine("Enter dictionary type or quit(q)");
                    dictionaryType = Console.ReadLine() + ".txt";
                    if (dictionaryType == "q.txt")
                    {
                        ShowMenu();
                    }
                    else if (!string.IsNullOrEmpty(dictionaryType))
                    {
                        RemoveDictionary(dictionaryType);
                    }
                    break;
                case 4:
                    return;
            }
        }
        public void ShowDictionaryOptions()
        {
            Console.WriteLine("=====      Add word   =====");
            Console.WriteLine("=====    Rename word  =====");
            Console.WriteLine("=====    Remove word  =====");
            Console.WriteLine("=====  Add translation ====");
            Console.WriteLine("==== Remove translation ===");
            Console.WriteLine("==== Rename translation ===");
            Console.WriteLine("==== Rename translations ==");
            Console.WriteLine("===== Find translation ====");
            Console.WriteLine("==== Export translation ===");
            Console.WriteLine("=====       Exit      =====");
        }
        public void LoadDictionaries()
        {
            var languageDicts = Directory.GetFiles(dictsDirectory,
                "*.*", SearchOption.AllDirectories);
            foreach (var language in languageDicts)
            {
                FileInfo dictionaryType = new FileInfo(language);
                _dictionaries.Add(new WordDictionary(dictionaryType.Name));
                _dictionaries.Last().LoadDictionary(language);
            }
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
        public void ExportTranslations(string dictionaryType, string word, string fullPath)
        {
            using (File.Create(fullPath))
            {

            }
            File.WriteAllText(fullPath, FindTranslation(dictionaryType, word, false));
        }
        public string FindTranslation(string dictionaryType, string word, bool DoWrite = true)
        {
            string results = _dictionaries.Where(x => x.DictionaryType == dictionaryType).First().FindTranslation(word);
            if(DoWrite)Console.WriteLine(results);
            return results;
        }
        public void SortDictionary(string dictionaryType)
        {
            _dictionaries.Where(x => x.DictionaryType == dictionaryType).First().SortDictionary();
        }
        public string[] ShowDictionaryTypes()
        {
            var languageDicts = Directory.GetFiles(dictsDirectory,
               "*.*", SearchOption.AllDirectories);
            foreach (var language in languageDicts)
            {
                FileInfo languageFile = new FileInfo(language);
                Console.WriteLine(languageFile.Name);
            }
            return languageDicts;
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
            Console.WriteLine("The dictionary was succesfully added");
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
            var languageDicts = Directory.GetFiles(dictsDirectory,
               "*.*", SearchOption.AllDirectories);
            foreach (var language in languageDicts)
            {
                FileInfo languageFile = new FileInfo(language);
                if (languageFile.Name == dictionaryType)
                {
                    File.Delete(language);
                }
            }
            Console.WriteLine("The dictionary was succesfully deleted");
        }
        public void AddWord(string dictionaryType, string word, List<string> translations)
        {
            _dictionaries
                .Where(x => x.DictionaryType == dictionaryType).First()
                .Add(word, translations);
            SortDictionary(dictionaryType);
        }
        public void RenameWord(string dictionaryType, string oldWord, string newWord)
        {
            _dictionaries
                .Where(x => x.DictionaryType == dictionaryType).First()
                .Rename(oldWord, newWord);
            SortDictionary(dictionaryType);
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
            SortDictionary(dictionaryType);
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
