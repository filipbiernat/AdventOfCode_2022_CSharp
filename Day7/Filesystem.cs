namespace AdventOfCode2022.Day7
{
    public class Filesystem
    {
        // The filesystem consists of a tree of files (plain data) and directories (which can contain other directories or files).
        private readonly FileOrDirectory OutermostDirectory;
        private FileOrDirectory CurrentDirectory;

        public Filesystem()
        {
            // The outermost directory is called /.
            OutermostDirectory = new("/");
            CurrentDirectory = OutermostDirectory;
        }

        public void NavigateAround(string command)
        {
            // You can navigate around the filesystem, moving into or out of directories and listing the contents of the directory you're currently in.
            string[] splitCommand = command.Split(null, 2);
            if (splitCommand[0] == "cd")
            {
                // cd means change directory. This changes which directory is the current directory, but the specific result depends on the argument.
                CurrentDirectory = CurrentDirectory.Cd(splitCommand[1]);
            }
            else if (splitCommand[0] == "ls")
            {
                // ls means list. It prints out all of the files and directories immediately contained by the current directory.
                CurrentDirectory.Ls(splitCommand[1].Trim());
            }
        }

        public int GetTotalUsedSpace() => OutermostDirectory.GetSize();
        public List<FileOrDirectory> GetListOfDirectories() => OutermostDirectory.BrowseForDirectories();
        public void DetermineTheTotalSizeOfEachDirectory() => OutermostDirectory.CalculateSizeOfTheDirectories();
    }
}
