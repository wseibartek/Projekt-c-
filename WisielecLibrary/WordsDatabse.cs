using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WisielecLibrary
{
    public class WordsDatabse
    {
        private string FileDatabseName;
        private string FileFullPath;

        public List<string> Words = new List<string>();

        public WordsDatabse(string name) { FileDatabseName = name; FileFullPath = Path.Combine(Directory.GetCurrentDirectory(), FileDatabseName); }
        public WordsDatabse() : this("databse.txt") { }

        public bool LoadDatabase()
        {
            try
            {
                //sprawdz czy plik istenieje
                if (!File.Exists(FileFullPath))
                    return false;

                //wczytanie wszystkich lini jako lista
                Words = File.ReadAllLines(FileFullPath).ToList();

                return true;
            }
            catch { return false; }
        }

        public bool SaveDatabase()
        {
            try
            {
                //wpisanie wszystkich slow do pliku
                File.WriteAllLines(FileFullPath, Words);
                return true;
            }
            catch { return false; }
        }

        public bool AddWord(string word)
        {
            if (word == null || word.Length == 0)
                return false;

            //sprawdzenie czy slowo juz istnieje w naszym slowniku
            if (Words.Exists(x => x.ToLower() == word.ToLower()))
                return false;

            Words.Add(word);

            return true;
        }

        public bool AddWordFromConsoleInput()
        {
            Console.Write("Podaj slowo do dodania: ");
            return this.AddWord(Console.ReadLine());
        }
    }
}
