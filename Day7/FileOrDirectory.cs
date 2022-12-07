namespace AdventOfCode2022.Day7
{
    public class FileOrDirectory
    {
        public string Name;
        private int Size;

        private readonly FileOrDirectory? Parent;
        private readonly bool IsDirectory;
        private readonly List<FileOrDirectory> Contents = new();

        public FileOrDirectory(string name, FileOrDirectory? parent = null, int size = 0, bool isDirectory = true)
        {
            Name = name;
            Size = size;
            Parent = parent;
            IsDirectory = isDirectory;
        }

        // cd means change directory. This changes which directory is the current directory, but the specific result depends on the argument.
        public FileOrDirectory Cd(string argument)
        {
            if (argument == "..")
            {
                // cd .. moves out one level: it finds the directory that contains the current directory, then makes that directory the current directory.
                return Parent is null ? this : Parent;
            }
            else if (argument == "/")
            {
                // cd / switches the current directory to the outermost directory, /.
                return Parent is null ? this : Parent.Cd("/");
            }
            else
            {
                // cd x moves in one level: it looks in the current directory for the directory named x and makes it the current directory.
                return Contents
                    .Where(file => file.Name == argument)
                    .First();
            }
        }

        // ls means list. It prints out all of the files and directories immediately contained by the current directory.
        public void Ls(string output)
        {
            foreach (string entry in output.Split("\r\n"))
            {
                string[] splitEntry = entry.Split(" ");
                if (splitEntry.First() == "dir")
                {
                    // dir xyz means that the current directory contains a directory named xyz.
                    Contents.Add(new FileOrDirectory(name: splitEntry.Last(), parent: this));
                }
                else
                {
                    // 123 abc means that the current directory contains a file named abc with size 123.
                    int size = int.Parse(splitEntry.First());
                    Contents.Add(new FileOrDirectory(name: splitEntry.Last(), parent: this, size: size, isDirectory: false));
                }
            }
        }

        public int CalculateSizeOfTheDirectories()
        {
            if (IsDirectory)
            {
                Size = Contents
                    .Select(file => file.CalculateSizeOfTheDirectories())
                    .Sum();
            }
            return Size;
        }

        public List<FileOrDirectory> BrowseForDirectories()
        {
            List<FileOrDirectory> directories = new() { this };
            Contents
                .Where(file => file.IsDirectory)
                .ToList()
                .ForEach(subdirectory => directories.AddRange(subdirectory.BrowseForDirectories()));
            return directories;
        }

        public int GetSize() => Size;
    }
}
