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
    /// Specialization of GeAngle when used for Latitudes
    /// </summary>
    public class Latitude : GeoAngle {

        const double HALF_PI = Math.PI / 2;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Latitude() {
        }

        /// <summary>
        /// Return value in radians; setter adds a value range sanity check 
        /// </summary>
        public override double Radians {
            get {
                return base.Radians;
            }
            set {
                if (value < -HALF_PI || value > HALF_PI) {
                    throw new ArgumentOutOfRangeException(nameof(Radians));
                }
                base.Radians = value;
            }
        }

        /// <summary>
        /// Return value in degrees; setter adds a value range sanity check
        /// </summary>
        public override double Degrees {
            get {
                return base.Degrees;
            }
            set {
                if (value < -90 || value > 90) {
                    throw new ArgumentOutOfRangeException(nameof(Degrees));
                }
                base.Degrees = value;
            }
        }

        /// <summary>
        /// Parses a string representation of latitude
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fmt"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Latitude Parse(string s, Format fmt, FormatOptions options) {
            return new Latitude() { Degrees = ParseDegrees(s, fmt, options) };
        }

        /// <summary>
        /// Implements string representation of DMM for longitudes
        /// </summary>
        /// <param name="wholeDegrees"></param>
        /// <param name="decimalMinutes"></param>
        /// <param name="sign"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected override string ToStringDMM(int wholeDegrees, double decimalMinutes, int sign, FormatOptions options) {
            string rc = null;
            char letter = sign < 0 ? 'S' : 'N';

            if (options == FormatOptions.Compact) {
                rc = string.Format("{0:00}{1:00.0000},{2}", wholeDegrees, decimalMinutes, letter);
            }
            else {
                rc = string.Format("{0}{1} {2:0.######}'", sign * wholeDegrees, Strings.degrees, decimalMinutes);
            }
            return rc;
        }

        /// <summary>
        /// Implements string representation of DMS for longitudes
        /// </summary>
        /// <param name="wholeDegrees"></param>
        /// <param name="decimalMinutes"></param>
        /// <param name="sign"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected override string ToStringDMS(int wholeDegrees, int wholeMinutes, double decimalSeconds, int sign, FormatOptions options) {
            string rc = null;
            char letter = sign < 0 ? 'S' : 'N';

            if (options == FormatOptions.Compact) {
                rc = string.Format("{0:00}{1:00}{2:00.0000},{3}", wholeDegrees, wholeMinutes, decimalSeconds, letter);
            }
            else {
                rc = string.Format("{0}{1} {2}' {3:0.####} {4}\"", wholeDegrees, Strings.degrees, wholeMinutes, decimalSeconds, letter);
            }
            return rc;
        }

        /// <summary>
        /// Implicit conversion from numeric value to Longitude
        /// </summary>
        /// <param name="degrees"></param>
        public static implicit operator Latitude(double degrees) {
            Latitude rc = new Latitude();
            rc.Degrees = degrees;
            return rc;
        }
    }
}
