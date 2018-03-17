using System;
using System.Collections.Generic;

namespace PlaylistMaker
{
    public class ErrorHandler
    {
        private ErrorHandler() { }

        private const int IsBreakpoint = 10;

        private static readonly IReadOnlyDictionary<int, string> errorDescription = new Dictionary<int, string>
        {
            // Errors with file: creating, reading, rewriting
            { 1, "Couldn't create a file!" },
            { 2,  "Couldn't read or rewrite file!"},
            { 3, "Name is incorrect!\n" +
                 "Name setted as \"Playlist\"" },
            { 4, "File is invalid!" },

            // Composition errors
            { 11, "Composition not found!" },
            { 12, "Composition is already added!" },
            { 13, "The entered file is not an audio file!" },
            { 14, "Playlist is empty!" },

            // Input errors
            { 31, "Inpus is empty!" },
            { 32, "Whitespace input!" },
            { 33, "Input is incorrect!\n" +
                  "Please try again!" },
            { 34, "Atypical error!\n" +
                  "Unknown error code!" },
            { 35, "Command not founded!" }
        };


        public static void DisplayError(int ID)
        {
            Console.WriteLine("Code error ID: " + ID);
            if (errorDescription.ContainsKey(ID))
                ID = 35;

            Console.WriteLine(errorDescription[ID]);
            
            if (ID < IsBreakpoint)
            {
                Program.isExit = true;
                System.Threading.Thread.Sleep(3000);
            }
        }
    }
}
