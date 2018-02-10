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
using System.Text;

namespace InvernessPark.Utilities.NMEA {

    /// <summary>
    /// Abstract base class wrapper for NMEA messages
    /// </summary>
    public abstract class BaseNmeaMessage : INmeaMessage {

        /// <summary>
        /// Field delimiter
        /// </summary>
        protected const string DELIM_FIELDS = ",";

        /// <summary>
        /// Field delimiter set
        /// </summary>
        static readonly char[] DELIMS = new char[] { ',' };

        /// <summary>
        /// Get the NMEA ckecksum
        /// </summary>
        public uint Checksum {
            get {
                byte[] payloadBytes = Encoding.ASCII.GetBytes(Payload);
                return ComputeChecksum(payloadBytes, 0, payloadBytes.Length);
            }
        }

        /// <summary>
        /// NMEA sentence without the leading "$" and the trailing "*" & checksum
        /// </summary>
        public abstract string Payload { get; }

        /// <summary>
        /// As per NMEA 0183 documentation
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Loads NMEA fields that have been parsed into stirng tokens
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public abstract INmeaMessage ParseFields(string[] tokens);

        /// <summary>
        /// Computes the checksum
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static UInt32 ComputeChecksum(byte[] bytes, int index, int count) {
            byte rc = 0;
            int maxLen = Math.Min(bytes.Length-index, count);
            for (int i = 0; i < maxLen; ++i) {
                rc ^= bytes[index + i];
            }
            return rc;
        }

        /// <summary>
        /// Stringification to full NMEA format, including SOM and EOM delimiters.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return "$" + Payload + "*" + (0xFF & Checksum).ToString("X2") + "\r\n";
        }
    }
}
