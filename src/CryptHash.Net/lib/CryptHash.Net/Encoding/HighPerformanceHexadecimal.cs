﻿/*
 *      Based on "the retired" unsafe fast implementation
 *      
 *      https://www.codeproject.com/Tips/447938/High-performance-Csharp-byte-array-to-hex-string-t
 */

using CryptHash.Net.Util;
using System;

namespace CryptHash.Net.Encoding
{
    /// <summary>
    /// This class produces the same results as "Hexadecimal.cs" class but uses unsafe code to do it really really faster.
    /// </summary>
    public static unsafe class HighPerformanceHexadecimal
    {
        #region fields

        private static readonly int[] _toHexTable = new int[] {
            3145776, 3211312, 3276848, 3342384, 3407920, 3473456, 3538992, 3604528, 3670064, 3735600,
            4259888, 4325424, 4390960, 4456496, 4522032, 4587568, 3145777, 3211313, 3276849, 3342385,
            3407921, 3473457, 3538993, 3604529, 3670065, 3735601, 4259889, 4325425, 4390961, 4456497,
            4522033, 4587569, 3145778, 3211314, 3276850, 3342386, 3407922, 3473458, 3538994, 3604530,
            3670066, 3735602, 4259890, 4325426, 4390962, 4456498, 4522034, 4587570, 3145779, 3211315,
            3276851, 3342387, 3407923, 3473459, 3538995, 3604531, 3670067, 3735603, 4259891, 4325427,
            4390963, 4456499, 4522035, 4587571, 3145780, 3211316, 3276852, 3342388, 3407924, 3473460,
            3538996, 3604532, 3670068, 3735604, 4259892, 4325428, 4390964, 4456500, 4522036, 4587572,
            3145781, 3211317, 3276853, 3342389, 3407925, 3473461, 3538997, 3604533, 3670069, 3735605,
            4259893, 4325429, 4390965, 4456501, 4522037, 4587573, 3145782, 3211318, 3276854, 3342390,
            3407926, 3473462, 3538998, 3604534, 3670070, 3735606, 4259894, 4325430, 4390966, 4456502,
            4522038, 4587574, 3145783, 3211319, 3276855, 3342391, 3407927, 3473463, 3538999, 3604535,
            3670071, 3735607, 4259895, 4325431, 4390967, 4456503, 4522039, 4587575, 3145784, 3211320,
            3276856, 3342392, 3407928, 3473464, 3539000, 3604536, 3670072, 3735608, 4259896, 4325432,
            4390968, 4456504, 4522040, 4587576, 3145785, 3211321, 3276857, 3342393, 3407929, 3473465,
            3539001, 3604537, 3670073, 3735609, 4259897, 4325433, 4390969, 4456505, 4522041, 4587577,
            3145793, 3211329, 3276865, 3342401, 3407937, 3473473, 3539009, 3604545, 3670081, 3735617,
            4259905, 4325441, 4390977, 4456513, 4522049, 4587585, 3145794, 3211330, 3276866, 3342402,
            3407938, 3473474, 3539010, 3604546, 3670082, 3735618, 4259906, 4325442, 4390978, 4456514,
            4522050, 4587586, 3145795, 3211331, 3276867, 3342403, 3407939, 3473475, 3539011, 3604547,
            3670083, 3735619, 4259907, 4325443, 4390979, 4456515, 4522051, 4587587, 3145796, 3211332,
            3276868, 3342404, 3407940, 3473476, 3539012, 3604548, 3670084, 3735620, 4259908, 4325444,
            4390980, 4456516, 4522052, 4587588, 3145797, 3211333, 3276869, 3342405, 3407941, 3473477,
            3539013, 3604549, 3670085, 3735621, 4259909, 4325445, 4390981, 4456517, 4522053, 4587589,
            3145798, 3211334, 3276870, 3342406, 3407942, 3473478, 3539014, 3604550, 3670086, 3735622,
            4259910, 4325446, 4390982, 4456518, 4522054, 4587590
        };

        private static readonly byte[] _fromHexTable = new byte[] {
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 0, 1,
            2, 3, 4, 5, 6, 7, 8, 9, 255, 255,
            255, 255, 255, 255, 255, 10, 11, 12, 13, 14,
            15, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 10, 11, 12,
            13, 14, 15
        };

        private static readonly byte[] _fromHexTable16 = new byte[] {
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 0, 16,
            32, 48, 64, 80, 96, 112, 128, 144, 255, 255,
            255, 255, 255, 255, 255, 160, 176, 192, 208, 224,
            240, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 160, 176, 192,
            208, 224, 240
        };

        #endregion fields


        #region public methods

        public static string ToHexString(string plainString, bool useHexIndicatorPrefix = false)
        {
            if (string.IsNullOrWhiteSpace(plainString))
            {
                return null;
            }

            var plainStringBytes = System.Text.Encoding.UTF8.GetBytes(plainString);

            return ToHexString(plainStringBytes, useHexIndicatorPrefix);
        }

        public static string ToHexString(byte[] byteArray, bool useHexIndicatorPrefix = false)
        {
            fixed (int* hexRef = _toHexTable)
            fixed (byte* sourceRef = byteArray)
            {
                var s = sourceRef;
                var resultLen = (byteArray.Length << 1);

                if (useHexIndicatorPrefix)
                {
                    resultLen += 2;
                }

                var result = new string(' ', resultLen);

                fixed (char* resultRef = result)
                {
                    var pair = (int*)resultRef;

                    if (useHexIndicatorPrefix)
                    {
                        *pair++ = 7864368;
                    }

                    while (*pair != 0)
                    {
                        *pair++ = hexRef[*s++];
                    }

                    return result;
                }
            }
        }

        public static string ToString(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
            {
                return null;
            }

            var byteArray = ToByteArray(hexString);

            return System.Text.Encoding.UTF8.GetString(byteArray);
        }

        public static byte[] ToByteArray(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
            {
                return null;
            }

            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(MessageDictionary.Instance["Common.IncorrectHexadecimalString"], nameof(hexString));
            }

            int index = 0, len = hexString.Length >> 1;

            fixed (char* sourceRef = hexString)
            {
                if (*(int*)sourceRef == 7864368)
                {
                    if (hexString.Length == 2)
                    {
                        throw new ArgumentException();
                    }

                    index += 2;
                    len -= 1;
                }

                byte add = 0;
                var result = new byte[len];
                fixed (byte* hiRef = _fromHexTable16)
                fixed (byte* lowRef = _fromHexTable)
                fixed (byte* resultRef = result)
                {
                    var s = &sourceRef[index];
                    var r = resultRef;

                    while (*s != 0)
                    {
                        if (*s > 102 || (*r = hiRef[*s++]) == 255 || *s > 102 || (add = lowRef[*s++]) == 255)
                        {
                            throw new ArgumentException();
                        }

                        *r++ += add;
                    }

                    return result;
                }
            }
        }

        #endregion public methods
    }
}
