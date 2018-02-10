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
using System.Collections.Generic;

namespace InvernessPark.Utilities.NMEA.Sentences {

    /// <summary>
    /// NMEA-0183 HDT
    /// </summary>
    public class HDT : BaseNmeaMessage {
        public override string Description => Strings.hdt;

        public string DataTypeName { get; set; }
        public float HeadingTrue { get; set; }

        public HDT() {
            DataTypeName = "GPHDT";
            Reset();
        }

        public void Reset() {
            HeadingTrue = 0;
        }

        public override string Payload {
            get {
                List<string> tokens = new List<string> {
                    DataTypeName,
                    HeadingTrue.ToString("0.0"), "T"
                };
                return string.Join(DELIM_FIELDS, tokens);
            }
        }

        public override INmeaMessage ParseFields(string[] tokens) {
            DataTypeName = tokens[0];
            HeadingTrue = float.Parse(tokens[1]);
            return this;
        }
    }
}
