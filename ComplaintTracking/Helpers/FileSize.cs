using System;
using JetBrains.Annotations;

namespace ComplaintTracking
{
    public static class FileSize
    {
        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private enum FileSizeUnits
        {
            bytes = 0,
            KB = 1,
            MB = 2,
            GB = 3,
            TB = 4,
            PB = 5,
            EB = 6
        }

        public static string ToFileSizeString(this long value, int precision = 2)
        {
            var pow = Math.Floor((value > 0 ? Math.Log(value) : 0) / Math.Log(1024));
            pow = Math.Min(pow, Enum.GetNames(typeof(FileSizeUnits)).Length); // Total number of FileSizeUnits available
            var valueString = (value / Math.Pow(1024, pow)).ToString(pow == 0 ? "N0" : "N" + precision);
            var unitString = ((FileSizeUnits) (int) pow).ToString();
            return valueString + " " + unitString;
        }
    }
}
