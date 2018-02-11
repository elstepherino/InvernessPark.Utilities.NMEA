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
using InvernessPark.Utilities.NMEA.Sentences;
using System;
using System.Text;

namespace InvernessPark.Utilities.NMEA {

    /// <summary>
    /// This class implements a NMEA receiver.  Use an instance ofthis objects to 
    /// feed bytes via the 'Receive()' method.  
    /// 
    /// </summary>
    public class NmeaReceiver {

        /// <summary>
        /// When a NMEA sentence fails its checksum check
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public delegate void OnNmeaMessageFailedChecksumHandler(byte[] bytes, int index, int count, UInt32 expected, UInt32 actual );
        public event OnNmeaMessageFailedChecksumHandler NmeaMessageFailedChecksum;

        /// <summary>
        /// When a NMEA sentence is corrupted and dropped
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="reason"></param>
        public delegate void OnNmeaMessageDroppedHandler(byte[] bytes, int index, int count, string reason);
        public event OnNmeaMessageDroppedHandler NmeaMessageDropped;

        /// <summary>
        /// When a NMEA sentence passes the checksum check, but is not supported
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="reason"></param>
        public delegate void OnNmeaMessageIgnoredHandler(byte[] bytes, int index, int count);
        public event OnNmeaMessageIgnoredHandler NmeaMessageIgnored;

        /// <summary>
        /// Field delimiter
        /// </summary>
        private static readonly char [] DELIMS = { ',' };

        /// <summary>
        /// Start of NMEA message delimiter
        /// </summary>
        private static readonly byte DELIM_SOM = Convert.ToByte('$');

        /// <summary>
        /// Checksum delimiter
        /// </summary>
        private static readonly byte DELIM_CKSUM = Convert.ToByte('*');

        /// <summary>
        /// First End of message delimiter
        /// </summary>
        private static readonly byte DELIM_CR = Convert.ToByte('\r');

        /// <summary>
        /// Second end of message delimiter
        /// </summary>
        private static readonly byte DELIM_LF = Convert.ToByte('\n');

        /// <summary>
        /// NMEA stream instance used to parse received bytes
        /// </summary>
        private INmeaStream _stream;

        /// <summary>
        /// NMEA message handler provided by the application
        /// </summary>
        private INmeaHandler _handler;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="stream"></param>
        /// <param name="handler"></param>
        public NmeaReceiver( Func<INmeaStream> streamFactory, INmeaHandler handler ) {
            _stream = streamFactory();
            _handler = handler ;
            _stream.NMEAMessageReceived += Stream_NMEAMessageReceived;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="streamFactory"></param>
        /// <param name="handler"></param>
        public NmeaReceiver(INmeaHandler handler) {
            _stream = new NmeaStream();
            _handler = handler;
            _stream.NMEAMessageReceived += Stream_NMEAMessageReceived;
        }

        /// <summary>
        /// Receives an arbitray number of bytes, to be parsed and handled
        /// </summary>
        /// <param name="bytes"></param>
        public void Receive(byte[] bytes) {
            foreach (byte b in bytes) {
                _stream.Append(b);
            }
        }

        /// <summary>
        /// Invokes the proper handler for a NMEA message
        /// 
        /// *** NOTE: If additional NMEA data types are added to the INmeaHandler
        /// contract, this method will need to be updated to support it.
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        private void Stream_NMEAMessageReceived(byte[] bytes, int index, int count) {

            // ... Parse the bytes
            int somOffset = index;
            int crOffset = index + count - 2;
            int lnOffset = index + count - 1;
            int cksumOffset = index + count - 5;
            int payloadLen = cksumOffset - 1;

            // .... Sanity check
            if (bytes.Length <= 4) {
                OnNmeaMessageDropped(bytes, index, count, "Insufficient number of bytes");
            }
            if (bytes[somOffset] != DELIM_SOM) {
                OnNmeaMessageDropped(bytes, index, count, "Invalid start of message");
            }
            if (bytes[crOffset] != DELIM_CR) {
                OnNmeaMessageDropped(bytes, index, count, "Invalid end of message delimiter (no CR)");
            }
            if (bytes[lnOffset] != DELIM_LF) {
                OnNmeaMessageDropped(bytes, index, count, "Invalid end of message delimiter (no LF)");
            }
            if (bytes[cksumOffset] != DELIM_CKSUM) {
                OnNmeaMessageDropped(bytes, index, count, "Invalid checksum delimiter");
            }

            // ... Verify checksum
            string cksumStr = Encoding.ASCII.GetString(bytes, cksumOffset+1, 2);
            UInt32 msgCksum = Convert.ToUInt32(cksumStr, 16);
            UInt32 compCksum = BaseNmeaMessage.ComputeChecksum(bytes, 1, payloadLen);
            if (msgCksum != compCksum) {
                OnNmeaMessageFailedChecksum(bytes, index, count, compCksum, msgCksum );
                return;
            }

            // ... Split up the NMEA sentence into parameters/tokens
            string nmeaSentence = Encoding.ASCII.GetString(bytes, index + 1, payloadLen);
            string[] tokens = nmeaSentence.Split(DELIMS);

            // ... Parse by NMEA data type
            try {
                switch (tokens[0].Substring(Math.Max(0, tokens[0].Length - 3)).ToUpper()) {
                    case "GGA":
                        _handler.HandleGGA(new GGA().ParseFields(tokens));
                        break;
                    case "GSA":
                        _handler.HandleGSA(new GSA().ParseFields(tokens));
                        break;
                    case "GST":
                        _handler.HandleGST(new GST().ParseFields(tokens));
                        break;
                    case "HDT":
                        _handler.HandleHDT(new HDT().ParseFields(tokens));
                        break;
                    case "GSV":
                        _handler.HandleGSV(new GSV().ParseFields(tokens));
                        break;
                    case "RMC":
                        _handler.HandleRMC(new RMC().ParseFields(tokens));
                        break;
                    case "VTG":
                        _handler.HandleVTG(new VTG().ParseFields(tokens));
                        break;
                    default:
                        OnNmeaMessageIgnored(bytes, index, count);
                        break;
                }
            }
            catch (NotImplementedException) {
                OnNmeaMessageIgnored(bytes, index, count);
            }
        }

        /// <summary>
        /// Called when ckecksum fails
        /// </summary>
        /// <param name="bytes"></param>
        protected void OnNmeaMessageFailedChecksum(byte[] bytes, int index, int count, UInt32 expected, UInt32 actual) {
            NmeaMessageFailedChecksum?.Invoke(bytes, index, count, expected, actual);
        }

        /// <summary>
        /// Called when a message payload is invalid and the message must be dropped
        /// </summary>
        /// <param name="bytes"></param>
        protected void OnNmeaMessageDropped(byte[] bytes, int index, int count, string reason) {
            NmeaMessageDropped?.Invoke(bytes, index, count, reason);
        }

        /// <summary>
        /// Called when a valid NMEA sentence is not supported
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        protected void OnNmeaMessageIgnored(byte[] bytes, int index, int count) {
            NmeaMessageIgnored?.Invoke(bytes,index,count);
        }
    }
}
