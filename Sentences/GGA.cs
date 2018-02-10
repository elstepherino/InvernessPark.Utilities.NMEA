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
using System;
using System.Collections.Generic;
using System.Globalization;

namespace InvernessPark.Utilities.NMEA.Sentences {

    /// <summary>
    /// NMEA-0183 GGA
    /// </summary>
    public class GGA : BaseNmeaMessage {

        public string DataTypeName { get; set; }
        public TimeSpan UTC { get; set; }
        public Latitude Latitude { get; set; }
        public Longitude Longitude { get; set; }
        public FixQualityEnum FixQuality { get ; set; }
        public int SatelliteCount { get; set; }
        public float HDOP { get; set; }
        public float Altitude { get; set; }
        public float GeoidHeight { get; set; }

        public GGA() {
            DataTypeName = "GPGGA";
            Reset();
        }

        public void Reset() {
            UTC = TimeSpan.MinValue;
            Latitude = 0;
            Longitude = 0;
            FixQuality = FixQualityEnum.Invalid;
            SatelliteCount = 0;
            HDOP = 99;
            Altitude = 0;
            GeoidHeight = 0;
        }

        public override INmeaMessage ParseFields(string[] tokens) {
            DataTypeName = tokens[0].TrimStart('$');
            UTC = TimeSpan.ParseExact(tokens[1], @"hhmmss\.FFF", CultureInfo.InvariantCulture);
            Latitude = Latitude.Parse(tokens[2]+ DELIM_FIELDS + tokens[3], GeoAngle.Format.DMM, GeoAngle.FormatOptions.Compact);
            Longitude = Longitude.Parse(tokens[4]+ DELIM_FIELDS + tokens[5], GeoAngle.Format.DMM, GeoAngle.FormatOptions.Compact);
            FixQuality = ((FixQualityEnum[])Enum.GetValues(typeof(FixQualityEnum)))[int.Parse(tokens[6])];
            SatelliteCount = Int32.Parse(tokens[7]);
            HDOP = float.Parse(tokens[8]);
            Altitude = float.Parse(tokens[9]);
            GeoidHeight = float.Parse(tokens[11]);
            return this;
        }

        public override string Payload {
            get {
                List<string> tokens = new List<string> {
                    DataTypeName,
                    UTC.ToString(@"hhmmss\.fff"),
                    Latitude.ToString(GeoAngle.Format.DMM, GeoAngle.FormatOptions.Compact),
                    Longitude.ToString(GeoAngle.Format.DMM, GeoAngle.FormatOptions.Compact),
                    ((int)FixQuality).ToString(),
                    SatelliteCount.ToString(),
                    HDOP.ToString("0.0#"),
                    Altitude.ToString("0.0") + DELIM_FIELDS + "M",
                    GeoidHeight.ToString("0.0") + DELIM_FIELDS + "M",
                    "",
                    ""
                };
                return string.Join(DELIM_FIELDS, tokens);
            }
        }

        public override string Description => Strings.gga;
    }
}
