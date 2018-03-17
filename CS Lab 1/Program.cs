using System;


namespace PlaylistMaker
{
    class Program
    {
        public static bool isExit = false;
        protected static bool isRestart = false;

        static void Main(string[] args)
        {
            bool IsStart = true;
            Console.WriteLine("Program by KrutNA. Beta v0.2.2");
            Menu.GetHelp();

            do
            {
                if (IsStart)
                    IsStart = false;

                Playlist playlist = new Playlist();
                do
                {
                    Menu.DisplayMenu(playlist);
                } while (!Menu.GetIsExit());
                
            } while (Menu.GetIsRestart());
        }
    }
}
