# InvernessPark.Utilities.NMEA
Simple NMEA-0183 parser in C#.

# Welcome!

This is a basic NMEA-0183 parser that processes bytes fed into its API. When a complete NMEA sentence is successfully read, application-level handlers are invoked.

# Quick Example

This is the simplest scenario: using the `DefaultNmeaHandler` class.  `DefaultNmeaHandler` implements the contract `INmeaHandler`, which defines support for the following NMEA-0183 data types:
* GGA : _Global Positioning System Fix Data_
* GSA : _Satellite status_
* GSV : _Satellites in view_
* GST : _GPS Pseudorange Noise Statistics_
* HDT : _NMEA heading log_
* RMC : _Recommended Minimum data for gps_
* VTG : _Track made good and ground speed_

`DefaultNmeaHandler` implements the contract by invoking the event handler `LogNmeaMessage` each time a supported NMEA sentence is successfully parsed.
    
>     using Invernesspark.Utilities.NMEA;
>     
>     ...
>     
>     // ... Create an object to handle parsed NMEA messages
>     DefaultNmeaHandler nmeaHandler = new DefaultNmeaHandler() ;
>     nmeaHandler.LogNmeaMessage += str => {
>         Console.WriteLine("New NMEA Message: {0}", str ) ;
>     };
>     
>     // ... Create the NMEA receiver
>     NmeaReceiver nmeaReceiver = new NmeaReceiver( nmeaHandler ) ;
>     
>     // ... Attach handler for NMEA messages that fail NMEA checksum verification
>     nmeaReceiver.NmeaMessageFailedChecksum += (bytes, index, count, expected, actual) => {
>         Console.Error.WriteLine("Failed Checksum: {0}; expected {1} but got {2}",
>             Encoding.ASCII.GetString(bytes.Skip(index).Take(count).ToArray()),
>             expected, actual );
>     };
>    
>     // ... Attach handler for NMEA messages that contain invalid syntax
>     nmeaReceiver.NmeaMessageDropped += (bytes, index, count, reason) => {
>         Console.WriteLine("Bad Syntax: {0}; reason: {1}",
>             Encoding.ASCII.GetString(bytes.Skip(index).Take(count).ToArray()),
>             reason );
>     };
>    
>     // ... Attach handler for NMEA messages that are ignored (unsupported)
>     nmeaReceiver.NmeaMessageIgnored += (bytes, index, count) => {
>         Console.WriteLine("Ignored: {0}", 
>             Encoding.ASCII.GetString(bytes.Skip(index).Take(count).ToArray()));
>     };
>    
>     // ... Your byte receiving logic...
>     bool keepReceiving = true ;
>     while ( keepReceiving ) {
>         byte [] bytesReceived = /* receive some bytes from socket, file, whatever... */ 
>    
>         // ... Feed the bytes into the NMEA receiver
>         nmeaReceiver.Receive( bytesReceived ) ;
>     }

The above code will invoke the appropriate callbacks each time a NMEA sentence is received.
