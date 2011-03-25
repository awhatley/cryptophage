using System;
using System.Runtime.Serialization;

namespace Cryptophage
{
    /// <summary>
    /// The exception that is thrown when a <see cref="CommandLineProcess"/> writes data to 
    /// the standard error stream.
    /// </summary>
    [Serializable]
    public class ProcessExecutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessExecutionException"/> class.
        /// </summary>
        public ProcessExecutionException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessExecutionException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ProcessExecutionException(string message) : base(message) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessExecutionException"/> class
        /// with a specified error message and a reference to an inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ProcessExecutionException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessExecutionException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object
        /// data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual data
        /// about the source or destination.</param>
        protected ProcessExecutionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}