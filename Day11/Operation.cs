namespace AdventOfCode2022.Day11
{
    // Operation shows how your worry level changes as that monkey inspects an item.
    public class Operation
    {
        private readonly bool IsLeftOperandOld;
        private readonly bool IsRightOperandOld;
        private readonly long LeftOperand;
        private readonly long RightOperand;
        private readonly char Operator;

        public Operation(string input)
        {
            string[] splitInput = input.Split(" ");

            IsLeftOperandOld = IsOperandOld(splitInput[0]);
            if (!IsLeftOperandOld)
                LeftOperand = long.Parse(splitInput[0]);

            IsRightOperandOld = IsOperandOld(splitInput[2]);
            if (!IsRightOperandOld)
                RightOperand = long.Parse(splitInput[2]);

            Operator = splitInput[1].First();
        }

        public long Apply(long old)
        {
            long leftOperand = IsLeftOperandOld ? old : LeftOperand;
            long rightOperand = IsRightOperandOld ? old : RightOperand;
            if (Operator == '*') return leftOperand * rightOperand;
            if (Operator == '+') return leftOperand + rightOperand;
            throw new ArgumentOutOfRangeException("Unsupported operator: " + Operator);
        }

        private static bool IsOperandOld(string operand) => operand == "old";
    }
}
