namespace Cryptophage
{
    /// <summary>
    /// Specifies the trust model to follow when using public keys.
    /// </summary>
    public enum GpgTrustModel
    {
        /// <summary>
        /// This is the Web of Trust combined with trust signatures as used in PGP 5.x and later.
        /// </summary>
        Pgp,

        /// <summary>
        /// This is the standard Web of Trust as used in PGP 2.x and earlier.
        /// </summary>
        Classic,

        /// <summary>
        /// Key validity is set directly by the user and not calculated via the Web of Trust.
        /// </summary>
        Direct,

        /// <summary>
        /// Skip key validation and assume that used keys are always fully trusted.
        /// </summary>
        Always,

        /// <summary>
        /// Select the trust model depending on whatever the internal trust database says.
        /// </summary>
        Auto,
    }
}