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
    public class GeoAngleUnitTest {
    /**
     * To test the Abstract base class, we need a concreate class
     */
    class GeoAngleStub : GeoAngle {
        public GeoAngleStub() {
        }

        public GeoAngleStub(double degrees) :base(degrees) {
        }

        protected override String ToStringDMM(DMMComponents dmm, GeoAngleFormatOptions options) {
            return null;
        }

        protected override String ToStringDMS(DMSComponents dms, GeoAngleFormatOptions options) {
            return null;
        }
    }

    Random _rnd = new Random(Convert.ToInt32( DateTime.Now.Ticks % Int32.MaxValue ) );

    [TestCase]
    public void DefaultConstructor() {
        GeoAngle stub = new GeoAngleStub();
        Assert.IsTrue(Math.Abs(stub.Degrees) < 0.000001);
    }

    [TestCase]
    public void Constructor() {
        double degrees = _rnd.NextDouble() * 360;
        GeoAngle stub = new GeoAngleStub(degrees);
        Assert.IsTrue(Math.Abs(stub.Degrees - degrees) < 0.000001);
    }

    [TestCase]
    public void DegreesToRadians() {
        double degrees = _rnd.NextDouble() * 360;
        GeoAngle stub = new GeoAngleStub();
        stub.Degrees = degrees;

        double radians = stub.Radians;
        double expected = degrees * Math.PI / 180.0;

        Assert.IsTrue(Math.Abs(radians - expected) < 0.000001);
    }

    [TestCase]
    public void RadiansToDegrees() {
        double radians = _rnd.NextDouble() * Math.PI * 2;
        GeoAngle stub = new GeoAngleStub();
        stub.Radians = radians;

        double degrees = stub.Degrees;
        double expected = radians * 180.0 / Math.PI;

        Assert.IsTrue(Math.Abs(degrees - expected) < 0.000001);
    }
}
#endif
}
