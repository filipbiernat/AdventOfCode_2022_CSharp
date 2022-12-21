using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day21
{
    public class Day21B : IDay
    {
        public void Run()
        {
            // Each monkey is given a job: either to yell a specific number or to yell the result of a math operation.
            List<string> input = File.ReadAllLines(@"..\..\..\Day21\Day21.txt").ToList();
            Dictionary<string, List<string>> yellingMonkeysJobs = input
                .Select(ParseMonkeyInput)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            MonkeyYellingResultOfMathOperation currentYellingMonkey = (MonkeyYellingResultOfMathOperation)
                YellingMonkey.Build(monkeysName: "root", yellingMonkeysJobs);

            do
            {
                if (currentYellingMonkey.GetLhs() is MonkeyYellingSpecificNumber)
                { // Trick: Take the number yelled by LHS and compare with the number yelled by the RHS using equality operator.
                    currentYellingMonkey = new MonkeyYellingResultOfMathOperation(
                        currentYellingMonkey.GetRhs(),
                        currentYellingMonkey.GetLhs(),
                        "==");
                }
                else
                { // Trick: Invert the LHS math operation and compare with the RHS math operation using equality operator.
                    MonkeyYellingResultOfMathOperation currentYellingMonkeylhs = (MonkeyYellingResultOfMathOperation)currentYellingMonkey.GetLhs();
                    YellingMonkey nestedLhs = currentYellingMonkeylhs.GetLhs();
                    YellingMonkey nestedRhs = currentYellingMonkeylhs.GetRhs();

                    (string newInternalOperator, YellingMonkey newInternalRhs, YellingMonkey newExternalLhs) = currentYellingMonkeylhs.GetOperator() switch
                    {
                        "+" => ("-", nestedLhs, nestedRhs),
                        "-" => ("+", nestedRhs, nestedLhs),
                        "*" => ("/", nestedLhs, nestedRhs),
                        _ => ("*", nestedRhs, nestedLhs),
                    };

                    YellingMonkey newInternalMonkey = new MonkeyYellingResultOfMathOperation(
                        currentYellingMonkey.GetRhs(),
                        newInternalRhs,
                        newInternalOperator).ResolveMonkeysNumber();

                    currentYellingMonkey = new MonkeyYellingResultOfMathOperation(
                        newExternalLhs,
                        newInternalMonkey,
                        "==");
                }
            }
            while (currentYellingMonkey.GetLhs() is not YellingHuman);

            MonkeyYellingSpecificNumber monkeyYellingNumberToPassEqualityTest = (MonkeyYellingSpecificNumber) currentYellingMonkey.GetRhs();

            // What number do you yell to pass root's equality test?
            long output = monkeyYellingNumberToPassEqualityTest.GetValue();

            Console.WriteLine("Solution: {0}.", output);
        }

        private KeyValuePair<string, List<string>> ParseMonkeyInput(string monkeyInput)
        {
            string[] splitInput = monkeyInput.Split(new string[] { ": ", " " }, StringSplitOptions.TrimEntries);
            return new(splitInput[0], splitInput[1..].ToList());
        }

        public abstract class YellingMonkey
        {
            public virtual YellingMonkey ResolveMonkeysNumber() => this;


            public static YellingMonkey Build(string monkeysName, Dictionary<string, List<string>> yellingMonkeysJobs)
            {
                if (monkeysName == "root")
                {
                    // First, you got the wrong job for the monkey named root; specifically, you got the wrong math operation.
                    // The correct operation for monkey root should be =, which means that it still listens for two numbers
                    // (from the same two monkeys as before), but now checks that the two numbers match.
                    return new MonkeyYellingResultOfMathOperation(
                        Build(yellingMonkeysJobs[monkeysName][0], yellingMonkeysJobs),
                        Build(yellingMonkeysJobs[monkeysName][2], yellingMonkeysJobs),
                        "==");
                }
                else if (monkeysName == "humn")
                {
                    // Second, you got the wrong monkey for the job starting with humn.
                    // It isn't a monkey - it's you.
                    // Actually, you got the job wrong, too: you need to figure out what number you need to yell so that root's equality check passes.
                    // (The number that appears after humn: in your input is now irrelevant.)
                    return new YellingHuman();
                }

                if (yellingMonkeysJobs[monkeysName].Count > 1)
                {
                    return new MonkeyYellingResultOfMathOperation(
                        Build(yellingMonkeysJobs[monkeysName][0], yellingMonkeysJobs),
                        Build(yellingMonkeysJobs[monkeysName][2], yellingMonkeysJobs),
                        yellingMonkeysJobs[monkeysName][1]);
                }
                else
                {
                    return new MonkeyYellingSpecificNumber(yellingMonkeysJobs[monkeysName][0]);
                }
            }
        }

        public class MonkeyYellingSpecificNumber : YellingMonkey
        {
            private readonly long Value;

            public MonkeyYellingSpecificNumber(string value) => Value = long.Parse(value);
            public MonkeyYellingSpecificNumber(long value) => Value = value;
            public long GetValue() => Value;
        }

        public class MonkeyYellingResultOfMathOperation : YellingMonkey
        {
            private readonly YellingMonkey Lhs, Rhs;
            private readonly string Operator;

            public MonkeyYellingResultOfMathOperation(YellingMonkey lhs, YellingMonkey rhs, string @operator)
            {
                Lhs = lhs;
                Rhs = rhs;
                Operator = @operator;
            }

            public YellingMonkey GetLhs() => Lhs;
            public YellingMonkey GetRhs() => Rhs;
            public string GetOperator() => Operator;

            public override YellingMonkey ResolveMonkeysNumber()
            {
                YellingMonkey lhs = Lhs.ResolveMonkeysNumber();
                YellingMonkey rhs = Rhs.ResolveMonkeysNumber();

                if (lhs is not MonkeyYellingSpecificNumber || rhs is not MonkeyYellingSpecificNumber)
                {
                    return new MonkeyYellingResultOfMathOperation(lhs, rhs, Operator);
                }
                else
                {
                    long lhsValue = ((MonkeyYellingSpecificNumber)lhs).GetValue();
                    long rhsValue = ((MonkeyYellingSpecificNumber)rhs).GetValue();
                    return Operator switch
                    {
                        "==" => new MonkeyYellingSpecificNumber(lhsValue == rhsValue ? 1 : 0),
                        "+" => new MonkeyYellingSpecificNumber(lhsValue + rhsValue),
                        "-" => new MonkeyYellingSpecificNumber(lhsValue - rhsValue),
                        "*" => new MonkeyYellingSpecificNumber(lhsValue * rhsValue),
                        _ => new MonkeyYellingSpecificNumber(lhsValue / rhsValue),
                    };
                }
            }
        }

        public class YellingHuman : YellingMonkey {}
    }
}
