using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WisielecLibrary.Menu;

namespace WisielecLibrary
{
    enum GameState
    {
        Selector,
        TypeChar,
        TypeWord,
        Victory,
        GameOver
    };

    public class Wisielec
    {
        private Random Randomizer = new Random();

        private WordsDatabse Database;
        private Selector GameOptions;

        private GameState State;
        private GameStats CurrentGameStats;
        private string GamePassword;
        private List<char> UsedChars;

        public void Init()
        {
            GameOptions = new Selector();
            {
                GameOptions.AddOption("Podaj litere");
                GameOptions.AddOption("Zgadnij haslo");
            };
        }

        public void SetWordDatabase(WordsDatabse database)
        {
            Database = database;
        }

        public void StartNewGame()
        {
            if (Database.Words.Count == 0)
            {
                Console.WriteLine("Slownik nie posiada zadnych slow!");
                Console.ReadKey();
                return;
            }

            //resetowanie ustawien z poprzedniej gry
            GamePassword = Database.Words[Randomizer.Next(Database.Words.Count)];

            UsedChars = new List<char>();

            CurrentGameStats = new GameStats();
            CurrentGameStats.wordLen = GamePassword.Length;

            State = GameState.Selector;

            while (State != GameState.GameOver && State != GameState.Victory)
                GameLoop();

            Game.SelectorMenu.OutputString = State == GameState.Victory ? "Udalo sie zwyciezyles!\n" : "Niestety sprobuj nastepnym razem!\n";
            Game.SelectorMenu.OutputString += "Liczba dobrze zgadnietych znakow: " + CurrentGameStats.successChars + "\n";
            Game.SelectorMenu.OutputString += "Liczba zle zgadnietych znakow: " + CurrentGameStats.failedChars + "\n";
            Game.SelectorMenu.OutputString += "Liczba zle zgednietych slow: " + CurrentGameStats.failedWords + "\n";

            if(State == GameState.Victory)
            {
                if(Game.Statistics.IsInTop10(CurrentGameStats))
                {
                    Console.WriteLine("\nPodaj imie aby wpisac cie do najlepszych graczy: ");
                    Game.Statistics.AddPlayer(Console.ReadLine(), CurrentGameStats);
                }
            }
        }

        private void PrintPassword()
        {
            for (var i = 0; i < GamePassword.Length; i++)
            {
                if (GamePassword[i] == ' ')
                    Console.Write("     ");
                else if (UsedChars.Exists(x => char.ToLower(x) == char.ToLower(GamePassword[i])))
                    Console.Write("_{0}_ ", GamePassword[i]);
                else
                    Console.Write("___ ");
            }
            Console.WriteLine("");
            Console.WriteLine("");
        }

        private void CheckWinLose()
        {
            var totalAttempts = CurrentGameStats.failedWords + CurrentGameStats.failedChars;
            if (totalAttempts > 10)
            {
                State = GameState.GameOver;
                return;
            }

            int whiteSpace = 0;
            int chars = 0;
            for (var i = 0; i < GamePassword.Length; i++)
            {
                if (GamePassword[i] == ' ')
                    whiteSpace++;
                else if (UsedChars.Exists(x => char.ToLower(x) == char.ToLower(GamePassword[i])))
                    chars++;
            }
            chars += whiteSpace;

            if (chars == GamePassword.Length)
                State = GameState.Victory;
            else
                CurrentGameStats.wordKnownLen = chars;
        }

        private void GameLoop()
        {
            CheckWinLose();

            if (State == GameState.Selector)
            {
                GameOptions.Print();

                this.PrintPassword();

                var option = GameOptions.Select();

                switch (option)
                {
                    case 0:
                        State = GameState.TypeChar;
                        break;
                    case 1:
                        State = GameState.TypeWord;
                        break;
                }
            }
            else if (State == GameState.TypeChar)
            {
                Console.Write("Podaj znak: ");

                var user_char = Console.ReadKey().KeyChar;

                if (UsedChars.Exists(x => char.ToLower(x) == char.ToLower(user_char)))
                {
                    GameOptions.OutputString = "Ten znak zostal juz uzyty!";
                    State = GameState.Selector;
                    return;
                }

                if (GamePassword.ToLower().Contains(char.ToLower(user_char).ToString()))
                {
                    CurrentGameStats.successChars++;
                    UsedChars.Add(user_char);
                    GameOptions.OutputString = "Udalo sie!";
                }
                else
                {
                    CurrentGameStats.failedChars++;
                    GameOptions.OutputString = "To slowo nie posiada tego znaku!";
                }

                State = GameState.Selector;
                return;
            }
            else if (State == GameState.TypeWord)
            {
                Console.Write("Podaj slowo: ");

                if (Console.ReadLine().ToLower() == GamePassword.ToLower())
                    State = GameState.Victory;
                else
                {
                    GameOptions.OutputString = "To nie to slowo!";
                    CurrentGameStats.failedWords++;
                    State = GameState.Selector;
                }
            }
        }
    }
}
