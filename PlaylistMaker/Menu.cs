using System;
using System.Collections.Generic;

namespace PlaylistMaker
{
    class Menu
    {
        private static bool IsExit = false;
        private static bool IsRestart = false;
        
        private Menu() { }

        private delegate Playlist ExecuteMenuFunction(Playlist playlist);
        private static bool IsInit = false;

        private static readonly Dictionary<string, Delegate> MenuItems = new Dictionary<string, Delegate>();

        public static void InitMenuItems()
        {
            ExecuteMenuFunction help = delegate (Playlist playlist)
            {
                GetHelp();
                return playlist;
            };
            ExecuteMenuFunction cls = delegate (Playlist playlist)
            {
                Console.Clear();
                return playlist;
            };
            ExecuteMenuFunction list = delegate (Playlist playlist)
            {
                playlist.Print();
                return playlist;
            };
            ExecuteMenuFunction search = delegate (Playlist playlist)
            {
                playlist.Find();
                return playlist;
            };
            ExecuteMenuFunction add = delegate (Playlist playlist)
            {
                playlist.Add();
                return playlist;
            };
            ExecuteMenuFunction del = delegate (Playlist playlist)
            {
                playlist.Delete();
                return playlist;
            };
            ExecuteMenuFunction save = delegate (Playlist playlist)
            {
                playlist.Save();
                return playlist;
            };
            ExecuteMenuFunction quit = delegate (Playlist playlist)
            {
                Quit(ref playlist);
                return playlist;
            };
            ExecuteMenuFunction fsc = delegate (Playlist playlist)
            {
                playlist.PrintAll();
                return playlist;

            };
            ExecuteMenuFunction restart = delegate (Playlist playlist)
            {
                Restart(ref playlist);
                return playlist;
            };

            MenuItems.Add("help", help);
            MenuItems.Add("cls", cls);
            MenuItems.Add("list", list);
            MenuItems.Add("search", search);
            MenuItems.Add("add", add);
            MenuItems.Add("del", del);
            MenuItems.Add("save", save);
            MenuItems.Add("quit", quit);
            MenuItems.Add("fsc", fsc);
            MenuItems.Add("restart", restart);

            IsInit = true;
        }

        private static void Quit(ref Playlist playlist)
        {
            playlist.Save();
            IsExit = true;
            return;
        }

        private static void Restart(ref Playlist playlist)
        {
            IsRestart = true;
            Quit(ref playlist);
            return;
        }

        public static void GetHelp()
        {
            Console.WriteLine("Usage:\n" +
                              "\t\"help\"   - to call this message\n" +
                              "\t\"cls\"    - to clear console\n" +
                              "\t\"list\"   - to display all items of catalog\n" +
                              "\t\"search\" - to go find ites in catalog\n" +
                              "\t\"add\"    - to add new item\n" +
                              "\t\"del\"    - to remove some item from list\n" +
                              "\t\"quit\"   - to save and exit\n" +
                              "\t\"restart\" - to save and open new file\n");
        }

        public static bool GetIsExit()
        {
            return IsExit;
        }

        public static bool GetIsRestart()
        {
            return IsRestart;
        }

        private static bool IsInvalidkInput(string input)
        {
            bool IsInvalid = false;
            if (String.IsNullOrEmpty(input))
            {
                ErrorHandler.DisplayError(31);
                IsInvalid = true;
            }
            else if (String.IsNullOrWhiteSpace(input))
            {
                ErrorHandler.DisplayError(32);
                IsInvalid = true;
            }
            return IsInvalid;
        }

        public static void DisplayMenu( ref Playlist playlist )
        {
            Console.Write("> ");
            bool IsReturn = false;

            string command = Console.ReadLine().ToLower();

            IsReturn = IsInvalidkInput(command);

            if (!IsInit)
                InitMenuItems();

            if (!MenuItems.ContainsKey(command) || IsReturn)
            {
                ErrorHandler.DisplayError(35);
            }
            else
            {
                playlist = (Playlist) MenuItems[command].DynamicInvoke(playlist);
            }
        }
    }
}
