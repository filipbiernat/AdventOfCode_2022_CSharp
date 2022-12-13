using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;

namespace AdventOfCode2022.Day13
{
    public class Day13B : IDay
    {
        public void Run()
        {
            // Your handheld device must still not be working properly; the packets from the distress signal got decoded out of order.
            // You'll need to re-order the list of received packets (your puzzle input) to decode the message.
            List<string> input = File.ReadAllLines(@"..\..\..\Day13\Day13.txt").ToList();

            // Disregard the blank lines in your list of received packets.
            input.RemoveAll(entry => string.IsNullOrEmpty(entry));

            // The distress signal protocol also requires that you include two additional divider packets: [[2]], [[6]].
            List<JsonNode> dividerPackets = ParsePackets(new List<string> { "[[2]]", "[[6]]" });
            List<JsonNode> packets = ParsePackets(input)
                .Concat(dividerPackets)
                .ToList();

            // Organize all of the packets into the correct order.
            packets.Sort(ComparePackets);

            // Afterward, locate the divider packets.
            // To find the decoder key for this distress signal, you need to determine the indices of the two divider packets and multiply them together. 
            // What is the decoder key for the distress signal?
            int output = packets
                .Select((packet, index) => dividerPackets.Contains(packet) ? index + 1 : 0)
                .Where(index => index != 0)
                .Aggregate((lhs, rhs) => lhs * rhs);

            Console.WriteLine("Solution: {0}.", output);
        }

        // Packet data consists of lists and integers. Each list starts with [, ends with ], and contains zero or more comma-separated values
        // (either integers or other lists). Each packet is always a list and appears on its own line.
        private static List<JsonNode> ParsePackets(List<string> packets) => packets
            .Select(packet => JsonNode.Parse(packet) ?? 0)
            .ToList();

        private static int ComparePackets(JsonNode left, JsonNode right)
        {
            // If both values are integers, the lower integer should come first.
            // If the left integer is lower than the right integer, the inputs are in the right order.
            if (left is JsonValue && right is JsonValue)
            {
                if ((int)left < (int)right) return -1;
                if ((int)left > (int)right) return 1;
                return 0;
            }

            // If exactly one value is an integer, convert the integer to a list which contains that integer as its only value, then retry the comparison.
            JsonArray leftList = left is JsonValue ? new JsonArray((int)left) : (JsonArray)(left);
            JsonArray rightList = right is JsonValue ? new JsonArray((int)right) : (JsonArray)(right);

            // If both values are lists, compare the first value of each list, then the second value, and so on.
            for (int index = 0; ; ++index)
            {
                // If the lists are the same length and no comparison makes a decision about the order, continue checking the next part of the input.
                if (index >= leftList.Count && index >= rightList.Count) return 0;
                // If the left list runs out of items first, the inputs are in the right order.
                if (index >= leftList.Count) return -1;
                // If the right list runs out of items first, the inputs are not in the right order.
                if (index >= rightList.Count) return 1;

                int result = ComparePackets(GetNode(leftList, index), GetNode(rightList, index));
                if (result != 0) return result;
            }
        }

        private static JsonNode GetNode(JsonArray list, int index) => list[index] ?? throw new NullReferenceException("Unexpected null JsonNode");
    }
}
