using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day7
{
    public class Day7B : IDay
    {
        public void Run()
        {
            // Perhaps you can delete some files to make space for the update?
            // You browse around the filesystem to assess the situation and save the resulting terminal output (your puzzle input).
            string input = File.ReadAllText(@"..\..\..\Day7\Day7.txt");

            // Within the terminal output, lines that begin with $ are commands you executed, very much like some modern computers.
            List<string> commands = input
                .Split("$ ")
                .Skip(1)
                .Select(command => command.Trim())
                .ToList();

            // You can navigate around the filesystem, moving into or out of directories and listing the contents of the directory you're currently in.
            Filesystem filesystem = new();
            commands.ForEach(filesystem.NavigateAround);

            // Since the disk is full, your first step should probably be to find directories that are good candidates for deletion.
            // To do this, you need to determine the total size of each directory.
            filesystem.DetermineTheTotalSizeOfEachDirectory();

            // The total disk space available to the filesystem is 70000000.
            const int totalDiskSpace = 70000000;
            // To run the update, you need unused space of at least 30000000.
            const int spaceForTheUpdate = 30000000;
            // You need to find a directory you can delete that will free up enough space to run the update.
            int unusedSpace = totalDiskSpace - filesystem.GetTotalUsedSpace();
            int enoughSpaceToRunTheUpdate = spaceForTheUpdate - unusedSpace;

            // Find the smallest directory that, if deleted, would free up enough space on the filesystem to run the update.
            // What is the total size of that directory?
            int output = filesystem
                .GetListOfDirectories()
                .Where(directory => directory.GetSize() >= enoughSpaceToRunTheUpdate)
                .Select(directory => directory.GetSize())
                .Min();

            Console.WriteLine("Solution: {0}.", output);
        }

    }
}
