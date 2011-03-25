using System;
using System.IO;
using System.Threading.Tasks;

namespace Cryptophage
{
    /// <summary>
    /// Provides methods for executing GPG commands.
    /// </summary>
    public class GpgCommandExecutor
    {
        private readonly string _gpgPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgCommandExecutor"/> class.
        /// </summary>
        public GpgCommandExecutor()
        {
            _gpgPath = GpgPathFinder.GetGpgPath();

            if(_gpgPath == null)
                throw new FileNotFoundException("Could not automatically determine the location of the GPG executable. Use the GpgCommandExecutor constructor to specify the file path.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgCommandExecutor"/> class using
        /// the specified path.
        /// </summary>
        /// <param name="gpgPath">The path to the GPG executable.</param>
        public GpgCommandExecutor(string gpgPath)
        {
            _gpgPath = gpgPath;
        }

        /// <summary>
        /// Executes the provided <see cref="GpgCommand"/>.
        /// </summary>
        /// <param name="command">A <see cref="GpgCommand"/> instance to execute.</param>
        public void Execute(GpgCommand command)
        {
            Execute(command, null, null, TimeSpan.MaxValue);
        }

        /// <summary>
        /// Executes the provided <see cref="GpgCommand"/> using the provided input stream        
        /// </summary>
        /// <param name="command">A <see cref="GpgCommand"/> instance to execute.</param>
        /// <param name="output">A <see cref="Stream"/> into which output data will be written, or null
        /// if no output stream is required for the execution of the command.</param>
        public void Execute(GpgCommand command, Stream output)
        {
            Execute(command, null, output, TimeSpan.MaxValue);
        }

        /// <summary>
        /// Executes the provided <see cref="GpgCommand"/> using the provided input and
        /// output streams.
        /// </summary>
        /// <param name="command">A <see cref="GpgCommand"/> instance to execute.</param>
        /// <param name="input">A <see cref="Stream"/> containing input data, or null if no input stream
        /// is required for the execution of the command.</param>
        /// <param name="output">A <see cref="Stream"/> into which output data will be written, or null
        /// if no output stream is required for the execution of the command.</param>
        public void Execute(GpgCommand command, Stream input, Stream output)
        {
            Execute(command, input, output, TimeSpan.MaxValue);
        }

        /// <summary>
        /// Executes the provided <see cref="GpgCommand"/> using the provided input and
        /// output streams and the specified timeout value.
        /// </summary>
        /// <param name="command">A <see cref="GpgCommand"/> instance to execute.</param>
        /// <param name="input">A <see cref="Stream"/> containing input data, or null if no input stream
        /// is required for the execution of the command.</param>
        /// <param name="output">A <see cref="Stream"/> into which output data will be written, or null
        /// if no output stream is required for the execution of the command.</param>
        /// <param name="timeout">The amount of time to wait for the command to execute before throwing
        /// an exception.</param>
        public void Execute(GpgCommand command, Stream input, Stream output, TimeSpan timeout)
        {
            var process = new CommandLineProcess(_gpgPath, command.ToString());
            process.Run(input, output, timeout);
        }

        /// <summary>
        /// Begins executing the provided <see cref="GpgCommand"/> asynchronously using the 
        /// provided input and output streams.
        /// </summary>
        /// <param name="command">A <see cref="GpgCommand"/> instance to execute.</param>
        /// <param name="input">A <see cref="Stream"/> containing input data, or null if no input stream
        /// is required for the execution of the command.</param>
        /// <param name="output">A <see cref="Stream"/> into which output data will be written, or null
        /// if no output stream is required for the execution of the command.</param>
        /// <param name="callback">An optional asynchronous callback, to be called when 
        /// the execution is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular 
        /// asynchronous request from other requests.</param>
        /// <returns></returns>
        public IAsyncResult BeginExecute(GpgCommand command, Stream input, Stream output, AsyncCallback callback, object state)
        {
            var result = new AsyncResult<Stream>(callback, state);
            Task.Factory.StartNew(() => ExecuteAsync(command, input, output, result));
            return result;
        }

        /// <summary>
        /// Waits for the pending asynchronous execution to complete.
        /// </summary>
        /// <param name="result">The reference to the pending asynchronous operation to finish.</param>
        public void EndExecute(IAsyncResult result)
        {
            var async = result as AsyncResult;

            if(async == null)
                throw new ArgumentOutOfRangeException("result", result, "A mismatched IAsyncResult was provided.");
            
            async.Wait();
        }

        private void ExecuteAsync(GpgCommand command, Stream input, Stream output, AsyncResult result)
        {
            try
            {
                Execute(command, input, output);
                result.Complete();
            }
            catch(Exception ex)
            {
                result.Throw(ex);
            }
        }
    }
}