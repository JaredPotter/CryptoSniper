using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSniper.Models
{
    public class ActiveOrderStatusResult
    {
        #region Properties

        public string E { get; set; }

        public string Ok { get; set; }

        public List<List<OrdersListItem>> Data {get; set;}

        #endregion
    }

    public class OrdersListItem
    {
        #region Properties

        public string Order_Id { get; set; }

        public string Amount { get; set; }

        #endregion
    }
}
