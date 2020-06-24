using System;
using System.Collections.Generic;
using System.Text;

namespace Welch
{
    class MainClass
    {
        public static void Main(string[] args)
        {


            while (true)
            {
                var engParam1 = randomNumber();
                var engParam2 = randomNumber();
                var engParam3 = randomString();

                var nodeId = new NodeId(engParam1, engParam2, engParam3);
                Console.WriteLine(nodeId);

                var stringRepresentation = nodeId.ToString();
                NodeId.TryParse(stringRepresentation, out NodeId nodeId2);
                Console.WriteLine($"engParam1: {nodeId2.EngParam1}, engParam2: {nodeId2.EngParam2}, engParam3: {nodeId2.EngParam3}");
            }


        }


        static Random random = new Random();

        public static int? randomNumber()
        {
            // 25% chance of null number.
            if (random.Next(4) == 0) return null;

            return random.Next(100);

        }

        public static string randomString()
        {
            // 25% chance of null string
            if (random.Next(4) == 0) return null;

            return $"{randomChar()}{randomChar()}{randomChar()}{randomChar()}{randomChar()}{randomChar()}";

        }

        public static char randomChar()
        {
            // 25% of the time a `-` char is returned
            if (random.Next(4) == 0) return '-';

            var nextChar = random.Next(26) + 65;
            return Convert.ToChar(nextChar);
        }
    }




    public struct NodeId
    {
        NumericParam engParam1;
        NumericParam engParam2;
        string engParam3;


        private struct NumericParam
        {
            private static string MissingRepresentation = "N";
            public static NumericParam Missing = new NumericParam();

            int? value;

            public int? Value => this.value;

            public NumericParam(int? value)
            {
                this.value = value;
            }

            public override string ToString()
            {
                if (value.HasValue) return value.ToString();
                return MissingRepresentation;
            }

            public static bool TryParse(string stringRepresentation, out NumericParam param)
            {
                if (int.TryParse(stringRepresentation, out int engParam))
                {
                    param = new NumericParam(engParam);
                    return true;
                }

                if (stringRepresentation == "N")
                {
                    param = Missing;
                    return true;
                }

                param = new NumericParam();
                return false;

            }
        }

        public int? EngParam1 => engParam1.Value;
        public int? EngParam2 => engParam2.Value;
        public string EngParam3 => engParam3;

        public NodeId(int? engParam1, int? engParam2, string engParam3)
        {
            this.engParam1 = new NumericParam(engParam1);
            this.engParam2 = new NumericParam(engParam2);
            this.engParam3 = engParam3;
        }

        private NodeId(NumericParam engParam1, NumericParam engParam2, string engParam3)
        {
            this.engParam1 = engParam1;
            this.engParam2 = engParam2;
            this.engParam3 = engParam3;
        }

        public override string ToString()
        {
            return $"{engParam1}-{engParam2}{EngParam3Suffix}";
        }

        /// <summary>
        /// Formats engParam3 for string representation of this NodeId
        /// </summary>
        /// <value>
        /// null if engParam3 is null, else a hypen followed by engParam3
        /// </value>
        private string EngParam3Suffix
        {
            get
            {
                return engParam3 == null ? null : $"-{engParam3}";
            }
        }

        public static bool TryParse(string stringRepresentation, out NodeId id)
        {
            var reader = new System.IO.StringReader(stringRepresentation);

            // Use a separate builder for each engineering parameter
            var bldr1 = new System.Text.StringBuilder();
            var bldr2 = new System.Text.StringBuilder();
            var bldr3 = new System.Text.StringBuilder();
            var stack = new Stack<System.Text.StringBuilder>(new System.Text.StringBuilder[] { bldr3, bldr2, bldr1 });

            var bldr = stack.Pop();

            int nextChar;
            while ((nextChar = reader.Read()) != -1)
            {
                // We only pop the stack at most two times in this loop. Every `-` after the second
                // gets appended to bldr3
                if (nextChar == '-' && !(stack.Count == 0))
                {
                    bldr = stack.Pop();
                }
                else
                {
                    bldr.Append(Convert.ToChar(nextChar));
                }
            }

            if (NumericParam.TryParse(bldr1.ToString(), out NumericParam engParam1)
                && NumericParam.TryParse(bldr2.ToString(), out NumericParam engParam2))
            {
                id = new NodeId(engParam1, engParam2, bldr3.ToString());
                return true;
            }

            id = new NodeId();
            return false;
        }

    }


}