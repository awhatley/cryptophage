using System;
using System.IO;

namespace Cryptophage
{
    /// <summary>
    /// Extension methods for <see cref="Stream"/> instances.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all the bytes from the source stream and writes them to the destination
        /// stream, dynamically expanding the buffer as required.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <param name="destination">The destination stream.</param>
        public static void CopyDynamic(this Stream source, Stream destination)
        {
            if(source == null)
                throw new ArgumentNullException("source", "Source stream cannot be null.");
            
            if(destination == null)
                throw new ArgumentNullException("destination", "Destination stream cannot be null.");
            
            if(!source.CanRead)
                throw new NotSupportedException("The source stream does not support reading.");

            if(!destination.CanWrite)
                throw new NotSupportedException("The destination stream does not support writing.");

            var bufferSize = 256;
            var buffer = new byte[bufferSize];

            while(true)
            {
                int bytesRead;
                
                do
                {
                    bytesRead = source.Read(buffer, 0, bufferSize);
                    if(bytesRead <= 0)
                        return;

                    destination.Write(buffer, 0, bytesRead);
                }
                while((bufferSize >= 65536) || (bytesRead != bufferSize));

                bufferSize *= 4;
                buffer = new byte[bufferSize];
            }
        }
    }
}