using System;


namespace PlaylistMaker
{
    class Program
    {
        static bool IsExit = false;
        static void Main(string[] args)
        {
            var menu = new Menu();
            menu.DisplayHelp();

            do
            {
                Playlist playlist = new Playlist();
                do
                {
                    menu.DisplayMenu(ref playlist);
                }
                while (!menu.IsExit || !IsExit);    
            }
            while (menu.IsRestart);
        }
    }
}
