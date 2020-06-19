using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WisielecLibrary
{
    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };

    public class GameStats
    {
        public int successChars = 0;
        public int failedChars = 0;
        public int failedWords = 0;
        public int wordLen = 0;
        public int wordKnownLen = 0;
    };

    public class GameStatistic
    {
        private string FileDatabseName;
        private string FileFullPath;

        public List<Pair<string, int>> TopPlayers = new List<Pair<string, int>>();

        public GameStatistic(string name) { FileDatabseName = name; FileFullPath = Path.Combine(Directory.GetCurrentDirectory(), FileDatabseName); }
        public GameStatistic() : this("top.txt") { }

        public bool LoadStatistics()
        {
            try
            {
                if (!File.Exists(FileFullPath))
                    return false;

                var Lines = File.ReadAllLines(FileFullPath);
                for (var i = 0; i < Lines.Length; i += 2)
                {
                    try
                    {
                        var score = int.Parse(Lines[i + 1]);
                        TopPlayers.Add(new Pair<string, int>(Lines[i], score));
                    }
                    catch { }
                }
                return true;
            }
            catch { return false; }
        }
        public bool SaveDatabase()
        {
            try
            {
                List<string> top_txt = new List<string>();

                foreach (var topPlayer in TopPlayers)
                {
                    top_txt.Add(topPlayer.First);
                    top_txt.Add(topPlayer.Second.ToString());
                }

                File.WriteAllLines(FileFullPath, top_txt);
                return true;
            }
            catch { return false; }
        }

        public int CalculateScore(GameStats stats)
        {
            var score = stats.wordLen - stats.wordKnownLen;
            if (stats.successChars - stats.failedChars > 0)
                score *= stats.successChars;

            score -= stats.failedWords;

            return score;
        }

        public bool IsInTop10(GameStats stats)
        {
            if (TopPlayers.Count < 10)
                return true;

            var current = CalculateScore(stats);

            foreach(var topPlayer in TopPlayers)
            {
                if (current > topPlayer.Second)
                    return true;
            }

            return false;
        }

        public void AddPlayer(string name, GameStats stats)
        {
            if (TopPlayers.Count >= 10)
            {
                Pair<string, int> lowestPlayer = null;

                foreach (var topPlayer in TopPlayers)
                {
                    if (lowestPlayer == null || topPlayer.Second < lowestPlayer.Second)
                        lowestPlayer = topPlayer;
                }

                TopPlayers.Remove(lowestPlayer);
            }

            TopPlayers.Add(new Pair<string, int>(name, CalculateScore(stats)));
        }

    }
}
