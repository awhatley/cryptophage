namespace Cryptophage
{
    /// <summary>
    /// Describes the calculated validity of a particular <see cref="GpgKey"/>.
    /// </summary>
    public enum GpgValidity
    {
        /// <summary>
        /// The key is of unknown validity.
        /// </summary>
        Unknown,

        /// <summary>
        /// The key is invalid (e.g. due to a missing self-signature).
        /// </summary>
        Invalid,

        /// <summary>
        /// The key has been disabled.
        /// </summary>
        Disabled,

        /// <summary>
        /// The key has been revoked.
        /// </summary>
        Revoked,

        /// <summary>
        /// The key has expired.
        /// </summary>
        Expired,

        /// <summary>
        /// The key is valid.
        /// </summary>
        Valid,

        /// <summary>
        /// The key is marginally valid.
        /// </summary>
        MarginallyValid,

        /// <summary>
        /// The key is fully valid.
        /// </summary>
        FullyValid,

        /// <summary>
        /// The key is ultimately valid.
        /// </summary>
        UltimatelyValid,

        /// <summary>
        /// The key is new to the system.
        /// </summary>
        New
    }
}