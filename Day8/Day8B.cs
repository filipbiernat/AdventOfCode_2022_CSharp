using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day8
{
    public class Day8B : IDay
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

            // What is the highest scenic score possible for any tree?
            int output = Trees
                .Select(tree => CalculateScenicScore(tree.Key, tree.Value))
                .Max();

            Console.WriteLine("Solution: {0}.", output);
        }

        // A tree's scenic score is found by multiplying together its viewing distance in each of the four directions. 
        private int CalculateScenicScore(Coords coords, int height) => CalculateViewingDistanceFromTop(coords, height) *
            CalculateViewingDistanceFromBottom(coords, height) *
            CalculateViewingDistanceFromLeft(coords, height) *
            CalculateViewingDistanceFromRight(coords, height);


        // To measure the viewing distance from a given tree, look up, down, left, and right from that tree;
        // stop if you reach an edge or at the first tree that is the same height or taller than the tree under consideration.
        private int CalculateViewingDistanceFromTop(Coords coords, int height)
        {
            int viewingDistance = 0;
            for (int row = coords.Row - 1; row >= 0; --row)
            {
                ++viewingDistance;
                if (Trees[new Coords(row, coords.Column)] >= height) break;
            }
            return viewingDistance;
        }
        private int CalculateViewingDistanceFromBottom(Coords coords, int height)
        {
            int viewingDistance = 0;
            for (int row = coords.Row + 1; row <= GridEdge.Row; ++row)
            {
                ++viewingDistance;
                if (Trees[new Coords(row, coords.Column)] >= height) break;
            }
            return viewingDistance;
        }
        private int CalculateViewingDistanceFromLeft(Coords coords, int height)
        {
            int viewingDistance = 0;
            for (int column = coords.Column - 1; column >= 0; --column)
            {
                ++viewingDistance;
                if (Trees[new Coords(coords.Row, column)] >= height) break;
            }
            return viewingDistance;
        }
        private int CalculateViewingDistanceFromRight(Coords coords, int height)
        {
            int viewingDistance = 0;
            for (int column = coords.Column + 1; column <= GridEdge.Column; ++column)
            {
                ++viewingDistance;
                if (Trees[new Coords(coords.Row, column)] >= height) break;
            }
            return viewingDistance;
        }
    }
}
