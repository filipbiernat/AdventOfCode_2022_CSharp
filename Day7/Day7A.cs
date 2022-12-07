using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day7
{
    public class Day7A : IDay
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

            // Find all of the directories with a total size of at most 100000.
            // What is the sum of the total sizes of those directories?
            int output = filesystem
                .GetListOfDirectories()
                .Where(directory => directory.GetSize() <= 100000)
                .Select(directory => directory.GetSize())
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
