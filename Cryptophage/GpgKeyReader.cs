using System;
using System.Collections.Generic;
using System.IO;

namespace Cryptophage
{
    /// <summary>
    /// Parses keys from a stream that is the result of a GPG key listing operation.
    /// </summary>
    public class GpgKeyReader : IDisposable
    {
        private readonly StreamReader _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgKeyReader"/> class with the
        /// specified stream.
        /// </summary>
        /// <param name="stream">The stream from which to parse keys.</param>
        public GpgKeyReader(Stream stream)
        {
            _reader = new StreamReader(stream);
        }

        /// <summary>
        /// Parses all keys from the current position of the stream to the end.
        /// </summary>
        /// <returns>An array of <see cref="GpgKey"/> instances.</returns>
        public GpgKey[] ReadAllKeys()
        {
            var keys = new List<GpgKey>();
            while(!_reader.EndOfStream)
            {
                var line = _reader.ReadLine();
                if(String.IsNullOrWhiteSpace(line)) continue;
                keys.Add(Parse(line));
            }

            return keys.ToArray();
        }

        private static GpgKey Parse(string line)
        {
            var fields = line.Split(':');
            return new GpgKey
            {
                RecordType = ParseRecordType(fields.Length > 0 ? fields[0] : null),
                Validity = ParseValidity(fields.Length > 1 ? fields[1] : null),
                KeyLength = ParseInt32(fields.Length > 2 ? fields[2] : null),
                Algorithm = ParseAlgorithm(fields.Length > 3 ? fields[3] : null),
                KeyId = fields.Length > 4 ? fields[4] : null,
                CreationDate = ParseDateTime(fields.Length > 5 ? fields[5] : null),
                ExpirationDate = ParseDateTime(fields.Length > 6 ? fields[6] : null),
                Hash = fields.Length > 7 ? fields[7] : null,
                OwnerTrust = fields.Length > 8 ? fields[8] : null,
                UserId = fields.Length > 9 ? fields[9] : null,
                SignatureClass = fields.Length > 10 ? fields[10] : null,
                KeyCapabilities = ParseKeyCapabilities(fields.Length > 11 ? fields[11] : null),
                Fingerprint = fields.Length > 12 ? fields[12] : null,
                Flag = fields.Length > 13 ? fields[13] : null,
                SerialNumber = fields.Length > 14 ? fields[14] : null,
            };
        }

        private static GpgKeyCapabilities ParseKeyCapabilities(string capabilities)
        {
            var value = GpgKeyCapabilities.None;
            
            if(capabilities == null)
                return value;
            
            if(capabilities.Contains("e")) value |= GpgKeyCapabilities.Encryption;
            if(capabilities.Contains("s")) value |= GpgKeyCapabilities.Signing;
            if(capabilities.Contains("c")) value |= GpgKeyCapabilities.Certification;
            if(capabilities.Contains("a")) value |= GpgKeyCapabilities.Authentication;
            if(capabilities.Contains("D")) value |= GpgKeyCapabilities.Disabled;

            return value;
        }

        private static GpgAlgorithm ParseAlgorithm(string algorithm)
        {
            int value;
            Int32.TryParse(algorithm, out value);

            switch(value)
            {
                case 1: return GpgAlgorithm.Rsa;
                case 16: return GpgAlgorithm.ElGamal;
                case 17: return GpgAlgorithm.Dsa;
                case 20: return GpgAlgorithm.ElgamalSignAndEncrypt;
                default: return GpgAlgorithm.Unknown;
            }
        }

        private static DateTime ParseDateTime(string value)
        {
            var dateTimeValue = DateTime.MinValue;
            if(value.Contains("T"))
            {
                DateTime.TryParse(value, out dateTimeValue);
                return dateTimeValue;
            }

            long secondsSinceEpoch;
            return Int64.TryParse(value, out secondsSinceEpoch) ? 
                new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secondsSinceEpoch) : 
                dateTimeValue;
        }

        private static int ParseInt32(string value)
        {
            int intValue;
            Int32.TryParse(value, out intValue);
            return intValue;
        }

        private static GpgValidity ParseValidity(string validity)
        {
            switch(validity)
            {
                case "o": return GpgValidity.New;
                case "i": return GpgValidity.Invalid;
                case "d": return GpgValidity.Disabled;
                case "r": return GpgValidity.Revoked;
                case "e": return GpgValidity.Expired;
                case "n": return GpgValidity.Valid;
                case "m": return GpgValidity.MarginallyValid;
                case "f": return GpgValidity.FullyValid;
                case "u": return GpgValidity.UltimatelyValid;
                default: return GpgValidity.Unknown;
            }
        }

        private static GpgRecordType ParseRecordType(string recordType)
        {
            switch(recordType)
            {
                case "pub": return GpgRecordType.PublicKey;
                case "crt": return GpgRecordType.X509Certificate;
                case "crs": return GpgRecordType.X509CertificatePrivateKey;
                case "sub": return GpgRecordType.PublicSubKey;
                case "sec": return GpgRecordType.SecretKey;
                case "ssb": return GpgRecordType.SecretSubKey;
                case "uid": return GpgRecordType.UserId;
                case "uat": return GpgRecordType.UserAttribute;
                case "sig": return GpgRecordType.Signature;
                case "rev": return GpgRecordType.RevocationCertificate;
                case "fpr": return GpgRecordType.Fingerprint;
                case "pkd": return GpgRecordType.PublicKeyData;
                case "grp": return GpgRecordType.KeyGrip;
                case "rvk": return GpgRecordType.RevocationKey;
                case "tru": return GpgRecordType.TrustRecord;
                case "spk": return GpgRecordType.SignatureSubpacket;
                default: return GpgRecordType.Unknown;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="GpgKeyReader"/>.
        /// </summary>
        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}