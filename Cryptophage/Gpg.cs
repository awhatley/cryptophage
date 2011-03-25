using System;
using System.IO;

namespace Cryptophage
{
    /// <summary>
    /// A simplified static helper class for basic cryptographic functions of the GPG command-line 
    /// client. For access to the full range of commands and options, see the <see cref="GpgCommandExecutor"/>
    /// class.
    /// </summary>
    public static partial class Gpg
    {
        /// <summary>
        /// Encrypts the specified input file using the provided recipient's public key, 
        /// storing the result in the specified output file.
        /// </summary>
        /// <param name="inputFile">The full path of the file to encrypt.</param>
        /// <param name="outputFile">A path for the resulting encrypted file.</param>
        /// <param name="recipient">An identifier for the target recipient, used to 
        /// determine which public key to use.</param>
        public static void Encrypt(string inputFile, string outputFile, string recipient)
        {
            var command = GpgCommand.Encrypt
                .Batch(true)
                .Quiet()
                .TrustModel(GpgTrustModel.Always)
                .InputFile(inputFile)
                .OutputFile(outputFile)
                .Recipient(recipient);

            new GpgCommandExecutor().Execute(command, null, null, TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Encrypts and cryptographically signs the specified input file using the provided 
        /// recipient's public key and the provided passphrase, storing the result in the specified 
        /// output file.
        /// </summary>
        /// <param name="inputFile">The full path of the file to encrypt.</param>
        /// <param name="outputFile">A path for the resulting encrypted file.</param>
        /// <param name="recipient">An identifier for the target recipient, used to 
        /// determine which public key to use.</param>
        /// <param name="passphrase">The private key passphrase to use for signing the encrypted file.</param>
        public static void EncryptAndSign(string inputFile, string outputFile, string recipient, string passphrase)
        {
            var command = GpgCommand.EncryptSign
                .Batch(true)
                .Quiet()
                .TrustModel(GpgTrustModel.Always)
                .InputFile(inputFile)
                .OutputFile(outputFile)
                .Recipient(recipient)
                .Passphrase(passphrase);
        
            new GpgCommandExecutor().Execute(command, null, null, TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Decrypts the specified input file using the provided private key passphrase, storing
        /// the result in the specified output file.
        /// </summary>
        /// <param name="inputFile">The full path of the file to decrypt.</param>
        /// <param name="outputFile">A path for the resulting decrypted file.</param>
        /// <param name="passphrase">The private key passphrase to use for decrypting the encrypted file.</param>
        public static void Decrypt(string inputFile, string outputFile, string passphrase)
        {
            var command = GpgCommand.Decrypt
                .Batch(true)
                .Quiet()
                .TrustModel(GpgTrustModel.Always)
                .InputFile(inputFile)
                .OutputFile(outputFile)
                .Passphrase(passphrase);
        
            new GpgCommandExecutor().Execute(command, null, null, TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Creates a signature for the specified input file, optionally making the signature detached
        /// (as a separate file) or in plain text.
        /// </summary>
        /// <param name="inputFile">The full path of the file to sign.</param>
        /// <param name="outputFile">A path for the resulting signature or signed file.</param>
        /// <param name="detached">Whether to store only the signature in the output file.</param>
        /// <param name="cleartext">Whether the output file should be written in plain text.</param>
        /// <param name="passphrase">The private key passphrase to use for signing the file.</param>
        public static void Sign(string inputFile, string outputFile, bool detached, bool cleartext, string passphrase)
        {
            var command = detached
                ? cleartext 
                    ? GpgCommand.SignDetached.ArmoredOutput() 
                    : GpgCommand.SignDetached.NonArmoredInput()
                : cleartext 
                    ? GpgCommand.ClearSign 
                    : GpgCommand.Sign
                .Batch(true)
                .Quiet()
                .TrustModel(GpgTrustModel.Always)
                .InputFile(inputFile)
                .OutputFile(outputFile)
                .Passphrase(passphrase);

            new GpgCommandExecutor().Execute(command, null, null, TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Verifies the signature of the specified file.
        /// </summary>
        /// <param name="inputFile">The full path of the signed file to verify.</param>
        public static bool Verify(string inputFile)
        {
            var command = GpgCommand.Verify
                .Batch(true)
                .Quiet()
                .TrustModel(GpgTrustModel.Always)
                .InputFile(inputFile);

            try
            {
                new GpgCommandExecutor().Execute(command, null, null, TimeSpan.FromSeconds(30));
            }

            catch(ProcessExecutionException e)
            {
                // There has got to be a better way to do this.
                if(e.Message.Contains("BAD signature"))
                    return false;

                throw;
            }

            return true;
        }

        /// <summary>
        /// Enumerates the public keys in the key store.
        /// </summary>
        /// <returns>An array of <see cref="GpgKey"/> values.</returns>
        public static GpgKey[] GetPublicKeys()
        {
            var command = GpgCommand.ListPublicKeys
                .WithColons()
                .FixedListMode()
                .Batch(true)
                .Quiet();

            var stream = new MemoryStream();
            new GpgCommandExecutor().Execute(command, null, stream, TimeSpan.FromSeconds(30));

            stream.Position = 0;
            using(var reader = new GpgKeyReader(stream))
                return reader.ReadAllKeys();
        }

        /// <summary>
        /// Enumerates the private keys in the key store.
        /// </summary>
        /// <returns>An array of <see cref="GpgKey"/> values.</returns>
        public static GpgKey[] GetPrivateKeys()
        {
            var command = GpgCommand.ListSecretKeys
                .WithColons()
                .FixedListMode()
                .Batch(true)
                .Quiet();

            var stream = new MemoryStream();
            new GpgCommandExecutor().Execute(command, null, stream, TimeSpan.FromSeconds(30));

            stream.Position = 0;
            using(var reader = new GpgKeyReader(stream))
                return reader.ReadAllKeys();
        }
    }
}