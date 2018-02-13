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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvernessPark.Utilities.NMEA.UnitTest {
#if DEBUG
    [TestFixture]
    public class NmeaStreamUnitTest : INmeaStreamUnitTest {
        /**
         * Default constructor uses a default NmeaStream
         */
        public NmeaStreamUnitTest() : base(new NmeaStream()) {
        }

        /**
         * placeholder for a bool variable that gets set from within a listener callback
         */
        class BoolPlaceholder {
            private bool _state = false;

            public bool is_state() {
                return _state;
            }

            public void set_state(bool _state) {
                this._state = _state;
            }
        }

        [TestCase]
        public void StateIsIdle() {
            get_strm().Reset();
            Assert.AreEqual(((NmeaStream)get_strm()).State, NmeaStream.StateEnum.Idle);
        }

        [TestCase]
        public void StateIsPayload() {
            get_strm().Reset();
            get_strm().Append(SOM);
            Assert.AreEqual(((NmeaStream)get_strm()).State, NmeaStream.StateEnum.Payload);
            get_strm().Append(SOM);
            Assert.AreEqual(((NmeaStream)get_strm()).State, NmeaStream.StateEnum.Payload);
            get_strm().Append(SOM);
            Assert.AreEqual(((NmeaStream)get_strm()).State, NmeaStream.StateEnum.Payload);
            get_strm().Append(SOM);
            Assert.AreEqual(((NmeaStream)get_strm()).State, NmeaStream.StateEnum.Payload);
        }

        [TestCase]
        public void ReceiveCompleteMessages() {
            get_strm().Reset();
            foreach (String nmeaSentence in get_sampleData()) {
                ReceiveCompleteMessage(nmeaSentence);
            }
        }

        private void ReceiveCompleteMessage(String randomNmeaString) {
            BoolPlaceholder msgIsReceived = new BoolPlaceholder();
            get_strm().NMEAMessageReceived += (bytes, index, count) => {
                msgIsReceived.set_state(true);
            };
            byte[] nmeaBytes = Encoding.ASCII.GetBytes(randomNmeaString);
            get_strm().Append(nmeaBytes);
            Assert.IsTrue(msgIsReceived.is_state());
        }
    }
#endif
}
