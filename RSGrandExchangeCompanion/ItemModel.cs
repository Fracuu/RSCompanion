using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSGrandExchangeCompanion
{
    public class ItemModel
    {
        public int ItemID { get; set; }

        public string ItemUrl { get; set; }

        public string ItemName { get; set; }

        public string ItemMembers { get; set; }

        public object ItemPrice { get; set; }

        public object ItemChange { get; set; }
    }
}
