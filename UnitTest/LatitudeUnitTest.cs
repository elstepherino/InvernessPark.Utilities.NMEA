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
    public class LatitudeUnitTest {
        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorOutOfBoundsDegreesPositive() {
            Latitude lat = new Latitude() { Degrees = 360 };
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorOutOfBoundsDegreesNegative() {
            Latitude lat = new Latitude() { Degrees = -360 };
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetterOutOfBoundsDegreesPositive() {
            Latitude lat = new Latitude();
            lat.Degrees = 360;
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetterOutOfBoundsDegreesNegative() {
            Latitude lat = new Latitude();
            lat.Degrees = -360;
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetterOutOfBoundsRadiansPositive() {
            Latitude lat = new Latitude();
            lat.Radians = 2 * Math.PI;
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetterOutOfBoundsRadiansNegative() {
            Latitude lat = new Latitude();
            lat.Radians = -2 * Math.PI;
        }

        [TestCase]
        public void SerializationToCompactDDD() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Latitude() { Degrees = 61.2180556 }, "61.2180556");
            testCases.Add(new Latitude() { Degrees = -31.5718352 }, "-31.5718352");
            testCases.Add(new Latitude() { Degrees = -57.1474915 }, "-57.1474915");
            testCases.Add(new Latitude() { Degrees = 52.4153030 }, "52.4153030");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DDD, GeoAngleFormatOptions.Compact);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToFormattedDDD() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Latitude() { Degrees = 61.2180556 }, "61.2180556°");
            testCases.Add(new Latitude() { Degrees = -31.5718352 }, "-31.5718352°");
            testCases.Add(new Latitude() { Degrees = -57.1474915 }, "-57.1474915°");
            testCases.Add(new Latitude() { Degrees = 52.4153030 }, "52.4153030°");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DDD, GeoAngleFormatOptions.ShowUnits);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToCompactDMM() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Latitude() { Degrees = 61.21805560 }, "6113.0833,N");
            testCases.Add(new Latitude() { Degrees = -31.5718352 }, "3134.3101,S");
            testCases.Add(new Latitude() { Degrees = -57.1474915 }, "5708.8495,S");
            testCases.Add(new Latitude() { Degrees = 52.41530300 }, "5224.9182,N");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DMM, GeoAngleFormatOptions.Compact);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToFormattedDMM() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Latitude() { Degrees = 61.21805560 }, "61° 13.083336'");
            testCases.Add(new Latitude() { Degrees = -31.5718352 }, "-31° 34.310112'");
            testCases.Add(new Latitude() { Degrees = -57.1474915 }, "-57° 8.84949'");
            testCases.Add(new Latitude() { Degrees = 52.41530300 }, "52° 24.91818'");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DMM, GeoAngleFormatOptions.ShowUnits);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToCompactDMS() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Latitude() { Degrees = 61.21805560 }, "611305.0002,N");
            testCases.Add(new Latitude() { Degrees = -31.5718352 }, "313418.6067,S");
            testCases.Add(new Latitude() { Degrees = -57.1474915 }, "570850.9694,S");
            testCases.Add(new Latitude() { Degrees = 52.41530300 }, "522455.0908,N");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DMS, GeoAngleFormatOptions.Compact);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }

        [TestCase]
        public void SerializationToFormattedDMS() {
            Dictionary<GeoAngle, String> testCases = new Dictionary<GeoAngle, String>();

            testCases.Add(new Latitude() { Degrees = 61.21805560 }, "61° 13' 5.0002\" N");
            testCases.Add(new Latitude() { Degrees = -31.5718352 }, "31° 34' 18.6067\" S");
            testCases.Add(new Latitude() { Degrees = -57.1474915 }, "57° 8' 50.9694\" S");
            testCases.Add(new Latitude() { Degrees = 52.41530300 }, "52° 24' 55.0908\" N");

            foreach (KeyValuePair<GeoAngle, String> kv in testCases) {
                String serialized = kv.Key.ToString(GeoAngleFormat.DMS, GeoAngleFormatOptions.ShowUnits);
                String msg = String.Format("{0} != {1}", kv.Value, serialized);
                Assert.IsTrue(kv.Value.Equals(serialized, StringComparison.OrdinalIgnoreCase), msg);
            }
        }
    }
#endif
}
