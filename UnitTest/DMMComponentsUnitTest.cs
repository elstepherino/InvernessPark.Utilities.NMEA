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
    public class DMMComponentsUnitTest {
        private Random _rnd = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

        [TestCase]
        public void DefaultConstructor() {
            DMMComponents dmm = new DMMComponents();
            Assert.AreEqual(dmm.get_sign(), 0);
            Assert.AreEqual(dmm.get_wholeDegrees(), 0);
            Assert.IsTrue( Math.Abs(dmm.get_decimalMinutes()) < 0.000001, "Decimal minutes default init");
        }

        [TestCase]
        public void ConstructorWithPositiveParameter() {
            double degrees = _rnd.NextDouble() * 180;

            DMMComponents dmm = new DMMComponents(degrees);

            DMMComponents dmm2 = new DMMComponents();
            dmm2.set_sign(dmm.get_sign());
            dmm2.set_wholeDegrees(dmm.get_wholeDegrees());
            dmm2.set_decimalMinutes(dmm.get_decimalMinutes());

            Assert.IsTrue(Math.Abs(dmm2.ToDDD() - degrees) < 0.000001, "Converting to and from should be consistent");
        }

        [TestCase]
        public void ConstructorWithNegaitiveParameter() {
            double degrees = -_rnd.NextDouble() * 180;

            DMMComponents dmm = new DMMComponents(degrees);

            DMMComponents dmm2 = new DMMComponents();
            dmm2.set_sign(dmm.get_sign());
            dmm2.set_wholeDegrees(dmm.get_wholeDegrees());
            dmm2.set_decimalMinutes(dmm.get_decimalMinutes());

            Assert.IsTrue( Math.Abs(dmm2.ToDDD() - degrees) < 0.000001, "Converting to and from should be consistent");
        }

        [TestCase]
        public void ParseFromValidCompactString() {
            Dictionary<String, DMMComponents> testCases = new Dictionary<String, DMMComponents>();
            testCases.Add("6113.083336,N", new DMMComponents(61.21805560));
            testCases.Add("3134.310112,E", new DMMComponents(31.5718352));
            testCases.Add("5708.849490,n", new DMMComponents(57.1474915));
            testCases.Add("5224.918180,e", new DMMComponents(52.41530300));
            testCases.Add("14954.016668,W", new DMMComponents(-149.90027780));
            testCases.Add("8515.029358,S", new DMMComponents(-85.2504893));
            testCases.Add("205.723820,w", new DMMComponents(-2.095397));
            testCases.Add("404.975200,s", new DMMComponents(-4.08292000));

            foreach (KeyValuePair<String, DMMComponents> kv in testCases) {
                DMMComponents dmm = DMMComponents.Parse(kv.Key, GeoAngleFormatOptions.Compact);
                Assert.AreEqual(dmm.get_sign(), kv.Value.get_sign());
                Assert.AreEqual(dmm.get_wholeDegrees(), kv.Value.get_wholeDegrees());
                Assert.IsTrue(Math.Abs(dmm.get_decimalMinutes() - kv.Value.get_decimalMinutes()) < 0.000001, "Decimal minutes equality");
            }
        }

        [TestCase]
        public void ParseFromValidFormattedString() {
            Dictionary<String, DMMComponents> testCases = new Dictionary<String, DMMComponents>();
            testCases.Add("61° 13.083336'", new DMMComponents(61.21805560));
            testCases.Add("31° 34.310112'", new DMMComponents(31.5718352));
            testCases.Add("57° 8.849490'", new DMMComponents(57.1474915));
            testCases.Add("52° 24.918180'", new DMMComponents(52.41530300));
            testCases.Add("-149° 54.016668'", new DMMComponents(-149.90027780));
            testCases.Add("-85° 15.029358'", new DMMComponents(-85.2504893));
            testCases.Add("-2° 5.723820'", new DMMComponents(-2.095397));
            testCases.Add("-4° 4.975200'", new DMMComponents(-4.08292000));

            foreach (KeyValuePair<String, DMMComponents> kv in testCases) {
                DMMComponents dmm = DMMComponents.Parse(kv.Key, GeoAngleFormatOptions.ShowUnits);
                Assert.AreEqual(dmm.get_sign(), kv.Value.get_sign());
                Assert.AreEqual(dmm.get_wholeDegrees(), kv.Value.get_wholeDegrees());
                Assert.IsTrue(Math.Abs(dmm.get_decimalMinutes() - kv.Value.get_decimalMinutes()) < 0.000001, "Decimal minutes equality");
            }
        }

        [TestCase]
        [ExpectedException(typeof(DMMFormatException))]
        public void ParseFromInvalidCompactString() {
            DMMComponents.Parse("61x13.083336;N", GeoAngleFormatOptions.Compact);
        }

        [TestCase]
        [ExpectedException(typeof(DMMFormatException))]
        public void ParseFromInvalidFormattedString() {
            DMMComponents.Parse("61° 61° 13;083336'", GeoAngleFormatOptions.ShowUnits);
        }
    }
#endif
}
