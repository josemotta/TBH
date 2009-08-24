using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace TheBeerHouse.Models
{
	[Serializable]
	public class ShoppingCartItem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ShoppingCartItem"/> class.
		/// </summary>
		/// <param name="product">The product.</param>
		public ShoppingCartItem(Product product)
		{
			Product = product;
		}

		/// <summary>
		/// Gets or sets the product.
		/// </summary>
		/// <value>The product.</value>
		public Product Product { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title { get { return Product.Title; } }

		/// <summary>
		/// Gets or sets the quantity.
		/// </summary>
		/// <value>The quantity.</value>
		public int Quantity { get; set; }

		/// <summary>
		/// Gets or sets the price.
		/// </summary>
		/// <value>The price.</value>
		public decimal Price { get { return Product.UnitPrice; } }

		/// <summary>
		/// Gets or sets the SKU.
		/// </summary>
		/// <value>The SKU.</value>
		public string SKU { get { return Product.SKU; } }

		/// <summary>
		/// Gets the total.
		/// </summary>
		/// <value>The total.</value>
		public decimal TotalPrice
		{
			get { return Price * Quantity; }
		}
	}
}
