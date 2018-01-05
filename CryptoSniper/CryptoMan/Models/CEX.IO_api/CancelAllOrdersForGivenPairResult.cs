using System.Collections.Generic;

namespace CryptoSniper.Models.CEX.IO_api
{
    public class CancelAllOrdersForGivenPairResult
    {
        #region Properties

        public string E { get; set; }

        public string Ok { get; set; }

        public List<string> Data {get; set;}

        #endregion
    }
}
