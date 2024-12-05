using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class SimpleRandomNumberGenerator
    {
        private static readonly Random _simpleGenerator = new Random();
        public static int SimpleNumberBetween(int minValue, int maxValue)
        {
            return _simpleGenerator.Next(minValue, maxValue + 1);
        }
    }
}
