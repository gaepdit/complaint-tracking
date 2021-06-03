using System;
using System.Text;

namespace ComplaintTracking
{
    public static class ShortID
    {
        private static readonly Random _random = new();
        private const string _pool = "ABCDEFGHKMNPQRSTUVWXYZ2345689";

        public static string GetShortID(int length = 4)
        {
            var poolLength = _pool.Length;
            var output = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                output.Append(_pool[_random.Next(0, poolLength)]);
            }

            return output.ToString();
        }
    }
}
