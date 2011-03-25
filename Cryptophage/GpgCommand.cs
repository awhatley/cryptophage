using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptophage
{
    /// <summary>
    /// Represents a GPG command.
    /// </summary>
    public class GpgCommand
    {
        #region Command Factory Methods

        /// <summary>
        /// Creates a GPG --sign operation.
        /// </summary>
        public static GpgCommand Sign { get { return new GpgCommand("--sign"); } }

        /// <summary>
        /// Creates a GPG --detach-sign operation.
        /// </summary>
        public static GpgCommand SignDetached { get { return new GpgCommand("--detach-sign"); } }

        /// <summary>
        /// Creates a GPG --clearsign operation.
        /// </summary>
        public static GpgCommand ClearSign { get { return new GpgCommand("--clearsign"); } }

        /// <summary>
        /// Creates a GPG --encrypt operation.
        /// </summary>
        public static GpgCommand Encrypt { get { return new GpgCommand("--encrypt"); } }

        /// <summary>
        /// Creates a GPG --encrypt --sign operation.
        /// </summary>
        public static GpgCommand EncryptSign { get { return new GpgCommand("--encrypt --sign"); } }

        /// <summary>
        /// Creates a GPG --decrypt operation.
        /// </summary>
        public static GpgCommand Decrypt { get { return new GpgCommand("--decrypt"); } }

        /// <summary>
        /// Creates a GPG --verify operation.
        /// </summary>
        public static GpgCommand Verify { get { return new GpgCommand("--verify"); } }

        /// <summary>
        /// Creates a GPG --list-public-keys operation.
        /// </summary>
        public static GpgCommand ListPublicKeys { get { return new GpgCommand("--list-public-keys"); } }

        /// <summary>
        /// Creates a GPG --list-secret-keys operation.
        /// </summary>
        public static GpgCommand ListSecretKeys { get { return new GpgCommand("--list-secret-keys"); } }

        /// <summary>
        /// Creates a custom GPG operation.
        /// </summary>
        /// <param name="command">The name of the command to pass to GPG.</param>
        public static GpgCommand Command(string command) { return new GpgCommand(command); }

        #endregion

        #region Option Methods

        /// <summary>
        /// Sets the GPG --recipient option.
        /// </summary>
        /// <param name="userId">The recipient user id to use for encryption. This can be the key ID, user name, or user email.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand Recipient(string userId) { return AddOption("--recipient \"{0}\"", userId); }

        /// <summary>
        /// Sets the GPG --local-user option.
        /// </summary>
        /// <param name="userId">The local user id to use for signing or decryption.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand LocalUser(string userId) { return AddOption("--local-user \"{0}\"", userId); }

        /// <summary>
        /// Sets the GPG --armor option.
        /// </summary>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand ArmoredOutput() { return AddOption("--armor"); }

        /// <summary>
        /// Set the GPG --no-armor option.
        /// </summary>
        /// <returns></returns>
        public GpgCommand NonArmoredInput() { return AddOption("--no-armor"); }

        /// <summary>
        /// Sets the GPG -z option.
        /// </summary>
        /// <param name="level">The compression level to use, from 0 to 9.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand CompressionLevel(int level) { return AddOption("-z {0}", level); }

        /// <summary>
        /// Sets the GPG --homedir option.
        /// </summary>
        /// <param name="path">The path to the home directory to use.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand HomeDirectory(string path) { return AddOption("--homedir \"{0}\"", path); }

        /// <summary>
        /// Sets the GPG --passphrase option.
        /// </summary>
        /// <param name="passphrase">The passphrase to use.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand Passphrase(string passphrase) { return AddOption("--passphrase \"{0}\"", passphrase); }

        /// <summary>
        /// Sets the GPG --no-verbose --quiet --no-tty option.
        /// </summary>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand Quiet() { return AddOption("--no-verbose --quiet --no-tty"); }

        /// <summary>
        /// Sets the GPG --batch or --no-batch option.
        /// </summary>
        /// <param name="useBatch">Whether to operate in batch (non-interactive) mode.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand Batch(bool useBatch) { return AddOption(useBatch ? "--batch" : "--no-batch"); }

        /// <summary>
        /// Sets the GPG --trust-model option.
        /// </summary>
        /// <param name="model">The GPG trust model to use.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand TrustModel(GpgTrustModel model) { return AddOption("--trust-model {0}", model.ToString().ToLowerInvariant()); }

        /// <summary>
        /// Sets the GPG --with-colons option.
        /// </summary>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand WithColons() { return AddOption("--with-colons"); }

        /// <summary>
        /// Sets the GPG --fixed-list-mode option.
        /// </summary>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand FixedListMode() { return AddOption("--fixed-list-mode"); }

        /// <summary>
        /// Sets the input file for the command.
        /// </summary>
        /// <param name="path">The full path of the input file.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand InputFile(string path) { _inputFile = path; return this; }

        /// <summary>
        /// Set the output file for the command.
        /// </summary>
        /// <param name="path">The full path of the output file.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand OutputFile(string path) { return AddOption("--output \"{0}\"", path); }

        /// <summary>
        /// Sets the GPG --yes option.
        /// </summary>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand Yes() { return AddOption("--yes"); }

        /// <summary>
        /// Sets a custom GPG option.
        /// </summary>
        /// <param name="option">The GPG option to use.</param>
        /// <returns>This <see cref="GpgCommand"/> instance.</returns>
        public GpgCommand Option(string option) { return AddOption(option); }

        private GpgCommand AddOption(string option)
        {
            _options.Add(option);
            return this;
        }

        private GpgCommand AddOption(string format, params object[] args)
        {
            _options.Add(String.Format(format, args));
            return this;
        }

        #endregion

        #region Implementation

        private readonly string _command;
        private readonly List<string> _options = new List<string>();
        private string _inputFile;

        private GpgCommand(string command)
        {
            _command = command;
        }

        /// <summary>
        /// Converts the GPG command into its associated command-line parameter representation.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach(var option in _options)
                sb.AppendFormat("{0} ", option);

            sb.Append(_command);

            if(_inputFile != null)
                sb.Append(String.Format(" \"{0}\"", _inputFile));

            return sb.ToString();
        }

        #endregion
    }
}