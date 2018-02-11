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
using System.Globalization;

namespace InvernessPark.Utilities.NMEA.Sentences {

    /// <summary>
    /// NMEA-0183 GST
    /// </summary>
    public class GST : BaseNmeaMessage {
        public override string Description => Strings.gst;

        public string DataTypeName { get; set; }
        public TimeSpan UTC { get; set; }
        public float RMS { get; set; }
        public float SmjrStdev { get ; set ; }
        public float SmnrStdev { get; set; }
        public float Orientation { get; set; } // degrees true north
        public float LatitudeErrorStdev { get; set; }
        public float LongitudeErrorStdev { get; set; }
        public float AltitudeErrorStdev { get; set; }

        public GST() {
            DataTypeName = "GPGST";
            Reset();
        }

        private void Reset() {
            UTC = TimeSpan.MinValue;
            RMS = 0;
            SmjrStdev = 0;
            SmnrStdev = 0;
            Orientation = 0;
            LatitudeErrorStdev = 0;
            LongitudeErrorStdev = 0;
            AltitudeErrorStdev = 0;
        }

        public override string Payload {
            get {
                List<string> tokens = new List<string> {
                    DataTypeName,
                    UTC.ToString(@"hhmmss\.ff"),
                    RMS.ToString("0.00"),
                    SmjrStdev.ToString("0.00"),
                    SmnrStdev.ToString("0.00"),
                    Orientation.ToString("0.0###"),
                    LatitudeErrorStdev.ToString("0.00"),
                    LongitudeErrorStdev.ToString("0.00"),
                    AltitudeErrorStdev.ToString("0.00")
                };
                return string.Join(DELIM_FIELDS, tokens);
            }
        }

        public override INmeaMessage ParseFields(string[] tokens) {
            DataTypeName = tokens[0].TrimStart('$');
            UTC = TimeSpan.ParseExact(tokens[1], @"hhmmss\.FF", CultureInfo.InvariantCulture);
            RMS = float.Parse(tokens[2]);
            SmjrStdev = float.Parse(tokens[3]);
            SmnrStdev = float.Parse(tokens[4]);
            Orientation = float.Parse(tokens[5]);
            LatitudeErrorStdev = float.Parse(tokens[6]);
            LongitudeErrorStdev = float.Parse(tokens[7]);
            AltitudeErrorStdev = float.Parse(tokens[8]);
            return this;
        }
    }
}
