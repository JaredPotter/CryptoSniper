using System.Collections.Generic;

namespace CryptoMan.Models.CEX.IO_api
{
    public class ArchivedOrdersResults
    {
        #region Properties

        public List<ArchivedOrderItem> List { get; set; }

        #endregion
    }

    public class ArchivedOrderItem
    {
        #region Properties

        public string Id { get; set; }

        public string Type { get; set; }

        public string Time { get; set; }

        public string LastTxTime { get; set; }

        public string LastTx { get; set; }

        public string Status { get; set; }

        public string Symbol1 { get; set; }

        public string Syemol2 { get; set; }

        public string Amount { get; set; }

        public string Price { get; set; }

        public string FaUSD { get; set; }

        public string TaUSD { get; set; }

        public string Remains { get; set; }

        public string ABTCCDS { get; set; }

        public string AUSDCDS { get; set; }

        public string FUSDCDS { get; set; }

        public string TradingFeeMaker { get; set; }

        public string TradingFeeTaker { get; set; }

        public string OrderId { get; set; }

        #endregion
    }
}
