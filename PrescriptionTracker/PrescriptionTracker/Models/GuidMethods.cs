using System;
using System.Security.Cryptography;

namespace PrescriptionDrugTracker.Models
{
    public static class GuidMethods
    {
        static int IntPow(int x, uint pow)
        {
            int ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
        }

        public static Guid makeId(int largePart)
        {
            string first8 = largePart.ToString("X8");
            string second4 = RandomNumberGenerator.GetInt32(IntPow(2, 12), IntPow(2, 16)).ToString("X");
            string third4 = RandomNumberGenerator.GetInt32(IntPow(2, 12), IntPow(2, 16)).ToString("X");
            string fourth4 = RandomNumberGenerator.GetInt32(IntPow(2, 12), IntPow(2, 16)).ToString("X");
            string last12Part1 = RandomNumberGenerator.GetInt32(IntPow(2, 20), IntPow(2, 24)).ToString("X");
            string last12Part2 = RandomNumberGenerator.GetInt32(IntPow(2, 20), IntPow(2, 24)).ToString("X");
            string last12 = last12Part1 + last12Part2;
            string guidString = $"{first8}-{second4}-{third4}-{fourth4}-{last12}";
            return new Guid(guidString);
        }
    }
}
