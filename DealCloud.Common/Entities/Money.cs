using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// value with the currency
    /// </summary>
    public class Money
    {
        /// <summary>
        /// value
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// in which currency (3 letters code)
        /// </summary>
        public string CurrencyCode { get; set; }

        public override bool Equals(object obj)
        {
            var y = obj as Money;

            if (y == null) return false;

            return Amount == y.Amount && CurrencyCode == y.CurrencyCode ;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var res = 17;
                res = res * 37 + (int)Amount;
                if (CurrencyCode != null)
                    res = res * 37 + CurrencyCode.GetHashCode();
                return res;
            }
        }
    }
}
