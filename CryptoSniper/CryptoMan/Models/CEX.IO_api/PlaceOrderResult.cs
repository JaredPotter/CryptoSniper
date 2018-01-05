using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMan.Models.CEX.IO_api
{
    public class PlaceOrderResult
    {
        #region Properties

        public bool Complate { get; set; }

        public string Id { get; set; }

        public string Time { get; set; }

        public string Pending { get; set; }

        public string Amount { get; set; }

        public string Price { get; set; }

        public string Type { get; set; }

        #endregion
    }
}
