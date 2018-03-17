using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace PlaylistMaker
{
    public class Playlist
    {
        public string Path { get; private set; }

        // Most of popular audio formats
        private IReadOnlyList<string> validExtensions = new List<string> { ".aac", ".ac3", ".aif", ".amr", ".opus", ".mp1",
                                                                           ".ape", ".asf", ".awb", ".cdr", ".flac", ".mp2",
                                                                           ".m4a", ".m4b", ".mid", ".mod", ".midi", ".mp3",
                                                                           ".aob", ".ogg", ".wma", ".wav", ".wave", ".mp4",
                                                                           ".ra", ".alac"};

        public List<Composition> Compositions;

        private bool HasPlsExtension(string Path)
        {
            if (Path.ToLower().EndsWith(".pls"))
            {
                return true;
            }
            return false;
        }

        // Calls with initialization of object of current class
        public Playlist()
        {
            // Making playlist file in the current directory
            Console.Write("Input playlist name: ");
            if (String.IsNullOrWhiteSpace(Path = Console.ReadLine()))
            {
                ErrorHandler.DisplayError(3);
                Path = Environment.CurrentDirectory + "\\Playlist.pls";
            }
            else
            {
                if (HasPlsExtension(Path))
                    Path = Environment.CurrentDirectory + "\\" + Path;
                else
                    Path = Environment.CurrentDirectory + "\\" + Path + ".pls";
            }

            Compositions = new List<Composition>();

            // Check
            if (!File.Exists(Path))
            {
                try { File.AppendAllText(Path, ""); }
                catch
                {
                    ErrorHandler.DisplayError(2);
                    return;
                }
            }
            else
            {
                // Rewrite
                string[] plsArray = File.ReadAllLines(Path);
                try { File.AppendAllText(Path, ""); }
                catch
                {
                    ErrorHandler.DisplayError(2);
                    return;
                }
                for (int i = 3; i < plsArray.Length; i += 4)
                {
                    Composition composition = new Composition(GetName(plsArray[i]), GetName(plsArray[i + 1]));
                    Compositions.Add(composition);
                }
            }
        }
        
        private string GetName(string TempString)
        {
            string[] tempArray = TempString.Split('=');
            string[] newTempArray = new string[tempArray.Length - 1];
            for (int i = 1; i < tempArray.Length; i++)
            {
                newTempArray[i - 1] = tempArray[i];
            }
            return String.Join("", newTempArray);
        }

        private string Input()
        {
            string input;
            if (String.IsNullOrWhiteSpace(input = Console.ReadLine()))
            {
                if (input == null)
                {
                    ErrorHandler.DisplayError(31);
                    return null;
                }
                else
                {
                    ErrorHandler.DisplayError(32);
                    return null;
                }
            }
            return input;
        }

        public void Add()
        {
            string[] arguments = new string[3];
            Console.Write("Input path to composition: ");
            if ((arguments[0] = Input()) == null) return;
            arguments[0] = arguments[0].Replace("\"", "");

            if (arguments[0].ToLower().StartsWith("http")) { }
            else if (!File.Exists(arguments[0]))
            {
                ErrorHandler.DisplayError(11);
                return;
            }

            // Checks is file in a path an audio file
            if (!validExtensions.Any(extension => arguments[0].ToLower().EndsWith(extension)))
            {
                ErrorHandler.DisplayError(13);
                return;
            }

            Console.Write("Input composition's author: ");
            if ((arguments[1] = Input()) == null) return;

            Console.Write("Input composition's title: ");
            if ((arguments[2] = Input()) == null) return;
            Composition composition = new Composition(arguments[0], arguments[1], arguments[2]);

            if (Compositions.Contains(composition))
            {
                ErrorHandler.DisplayError(12);
                return;
            }

            Compositions.Add(composition);
            return;
        }

        public void Delete()
        {
            if (Compositions.Count() == 0)
            {
                ErrorHandler.DisplayError(14);
            }
            else
            {
                string[] arguments = new string[2];
                Console.Write("Input composition's author: ");
                if ((arguments[0] = Input()) == null) return;

                Console.Write("Input composition's title: ");
                if ((arguments[1] = Input()) == null) return;

                Compositions.Remove(Compositions.Find(composition => (composition.Author.ToLower() == arguments[0].ToLower() && composition.Title.ToLower() == arguments[1].ToLower())));
            }
        }

        public void Find()
        {
            if (Compositions.Count() == 0)
            {
                ErrorHandler.DisplayError(14);
            }
            else
            {
                string[] arguments = new string[2];

                Console.Write("Input composition's author: ");
                arguments[0] = Console.ReadLine();

                Console.Write("Input composition's title: ");
                arguments[1] = Console.ReadLine();

                if (String.IsNullOrWhiteSpace(arguments[0]) && String.IsNullOrWhiteSpace(arguments[1]))
                {
                    ErrorHandler.DisplayError(34);
                    return;
                }

                if (String.IsNullOrWhiteSpace(arguments[0])) arguments[0] = "";
                if (String.IsNullOrWhiteSpace(arguments[1])) arguments[1] = "";

                List<Composition> compositions = Compositions.FindAll(composition => (composition.Author.ToLower().Contains(arguments[0].ToLower()) && composition.Title.ToLower().Contains(arguments[1].ToLower())));

                if (compositions == null)
                {
                    ErrorHandler.DisplayError(11);
                    return;
                }

                if (compositions.Count == 1)
                    Console.WriteLine("Founded 1 composition: ");
                else
                    Console.WriteLine("Founded " + compositions.Count.ToString() + " compositions");

                foreach (Composition composition in compositions)
                {
                    Console.WriteLine("\t" + composition.FullTitle);
                }
            }
        }

        public void Print()
        {
            if (Compositions.Count() == 0)
            {
                ErrorHandler.DisplayError(14);
            }
            else
            {
                Console.WriteLine("Compositions in current playlist: ");
                int counter = 0;
                foreach (Composition composition in Compositions)
                {
                    counter++;
                    Console.WriteLine("\t#" + counter.ToString() + ": " + composition.FullTitle);
                }
            }
        }

        public void PrintAll()
        {
            if (Compositions.Count() == 0)
            {
                ErrorHandler.DisplayError(14);
            }
            else
            {
                int counter = 0;
                foreach (Composition composition in Compositions)
                {
                    counter++;
                    Console.WriteLine("Music #" + counter.ToString() + "\n" +
                                       "\tAuthor: " + composition.Author + "\n" +
                                       "\tTitle: " + composition.Title + "\n" +
                                       "\tPath: " + composition.Path + "\n" +
                                       "\tLength: " + composition.Length + "\n");
                }
            }
        }

        public void Save()
        {
            if (Compositions == null)
            {
                Console.WriteLine("Playlist now is clear!");
                File.WriteAllText(Path, "[Playlist]\nNumberOfEntries=0\n");
                Console.WriteLine("Playlist now is empty!\n" +
                                  "Programm will closed in few seconds!\n" +
                                  "Please wait!");
                System.Threading.Thread.Sleep(5000);
            }
            else
            {
                File.WriteAllText(Path, "[Playlist]\nNumberOfEntries=" + Compositions.Count + "\n\n");
                int counter = 0;
                foreach (Composition composition in Compositions)
                {
                    counter++;
                    string separator = counter.ToString() + "=";
                    File.AppendAllText(Path,
                            "File" + separator + composition.Path + "\n" +
                            "Title" + separator + composition.FullTitle + "\n" +
                            "Length" + separator + composition.Length + "\n" +
                            "\n");
                }
            }
        }

    }
}
