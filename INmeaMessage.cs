﻿/*
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
    /// NMEA message contract
    /// </summary>
    public interface INmeaMessage {

        /// <summary>
        /// As per NMEA 0183
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Loads NMEA fields that have been parsed into string tokens
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        INmeaMessage ParseFields(string[] tokens);
        
        /// <summary>
        /// NMEA sentence without the leading "$" and the trailing "*" & checksum
        /// </summary>
        string Payload { get; }

        /// <summary>
        /// Payload checksum
        /// </summary>
        UInt32 Checksum { get; }
    }
}
