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
using InvernessPark.Utilities.NMEA.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvernessPark.Utilities.NMEA.UnitTest {
#if DEBUG
    [TestFixture]
    public class LongitudeUnitTest {
        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorOutOfBoundsDegreesPositive() {
            Longitude lat = new Longitude() { Degrees = 360 };
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorOutOfBoundsDegreesNegative() {
            Longitude lat = new Longitude() { Degrees = -360 };
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetterOutOfBoundsDegreesPositive() {
            Longitude lat = new Longitude();
            lat.Degrees = 360 ;
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetterOutOfBoundsDegreesNegative() {
            Longitude lat = new Longitude();
            lat.Degrees = -360 ;
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetterOutOfBoundsRadiansPositive() {
            Longitude lat = new Longitude();
            lat.Radians = 2 * Math.PI ;
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetterOutOfBoundsRadiansNegative() {
            Longitude lat = new Longitude();
            lat.Radians = -2 * Math.PI ;
        }

        [TestCase]
        public void SerializationToCompactDDD() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Longitude() { Degrees = -149.90027780 }, "-149.9002778");
            testCases.Add(new Longitude() { Degrees = 85.2504893 }, "85.2504893");
            testCases.Add(new Longitude() { Degrees = 2.095397 }, "2.0953970");
            testCases.Add(new Longitude() { Degrees = -4.08292000 }, "-4.0829200");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DDD, GeoAngleFormatOptions.Compact);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToFormattedDDD() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Longitude() { Degrees = -149.90027780 }, "-149.9002778°");
            testCases.Add(new Longitude() { Degrees = 85.2504893 }, "85.2504893°");
            testCases.Add(new Longitude() { Degrees = 2.095397 }, "2.0953970°");
            testCases.Add(new Longitude() { Degrees = -4.08292000 }, "-4.0829200°");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DDD, GeoAngleFormatOptions.ShowUnits);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToCompactDMM() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Longitude() { Degrees = -149.90027780 }, "14954.0167,W");
            testCases.Add(new Longitude() { Degrees = 85.2504893 }, "08515.0294,E");
            testCases.Add(new Longitude() { Degrees = 2.095397 }, "00205.7238,E");
            testCases.Add(new Longitude() { Degrees = -4.08292000 }, "00404.9752,W");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DMM, GeoAngleFormatOptions.Compact);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToFormattedDMM() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Longitude() { Degrees = -149.90027780 }, "-149° 54.016668'");
            testCases.Add(new Longitude() { Degrees = 85.2504893 }, "85° 15.029358'");
            testCases.Add(new Longitude() { Degrees = 2.095397 }, "2° 5.72382'");
            testCases.Add(new Longitude() { Degrees = -4.08292000 }, "-4° 4.9752'");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DMM, GeoAngleFormatOptions.ShowUnits);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToCompactDMS() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Longitude() { Degrees = -149.90027780 }, "1495401.0001,W");
            testCases.Add(new Longitude() { Degrees = 85.2504893 }, "0851501.7615,E");
            testCases.Add(new Longitude() { Degrees = 2.095397 }, "0020543.4292,E");
            testCases.Add(new Longitude() { Degrees = -4.08292000 }, "0040458.5120,W");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DMS, GeoAngleFormatOptions.Compact);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToFormattedDMS() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Longitude() { Degrees = -149.90027780 }, "149° 54' 1.0001\" W");
            testCases.Add(new Longitude() { Degrees = 85.2504893 }, "85° 15' 1.7615\" E");
            testCases.Add(new Longitude() { Degrees = 2.095397 }, "2° 5' 43.4292\" E");
            testCases.Add(new Longitude() { Degrees = -4.08292000 }, "4° 4' 58.512\" W");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DMS, GeoAngleFormatOptions.ShowUnits);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }
    }
#endif
}
