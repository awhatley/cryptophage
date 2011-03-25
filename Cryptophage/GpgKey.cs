using System;

namespace Cryptophage
{
    /// <summary>
    /// Describes an entry in a GPG key ring file.
    /// </summary>
    public class GpgKey
    {
        /// <summary>
        /// The record type of the key.
        /// </summary>
        public GpgRecordType RecordType { get; set; }

        /// <summary>
        /// The calculated validity of the key.
        /// </summary>
        public GpgValidity Validity { get; set; }

        /// <summary>
        /// The key length in bits, if applicable.
        /// </summary>
        public int KeyLength { get; set; }

        /// <summary>
        /// The algorithm used by the key.
        /// </summary>
        public GpgAlgorithm Algorithm { get; set; }

        /// <summary>
        /// The key identifier.
        /// </summary>
        public string KeyId { get; set; }

        /// <summary>
        /// The creation date of the key.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The expiration date of the key.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Used for serial number in crt records (used to be the Local-ID).
        /// For UID and UAT records, this is a hash of the user ID contents
        /// used to represent that exact user ID.  For trust signatures,
        /// this is the trust depth seperated by the trust value by a
        /// space.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// This is a single letter, but be prepared that additional
	    /// information may follow in some future versions.  For trust
	    /// signatures with a regular expression, this is the regular
	    /// expression value, quoted as in field 10.
        /// </summary>
        public string OwnerTrust { get; set; }

        /// <summary>
        /// The user ID associated with the key.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Signature class as per RFC-4880.  This is a 2 digit
        /// hexnumber followed by either the letter 'x' for an
        /// exportable signature or the letter 'l' for a local-only
        /// signature.  The class byte of an revocation key is also
        /// given here, 'x' and 'l' is used the same way.
        /// </summary>
        public string SignatureClass { get; set; }

        /// <summary>
        /// The key's encryption capabilities.
        /// </summary>
        public GpgKeyCapabilities KeyCapabilities { get; set; }

        /// <summary>
        /// Used in FPR records for S/MIME keys to store the
        /// fingerprint of the issuer certificate.
        /// </summary>
        public string Fingerprint { get; set; }

        /// <summary>
        /// Flag field used in the --edit menu output.
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// Used in sec/sbb to print the serial number of a token
        /// (internal protect mode 1002) or a '#' if that key is a
        /// simple stub (internal protect mode 1001).
        /// </summary>
        public string SerialNumber { get; set; }
    }
}