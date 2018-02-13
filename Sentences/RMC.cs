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
    /// NMEA-0183 RMC
    /// </summary>
    public class RMC : BaseNmeaMessage {
        public override string Description => Strings.rmc;

        public string DataTypeName { get; set; }
        public DateTime UTC {
            get {
                return _utc.ToLocalTime();
            }
            set {
                _utc = value.ToUniversalTime();
            }
        }
        private DateTime _utc;
        public StatusEnum Status { get; set; }
        public Latitude Latitude { get; set; }
        public Longitude Longitude { get; set; }
        public float SpeedAboveGroundKnots { get; set; }
        public float TrackAngleTrueNorthDegrees { get; set; }
        public float ?MagneticVariation { get; set; }
        public string ExtraField { get; set; }

        public RMC() {
            DataTypeName = "GPSRMC";
            Reset();
        }

        public void Reset() {
            _utc = DateTime.MinValue;
            Status = StatusEnum.Void;
            Latitude = 0;
            Longitude = 0;
            SpeedAboveGroundKnots = 0;
            TrackAngleTrueNorthDegrees = 0;
            MagneticVariation = null;
            ExtraField = null;
        }

        public override string Payload {
            get {
                List<string> tokens = new List<string> {
                    DataTypeName,
                    _utc.TimeOfDay.ToString(@"hhmmss\.fff"),
                    Status.ToNmeaString(),
                    Latitude.ToString(GeoAngleFormat.DMM, GeoAngleFormatOptions.Compact),
                    Longitude.ToString(GeoAngleFormat.DMM, GeoAngleFormatOptions.Compact),
                    SpeedAboveGroundKnots.ToString("0.0#"),
                    TrackAngleTrueNorthDegrees.ToString("0.0#"),
                    _utc.Date.ToString("ddMMyy"),
                    MagneticVariation == null ? string.Empty : Math.Abs( MagneticVariation.Value ).ToString("0.0"),
                    MagneticVariation == null ? string.Empty : MagneticVariation.Value < 0 ? "W" : "E"
                };
                if (ExtraField != null ) {
                    tokens.Add(ExtraField);
                }
                return string.Join(DELIM_FIELDS, tokens);
            }
        }

        public override INmeaMessage ParseFields(string[] tokens) {
            DataTypeName = tokens[0].TrimStart('$');
            TimeSpan ts = TimeSpan.ParseExact(tokens[1], @"hhmmss\.FFF", CultureInfo.InvariantCulture);
            Status = StatusEnumExtensions.FromNmeaString(tokens[2]);
            Latitude = Latitude.Parse(tokens[3] + DELIM_FIELDS + tokens[4], GeoAngleFormat.DMM, GeoAngleFormatOptions.Compact);
            Longitude = Longitude.Parse(tokens[5] + DELIM_FIELDS + tokens[6], GeoAngleFormat.DMM, GeoAngleFormatOptions.Compact);
            SpeedAboveGroundKnots = float.Parse(tokens[7]);
            TrackAngleTrueNorthDegrees = float.Parse(tokens[8]);
            DateTime date = DateTime.ParseExact(tokens[9], "ddMMyy", CultureInfo.InvariantCulture);
            _utc = new DateTime(date.Year, date.Month, date.Day, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

            if (!string.IsNullOrWhiteSpace(tokens[10])) {
                MagneticVariation = float.Parse(tokens[10]);
                if (!string.IsNullOrWhiteSpace(tokens[11])) {
                    int sign = tokens[11].Trim().ToUpper() == "W" ? -1 : 1;
                    MagneticVariation *= sign;
                }
            }
            if (tokens.Length > 12) {
                ExtraField = tokens[12];
            }
            return this;
        }
    }
}
