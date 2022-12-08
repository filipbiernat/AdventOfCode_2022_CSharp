using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day8
{
    public class Day8A : IDay
    {
        // Each tree is represented as a single digit whose value is its height, where 0 is the shortest and 9 is the tallest.
        private Dictionary<Coords, int> Trees = new();
        private Coords GridEdge = new(0, 0);

        public void Run()
        {
            // First, determine whether there is enough tree cover here to keep a tree house hidden.
            // To do this, you need to count the number of trees that are visible from outside the grid when looking directly along a row or column.
            // The Elves have already launched a quadcopter to generate a map with the height of each tree (your puzzle input). 
            List<string> input = File.ReadAllLines(@"..\..\..\Day8\Day8.txt").ToList();

            // Each tree is represented as a single digit whose value is its height, where 0 is the shortest and 9 is the tallest.
            Trees = input
                .SelectMany((row, rowIndex) => row
                    .ToCharArray()
                    .Select((elem, colIndex) => new KeyValuePair<Coords, int>(
                        new Coords(rowIndex, colIndex), (int)char.GetNumericValue(elem))))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            GridEdge = Trees.Last().Key;

            // How many trees are visible from outside the grid?
            int output = Trees
                .Where(tree => IsTreeVisible(tree.Key, tree.Value))
                .Count();

            Console.WriteLine("Solution: {0}.", output);
        }

        private bool IsTreeVisible(Coords coords, int height) =>
            IsTreeOnTheEdge(coords) ||
            IsTreeVisibleFromTop(coords, height) ||
            IsTreeVisibleFromBottom(coords, height) ||
            IsTreeVisibleFromLeft(coords, height) ||
            IsTreeVisibleFromRight(coords, height);

        // All of the trees around the edge of the grid are visible - since they are already on the edge, there are no trees to block the view.
        private bool IsTreeOnTheEdge(Coords coords) =>
            coords.Row == 0 ||
            coords.Column == 0 ||
            coords.Row == GridEdge.Row ||
            coords.Column == GridEdge.Column;

        // A tree is visible if all of the other trees between it and an edge of the grid are shorter than it.
        private bool IsTreeVisibleFromTop(Coords coords, int height) => Enumerable
            .Range(0, coords.Row)
            .Select(row => new Coords(row, coords.Column))
            .All(coords => Trees[coords] < height);
        private bool IsTreeVisibleFromBottom(Coords coords, int height) => Enumerable
            .Range(coords.Row + 1, GridEdge.Row - coords.Row)
            .Select(row => new Coords(row, coords.Column))
            .All(coords => Trees[coords] < height);
        private bool IsTreeVisibleFromLeft(Coords coords, int height) => Enumerable
            .Range(0, coords.Column)
            .Select(column => new Coords(coords.Row, column))
            .All(coords => Trees[coords] < height);
        private bool IsTreeVisibleFromRight(Coords coords, int height) => Enumerable
            .Range(coords.Column + 1, GridEdge.Column - coords.Column)
            .Select(column => new Coords(coords.Row, column))
            .All(coords => Trees[coords] < height);
    }
}
