using System;
using System.Collections.Generic;

namespace PlaylistMaker
{
    class Menu
    {
        private static bool IsExit = false;
        private static bool IsReturn = false;
        
        private Menu() { }

        private delegate DelegateValues ExecuteMenuFunction(DelegateValues delegateValues);
        private static bool IsInit = false;

        public struct DelegateValues
        {
            public Playlist playlist { get; set; }
            public bool IsExit { get; set; }
            public bool IsReturn { get; set; }

            public DelegateValues(Playlist playlist, bool IsExit, bool IsReturn)
            {
                this.playlist = playlist;
                this.IsExit = IsExit;
                this.IsReturn = IsReturn;
            }
        }

        private static readonly Dictionary<string, Delegate> MenuItems = new Dictionary<string, Delegate>();

        public static void InitMenuItems()
        {
            ExecuteMenuFunction help = delegate (DelegateValues delegateValues)
            {
                GetHelp();
                return delegateValues;
            };
            ExecuteMenuFunction cls = delegate (DelegateValues delegateValues)
            {
                Console.Clear();
                return delegateValues;
            };
            ExecuteMenuFunction list = delegate (DelegateValues delegateValues)
            {
                delegateValues.playlist.Print();
                return delegateValues;
            };
            ExecuteMenuFunction search = delegate (DelegateValues delegateValues)
            {
                delegateValues.playlist.Find();
                return delegateValues;
            };
            ExecuteMenuFunction add = delegate (DelegateValues delegateValues)
            {
                Playlist playlist = delegateValues.playlist;
                playlist.Add();
                delegateValues.playlist = playlist;
                return delegateValues;
            };
            ExecuteMenuFunction del = delegate (DelegateValues delegateValues)
            {
                Playlist playlist = delegateValues.playlist;
                playlist.Delete();
                delegateValues.playlist = playlist;
                return delegateValues;
            };
            ExecuteMenuFunction save = delegate (DelegateValues delegateValues)
            {
                Playlist playlist = delegateValues.playlist;
                playlist.Save();
                delegateValues.playlist = playlist;
                return delegateValues;
            };
            ExecuteMenuFunction quit = delegate (DelegateValues delegateValues)
            {
                Playlist playlist = delegateValues.playlist;
                bool IsExit = delegateValues.IsExit;
                Quit(ref playlist, ref IsExit);
                delegateValues.IsExit = IsExit;
                delegateValues.playlist = playlist;
                return delegateValues;
            };
            ExecuteMenuFunction fsc = delegate (DelegateValues delegateValues)
            {
                delegateValues.playlist.PrintAll();
                return delegateValues;

            };
            ExecuteMenuFunction restart = delegate (DelegateValues delegateValues)
            {
                Playlist playlist = delegateValues.playlist;
                bool IsExit = delegateValues.IsExit;
                bool IsReturn = delegateValues.IsReturn;
                Restart(ref playlist, ref IsExit, ref IsReturn);
                delegateValues.playlist = playlist;
                delegateValues.IsExit = IsExit;
                delegateValues.IsReturn = IsReturn;
                return delegateValues;
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

        private static void Quit(ref Playlist playlist, ref bool IsExit)
        {
            playlist.Save();
            IsExit = true;
            return;
        }

        private static void Restart(ref Playlist playlist, ref bool IsExit, ref bool IsReturn)
        {
            IsReturn = true;
            Quit(ref playlist, ref IsExit);
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
            return IsReturn;
        }

        public static void DisplayMenu( Playlist playlist )
        {
            Console.Write("> ");
            bool IsReturn = false;

            string command = Console.ReadLine();
            if (String.IsNullOrEmpty(command))
            {
                ErrorHandler.DisplayError(31);
                IsReturn = true;
            }
            else if (String.IsNullOrWhiteSpace(command))
            {
                ErrorHandler.DisplayError(32);
                IsReturn = true;
            }

            if (!IsInit)
                InitMenuItems();

            if (!MenuItems.ContainsKey(command) || IsReturn)
            {
                ErrorHandler.DisplayError(35);
            }
            else
            {
                DelegateValues delegateValues = new DelegateValues(playlist, IsExit, IsReturn);
                delegateValues = (DelegateValues)MenuItems[command].DynamicInvoke(delegateValues);
                playlist = delegateValues.playlist;
                IsExit = delegateValues.IsExit;
                IsReturn = delegateValues.IsReturn;
            }
        }
    }
}
