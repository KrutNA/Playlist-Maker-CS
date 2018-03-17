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
        public Composition(string Path, string Author, string Title)
        {
            FullTitle = Author + "\t-\t" + Title;
            this.Author = Author;
            this.Path = Path;
            this.Title = Title;
        }

        // Create object then reading
        public Composition(string Path, string FullTitle)
        {
            this.FullTitle = FullTitle;
            this.Author = Regex.Split(FullTitle, "\t-\t")[0];
            this.Title = Regex.Split(FullTitle, "\t-\t")[1];
            this.Path = Path;
        }
    }
}
