using System;


namespace PlaylistMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu.DisplayHelp();

            do
            {
                Playlist playlist = new Playlist();
                do
                {
                    Menu.DisplayMenu(ref playlist);
                } while (!Menu.IsExit);
                
            } while (Menu.IsRestart);
        }
    }
}
