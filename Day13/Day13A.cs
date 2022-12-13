using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;

namespace AdventOfCode2022.Day13
{
    public class Day13A : IDay
    {
        public enum Result { True, Continue, False };

        public void Run()
        {
            // Your handheld device must still not be working properly; the packets from the distress signal got decoded out of order.
            // You'll need to re-order the list of received packets (your puzzle input) to decode the message.
            List<string> input = File.ReadAllText(@"..\..\..\Day13\Day13.txt").Split("\r\n\r\n").ToList();

            // Your list consists of pairs of packets; pairs are separated by a blank line.
            IEnumerable<List<JsonNode?>> pairsOfPackets = input.Select(ParsePairOfPackets);

            // What are the indices of the pairs that are already in the right order?
            // Determine which pairs of packets are already in the right order. What is the sum of the indices of those pairs?
            int output = pairsOfPackets
                .Select((pairOfPackets, index) => IsPairOfPacketsInTheRightOrder(pairOfPackets) ? index + 1 : 0)
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }

        // Packet data consists of lists and integers. Each list starts with [, ends with ], and contains zero or more comma-separated values
        // (either integers or other lists). Each packet is always a list and appears on its own line.
        private static List<JsonNode?> ParsePairOfPackets(string pairOfPackets) => pairOfPackets
            .Split("\r\n")
            .Select(list => JsonNode.Parse(list))
            .ToList();

        private static bool IsPairOfPacketsInTheRightOrder(List<JsonNode?> pairOfPackets)
        {
            // When comparing two values, the first value is called left and the second value is called right.
            JsonNode left = GetNode(pairOfPackets, 0);
            JsonNode right = GetNode(pairOfPackets, 1);
            return IsPairOfPacketsInTheRightOrder(left, right) == Result.True;
        }

        private static Result IsPairOfPacketsInTheRightOrder(JsonNode left, JsonNode right)
        {
            // If both values are integers, the lower integer should come first.
            // If the left integer is lower than the right integer, the inputs are in the right order.
            if (left is JsonValue && right is JsonValue)
            {
                if ((int)left < (int)right) return Result.True;
                if ((int)left > (int)right) return Result.False;
                return Result.Continue;
            }

            // If exactly one value is an integer, convert the integer to a list which contains that integer as its only value, then retry the comparison.
            JsonArray leftList = left is JsonValue ? new JsonArray((int)left) : (JsonArray)(left);
            JsonArray rightList = right is JsonValue ? new JsonArray((int)right) : (JsonArray)(right);

            // If both values are lists, compare the first value of each list, then the second value, and so on.
            for (int index = 0; ; ++index)
            {
                // If the lists are the same length and no comparison makes a decision about the order, continue checking the next part of the input.
                if (index >= leftList.Count && index >= rightList.Count) return Result.Continue;
                // If the left list runs out of items first, the inputs are in the right order.
                if (index >= leftList.Count) return Result.True;
                // If the right list runs out of items first, the inputs are not in the right order.
                if (index >= rightList.Count) return Result.False;

                Result result = IsPairOfPacketsInTheRightOrder(GetNode(leftList, index), GetNode(rightList, index));
                if (result != Result.Continue) return result;
            }
        }

        private static JsonNode GetNode(List<JsonNode?> list, int index) => list[index] ?? throw new NullReferenceException("Unexpected null JsonNode");
        private static JsonNode GetNode(JsonArray list, int index) => list[index] ?? throw new NullReferenceException("Unexpected null JsonNode");
    }
}
