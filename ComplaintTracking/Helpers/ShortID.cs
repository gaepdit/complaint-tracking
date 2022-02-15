using System;
using System.Text;

namespace ComplaintTracking
{
    public static class ShortID
    {
        private static readonly Random Random = new();
        private const string Pool = "ABCDEFGHKMNPQRSTUVWXYZ2345689";

        public static string GetShortID(int length = 4)
        {
            var poolLength = Pool.Length;
            var output = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                output.Append(Pool[Random.Next(0, poolLength)]);
            }

            return output.ToString();
        }
    }
}
