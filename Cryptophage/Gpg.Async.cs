using System;
using System.Threading.Tasks;

namespace Cryptophage
{
    public partial class Gpg
    {
        /// <summary>
        /// Provides asynchronous APIs for basic GPG cryptographic functions.
        /// </summary>
        public static class Async
        {
            /// <summary>
            /// Begins an asynchronous encrypt operation.
            /// </summary>
            /// <param name="inputFile">The full path of the file to encrypt.</param>
            /// <param name="outputFile">A path for the resulting encrypted file.</param>
            /// <param name="recipient">An identifier for the target recipient, used to 
            /// determine which public key to use.</param>
            /// <param name="callback">An optional asynchronous callback, to be called when 
            /// the encrypt operation is complete.</param>
            /// <param name="state">A user-provided object that distinguishes this particular 
            /// asynchronous encrypt request from other requests.</param>
            /// <returns>An <see cref="IAsyncResult"/> that represents the asynchronous encrypt
            /// operation, which could still be pending.</returns>
            public static IAsyncResult BeginEncrypt(string inputFile, string outputFile, string recipient, AsyncCallback callback, object state)
            {
                return BeginExecute(callback, state, () => Encrypt(inputFile, outputFile, recipient));
            }

            /// <summary>
            /// Waits for the pending asynchronous encrypt operation to complete.
            /// </summary>
            /// <param name="result">The reference to the pending asynchronous encrypt operation 
            /// to finish.</param>
            public static void EndEncrypt(IAsyncResult result)
            {
                EndExecute(result);
            }

            /// <summary>
            /// Begins an asynchronous encrypt and sign operation.
            /// </summary>
            /// <param name="inputFile">The full path of the file to encrypt.</param>
            /// <param name="outputFile">A path for the resulting encrypted file.</param>
            /// <param name="recipient">An identifier for the target recipient, used to 
            /// determine which public key to use.</param>
            /// <param name="passphrase">The private key passphrase to use for signing the encrypted file.</param>
            /// <param name="callback">An optional asynchronous callback, to be called when 
            /// the encrypt and sign operation is complete.</param>
            /// <param name="state">A user-provided object that distinguishes this particular 
            /// asynchronous encrypt and sign request from other requests.</param>
            /// <returns>An <see cref="IAsyncResult"/> that represents the asynchronous encrypt
            /// and sign operation, which could still be pending.</returns>
            public static IAsyncResult BeginEncryptAndSign(string inputFile, string outputFile, string recipient, string passphrase, AsyncCallback callback, object state)
            {
                return BeginExecute(callback, state, () => EncryptAndSign(inputFile, outputFile, recipient, passphrase));
            }

            /// <summary>
            /// Waits for the pending asynchronous encrypt and sign operation to complete.
            /// </summary>
            /// <param name="result">The reference to the pending asynchronous encrypt and sign
            /// operation to finish.</param>
            public static void EndEncryptAndSign(IAsyncResult result)
            {
                EndExecute(result);
            }

            /// <summary>
            /// Begins an asynchronous decrypt operation.
            /// </summary>
            /// <param name="inputFile">The full path of the file to decrypt.</param>
            /// <param name="outputFile">A path for the resulting decrypted file.</param>
            /// <param name="passphrase">The private key passphrase to use for decrypting the encrypted file.</param>
            /// <param name="callback">An optional asynchronous callback, to be called when 
            /// the decrypt operation is complete.</param>
            /// <param name="state">A user-provided object that distinguishes this particular 
            /// asynchronous decrypt request from other requests.</param>
            /// <returns>An <see cref="IAsyncResult"/> that represents the asynchronous decrypt
            /// operation, which could still be pending.</returns>
            public static IAsyncResult BeginDecrypt(string inputFile, string outputFile, string passphrase, AsyncCallback callback, object state)
            {
                return BeginExecute(callback, state, () => Decrypt(inputFile, outputFile, passphrase));
            }

            /// <summary>
            /// Waits for the pending asynchronous decrypt operation to complete.
            /// </summary>
            /// <param name="result">The reference to the pending asynchronous decrypt operation 
            /// to finish.</param>
            public static void EndDecrypt(IAsyncResult result)
            {
                EndExecute(result);
            }

            /// <summary>
            /// Begins an asynchronous sign operation.
            /// </summary>
            /// <param name="inputFile">The full path of the file to sign.</param>
            /// <param name="outputFile">A path for the resulting signature or signed file.</param>
            /// <param name="detached">Whether to store only the signature in the output file.</param>
            /// <param name="cleartext">Whether the output file should be written in plain text.</param>
            /// <param name="passphrase">The private key passphrase to use for signing the file.</param>
            /// <param name="callback">An optional asynchronous callback, to be called when 
            /// the sign operation is complete.</param>
            /// <param name="state">A user-provided object that distinguishes this particular 
            /// asynchronous sign request from other requests.</param>
            /// <returns>An <see cref="IAsyncResult"/> that represents the asynchronous sign
            /// operation, which could still be pending.</returns>
            public static IAsyncResult BeginSign(string inputFile, string outputFile, bool detached, bool cleartext, string passphrase, AsyncCallback callback, object state)
            {
                return BeginExecute(callback, state, () => Sign(inputFile, outputFile, detached, cleartext, passphrase));
            }

