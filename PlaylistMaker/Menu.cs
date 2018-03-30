using System;
using System.Collections.Generic;

namespace PlaylistMaker
{
    public class Menu
    {
        public bool IsExit { get; set; } = false;
        public bool IsRestart { get; private set; } = false;
        private bool isInit = false;
        private delegate Playlist MenuFunctionCalling(Playlist playlist);
        private Dictionary<string, Delegate> menuFunctions = new Dictionary<string, Delegate>();

        public Menu() { }

        public void InitMenuItems()
        {
            MenuFunctionCalling CommmandHelpCalling;
            menuFunctions.Add("help",
                CommmandHelpCalling = playlist =>
                {
                    DisplayHelp();
                    return playlist;
                }
            );

            MenuFunctionCalling ConsoleClearing;
            menuFunctions.Add("cls",
                ConsoleClearing = playlist =>
                {
                    Console.Clear();
                    return playlist;
                }
            );

            MenuFunctionCalling DisplayPlaylistCalling;
            menuFunctions.Add("list", DisplayPlaylistCalling = playlist =>
                {
                    playlist.Display();
                    return playlist;
                }
            );

            MenuFunctionCalling SearchMethodCalling;
            menuFunctions.Add("search", SearchMethodCalling = playlist =>
                {
                    playlist.FindComposition();
                    return playlist;
                }
            );

            MenuFunctionCalling AdditionCompositionCalling;
            menuFunctions.Add("add", AdditionCompositionCalling = playlist =>
                {
                    playlist.AddComposition();
                    return playlist;
                }
            );

            MenuFunctionCalling DeleteCompositionCalling;
            menuFunctions.Add("del", DeleteCompositionCalling = playlist =>
                {
                    playlist.DeleteCompsition();
                    return playlist;
                }
            );

            MenuFunctionCalling SavePlaylistCalling;
            menuFunctions.Add("save", SavePlaylistCalling = playlist =>
                {
                    playlist.Save();
                    return playlist;
                }
            );

            MenuFunctionCalling QuitMethodCalling;
            menuFunctions.Add("quit", QuitMethodCalling = playlist =>
                {
                    Quit(ref playlist);
                    return playlist;
                }
            );

            MenuFunctionCalling SpecialDisplayPlaylistCalling;
            menuFunctions.Add("fsc", SpecialDisplayPlaylistCalling = playlist =>
                {
                    playlist.DisplayAll();
                    return playlist;

                }
            );

            MenuFunctionCalling RestartMethodCalling;
            menuFunctions.Add("restart", RestartMethodCalling = playlist =>
                {
                    Restart(ref playlist);
                    return playlist;
                }
            );

            isInit = true;
        }

        private void Quit(ref Playlist playlist)
        {
            playlist.Save();
            this.IsExit = true;
        }

        private void Restart(ref Playlist playlist)
        {
            IsRestart = true;
            Quit(ref playlist);
        }

        public void DisplayHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("\t\"help\"   - to call this message\n\t\"cls\"    - to clear console");
            Console.WriteLine("\t\"list\"   - to display all items of catalog\n\t\"search\" - to go find ites in catalog");
            Console.WriteLine("\t\"add\"    - to add new item\n\t\"del\"    - to remove some item from list");
            Console.WriteLine("\t\"quit\"   - to save and exit\n\t\"restart\" - to save and open new file\n");
        }

        private bool IsInvalidInput(string input)
        {
            var IsInvalid = false;
            if (String.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input is empty!");
                IsInvalid = true;
            }
            else if (String.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Whitespace input!");
                IsInvalid = true;
            }
            return IsInvalid;
        }

        public void DisplayMenu( ref Playlist playlist )
        {
            string command;
            var isReturn = false;
            IsRestart = false;
            IsExit = false;

            Console.Write("> ");
            command = Console.ReadLine().ToLower();

            isReturn = IsInvalidInput(command);

            if (!isInit)
            {
                InitMenuItems();
            }

            if (!menuFunctions.ContainsKey(command))
            {
                ErrorHandler.DisplayError(35);
            }
            else if (!isReturn)
            {
                playlist = (Playlist)menuFunctions[command].DynamicInvoke(playlist);
            }
        }
    }
}
