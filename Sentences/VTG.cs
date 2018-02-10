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
    /// NMEA-0183 VTG
    /// </summary>
    public class VTG : BaseNmeaMessage {
        public override string Description => Strings.vtg;

        public string DataTypeName { get; set; }
        public float TrueTrackMadeGoodDegrees { get; set; }
        public float MagneticTrackMadeGoodDegrees { get; set; }
        public float GroundSpeedKnots { get; set; }
        public float GroundSpeedKph { get; set; }

        public VTG() {
            DataTypeName = "GPVTG";
            Reset();
        }

        public void Reset() {
            TrueTrackMadeGoodDegrees = 0;
            MagneticTrackMadeGoodDegrees = 0;
            GroundSpeedKnots = 0;
            GroundSpeedKph = 0;
        }

        public override string Payload {
            get {
                List<string> tokens = new List<string> {
                    DataTypeName,
                    TrueTrackMadeGoodDegrees.ToString(), "T",
                    MagneticTrackMadeGoodDegrees.ToString(), "M",
                    GroundSpeedKnots.ToString(),"N",
                    GroundSpeedKph.ToString(),"K"
                };
                return string.Join(DELIM_FIELDS, tokens);
            }
        }

        public override INmeaMessage ParseFields(string[] tokens) {
            DataTypeName = tokens[0];
            TrueTrackMadeGoodDegrees = float.Parse(tokens[1]);
            MagneticTrackMadeGoodDegrees = float.Parse(tokens[3]);
            GroundSpeedKnots = float.Parse(tokens[5]);
            GroundSpeedKph = float.Parse(tokens[7]);
            return this;
        }
    }
}
