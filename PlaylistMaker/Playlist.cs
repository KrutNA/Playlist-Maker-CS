using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace PlaylistMaker
{
    public class Playlist
    {
        public string Path { get; private set; }

        // Most of popular audio formats
        private IReadOnlyList<string> validExtensions = new List<string> {
            ".aac", ".ac3", ".aif", ".amr", ".opus", ".mp1",
            ".ape", ".asf", ".awb", ".cdr", ".flac", ".mp2",
            ".m4a", ".m4b", ".mid", ".mod", ".midi", ".mp3",
            ".aob", ".ogg", ".wma", ".wav", ".wave", ".mp4",
            ".ra", ".alac"
        };

        private List<Composition> Compositions;
        
        public Playlist()
        {
            Compositions = new List<Composition>();

            // Making playlist file in the current directory
            Console.Write("Input playlist name: ");
            Path = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(Path))
            {
                ErrorHandler.DisplayError(3);
                Path = Environment.CurrentDirectory + "\\Playlist.pls";
            }
            else
            {
                Path = Environment.CurrentDirectory + "\\" + Path;
                if (HasPlsExtension(Path))
                {
                    Path = Path + ".pls";
                }
            }
            // Check
            if (!File.Exists(Path))
            {
                try { File.AppendAllText(Path, ""); }
                catch
                {
                    ErrorHandler.DisplayError(2);
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
                    var composition = new Composition(GetName(plsArray[i]), GetName(plsArray[i + 1]));
                    Compositions.Add(composition);
                }
            }
        }

        private bool URLExists(string url)
        {
            var result = false;

            var webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200;
            webRequest.Method = "HEAD";

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)webRequest.GetResponse();
                result = true;
            }
            catch (WebException webException)
            {
                Console.WriteLine($"{url} doesn't exist: {webException.Message}");
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }

        private bool HasPlsExtension(string path)
        {
            return path.ToLower().EndsWith(".pls");
        }

        private string GetName(string tempString)
        {
            var tempArray = tempString.Split('=');
            var newTempArray = new string[tempArray.Length - 1];
            for (int i = 1; i < tempArray.Length; i++)
            {
                newTempArray[i - 1] = tempArray[i];
            }
            return String.Join("", newTempArray);
        }

        private string Input()
        {
            var input = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(input))
            {
                if (String.IsNullOrEmpty(input))
                {
                    ErrorHandler.DisplayError(31);
                }
                else
                {
                    ErrorHandler.DisplayError(32);
                }
                return null;
            }
            return input;
        }

        public void AddComposition()
        {
            string path;
            string author;
            string title;

            Console.Write("Input path to composition: ");
            path = Input();
            if (!String.IsNullOrEmpty(path))
            {
                return;
            }
            path = path.Replace("\"", "");
            
            if (path.ToLower().StartsWith("http") && URLExists(path)) { }
            else if (!File.Exists(path))
            {
                ErrorHandler.DisplayError(11);
                return;
            }

            // Checks is file in a path an audio file
            if (!validExtensions.Any(extension => path.ToLower().EndsWith(extension)))
            {
                ErrorHandler.DisplayError(13);
                return;
            }
            
            Console.Write("Input composition's author: ");
            author = Input();
            if (String.IsNullOrEmpty(author))
            {
                return;
            }

            Console.Write("Input composition's title: ");
            title = Input();
            if (String.IsNullOrEmpty(title))
            {
                return;
            }
            Composition composition = new Composition(path, author, title);

            if (Compositions.Contains(composition))
            {
                ErrorHandler.DisplayError(12);
                return;
            }

            Compositions.Add(composition);
        }

        public void DeleteCompsition()
        {
            if (Compositions.Count() == 0)
            {
                ErrorHandler.DisplayError(14);
            }
            else
            {
                string author;
                string title;
                Console.Write("Input composition's author: ");
                author = Input();
                if (String.IsNullOrEmpty(author))
                {
                    return;
                }

                Console.Write("Input composition's title: ");
                title = Input();
                if (String.IsNullOrEmpty(title))
                {
                    return;
                }

                Compositions.Remove(Compositions.Find(composition =>
                    (composition.Author.ToLower() == author.ToLower() &&
                    composition.Title.ToLower() == title.ToLower())));
            }
        }

        public void FindComposition()
        {
            if (Compositions.Count() == 0)
            {
                ErrorHandler.DisplayError(14);
            }
            else
            {
                string author;
                string title;

                Console.Write("Input composition's author: ");
                author = Console.ReadLine();

                Console.Write("Input composition's title: ");
                title = Console.ReadLine();

                if (String.IsNullOrWhiteSpace(author) && String.IsNullOrWhiteSpace(title))
                {
                    ErrorHandler.DisplayError(34);
                    return;
                }
                
                if (String.IsNullOrWhiteSpace(author))
                {
                    author = "";
                }
                if (String.IsNullOrWhiteSpace(title))
                {
                    title = "";
                }

                var compositions = Compositions.FindAll(composition =>
                    (composition.Author.ToLower().Contains(author.ToLower()) &&
                     composition.Title.ToLower().Contains(title.ToLower())));

                if (compositions.Count() == 0)
                {
                    Console.WriteLine("Composition not found!");
                    return;
                }

                if (compositions.Count == 1)
                    Console.WriteLine("Founded 1 composition: ");
                else
                    Console.WriteLine($"Founded {compositions.Count} compositions");

                for (int i = 0; i < compositions.Count(); i++)
                {
                    Console.WriteLine($"\t {compositions[i].FullTitle}");
                }
            }
        }

        public void Display()
        {
            if (Compositions.Count() == 0)
            {
                ErrorHandler.DisplayError(14);
            }
            else
            {
                Console.WriteLine("Compositions in current playlist: ");
                for (int i = 0; i < Compositions.Count(); i++)
                {
                    Console.WriteLine($"\t#{i+1}: {Compositions[i].FullTitle}");
                }
            }
        }

        public void DisplayAll()
        {
            if (Compositions.Count() == 0)
            {
                ErrorHandler.DisplayError(14);
            }
            else
            {
                for (int i = 0; i < Compositions.Count(); i++)
                {
                    Console.WriteLine(
                        $"Music #{i + 1}\n\tAuthor: {Compositions[i].Author}\n\tTitle: {Compositions[i].Title}\n\tPath: {Compositions[i].Path}\n\tLength: {Compositions[i].Length}\n");
                }
            }
        }

        public void Save()
        {
            if (Compositions == null)
            {
                Console.WriteLine("Playlist now is clear!");
                File.WriteAllText(Path,
                    "[Playlist]\nNumberOfEntries=0\n");
                Console.WriteLine(
                    "Playlist now is empty!\nProgramm will closed in few seconds!\nPlease wait!");
            }
            else
            {
                File.WriteAllText(Path, $"[Playlist]\nNumberOfEntries={Compositions.Count}\n\n");
                for (int i = 0; i < Compositions.Count(); i++)
                {
                    string separator = Convert.ToString(i + 1) + "=";
                    File.AppendAllText(Path,
                        $"File{separator}{Compositions[i].Path}\nTitle{separator}{Compositions[i].FullTitle}\nLength{separator}{Compositions[i].Length}\n\n");
                }
            }
        }

    }
}
