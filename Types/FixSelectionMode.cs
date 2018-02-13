/*
MIT License

Copyright (c) 2018 Inverness Park Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
namespace InvernessPark.Utilities.NMEA.Types {

    /// <summary>
    /// NMEA-0183 GPS Fix SMode
    /// </summary>
    public enum FixSelectionMode {
        Auto,
        Manual
    }

    /// <summary>
    /// Extensions use to v=convert to/from NMEA representation and enum
    /// </summary>
    public static class FixSelectionModeExtensions {

        /// <summary>
        /// From enum to NMEA representation
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string ToNmeaString(this FixSelectionMode mode) {
            switch (mode) {
                case FixSelectionMode.Auto:
                    return "A";
                case FixSelectionMode.Manual:
                    return "M";
                default:
                    throw new ArgumentException(nameof(mode));
            }
        }

        /// <summary>
        /// From NMEA representation to enum
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static FixSelectionMode FromNmeaString(string s) {
            switch (s.Trim().ToUpper()) {
                case "A":
                    return FixSelectionMode.Auto;
                case "M":
                    return FixSelectionMode.Manual;
                default:
                    throw new ArgumentException(nameof(s));
            }
        }
    }
}
