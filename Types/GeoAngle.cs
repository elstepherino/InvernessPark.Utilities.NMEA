﻿/*
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
    /// Base wrapper class for geographic coordinate angles (e.g. Latitude, Longitude)
    /// that provides useful conversions between degrees (internal representaiton of 
    /// the angle value), to any of the 3 most ccommon string representaitons: DDD, DMM, 
    /// DMS.
    /// 
    /// Conversion to/from Radians is also provided, not that it's used anywhere.
    /// 
    /// </summary>
    public abstract class GeoAngle {

        /// <summary>
        /// Delimiters to be used when parsing a string representation
        /// </summary>
        static readonly char[] DELIMS = new char[] { ',', ' ', '\'', '"', Strings.degrees[0] };

        /// <summary>
        /// Paceholder for the angle value in degrees
        /// </summary>
        private double _degrees;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GeoAngle() {
            _degrees = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="degrees"></param>
        public GeoAngle( double degrees ) {
            _degrees = degrees;
        }

        /// <summary>
        /// Accessor for degrees
        /// </summary>
        public virtual double Degrees {
            get {
                return _degrees;
            }
            set {
                _degrees = value;
            }
        }

        /// <summary>
        /// Accessor for radians representation
        /// </summary>
        public virtual double Radians {
            get {
                return Degrees * Math.PI / 180.0 ;
            }
            set {
                Degrees = value * 180.0 / Math.PI ;
            }
        }

        /// <summary>
        /// Stringify, with format
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public string ToString(GeoAngleFormat fmt, GeoAngleFormatOptions options = GeoAngleFormatOptions.ShowUnits) {
            string rc = null;
            switch (fmt) {
                default:
                    throw new ArgumentException("Unsupported format: " + fmt);

                case GeoAngleFormat.DDD:
                    if (options == GeoAngleFormatOptions.Compact) {
                        rc = Degrees.ToString("0.0000000") ;
                    }
                    else {
                        rc = Degrees.ToString("0.0000000") + Strings.degrees;
                    }
                    break;

                case GeoAngleFormat.DMM: {
                        DMMComponents dmmComponents = new DMMComponents(Degrees);
                        rc = ToStringDMM(dmmComponents, options);
                    }
                    break;

                case GeoAngleFormat.DMS: {
                        DMSComponents dmsComponents = new DMSComponents(Degrees);
                        rc = ToStringDMS(dmsComponents, options);
                    }
                    break;
            }
            return rc;
        }

        /// <summary>
        /// Converts a string representation back to numeric degrees
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fmt"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected static double ParseDegrees(string s, GeoAngleFormat fmt, GeoAngleFormatOptions options) {
            double rc = 0;

            switch (fmt) {
                case GeoAngleFormat.DDD:
                    rc = double.Parse(s);
                    break;
                case GeoAngleFormat.DMM:
                    rc = ParseDMM(s, options);
                    break;
                case GeoAngleFormat.DMS:
                    rc = ParseDMS(s, options);
                    break;
            }

            return rc;
        }

        ///// <summary>
        ///// Converts the current value in degrees, to DMM components
        ///// </summary>
        ///// <param name="wholeDegrees"></param>
        ///// <param name="decimalMinutes"></param>
        ///// <param name="sign"></param>
        //protected void computeDMM(ref int wholeDegrees, ref double decimalMinutes, ref int sign) {
        //    double degrees = Degrees;
        //    sign = degrees < 0 ? -1 : 1;
        //    degrees = Math.Abs(degrees);
        //    wholeDegrees = Convert.ToInt32( Math.Floor(degrees) );
        //    double fraction = degrees - wholeDegrees;
        //    decimalMinutes = fraction * 60;
        //}

        ///// <summary>
        ///// Converts the current value in degrees to DMS components
        ///// </summary>
        ///// <param name="wholeDegrees"></param>
        ///// <param name="wholeMinutes"></param>
        ///// <param name="decimalSeconds"></param>
        ///// <param name="sign"></param>
        //protected void computeDMS(ref int wholeDegrees, ref int wholeMinutes, ref double decimalSeconds, ref int sign) {
        //    double decimalMinutes = 0;
        //    computeDMM(ref wholeDegrees, ref decimalMinutes, ref sign);
        //    wholeMinutes = Convert.ToInt32(Math.Floor(decimalMinutes));
        //    double fraction = decimalMinutes - wholeMinutes;
        //    decimalSeconds = fraction * 60;
        //}

        ///// <summary>
        ///// Converts from DMM to degrees
        ///// </summary>
        ///// <param name="wholeDegrees"></param>
        ///// <param name="decimalMinutes"></param>
        ///// <param name="sign"></param>
        ///// <returns></returns>
        //protected static double computeDDD(int wholeDegrees, double decimalMinutes, int sign) {
        //    double fraction = decimalMinutes / 60;
        //    return sign * (1.0 * wholeDegrees + fraction);
        //}

        ///// <summary>
        ///// Converts from DMS to degrees
        ///// </summary>
        ///// <param name="wholeDegrees"></param>
        ///// <param name="wholeMinutes"></param>
        ///// <param name="decimalSeconds"></param>
        ///// <param name="sign"></param>
        ///// <returns></returns>
        //protected static double computeDDD(int wholeDegrees, int wholeMinutes, double decimalSeconds, int sign) {
        //    double fraction = decimalSeconds / 60;
        //    fraction = (1.0 * wholeMinutes + fraction) / 60;
        //    return sign * (1.0 * wholeDegrees + fraction);
        //}

        /// <summary>
        /// Parses a DMM representation and converts to degrees
        /// </summary>
        /// <param name="s"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected static double ParseDMM(string s, GeoAngleFormatOptions options) {
            DMMComponents dmm = DMMComponents.Parse(s, options);
            return dmm.ToDDD();
            //if (options == GeoAngleFormatOptions.Compact) {
            //    // ... Split value and hemisphere letter
            //    string[] tokens = s.Split(DELIMS);
            //    if (tokens.Length != 2) {
            //        throw new ArgumentException(nameof(s));
            //    }
            //    int sign = 0;
            //    switch (tokens[1].ToUpper().Trim()[0]) {
            //        case 'N': case 'E': sign = 1; break;
            //        case 'S': case 'W': sign = -1; break;
            //        default:
            //            throw new ArgumentException(nameof(s));
            //    }

            //    double number = double.Parse(tokens[0].Trim());
            //    int wholeDegrees = Convert.ToInt32(Math.Floor(number / 100));
            //    double decimalMinutes = number - wholeDegrees*100;
            //    return computeDDD(wholeDegrees, decimalMinutes, sign);
            //}
            //else {
            //    // ... Split value and hemisphere letter
            //    string[] tokens = s.Split(DELIMS, StringSplitOptions.RemoveEmptyEntries);
            //    if (tokens.Length != 2) {
            //        throw new ArgumentException(nameof(s));
            //    }
            //    int wholeDegrees = Int32.Parse(tokens[0]);
            //    int sign = wholeDegrees < 0 ? -1 : 1;
            //    wholeDegrees *= sign; // Abs
            //    double decimalMinutes = double.Parse(tokens[1]);
            //    return computeDDD(wholeDegrees, decimalMinutes, sign);
            //}
        }

        /// <summary>
        /// Parses a DMS representation and converts to degrees
        /// </summary>
        /// <param name="s"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected static double ParseDMS(string s, GeoAngleFormatOptions options) {
            DMSComponents dms = DMSComponents.Parse(s, options);
            return dms.ToDDD();
            //if (options == GeoAngleFormatOptions.Compact) {
            //    // ... Split value and hemisphere letter
            //    string[] tokens = s.Split(DELIMS);
            //    if (tokens.Length != 2) {
            //        throw new ArgumentException(nameof(s));
            //    }
            //    int sign = 0;
            //    switch (tokens[1].ToUpper().Trim()[0] ) {
            //        case 'N': case 'E': sign = 1;break;
            //        case 'S': case 'W': sign = -1; break;
            //        default:
            //            throw new ArgumentException(nameof(s));
            //    }

            //    double number = double.Parse(tokens[0].Trim());
            //    int wholeDegrees = Convert.ToInt32(Math.Floor(number / 10000));
            //    double decimalMinutes = number - wholeDegrees*10000;
            //    int wholeMinutes = Convert.ToInt32(Math.Floor(decimalMinutes / 100));
            //    double decimalSeconds = decimalMinutes - wholeMinutes*100;
            //    return computeDDD(wholeDegrees, wholeMinutes, decimalSeconds, sign);
            //}
            //else {
            //    // ... Split value and hemisphere letter
            //    string[] tokens = s.Split(DELIMS, StringSplitOptions.RemoveEmptyEntries);

            //    int wholeDegrees = Int32.Parse(tokens[0]);
            //    int wholeMinutes = Int32.Parse(tokens[1]);
            //    double decimalSeconds = double.Parse(tokens[2]);
            //    int sign = 1;

            //    switch (tokens.Length) {
            //        case 3:
            //            sign = wholeDegrees < 0 ? -1 : 1;
            //            wholeDegrees *= sign; // Abs
            //            break;
            //        case 4:
            //            switch (tokens[1].ToUpper().Trim()[0]) {
            //                case 'N': case 'E': sign = 1; break;
            //                case 'S': case 'W': sign = -1; break;
            //                default:
            //                    throw new ArgumentException(nameof(s));
            //            }
            //            break;
            //        default:
            //            throw new ArgumentException(nameof(s));
            //    }

            //    return computeDDD(wholeDegrees, wholeMinutes, decimalSeconds, sign);
            //}
        }

        /// <summary>
        /// Converts from DMM components to its string representation
        /// </summary>
        /// <param name="wholeDegrees"></param>
        /// <param name="decimalMinutes"></param>
        /// <param name="sign"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected abstract string ToStringDMM(DMMComponents dmm, GeoAngleFormatOptions options );

        /// <summary>
        /// Converts from DMS components to its string representation
        /// </summary>
        /// <param name="wholeDegrees"></param>
        /// <param name="wholeMinutes"></param>
        /// <param name="decimalSeconds"></param>
        /// <param name="sign"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected abstract string ToStringDMS(DMSComponents dms, GeoAngleFormatOptions options );
    }
}
