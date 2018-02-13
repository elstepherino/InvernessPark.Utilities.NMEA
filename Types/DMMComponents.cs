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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvernessPark.Utilities.NMEA.Types {
    public class DMMComponents {
        static readonly char[] DELIMS = new char[] { ',', ' ', '\'', '"', Strings.degrees.Trim()[0] };

        private int _sign;
        private int _wholeDegrees;
        private double _decimalMinutes;

        public DMMComponents() {
            _sign = 0;
            _wholeDegrees = 0;
            _decimalMinutes = 0;
        }

        public DMMComponents(double degrees) {
            _sign = degrees < 0 ? -1 : 1;
            degrees = Math.Abs(degrees);
            _wholeDegrees = (int)Math.Floor(degrees);
            double fraction = degrees - _wholeDegrees;
            _decimalMinutes = fraction * 60;
        }

        public double ToDDD() {
            double fraction = _decimalMinutes / 60;
            return _sign * (1.0 * _wholeDegrees + fraction);
        }

        public int get_sign() {
            return _sign;
        }

        public void set_sign(int _sign) {
            this._sign = _sign;
        }

        public int get_wholeDegrees() {
            return _wholeDegrees;
        }

        public void set_wholeDegrees(int _wholeDegrees) {
            this._wholeDegrees = _wholeDegrees;
        }

        public double get_decimalMinutes() {
            return _decimalMinutes;
        }

        public void set_decimalMinutes(double _decimalMinutes) {
            this._decimalMinutes = _decimalMinutes;
        }

        /**
         * Returns the numeric sign for a given NMEA hemisphere character
         * @param hemisphereChar
         * @return
         */
        private static int getHemisphereSign(char hemisphereChar) {
            int sign = 0;
            switch (hemisphereChar) {
                case 'N':
                case 'n':
                case 'E':
                case 'e':
                    sign = 1;
                    break;
                case 'S':
                case 's':
                case 'W':
                case 'w':
                    sign = -1;
                    break;
                default:
                    throw new DMMFormatException("hemisphereChar: " + hemisphereChar );
            }
            return sign;
        }

        /**
         * Parses a DMM string representation
         * @param s
         * @return
         */
        public static DMMComponents Parse(String s, GeoAngleFormatOptions options) {
            int sign = 1;
            int wholeDegrees = 0;
            double decimalMinutes = 0;

            if (options == GeoAngleFormatOptions.Compact) {
                // ... Split value and hemisphere letter

                String[] tokens = s.Split(DELIMS);
                if (tokens.Length != 2) {
                    throw new DMMFormatException(s);
                }

                sign = getHemisphereSign(tokens[1].Trim()[0]);

                double number = Double.Parse(tokens[0].Trim());
                wholeDegrees = (int)Math.Floor(number / 100);
                decimalMinutes = number - wholeDegrees * 100;
            }
            else {
                // ... Split value and hemisphere letter
                List<String> tokens = new List<String>(s.Split(DELIMS, StringSplitOptions.RemoveEmptyEntries));
                if (tokens.Count != 2) {
                    throw new DMMFormatException(s);
                }
                wholeDegrees = Int32.Parse(tokens[0]);
                sign = wholeDegrees < 0 ? -1 : 1;
                wholeDegrees *= sign; // Abs
                decimalMinutes = Double.Parse(tokens[1]);
            }

            DMMComponents rc = new DMMComponents();
            rc.set_sign(sign);
            rc.set_wholeDegrees(wholeDegrees);
            rc.set_decimalMinutes(decimalMinutes);
            return rc;
        }
    }
}
