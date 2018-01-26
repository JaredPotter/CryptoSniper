namespace CryptoSniper
{
    public class PlaceInstantOrderResult
    {
        #region Properties

        public string Id { get; set; }

        public string Time { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// For BUY request this is BTC in the smallest denomination, which is Satoshis
        /// For SELL request this is USD in the smallest denomination, which is Satoshis
        /// </summary>
        public string Symbol1Amount { get; set; }

        /// <summary>
        /// For BUY request this is USD in the smallest denomination, which is pennies
        /// For SELL request, this is BTC in the smallest denomination, which is Satoshis
        /// </summary>
        public string Symbol2Amount { get; set; }

        #endregion
    }
}