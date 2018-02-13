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
    /// GPS Status (NMEA-0183)
    /// </summary>
    public enum StatusEnum {
        Active,
        Void
    }

    /// <summary>
    /// Extensions used to parse the NMEA representaiton of GPS status in NEMA messages
    /// </summary>
    public static class StatusEnumExtensions {

        /// <summary>
        /// From enum to the NMEA representation
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string ToNmeaString(this StatusEnum mode) {
            switch (mode) {
                case StatusEnum.Active:
                    return "A";
                case StatusEnum.Void:
                    return "V";
                default:
                    throw new ArgumentException(nameof(mode));
            }
        }

        /// <summary>
        /// From NMEA representation to enum
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static StatusEnum FromNmeaString(string s) {
            switch (s.Trim().ToUpper()) {
                case "A":
                    return StatusEnum.Active;
                case "V":
                    return StatusEnum.Void;
                default:
                    throw new ArgumentException(nameof(s));
            }
        }
    }
}
