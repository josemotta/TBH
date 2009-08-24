using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBeerHouse.Models;
using System.Web.Profile;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Mail;
using ManagedFusion.Web.Mvc;

namespace TheBeerHouse.Controllers
{
	[HandleError]
	public class CommerceController : Controller
	{
		/// <summary>
		/// Indexes this instance.
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Departments.GetDepartments();

			ViewData["PageTitle"] = "Welcome to The Beer House Shop";
			return View(viewData);
		}

		/// <summary>
		/// Views the products.
		/// </summary>
		/// <param name="departmentId">The department id.</param>
		/// <returns></returns>
		public ActionResult ViewDepartment(int departmentId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var department = dc.Departments.GetDepartment(departmentId);
			var viewData = dc.Products.GetProducts().ByDepartment(departmentId).InStock();

			ViewData["PageTitle"] = "Available Products for " + department.Title;
            return View(viewData);
		}

		/// <summary>
		/// Products the detail.
		/// </summary>
		/// <param name="productID">The product ID.</param>
		/// <returns></returns>
		public ActionResult ViewProduct(int productId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Product viewData = dc.Products.GetProduct(productId);

			ViewData["PageTitle"] = viewData.Title;
			return View(viewData);
		}

		/// <summary>
		/// Views the shopping cart.
		/// </summary>
		/// <param name="productID">The product ID.</param>
		/// <param name="shippingMethod">The shipping method.</param>
		/// <returns></returns>
		public ActionResult ViewShoppingCart(int? shippingMethod)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			ProfileBase profileBase = HttpContext.Profile as ProfileBase;
			ShoppingCart shoppingCart = (ShoppingCart)profileBase.GetPropertyValue("ShoppingCart");

			if (shoppingCart == null)
			{
				shoppingCart = new ShoppingCart();

				// get the cheapest shipping method for our user
				var cheapestShippingMethod = dc.ShippingMethods.GetShippingMethods().Cheapest();
				shoppingCart.ShippingMethod = cheapestShippingMethod;

				profileBase.SetPropertyValue("ShoppingCart", shoppingCart);
			}

			// set the shipping method if one exists
			if (shippingMethod.HasValue)
				shoppingCart.ShippingMethod = dc.ShippingMethods.GetShippingMethod(shippingMethod.Value);

			// make sure the shipping method is set
			if (shoppingCart.ShippingMethod == null)
				shoppingCart.ShippingMethod = dc.ShippingMethods.GetShippingMethods().Cheapest();

