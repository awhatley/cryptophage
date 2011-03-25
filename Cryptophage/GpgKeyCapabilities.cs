using System;

namespace Cryptophage
{
    /// <summary>
    /// Describes the encryption capabilities of a particular <see cref="GpgKey"/>.
    /// </summary>
    [Flags]
    public enum GpgKeyCapabilities
    {
        /// <summary>
        /// The key has no capabilities.
        /// </summary>
        None = 0,

        /// <summary>
        /// The key supports encryption.
        /// </summary>
        Encryption = 0x01,

        /// <summary>
        /// The key supports signing.
        /// </summary>
        Signing = 0x02,

        /// <summary>
        /// The key supports certification.
        /// </summary>
        Certification = 0x04,

        /// <summary>
        /// The key supports authentication.
        /// </summary>
        Authentication = 0x08,

        /// <summary>
        /// The key is disabled.
        /// </summary>
        Disabled = 0x10,
    }
}