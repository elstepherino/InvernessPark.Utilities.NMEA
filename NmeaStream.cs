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

namespace InvernessPark.Utilities.NMEA {

    /// <summary>
    /// Default implementation of the NMEA stream contract
    /// </summary>
    public class NmeaStream : INmeaStream {

        /// <summary>
        /// NMEA-0183 mandates an 80 byte message limit, but nobody follows this rule.
        /// So let's be safe and use a big enough buffer.
        /// </summary>
        private const int MAX_RECEIVE_SIZE = 1024;

        /// <summary>
        /// Start of NMEA sentence
        /// </summary>
        private readonly byte DELIM_SOM = Convert.ToByte('$');

        /// <summary>
        /// End of NMEA sentence
        /// </summary>
        private readonly byte DELIM_EOM = Convert.ToByte('\n');

        /// <summary>
        /// Placeholder for received bytes
        /// </summary>
        private byte[] _buffer = new byte[MAX_RECEIVE_SIZE];

        /// <summary>
        /// Offset in the buffer, indicating the number of bytes buffered thus far.
        /// </summary>
        private int _offset = 0;

        /// <summary>
        /// State of NMEA parsing
        /// </summary>
        private enum State {
            Idle,   // Currently not parsing a NMEA sentence (buffering bytes)
            Payload // Actively parsing a NMEA sentence
        }

        /// <summary>
        /// DEfault starting state
        /// </summary>
        private State _state = State.Idle;
        
        /// <summary>
        /// NUmber of bytes buffered thus far.
        /// </summary>
        public int Length {
            get {
                return _offset;
            }
        }

        /// <summary>
        /// Total number of bytes that can be buffered
        /// </summary>
        public int Capacity {
            get {
                return _buffer.Length;
            }
        }

        /// <summary>
        /// Available space left in the buffer
        /// </summary>
        public int Available {
            get {
                return Capacity - Length;
            }
        }

        /// <summary>
        /// Callback invoked when a new NMEA message has been parsed
        /// </summary>
        public event OnNMEAMessageReceivedHandler NMEAMessageReceived;

        /// <summary>
        /// Appends a byte to the buffer, and invokes the NMEA message callback if a complete NMEA sentence has been received.
        /// </summary>
        /// <param name="b"></param>
        public void Append(byte b) {

            switch (_state) {
                // ... If we're waiting for the start of NMEA message
                case State.Idle:

                    // ... If the byte is indeed the SOM
                    if (b == DELIM_SOM) {
                        
                        // ... Append the byte to the buffer
                        _buffer[_offset++] = b;
                        
                        // ... We are now officially parsing a NMEA message
                        _state = State.Payload;
                    }

                    // *** Note: we only buffe rbytes if we're parsing a NMEA sentence
                    break;

                // ... If we're actively in the process of parsing a NMEA message
                case State.Payload:
                    
                    bool isMessageReceived = false;

                    // ... If we get yet another SOM, assume corruption and use this as the new SOM position
                    if (b == DELIM_SOM) {
                        _offset = 0;
                    }
                    // ... If the byte is the End of NMEA message, we have received a complete NMEA sentence
                    else if (b == DELIM_EOM) {
                        isMessageReceived = true;
                    }

                    // ... AAppend the byte to the buffer
                    _buffer[_offset++] = b;

                    // ... If we have a complete NMEA sentence
                    if (isMessageReceived) {
                        // ... Invoke the callback
                        OnNMEAMessageReceived(_buffer, 0, Length);
                        // ... Then reset the state and the buffer
                        Reset();
                    }
                    break;
            }

            // ... If we;ve run out of space
            if (Available == 0) {
                // ... reset everything
                _state = State.Idle;
                Reset();
            }
        }

        /// <summary>
        /// ersets the parsing state and offset in the receive buffer
        /// </summary>
        public void Reset() {
            _offset = 0;
            _state = State.Idle;
        }

        /// <summary>
        /// Invokes the NMEA message handler callback
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        private void OnNMEAMessageReceived(byte[] bytes, int index, int count) {
            NMEAMessageReceived?.Invoke(bytes, index, count);
        }
    }
}
