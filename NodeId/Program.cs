using System;

namespace NodeId
{
    class MainClass
    {
        public static void Main(string[] args)
        {


            while(true)
            {
                var engParam1 = randomNumber();
                var engParam2 = randomNumber();
                var engParam3 = randomString();

                Console.WriteLine(GenerateId(engParam1, engParam2, engParam3));
            }


        }

        public static string GenerateId(int? engParam1, int? engParam2, string engParam3)
        {
            var idSuffix = engParam3 == null ? null : $"-{engParam3}";
            return $"{engParam1?.ToString() ?? "N"}-{engParam2?.ToString() ?? "N"}{idSuffix}";
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
}
