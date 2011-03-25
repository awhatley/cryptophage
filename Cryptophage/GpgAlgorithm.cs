namespace Cryptophage
{
    /// <summary>
    /// Specifies an encryption algorithm used by GPG.
    /// </summary>
    public enum GpgAlgorithm
    {
        /// <summary>
        /// An unknown algorithm.
        /// </summary>
        Unknown,

        /// <summary>
        /// The RSA algorithm.
        /// </summary>
        Rsa,

        /// <summary>
        /// The ElGamal algorithm for encryption only.
        /// </summary>
        ElGamal,

        /// <summary>
        /// The DSA algorithm.
        /// </summary>
        Dsa,

        /// <summary>
        /// The ElGamal algorithm for signing and encryption.
        /// </summary>
        ElgamalSignAndEncrypt,
    }
}