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

namespace InvernessPark.Utilities.NMEA.Sentences {

    /// <summary>
    /// NMEA-0183 GSV
    /// </summary>
    public class GSV : BaseNmeaMessage {

        public override string Description => Strings.gsv;

        public string DataTypeName { get; set; }
        public int NumSentences { get ; set; }
        public int SentenceIndex { get; set; }
        public int NumSatellitesInView { get; set; }
        public Satellite[] SatelliteInfo { get; private set; }

        public GSV() {
            DataTypeName = "GSPGSV";
            NumSentences = 0;
            SentenceIndex = 0;
            NumSatellitesInView = 0;
            SatelliteInfo = new Satellite[] {
                new Satellite(),
                new Satellite(),
                new Satellite(),
                new Satellite(),
            };
        }

        private void ClearSatelliteInfo() {
            for (int i = 0; i < SatelliteInfo.Length; ++i) {
                SatelliteInfo[i].Clear();
            }
        }

        public override string Payload {
            get {
                List<string> tokens = new List<string> {
                    DataTypeName,
                    NumSentences.ToString(),
                    SentenceIndex.ToString(),
                    NumSatellitesInView.ToString("00")
                };
                for (int i = 0; i < SatelliteInfo.Length; ++i ) {
                    tokens.AddRange(SatelliteInfo[i].ToArray());
                }
                
                return string.Join(DELIM_FIELDS, tokens);
            }
        }

        public override INmeaMessage ParseFields(string[] tokens) {
            DataTypeName = tokens[0];
            NumSentences = int.Parse(tokens[1]);
            SentenceIndex = int.Parse(tokens[2]);
            NumSatellitesInView = int.Parse(tokens[3]);
            for (int i = 0; i < SatelliteInfo.Length; ++i) {
                int offset = 4 + i * 4;
                if (offset < tokens.Length) {
                    SatelliteInfo[i].FromArray(tokens, offset);
                }
            }
            return this;
        }

        /// <summary>
        /// Placeholder for specific satellite info
        /// </summary>
        public class Satellite {
            public string PRN { get; set; }
            public int ? ElevationDegrees { get; set; } //may be empty
            public int ? AzimuthDegrees { get; set; } //may be empty
            public int ? SNR { get; set; } //may be empty

            public Satellite() {
                Clear();
            }

            public void Clear() {
                PRN = null;
                ElevationDegrees = null;
                AzimuthDegrees = null;
                SNR = null;
            }

            public string [] ToArray() {
                string[] tokens = new string[] {
                    PRN??string.Empty,
                    ElevationDegrees?.ToString("00")??string.Empty,
                    AzimuthDegrees?.ToString("000")??string.Empty,
                    SNR?.ToString()??string.Empty,
                };
                return tokens;
            }

            public void FromArray(string[] tokens, int index) {
                if (tokens.Length - index < 0) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                Clear();

                PRN = tokens[index];
                if (!string.IsNullOrWhiteSpace(tokens[index + 1])) {
                    ElevationDegrees = int.Parse(tokens[index + 1]);
                }
                if (!string.IsNullOrWhiteSpace(tokens[index + 2])) {
                    AzimuthDegrees = int.Parse(tokens[index + 2]);
                }
                if (!string.IsNullOrWhiteSpace(tokens[index + 3])) {
                    SNR = int.Parse(tokens[index + 3]);
                }
            }
        }
    }
}
