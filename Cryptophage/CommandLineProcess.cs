using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Cryptophage
{
    /// <summary>
    /// Provides methods for executing and reading data from non-interactive command-line processes.
    /// </summary>
    public class CommandLineProcess
    {
        private readonly string _command;
        private readonly string _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineProcess"/> class for the
        /// specified executable command with the specified arguments.
        /// </summary>
        /// <param name="command">The command-line executable to run.</param>
        /// <param name="arguments">The arguments to be passed to the executable.</param>
        public CommandLineProcess(string command, string arguments)
        {
            _command = command;
            _arguments = arguments;
        }

        /// <summary>
        /// Executes the associated command-line process without redirecting standard input or output.
        /// </summary>
        public void Run()
        {
            Run(null, null, TimeSpan.Zero);
        }

        /// <summary>
        /// Executes the associated command-line process without redirecting standard input or
        /// output, waiting the specified amount of time for the process to complete.
        /// </summary>
        /// <param name="timeout">The amount of time to wait for the process to finish executing
        /// before throwing an exception.</param>
        public void Run(TimeSpan timeout)
        {
            Run(null, null, timeout);
        }

        /// <summary>
        /// Executes the associated command-line process, redirecting standard output to the provided
        /// stream.
        /// </summary>
        /// <param name="output">A stream into which the output of the process will be written, or null
        /// to disable standard output redirection.</param>
        public void Run(Stream output)
        {
            Run(null, output, TimeSpan.Zero);
        }

        /// <summary>
        /// Executes the associated command-line process, redirecting standard output to the provided
        /// stream and waiting the specified amount of time for the process to complete.
        /// </summary>
        /// <param name="output">A stream into which the output of the process will be written, or null
        /// to disable standard output redirection.</param>
        /// <param name="timeout">The amount of time to wait for the process to finish executing
        /// before throwing an exception.</param>
        public void Run(Stream output, TimeSpan timeout)
        {
            Run(null, output, timeout);
        }

        /// <summary>
        /// Executes the associated command-line process, redirecting standard input and output 
        /// to the provided streams.
        /// </summary>
        /// <param name="input">A stream whose contents will be written to standard input, or null
        /// to disable standard input redirection.</param>
        /// <param name="output">A stream into which the output of the process will be written, or null
        /// to disable standard output redirection.</param>
        public void Run(Stream input, Stream output)
        {
            Run(input, output, TimeSpan.Zero);
        }

        /// <summary>
        /// Executes the associated command-line process, redirecting standard input and output 
        /// to the provided streams and waiting the specified amount of time for the process 
        /// to complete.
        /// </summary>
        /// <param name="input">A stream whose contents will be written to standard input, or null
        /// to disable standard input redirection.</param>
        /// <param name="output">A stream into which the output of the process will be written, or null
        /// to disable standard output redirection.</param>
        /// <param name="timeout">The amount of time to wait for the process to finish executing
        /// before throwing an exception.</param>
        public void Run(Stream input, Stream output, TimeSpan timeout)
        {
            var startInfo = new ProcessStartInfo {
                    FileName = _command,
                    Arguments = _arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = input != null,
                    RedirectStandardOutput = output != null,
                };

            using(var process = new Process { StartInfo = startInfo })
            {
                process.Start();

                using(var inputTask = Task.Factory.StartNew(() => CopyInputStream(process, input)))
                using(var outputTask = Task.Factory.StartNew(() => CopyOutputStream(process, output)))
                using(var errorTask = Task.Factory.StartNew(() => ReadErrorStream(process)))
                using(var timeoutTask = Task.Factory.StartNew(() => WaitForExit(process, timeout)))
                {
                    try
                    {
                        Task.WaitAll(inputTask, outputTask, errorTask, timeoutTask);
                    }

                    catch(AggregateException ex)
                    {
                        var flattened = ex.Flatten();
                        if(flattened.InnerExceptions.Count == 1)
                            throw flattened.InnerException;

                        throw flattened;
                    }

                    if(process.ExitCode != 0 && !String.IsNullOrWhiteSpace(errorTask.Result))
                        throw new ProcessExecutionException(errorTask.Result);
                }
            }
        }

        private static void CopyInputStream(Process process, Stream input)
        {
            if(input == null)
                return;

            input.CopyDynamic(process.StandardInput.BaseStream);
            process.StandardInput.Dispose();
        }

        private static void CopyOutputStream(Process process, Stream output)
        {
            if(output == null)
                return;

            process.StandardOutput.BaseStream.CopyDynamic(output);
            process.StandardOutput.Dispose();
        }

        private static string ReadErrorStream(Process process)
        {
            var errorMessage = process.StandardError.ReadToEnd();
            process.StandardError.Dispose();

            return errorMessage;
        }

        private static void WaitForExit(Process process, TimeSpan timeout)
        {
            var milliseconds = (timeout == TimeSpan.MaxValue) ? -1 : 
                (int)Math.Min(timeout.TotalMilliseconds, Int32.MaxValue);

            if(!process.WaitForExit(milliseconds))
            {
                process.Kill();
                
                throw new TimeoutException(String.Format(
                    "A timeout occurred after {0} milliseconds waiting for the {1} process to complete.",
                    timeout, process.ProcessName));
            }

            process.WaitForExit(); // ensures asynchronous stream redirection has completed
        }
    }
}