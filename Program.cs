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
                Console.WriteLine($"{{engParam1: `{nodeId.EngParam1}`, engParam2: `{nodeId.EngParam2}`, engParam3: \"{nodeId.EngParam3}\"}} --> {nodeId}");

                var stringRepresentation = nodeId.ToString();
                NodeId.TryParse(stringRepresentation, out NodeId nodeId2);
                Console.WriteLine($"{nodeId2} --> {{engParam1: `{nodeId2.EngParam1}`, engParam2: `{nodeId2.EngParam2}`, engParam3: \"{nodeId2.EngParam3}\"}}");
                Console.WriteLine();

                System.Diagnostics.Debug.Assert(nodeId == nodeId2);

            }


        }


        static Random random = new Random();

        public static uint randomNumber()
        {

            return (uint)random.Next(100);

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




    public struct NodeId : IEquatable<NodeId>
    {

        public uint EngParam1 { get; }
        public uint EngParam2 { get; }
        public string EngParam3 { get; }

        public NodeId(uint engParam1, uint engParam2, string engParam3)
        {
            this.EngParam1 = engParam1;
            this.EngParam2 = engParam2;
            this.EngParam3 = string.IsNullOrWhiteSpace(engParam3) ? null : engParam3;
        }


        public override string ToString()
        {
            return $"{EngParam1}-{EngParam2}{EngParam3Suffix}";
        }

        /// <summary>
        /// Formats engParam3 for string representation of this NodeId
        /// </summary>
        /// <value>
        /// null if engParam3 is null, else a hyphen followed by engParam3
        /// </value>
        private string EngParam3Suffix
        {
            get
            {
                return EngParam3 == null ? null : $"-{EngParam3}";
            }
        }

        public static bool TryParse(string stringRepresentation, out NodeId id)
        {
            var engParam3 = string.Empty; // default value for engParam3
            id = new NodeId(); // default return value if something goes wrong
            var firstHyphenIndex = stringRepresentation.IndexOf('-');

            if (firstHyphenIndex == -1) return false;

            var secondHyphenIndex = stringRepresentation.IndexOf('-', firstHyphenIndex + 1);

            if (secondHyphenIndex == -1)
            {
                secondHyphenIndex = stringRepresentation.Length;
            }
            else
            {
                engParam3 = stringRepresentation.Substring(secondHyphenIndex + 1, stringRepresentation.Length - secondHyphenIndex - 1);
            }



            if (!uint.TryParse(stringRepresentation.Substring(0, firstHyphenIndex), out uint engParam1)) return false;

            if (!uint.TryParse(stringRepresentation.Substring(firstHyphenIndex + 1, secondHyphenIndex - firstHyphenIndex - 1), out uint engParam2)) return false;

            id = new NodeId(engParam1, engParam2, engParam3);
            return true;

        }

        public override bool Equals(object obj)
        {
            return obj is NodeId id && Equals(id);
        }

        public bool Equals(NodeId other)
        {
            return EngParam1.Equals(other.EngParam1) &&
                   EngParam2.Equals(other.EngParam2) &&
                   EngParam3 == other.EngParam3;
        }

        public override int GetHashCode()
        {
            int hashCode = -1700740666;
            hashCode = hashCode * -1521134295 + EngParam1.GetHashCode();
            hashCode = hashCode * -1521134295 + EngParam2.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EngParam3);
            return hashCode;
        }

        public static bool operator ==(NodeId left, NodeId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NodeId left, NodeId right)
        {
            return !(left == right);
        }
    }


}