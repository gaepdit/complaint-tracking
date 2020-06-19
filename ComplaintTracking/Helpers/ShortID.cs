using System;

namespace ComplaintTracking
{
    public static class ShortID
    {
        private static readonly Random _random = new Random();
        private static readonly string _pool = "ABCDEFGHKMNPQRSTUVWXYZ2345689";

        public static string GetShortID(int length = 4)
        {
            int poolLength = _pool.Length;
            string output = string.Empty;

            for (int i = 0; i < length; i++)
            {
                output += _pool[_random.Next(0, poolLength)];
            }

            return output;
        }
    }
}