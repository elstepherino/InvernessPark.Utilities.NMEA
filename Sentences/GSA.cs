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
using System.Collections.Generic;

namespace InvernessPark.Utilities.NMEA.Sentences {

    /// <summary>
    /// NMEA-0183 GSA
    /// </summary>
    public class GSA : BaseNmeaMessage {
        public override string Description => Strings.gsa;

        public string DataTypeName { get; set; }
        public FixSelectionMode FixSelectionMode { get; set; }
        public Fix3DEnum Fix3D { get; set; }
        public string [] PRN { get; private set; }
        public float PDOP { get; set; }
        public float HDOP { get; set; }
        public float VDOP { get; set; }

        public GSA() {
            DataTypeName = "GPGSA";
            Reset();
        }

        public void Reset() {
            FixSelectionMode = FixSelectionMode.Auto;
            Fix3D = Fix3DEnum.NoFix;
            PRN = new string[12];
            ClearPRN();
            PDOP = 99;
            HDOP = 99;
            VDOP = 99;
        }
        
        public override string Payload {
            get {
                List<string> tokens = new List<string> {
                    DataTypeName,
                    FixSelectionMode.ToNmeaString(),
                    ((int)Fix3D).ToString()
                };
                tokens.AddRange(PRN);
                tokens.Add(PDOP.ToString("0.0#"));
                tokens.Add(HDOP.ToString("0.0#"));
                tokens.Add(VDOP.ToString("0.0#"));
                return string.Join(DELIM_FIELDS, tokens);
            }
        }

        public override INmeaMessage ParseFields(string[] tokens) {
            DataTypeName = tokens[0].TrimStart('$');
            FixSelectionMode = FixSelectionModeExtensions.FromNmeaString(tokens[1]);

            Fix3D = (Fix3DEnum)int.Parse(tokens[2]);
            for (int i = 0; i < 12; ++i) {
                PRN[i] = tokens[3 + i];
            }
            PDOP = float.Parse(tokens[15]);
            HDOP = float.Parse(tokens[16]);
            VDOP = float.Parse(tokens[17]);
            return this;
        }

        private void ClearPRN() {
            for (int i = 0; i < PRN.Length; ++i) {
                PRN[i] = string.Empty;
            }
        }
    }
}
