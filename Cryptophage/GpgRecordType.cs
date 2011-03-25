namespace Cryptophage
{
    /// <summary>
    /// Describes the type of record represented by a <see cref="GpgKey"/>.
    /// </summary>
    public enum GpgRecordType
    {
        /// <summary>
        /// An unknown record type.
        /// </summary>
        Unknown,

        /// <summary>
        /// A public key.
        /// </summary>
        PublicKey,

        /// <summary>
        /// An X.509 certificate.
        /// </summary>
        X509Certificate,

        /// <summary>
        /// An X.509 certificate with an available private key.
        /// </summary>
        X509CertificatePrivateKey,

        /// <summary>
        /// A public subkey (secondary key).
        /// </summary>
        PublicSubKey,

        /// <summary>
        /// A secret key.
        /// </summary>
        SecretKey,

        /// <summary>
        /// A secret subkey (secondary key).
        /// </summary>
        SecretSubKey,

        /// <summary>
        /// A user id.
        /// </summary>
        UserId,

        /// <summary>
        /// A user attribute.
        /// </summary>
        UserAttribute,

        /// <summary>
        /// A signature.
        /// </summary>
        Signature,

        /// <summary>
        /// A revocation certificate.
        /// </summary>
        RevocationCertificate,

        /// <summary>
        /// A fingerprint.
        /// </summary>
        Fingerprint,

        /// <summary>
        /// Custom public key data.
        /// </summary>
        PublicKeyData,

        /// <summary>
        /// A keygrip.
        /// </summary>
        KeyGrip,

        /// <summary>
        /// A revocation key.
        /// </summary>
        RevocationKey,

        /// <summary>
        /// A trust record.
        /// </summary>
        TrustRecord,

        /// <summary>
        /// A signature subpacket.
        /// </summary>
        SignatureSubpacket,
    }
}