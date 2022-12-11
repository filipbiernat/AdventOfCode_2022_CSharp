namespace AdventOfCode2022.Day11
{
    public class Monkey
    {
        // Each monkey has several attributes:
        // - Starting items lists your worry level for each item the monkey is currently holding in the order they will be inspected.
        private readonly Queue<long> Items;
        // - Operation shows how your worry level changes as that monkey inspects an item.
        private readonly Operation Operation;
        // - Test shows how the monkey uses your worry level to decide where to throw an item next. 
        private readonly long DivisableBy;
        // - If true shows what happens with an item if the Test was true.
        private readonly int MonkeyIfTrue;
        // - If false shows what happens with an item if the Test was false.
        private readonly int MonkeyIfFalse;

        // Count the total number of times each monkey inspects items.
        private long InspectionCounter = 0;

        public Monkey(Queue<long> items, Operation operation, long divisableBy, int monkeyIfTrue, int monkeyIfFalse)
        {
            Items = items;
            Operation = operation;
            DivisableBy = divisableBy;
            MonkeyIfTrue = monkeyIfTrue;
            MonkeyIfFalse = monkeyIfFalse;
        }

        public void CatchItem(long item) => Items.Enqueue(item);
        public long GetInspectionCounter() => InspectionCounter;
        public long GetDivisableBy() => DivisableBy;

        // The monkeys take turns inspecting and throwing items.
        // On a single monkey's turn, it inspects and throws all of the items it is holding one at a time and in the order listed.
        public void TakeTurn(ref List<Monkey> monkeys, Func<long, long> afterInspection)
        {
            while (Items.Count > 0)
            {
                long item = Items.Dequeue();
                item = InspectItem(item);
                item = afterInspection(item);
                ThrowItem(item, ref monkeys);
            }
        }

        private long InspectItem(long item)
        {
            ++InspectionCounter;
            return Operation.Apply(item);
        }

        private void ThrowItem(long item, ref List<Monkey> monkeys)
        {
            int receipientMonkey = item % DivisableBy == 0 ? MonkeyIfTrue : MonkeyIfFalse;
            monkeys[receipientMonkey].CatchItem(item);
        }
    }
}
