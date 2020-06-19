using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace WisielecLibrary.Menu
{
    public class Selector
    {
        public List<string> Options = new List<string>();
        public string OutputString = "";

        public void AddOption(string option)
        {
            if (Options.Exists(x => x.ToLower() == option.ToLower()))
                return;

            Options.Add(option);
        }

        public void Print()
        {
            Console.Clear();

            Console.WriteLine("====");
            for (var i = 1; i <= Options.Count; i++)
                Console.WriteLine(" {0} {1}", i, Options[i - 1]);
            Console.WriteLine("====");

            if (OutputString.Length > 0)
            {
                Console.WriteLine(OutputString);
                OutputString = "";
            }
        }

        public int Select()
        {
            Console.Write("Wybierz akcje: ");

            try
            {
                var selectedKey = Console.ReadKey().KeyChar.ToString();

                Console.WriteLine("");

                var selectedOption = int.Parse(selectedKey) - 1;

                if (selectedOption >= 0 && selectedOption < Options.Count)
                    return selectedOption;
            }
            catch { }

            return Select();
        }
    }
}
