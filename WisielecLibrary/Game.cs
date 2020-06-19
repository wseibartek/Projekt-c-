using System;
using System.Collections.Generic;
using System.Text;
using WisielecLibrary.Menu;

namespace WisielecLibrary
{
    public static class Game
    {
        public static WordsDatabse Database;
        public static Selector SelectorMenu;
        public static Wisielec WisielecGame;
        public static GameStatistic Statistics;

        public static void Init()
        {
            Database = new WordsDatabse();
            {
                Database.LoadDatabase();
            };

            Statistics = new GameStatistic();
            {
                Statistics.LoadStatistics();
            };

            SelectorMenu = new Selector();
            {
                SelectorMenu.AddOption("Rozpocznij gre");
                SelectorMenu.AddOption("Pokaz historie gier (top10)");
                SelectorMenu.AddOption("Dodaj slowo");
                SelectorMenu.AddOption("Wyjscie");
            };

            WisielecGame = new Wisielec();
            {
                WisielecGame.Init();
                WisielecGame.SetWordDatabase(Database);
            };
        }
    }
}