			ViewData["shippingMethod"] = new SelectList(dc.ShippingMethods.GetShippingMethods(), "ShippingMethodID", "Title", shoppingCart.ShippingMethod.ShippingMethodID);
			ViewData["PageTitle"] = "Your Shopping Cart";
			return View(shoppingCart);
		}

		/// <summary>
		/// Adds the shopping cart item.
		/// </summary>
		/// <param name="productId">The product id.</param>
		/// <param name="price">The price.</param>
		/// <param name="title">The title.</param>
		/// <param name="quantity">The quantity.</param>
		/// <param name="sku">The sku.</param>
		/// <returns></returns>
		[AcceptVerbs("POST")]
		public ActionResult AddShoppingCartItem(int productId, int? quantity)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			ProfileBase profileBase = HttpContext.Profile as ProfileBase;
			ShoppingCart shoppingCart = (ShoppingCart)profileBase.GetPropertyValue("ShoppingCart");

			if (shoppingCart == null)
			{
				shoppingCart = new ShoppingCart();

				// get the cheapest shipping method for our user
				var cheapestShippingMethod = dc.ShippingMethods.GetShippingMethods().Cheapest();
				shoppingCart.ShippingMethod = cheapestShippingMethod;

				profileBase.SetPropertyValue("ShoppingCart", shoppingCart);
			}

			Product product = dc.Products.GetProduct(productId);

			// throw a 404 Not Found if the requested forum is not in the database
			if (product == null)
				throw new HttpException(404, "The product could not be found.");

			ShoppingCartItem item = new ShoppingCartItem(product) {
				Quantity = quantity ?? 1
			};

			shoppingCart.Add(item);

			return RedirectToAction("ViewShoppingCart");
		}

		/// <summary>
		/// Deletes the shopping cart item.
		/// </summary>
		/// <param name="productId">The product id.</param>
		/// <returns></returns>
		[Service, HttpPostOnly]
		public ActionResult DeleteShoppingCartItem(int productId)
		{
			ProfileBase profileBase = HttpContext.Profile as ProfileBase;
			ShoppingCart shoppingCart = (ShoppingCart)profileBase.GetPropertyValue("ShoppingCart");

			if (shoppingCart == null)
				throw new HttpException(404, "The shopping cart could not be found.");

			foreach (var item in shoppingCart)
			{
				if (item.Product.ProductID == productId)
				{
					shoppingCart.Remove(item);
					break;
				}
			}

			return View(new { id = productId });
		}

		/// <summary>
		/// Completes the order.
		/// </summary>
		/// <param name="tx">The transaction ID from PayPal.</param>
		/// <param name="amt">The amount paid from PayPal.</param>
		/// <returns></returns>
		[AcceptVerbs("GET")]
		public ActionResult CompleteOrder(string tx, decimal amt)
		{
			// get transaction response from PayPal
			string response = TransactionDataRequest(tx);

			ProfileBase profileBase = HttpContext.Profile as ProfileBase;
			ShoppingCart shoppingCart = (ShoppingCart)profileBase.GetPropertyValue("ShoppingCart");

			// save transaction to database
			if (shoppingCart.Total == amt)
			{
				TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
				Order order = new Order() {
					AddedBy = User.Identity.Name,
					AddedDate = DateTime.Now,
					CustomerEmail = ExtractValue(response, "payer_email"),
					Shipping = shoppingCart.ShippingPrice,
					ShippingCity = ExtractValue(response, "address_city"),
					ShippingCountry = ExtractValue(response, "address_country"),
					ShippingFirstName = ExtractValue(response, "first_name"),
					ShippingLastName = ExtractValue(response, "last_name"),
					ShippingMethod = shoppingCart.ShippingMethod.Title,
					ShippingStreet = ExtractValue(response, "address_street"),
					ShippingPostalCode = ExtractValue(response, "address_zip"),
					ShippingState = ExtractValue(response, "address_state"),
					Status = "Order Recieved",
					TransactionID = tx,
					SubTotal = shoppingCart.Total
				};

				var products = dc.Products.GetProducts(shoppingCart);
				foreach (ShoppingCartItem item in shoppingCart)
				{
					order.OrderItems.Add(new OrderItem() {
						AddedBy = User.Identity.Name,
						AddedDate = DateTime.Now,
						ProductID = item.Product.ProductID,
						Quantity = item.Quantity,
						Title = item.Title,
						UnitPrice = item.Price,
						SKU = item.SKU
					});

					var productToUpdate = products.FirstOrDefault(p => p.ProductID == item.Product.ProductID);
					productToUpdate.UnitsInStock -= item.Quantity;
				}

				dc.Orders.InsertOnSubmit(order);
				dc.SubmitChanges();

				profileBase.SetPropertyValue("ShoppingCart", new ShoppingCart());
				ViewData["OrderNumber"] = order.OrderID;

				ViewData["PageTitle"] = "Order Received";
				return View();
			}
			else
			{
				return View("TransactionError");
			}
		}

		#region Admin

		/// <summary>
		/// Manages the store.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult ManageStore()
		{
			ViewData["PageTitle"] = "Manage Store";
			return View();
		}

		#region Departments

		/// <summary>
		/// Manages the departments.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult ManageDepartments()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Departments.GetDepartments();

			ViewData["PageTitle"] = "Manage Departments";
			return View(viewData);
		}

		/// <summary>
		/// Creates the department.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult CreateDepartment()
		{
			ViewData["PageTitle"] = "Create Department";
			return View();
		}

		/// <summary>
		/// Creates the department.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="imageUrl">The image URL.</param>
		/// <param name="description">The description.</param>
		/// <param name="importance">The importance.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		[AcceptVerbs("POST"), ActionName("CreateDepartment")]
		public ActionResult CreateDepartment_OnPost(string title, string imageUrl, string description, int? importance)
		{
			// Create new Department
			if (!String.IsNullOrEmpty(title)
				&& !String.IsNullOrEmpty(imageUrl)
				&& !String.IsNullOrEmpty(description))
			{
				try
				{
					TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
					Department department = new Department {
						Title = title,
						Importance = importance ?? -1,
						ImageUrl = imageUrl,
						Description = description,
						AddedBy = User.Identity.Name,
						AddedDate = DateTime.Now
					};
					dc.Departments.InsertOnSubmit(department);

					// save changes to database
					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your department has been created.";
					return RedirectToAction("ManageDepartments");
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			// Bring up blank form
			ViewData["PageTitle"] = "Create Department";
			return View();
		}

		/// <summary>
		/// Edits the department.
		/// </summary>
		/// <param name="departmentId">The department id.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult EditDepartment(int departmentId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Department viewData = dc.Departments.GetDepartment(departmentId);

			ViewData["id"] = viewData.DepartmentID;
			ViewData["PageTitle"] = "Update Department";
			return View("CreateDepartment", viewData);
		}

		/// <summary>
		/// Edits the department.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="title">The title.</param>
		/// <param name="imageUrl">The image URL.</param>
		/// <param name="description">The description.</param>
		/// <param name="importance">The importance.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		[AcceptVerbs("POST"), ActionName("EditDepartment")]
		public ActionResult EditDepartment_OnPost(int id, string title, string imageUrl, string description, int? importance)
		{
			// Update Exsisting Department Data
			if (!String.IsNullOrEmpty(title)
				&& !String.IsNullOrEmpty(imageUrl)
				&& !String.IsNullOrEmpty(description))
			{
				try
				{
					TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
					Department department = dc.Departments.GetDepartment(id);

					department.Importance = importance ?? -1;
					department.Title = title;
					department.ImageUrl = imageUrl;
					department.Description = description;
					department.AddedBy = User.Identity.Name;
					department.AddedDate = DateTime.Now;
					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your department has been modified.";
					return RedirectToAction("ManageDepartments");
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			return EditDepartment(id);
		}

		/// <summary>
		/// Manages the departments.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		[Service, HttpPostOnly]
		public ActionResult DeleteDepartment(int id)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();

			Department department = dc.Departments.GetDepartment(id);
			dc.Departments.DeleteOnSubmit(department);
			dc.SubmitChanges();

			return View(new { id = id });
		}

		#endregion

		#region Products

		/// <summary>
		/// Manages the products.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult ManageProducts()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();

			var viewData = dc.Products.GetProducts();

			ViewData["PageTitle"] = "Manage Products";
			return View(viewData);
		}

		/// <summary>
		/// Creates the product.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult CreateProduct()
		{
            TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
            var departments = dc.Departments.GetDepartments();

            ViewData["departmentID"] = new SelectList(departments, "DepartmentID", "Title");
			ViewData["PageTitle"] = "Create Product";
			return View();
		}

		/// <summary>
		/// Creates the product.
		/// </summary>
		/// <param name="productID">The product ID.</param>
		/// <param name="departmentID">The department ID.</param>
		/// <param name="title">The title.</param>
		/// <param name="description">The description.</param>
		/// <param name="sku">The sku.</param>
		/// <param name="unitPrice">The unit price.</param>
		/// <param name="discountPercentage">The discount percentage.</param>
		/// <param name="unitsInStock">The units in stock.</param>
		/// <param name="smallImageURL">The small image URL.</param>
		/// <param name="fullImageURL">The full image URL.</param>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		[AcceptVerbs("POST"), ActionName("CreateProduct")]
        [ValidateInput(false)]   
		public ActionResult CreateProduct_OnPost(int? departmentId, string title, string description, string sku, decimal? unitPrice, int? discountPercentage, int? unitsInStock, string smallImageURL, string fullImageURL)
		{
			// Populate Drop Downs
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var departments = dc.Departments.GetDepartments();

			// Create new Product
			if (!String.IsNullOrEmpty(title))
			{
				try
				{
					Product product = new Product {
						Title = title,
						Description = description,
						DepartmentID = departmentId ?? -1,
						DiscountPercentage = discountPercentage ?? 0,
						FullImageUrl = fullImageURL,
						SmallImageUrl = smallImageURL,
						SKU = sku,
						UnitPrice = unitPrice ?? 0,
						UnitsInStock = unitsInStock ?? 0,
						AddedBy = User.Identity.Name,
						AddedDate = DateTime.Now
					};
					dc.Products.InsertOnSubmit(product);

					// save changes to database
					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your product has been created.";
					return RedirectToAction("ManageProducts");
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["departmentID"] = new SelectList(departments, "DepartmentID", "Title", departmentId);

			// Bring up blank form
			ViewData["PageTitle"] = "Create Product";
			return View();
		}

		/// <summary>
		/// Edits the product.
		/// </summary>
		/// <param name="productID">The product ID.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult EditProduct(int productId)
		{
			// Populate Drop Downs
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var departments = dc.Departments.GetDepartments();
			Product viewData = dc.Products.GetProduct(productId);

			ViewData["id"] = viewData.ProductID;
			ViewData["title"] = viewData.Title;
			ViewData["sku"] = viewData.SKU;
			ViewData["description"] = viewData.Description;
			ViewData["smallImageURL"] = viewData.SmallImageUrl;
			ViewData["FullImageURL"] = viewData.FullImageUrl;
			ViewData["discountPercentage"] = viewData.DiscountPercentage;
			ViewData["unitPrice"] = viewData.UnitPrice;
			ViewData["unitsInStock"] = viewData.UnitsInStock;
			ViewData["departmentID"] = new SelectList(departments, "DepartmentID", "Title", viewData.DepartmentID);

			ViewData["PageTitle"] = "Update Product";
			return View("CreateProduct", viewData);
		}

		/// <summary>
		/// Edits the product_ on post.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="departmentId">The department id.</param>
		/// <param name="title">The title.</param>
		/// <param name="description">The description.</param>
		/// <param name="sku">The sku.</param>
		/// <param name="unitPrice">The unit price.</param>
		/// <param name="discountPercentage">The discount percentage.</param>
		/// <param name="unitsInStock">The units in stock.</param>
		/// <param name="smallImageURL">The small image URL.</param>
		/// <param name="fullImageURL">The full image URL.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		[AcceptVerbs("POST"), ActionName("EditProduct")]
        [ValidateInput(false)]   
		public ActionResult EditProduct_OnPost(int id, int? departmentId, string title, string description, string sku, decimal? unitPrice, int? discountPercentage, int? unitsInStock, string smallImageURL, string fullImageURL)
		{
			// Populate Drop Downs
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var departments = dc.Departments.GetDepartments();

			// Update Exsisting Department Data
			if (!String.IsNullOrEmpty(title))
			{
				try
				{
					Product product = dc.Products.GetProduct(id);

					product.Title = title;
					product.Description = description;
					product.AddedBy = User.Identity.Name;
					product.AddedDate = DateTime.Now;
					product.DepartmentID = departmentId ?? -1;
					product.DiscountPercentage = discountPercentage ?? 0;
					product.FullImageUrl = fullImageURL;
					product.SmallImageUrl = smallImageURL;
					product.SKU = sku;
					product.UnitPrice = unitPrice ?? 0;
					product.UnitsInStock = unitsInStock ?? 0;

					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your product has been modified.";
					return RedirectToAction("ManageProducts");
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["departmentID"] = new SelectList(departments, "DepartmentID", "Title", departmentId);

			// Bring up blank form
			ViewData["PageTitle"] = "Create Product";
			return View();
		}


		/// <summary>
		/// Deletes the product.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		[Service, HttpPostOnly]
		public ActionResult DeleteProduct(int id)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();

			Product product = dc.Products.GetProduct(id);
			dc.Products.DeleteOnSubmit(product);
			dc.SubmitChanges();

			return View(new { id = id });
		}

		#endregion

		#region Orders

		/// <summary>
		/// Manages the orders.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult ManageOrders()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var orders = dc.Orders.OrderBy(o => o.AddedDate);

			ViewData["PageTitle"] = "Manage Orders";
			return View(orders);
		}

		/// <summary>
		/// Orders the detail.
		/// </summary>
		/// <param name="orderId">The order id.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult OrderDetail(int orderId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Order viewData = dc.Orders.FirstOrDefault(o => o.OrderID == orderId);

			ViewData["PageTitle"] = "Order Details";
			return View(viewData);
		}

		/// <summary>
		/// Orders the detail_ on post.
		/// </summary>
		/// <param name="orderId">The order id.</param>
		/// <param name="trackingId">The tracking id.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		[AcceptVerbs("POST"), ActionName("OrderDetail")]
		public ActionResult OrderDetail_OnPost(int orderId, string trackingId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Order viewData = dc.Orders.FirstOrDefault(o => o.OrderID == orderId);

			if (!String.IsNullOrEmpty(trackingId))
			{
				viewData.TrackingID = trackingId;
				viewData.Status = "Shipped";
				dc.SubmitChanges();

				// send tracking information to shopper
				SendTrackingEmail(viewData.TrackingID, viewData.CustomerEmail);

				TempData["SuccessMessage"] = "Order Status has been Changed, the customer has been Notified.";
			}

			ViewData["PageTitle"] = "Order Details";
			return View(viewData);
		}

		#endregion

		#region Shipping Methods

		/// <summary>
		/// Manages the shipping.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		public ActionResult ManageShipping()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.ShippingMethods.OrderBy(sm => sm.Title);

			ViewData["PageTitle"] = "Manage Shipping";
			return View(viewData);
		}

		/// <summary>
		/// Manages the shipping.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="price">The price.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		[Service, HttpPostOnly]
        public ActionResult CreateShipping(string title, decimal price)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			ShippingMethod shippingMethod = new ShippingMethod() {
				Title = title,
				Price = price,
				AddedDate = DateTime.Now,
				AddedBy = User.Identity.Name
			};
			dc.ShippingMethods.InsertOnSubmit(shippingMethod);
			dc.SubmitChanges();

			return View(new { id = shippingMethod.ShippingMethodID });
		}

		/// <summary>
		/// Deletes the shipping.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="title">The title.</param>
		/// <param name="price">The price.</param>
		/// <returns></returns>
		[Authorize(Roles = "StoreKeeper")]
		[Service, HttpPostOnly]
		public ActionResult DeleteShipping(int id)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			ShippingMethod shippingMethod = dc.ShippingMethods.GetShippingMethod(id);

			dc.ShippingMethods.DeleteOnSubmit(shippingMethod);
			dc.SubmitChanges();

			return View(new { id = id });
		}

		#endregion

		#endregion

		/// <summary>
		/// Transactions the data request.
		/// </summary>
		/// <param name="requestPaypalData">The request paypal data.</param>
		/// <returns></returns>
		[NonAction]
		private string TransactionDataRequest(string tx)
		{
			// read the original IPN post
			string payPalServer = Configuration.TheBeerHouseSection.Current.Commerce.PayPalServer;
			string formValues = Encoding.ASCII.GetString(HttpContext.Request.BinaryRead(HttpContext.Request.ContentLength));
			string requestFormValues = formValues + String.Format("&cmd={0}&at={1}&tx={2}",
				"_notify-synch",
				Configuration.TheBeerHouseSection.Current.Commerce.PayPalIdentityToken,
				tx
			);

			// create the pay pal request
			HttpWebRequest payPalRequest = (HttpWebRequest)WebRequest.Create(payPalServer);
			payPalRequest.Method = "POST";
			payPalRequest.ContentType = "application/x-www-form-urlencoded";
			payPalRequest.ContentLength = requestFormValues.Length;

			// write the request back IPN strings
			using (StreamWriter writer = new StreamWriter(payPalRequest.GetRequestStream(), Encoding.ASCII))
			{
				writer.Write(requestFormValues);
				writer.Close();
			}

			// send the request to pay pal
			using (HttpWebResponse payPayResponse = (HttpWebResponse)payPalRequest.GetResponse())
			using (Stream payPalResponseStream = payPayResponse.GetResponseStream())
			using (StreamReader reader = new StreamReader(payPalResponseStream, Encoding.UTF8))
			{
				string ipnStatus = reader.ReadToEnd();
				return ipnStatus;
			}
		}

		/// <summary>
		/// Extracts the value.
		/// </summary>
		/// <param name="pdt">The PDT.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		/// <seealso href="http://mvcsamples.codeplex.com/SourceControl/changeset/view/22882#386911">Thanks to Rob Conery of wekeroad.com for this code</seealso>
		[NonAction]
		private string ExtractValue(string pdt, string key)
		{
			string[] keys = pdt.Split('\n');
			string output = String.Empty;
			string thisKey = String.Empty;

			foreach (string item in keys)
			{
				string[] bits = item.Split('=');
				if (bits.Length > 1)
				{
					output = bits[1];
					thisKey = bits[0];
					if (thisKey.Equals(key, StringComparison.InvariantCultureIgnoreCase))
						break;
				}
			}
			
			return HttpContext.Server.UrlDecode(output);
		}

		/// <summary>
		/// Sends the tracking email.
		/// </summary>
		/// <param name="trackingID">The tracking ID.</param>
		/// <param name="customerEmail">The customer email.</param>
		[NonAction]
		private void SendTrackingEmail(string trackingID, string customerEmail)
		{
			MailMessage mailMessage = new MailMessage();

			mailMessage.From = new MailAddress(Configuration.TheBeerHouseSection.Current.Commerce.PayPalAccount);
			mailMessage.To.Add(new MailAddress(customerEmail));

			mailMessage.Subject = "Order has been shipped";
			mailMessage.Body = "Your package has been shipped, your tracking # is " + trackingID;
			
			SmtpClient smtpClient = new SmtpClient();
			smtpClient.Send(mailMessage);
		}
	}
}