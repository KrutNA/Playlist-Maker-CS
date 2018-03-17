using System;
using System.Collections.Generic;

namespace PlaylistMaker
{
    class Menu
    {
        private static bool isExit = false;
        private static bool isRestart = false;
        private static bool isInit = false;
        private delegate Playlist ExecuteMenuFunction(Playlist playlist);
        private static readonly Dictionary<string, Delegate> menuItems = new Dictionary<string, Delegate>();

        private Menu() { }
        
        public static void InitMenuItems()
        {
            ExecuteMenuFunction help = delegate (Playlist playlist)
            {
                DisplayHelp();
                return playlist;
            };
            ExecuteMenuFunction cls = delegate (Playlist playlist)
            {
                Console.Clear();
                return playlist;
            };
            ExecuteMenuFunction list = delegate (Playlist playlist)
            {
                playlist.Display();
                return playlist;
            };
            ExecuteMenuFunction search = delegate (Playlist playlist)
            {
                playlist.FindComposition();
                return playlist;
            };
            ExecuteMenuFunction add = delegate (Playlist playlist)
            {
                playlist.AddComposition();
                return playlist;
            };
            ExecuteMenuFunction del = delegate (Playlist playlist)
            {
                playlist.DeleteCompsition();
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
                playlist.DisplayAll();
                return playlist;

            };
            ExecuteMenuFunction restart = delegate (Playlist playlist)
            {
                Restart(ref playlist);
                return playlist;
            };

            menuItems.Add("help", help);
            menuItems.Add("cls", cls);
            menuItems.Add("list", list);
            menuItems.Add("search", search);
            menuItems.Add("add", add);
            menuItems.Add("del", del);
            menuItems.Add("save", save);
            menuItems.Add("quit", quit);
            menuItems.Add("fsc", fsc);
            menuItems.Add("restart", restart);

            isInit = true;
        }

        private static void Quit(ref Playlist playlist)
        {
            playlist.Save();
            isExit = true;
        }

        private static void Restart(ref Playlist playlist)
        {
            isRestart = true;
            Quit(ref playlist);
        }

        public static void DisplayHelp()
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
            return isExit;
        }

        public static void SeiIsExit(bool newIsExit)
        {
            isExit = newIsExit; 
        }

        public static bool GetIsRestart()
        {
            return isRestart;
        }

        private static bool IsInvalidInput(string input)
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
            bool isReturn = false;

            string command = Console.ReadLine().ToLower();

            isReturn = IsInvalidInput(command);

            if (!isInit)
                InitMenuItems();

            if (!menuItems.ContainsKey(command))
            {
                ErrorHandler.DisplayError(35);
            }
            else if (isReturn) { }
            else
            {
                playlist = (Playlist)menuItems[command].DynamicInvoke(playlist);
            }
        }
    }
}
