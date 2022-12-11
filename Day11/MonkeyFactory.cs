namespace AdventOfCode2022.Day11
{
    public static class MonkeyFactory
    {
        public static Monkey Build(string input)
        {
            // Each monkey has several attributes:
            List<string> attributes = input.Split("\r\n").ToList();
            // - Starting items lists your worry level for each item the monkey is currently holding in the order they will be inspected.
            Queue<long> items = BuildItems(attributes[1]);
            // - Operation shows how your worry level changes as that monkey inspects an item.
            Operation operation = BuildOperation(attributes[2]);
            // - Test shows how the monkey uses your worry level to decide where to throw an item next.
            long divisableBy = BuildDivisableBy(attributes[3]);
            // - If true shows what happens with an item if the Test was true.
            int monkeyIfTrue = BuildMonkeyIf(attributes[4]);
            // - If false shows what happens with an item if the Test was false.
            int monkeyIfFalse = BuildMonkeyIf(attributes[5]);

            return new Monkey(items, operation, divisableBy, monkeyIfTrue, monkeyIfFalse);
        }

        private static Queue<long> BuildItems(string startingItems)
        {
            IEnumerable<long> items = SplitAndTakeTheLastPart(startingItems, ": ")
                .Split(", ")
                .Select(long.Parse);
            return new Queue<long>(items);
        }

        private static Operation BuildOperation(string operation) => new (SplitAndTakeTheLastPart(operation, "new = "));
        private static long BuildDivisableBy(string divisableBy) => SplitTakeTheLastPartAndParse(divisableBy, "by ");
        private static int BuildMonkeyIf(string monkeyIf) => SplitTakeTheLastPartAndParse(monkeyIf, "monkey ");

        private static int SplitTakeTheLastPartAndParse(string phrase, string separator) => int.Parse(SplitAndTakeTheLastPart(phrase, separator));
        private static string SplitAndTakeTheLastPart(string phrase, string separator) => phrase.Split(separator).Last();
    }
}
