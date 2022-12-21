using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day21
{
    public class Day21A : IDay
    {
        public void Run()
        {
            // Each monkey is given a job: either to yell a specific number or to yell the result of a math operation.
            List<string> input = File.ReadAllLines(@"..\..\..\Day21\Day21.txt").ToList();
            Dictionary<string, List<string>> yellingMonkeysJobs = input
                .Select(ParseMonkeyInput)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            // Your job is to work out the number the monkey named root will yell before the monkeys figure it out themselves.
            MonkeyYellingSpecificNumber rootYellingMonkey = (MonkeyYellingSpecificNumber)
                YellingMonkey.Build(monkeysName: "root", yellingMonkeysJobs).ResolveMonkeysNumber();

            // What number will the monkey named root yell?
            long output = rootYellingMonkey.GetValue();

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
                        "+" => new MonkeyYellingSpecificNumber(lhsValue + rhsValue),
                        "-" => new MonkeyYellingSpecificNumber(lhsValue - rhsValue),
                        "*" => new MonkeyYellingSpecificNumber(lhsValue * rhsValue),
                        _ => new MonkeyYellingSpecificNumber(lhsValue / rhsValue),
                    };
                }
            }
        }
    }
}