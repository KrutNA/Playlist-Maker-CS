using System;


namespace PlaylistMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program by KrutNA. Beta v0.2.2");
            Menu.DisplayHelp();

            do
            {
                Playlist playlist = new Playlist();
                do
                {
                    Menu.DisplayMenu(ref playlist);
                } while (!Menu.GetIsExit());
                
            } while (Menu.GetIsRestart());
        }
    }
}