            /// <summary>
            /// Waits for the pending asynchronous sign operation to complete.
            /// </summary>
            /// <param name="result">The reference to the pending asynchronous sign operation 
            /// to finish.</param>
            public static void EndSign(IAsyncResult result)
            {
                EndExecute(result);
            }

            /// <summary>
            /// Begins an asynchronous verify operation on the specified message stream.
            /// </summary>
            /// <param name="inputFile">The full path of the signed file to verify.</param>
            /// <param name="callback">An optional asynchronous callback, to be called when 
            /// the verify operation is complete.</param>
            /// <param name="state">A user-provided object that distinguishes this particular 
            /// asynchronous verify request from other requests.</param>
            /// <returns>An <see cref="IAsyncResult"/> that represents the asynchronous verify
            /// operation, which could still be pending.</returns>
            public static IAsyncResult BeginVerify(string inputFile, AsyncCallback callback, object state)
            {
                return BeginExecute(callback, state, () => Verify(inputFile));
            }

            /// <summary>
            /// Waits for the pending asynchronous verify operation to complete.
            /// </summary>
            /// <param name="result">The reference to the pending asynchronous verify operation 
            /// to finish.</param>
            /// <returns>A value indicating whether the signature is valid.</returns>
            public static bool EndVerify(IAsyncResult result)
            {
                return EndExecute<bool>(result);
            }

            /// <summary>
            /// Begins retrieving an array of public keys stored by the key store.
            /// </summary>
            /// <param name="callback">An optional asynchronous callback, to be called when 
            /// the retrieval operation is complete.</param>
            /// <param name="state">A user-provided object that distinguishes this particular 
            /// asynchronous retrieval request from other requests.</param>
            /// <returns>An <see cref="IAsyncResult"/> that represents the asynchronous retrieval
            /// operation, which could still be pending.</returns>
            public static IAsyncResult BeginGetPublicKeys(AsyncCallback callback, object state)
            {
                return BeginExecute(callback, state, () => GetPublicKeys());
            }

            /// <summary>
            /// Waits for the pending asynchronous retrieval operation to complete.
            /// </summary>
            /// <param name="result">The reference to the pending asynchronous retrieval operation 
            /// to finish.</param>
            /// <returns>An array containing key information for the public keys.</returns>
            public static GpgKey[] EndGetPublicKeys(IAsyncResult result)
            {
                return EndExecute<GpgKey[]>(result);
            }

            /// <summary>
            /// Begins retrieving an array of private keys stored by the key store.
            /// </summary>
            /// <param name="callback">An optional asynchronous callback, to be called when 
            /// the retrieval operation is complete.</param>
            /// <param name="state">A user-provided object that distinguishes this particular 
            /// asynchronous retrieval request from other requests.</param>
            /// <returns>An <see cref="IAsyncResult"/> that represents the asynchronous retrieval
            /// operation, which could still be pending.</returns>
            public static IAsyncResult BeginGetPrivateKeys(AsyncCallback callback, object state)
            {
                return BeginExecute(callback, state, () => GetPrivateKeys());
            }

            /// <summary>
            /// Waits for the pending asynchronous retrieval operation to complete.
            /// </summary>
            /// <param name="result">The reference to the pending asynchronous retrieval operation 
            /// to finish.</param>
            /// <returns>An array containing key information for the private keys.</returns>
            public static GpgKey[] EndGetPrivateKeys(IAsyncResult result)
            {
                return EndExecute<GpgKey[]>(result);
            }

            private static IAsyncResult BeginExecute(AsyncCallback callback, object state, Action command)
            {
                var result = new AsyncResult(callback, state);
            
                Task.Factory.StartNew(() => {
                    try
                    {
                        command();
                        result.Complete();
                    }
                    catch(Exception e)
                    {
                        result.Throw(e);
                    }
                });
            
                return result;
            }

            private static IAsyncResult BeginExecute<T>(AsyncCallback callback, object state, Func<T> command)
            {
                var result = new AsyncResult<T>(callback, state);
            
                Task.Factory.StartNew(() => {
                    try
                    {
                        result.Complete(command());
                    }
                    catch(Exception e)
                    {
                        result.Throw(e);
                    }
                });
            
                return result;
            }

            private static void EndExecute(IAsyncResult result)
            {
                var async = result as AsyncResult;
            
                if(async == null)
                    throw new ArgumentOutOfRangeException("result", result, "A mismatched IAsyncResult was provided.");

                async.Wait();
            }

            private static T EndExecute<T>(IAsyncResult result)
            {
                var async = result as AsyncResult<T>;
            
                if(async == null)
                    throw new ArgumentOutOfRangeException("result", result, "A mismatched IAsyncResult was provided.");

                return async.ReturnValue;
            }
        }
    }
}