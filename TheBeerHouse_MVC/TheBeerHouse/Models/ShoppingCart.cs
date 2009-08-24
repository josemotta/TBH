using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheBeerHouse.Models
{
	[Serializable]
	public class ShoppingCart : List<ShoppingCartItem>
	{
		/// <summary>
		/// Gets or sets the shipping method.
		/// </summary>
		/// <value>The shipping method.</value>
		public ShippingMethod ShippingMethod { get; set; }

		/// <summary>
		/// Gets or sets the shipping price.
		/// </summary>
		/// <value>The shipping price.</value>
		public decimal ShippingPrice { get { return ShippingMethod.Price; } }

		/// <summary>
		/// Gets the sub total.
		/// </summary>
		/// <value>The sub total.</value>
		public decimal SubTotal
		{
			get
			{
				decimal totalPrice = 0M;
				foreach (var item in this)
					totalPrice += item.TotalPrice;

				return totalPrice;
			}
		}

		/// <summary>
		/// Gets or sets the total price.
		/// </summary>
		/// <value>The total price.</value>
		public decimal Total
		{
			get { return SubTotal + ShippingPrice; }
		}
	}
}