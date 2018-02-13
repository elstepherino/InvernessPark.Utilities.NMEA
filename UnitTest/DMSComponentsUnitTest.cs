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
    public class DMSComponentsUnitTest {
        private Random _rnd = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

        [TestCase]
        public void DefaultConstructor() {
            DMSComponents dms = new DMSComponents();
            Assert.AreEqual(dms.get_sign(), 0);
            Assert.AreEqual(dms.get_wholeDegrees(), 0);
            Assert.AreEqual(dms.get_wholeMinutes(), 0);
            Assert.IsTrue(Math.Abs(dms.get_decimalSeconds()) < 0.000001, "Decimal seconds default init");
        }

        [TestCase]
        public void ConstructorWithPositiveParameter() {
            double degrees = _rnd.NextDouble() * 180;

            DMSComponents dms = new DMSComponents(degrees);

            DMSComponents dms2 = new DMSComponents();
            dms2.set_sign(dms.get_sign());
            dms2.set_wholeDegrees(dms.get_wholeDegrees());
            dms2.set_wholeMinutes(dms.get_wholeMinutes());
            dms2.set_decimalSeconds(dms.get_decimalSeconds());

            Assert.IsTrue(Math.Abs(dms2.ToDDD() - degrees) < 0.000001, 
                String.Format("ToDDD({0}) != {1}", dms2.ToDDD(), degrees));
        }

        [TestCase]
        public void ConstructorWithNegaitiveParameter() {
            double degrees = -_rnd.NextDouble() * 180;

            DMSComponents dms = new DMSComponents(degrees);

            DMSComponents dms2 = new DMSComponents();
            dms2.set_sign(dms.get_sign());
            dms2.set_wholeDegrees(dms.get_wholeDegrees());
            dms2.set_wholeMinutes(dms.get_wholeMinutes());
            dms2.set_decimalSeconds(dms.get_decimalSeconds());

            Assert.IsTrue(Math.Abs(dms2.ToDDD() - degrees) < 0.000001, 
                String.Format("ToDDD({0}) != {1}", dms2.ToDDD(), degrees));
        }

        [TestCase]
        public void ParseFromValidCompactString() {
            Dictionary<String, DMSComponents> testCases = new Dictionary<String, DMSComponents>();
            testCases.Add("611305.0002,N", new DMSComponents(61.21805560));
            testCases.Add("313418.6067,n", new DMSComponents(31.5718352));
            testCases.Add("570850.9694,N", new DMSComponents(57.1474915));
            testCases.Add("522455.0908,n", new DMSComponents(52.41530300));
            testCases.Add("1495401.0001,W", new DMSComponents(-149.90027780));
            testCases.Add("0851501.7615,w", new DMSComponents(-85.2504893));
            testCases.Add("0020543.4292,W", new DMSComponents(-2.095397));
            testCases.Add("0040458.5120,w", new DMSComponents(-4.08292000));

            foreach (KeyValuePair<String, DMSComponents> kv in testCases) {
                DMSComponents dms = DMSComponents.Parse(kv.Key, GeoAngleFormatOptions.Compact);
                Assert.AreEqual(dms.get_sign(), kv.Value.get_sign());
                Assert.AreEqual(dms.get_wholeDegrees(), kv.Value.get_wholeDegrees());
                Assert.AreEqual(dms.get_wholeMinutes(), kv.Value.get_wholeMinutes());
                String msg = String.Format("Parsed: {0}: {1} != {2} (Expected)",
                        kv.Key,
                        dms.get_decimalSeconds(),
                        kv.Value.get_decimalSeconds());
                Assert.IsTrue(Math.Abs(dms.get_decimalSeconds() - kv.Value.get_decimalSeconds()) < 0.0001, msg);
            }
        }


        [TestCase]
        public void ParseFromValidFormattedString() {
            Dictionary<String, DMSComponents> testCases = new Dictionary<String, DMSComponents>();
            testCases.Add("61° 13' 5.0002\" N", new DMSComponents(61.21805560));
            testCases.Add("31° 34' 18.6067\" N", new DMSComponents(31.5718352));
            testCases.Add("57° 8' 50.9694\" N", new DMSComponents(57.1474915));
            testCases.Add("52° 24' 55.0908\" N", new DMSComponents(52.41530300));
            testCases.Add("149° 54' 1.0001\" W", new DMSComponents(-149.90027780));
            testCases.Add("85° 15' 1.7615\" W", new DMSComponents(-85.2504893));
            testCases.Add("2° 5' 43.4292\" W", new DMSComponents(-2.095397));
            testCases.Add("4° 4' 58.5120\" W", new DMSComponents(-4.08292000));

            foreach (KeyValuePair<String, DMSComponents> kv in testCases) {
                DMSComponents dms = DMSComponents.Parse(kv.Key, GeoAngleFormatOptions.ShowUnits);
                Assert.AreEqual(dms.get_sign(), kv.Value.get_sign());
                Assert.AreEqual(dms.get_wholeDegrees(), kv.Value.get_wholeDegrees());
                Assert.AreEqual(dms.get_wholeMinutes(), kv.Value.get_wholeMinutes());
                String msg = String.Format("Parsed: {0}: {1} != {2} (Expected)",
                        kv.Key,
                        dms.get_decimalSeconds(),
                        kv.Value.get_decimalSeconds());
                Assert.IsTrue(Math.Abs(dms.get_decimalSeconds() - kv.Value.get_decimalSeconds()) < 0.0001, msg);
            }
        }

        [TestCase]
        [ExpectedException(typeof(DMSFormatException))]
        public void ParseFromInvalidCompactString() {
            DMSComponents.Parse("611c305.0002;N", GeoAngleFormatOptions.Compact);
        }

        [TestCase]
        [ExpectedException(typeof(DMSFormatException))]
        public void ParseFromInvalidFormattedString() {
            DMSComponents.Parse("61° 13' 13' 5.0002\" N", GeoAngleFormatOptions.ShowUnits);
        }
    }
#endif
}
