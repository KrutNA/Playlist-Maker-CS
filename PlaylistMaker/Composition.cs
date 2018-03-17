using System.Text.RegularExpressions;

namespace PlaylistMaker
{
    // Composition object
    public class Composition
    {
        // Path to composition file
        public string Path { get; private set; }

        // Author's name
        public string Author { get; private set; }

        // Composition's title
        public string Title { get; private set; }

        // Full music name like
        //"%Author%   -   %Title%"
        public string FullTitle { get; private set; }

        //Length of composition in seconds, sets always as "-1"
        public string Length { get; private set; } = "-1";

        // Create object then writing
        public Composition(string path, string author, string title)
        {
            FullTitle = author + "  -  " + title;
            this.Author = author;
            this.Path = path;
            this.Title = title;
        }

        // Create object then reading
        public Composition(string path, string fullTitle)
        {
            this.FullTitle = fullTitle;
            this.Author = Regex.Split(fullTitle, "  -  ")[0];
            this.Title = Regex.Split(fullTitle, "  -  ")[1];
            this.Path = path;
        }
    }
}
