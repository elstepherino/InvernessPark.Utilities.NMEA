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

namespace InvernessPark.Utilities.NMEA {

    /// <summary>
    /// Convenience NMEA handler class, where all received messages are routed through 
    /// a single handler callback.
    /// 
    /// *** NOTE:
    /// If more NMEA data types are added to the INmeaHandler contract, this class will 
    /// need to be updated. 
    /// </summary>
    public class DefaultNmeaHandler : INmeaHandler {

        /// <summary>
        /// Handler callback for received NMEA messages
        /// </summary>
        /// <param name="msg"></param>
        public delegate void OnLogNmeaMessageHandler(string msg);
        public OnLogNmeaMessageHandler LogNmeaMessage;

        public void HandleGGA(INmeaMessage msg) {
            OnLogNmeaMessage(msg);
        }

        public void HandleGSA(INmeaMessage msg) {
            OnLogNmeaMessage(msg);
        }

        public void HandleGST(INmeaMessage msg) {
            OnLogNmeaMessage(msg);
        }

        public void HandleGSV(INmeaMessage msg) {
            OnLogNmeaMessage(msg);
        }

        public void HandleHDT(INmeaMessage msg) {
            OnLogNmeaMessage(msg);
        }

        public void HandleRMC(INmeaMessage msg) {
            OnLogNmeaMessage(msg);
        }

        public void HandleVTG(INmeaMessage msg) {
            OnLogNmeaMessage(msg);
        }

        private void OnLogNmeaMessage(INmeaMessage msg) {
            LogNmeaMessage?.Invoke(msg.ToString());
        }
    }
}
