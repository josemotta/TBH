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
using System.Runtime.Serialization;
using System.Reflection;

namespace TheBeerHouse.Models
{
	[Serializable]
	public partial class Product : ISerializable
	{
		#region ISerializable Members

		/// <summary>
		/// Initializes a new instance of the <see cref="ShoppingCartItem"/> class.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		internal Product(SerializationInfo info, StreamingContext context)
		{
			PropertyInfo[] properties = GetType().GetProperties();
			SerializationInfoEnumerator enumerator = info.GetEnumerator();

			while (enumerator.MoveNext())
			{
				SerializationEntry se = enumerator.Current;
				foreach (PropertyInfo pi in properties)
				{
					if (pi.Name == se.Name)
					{
						pi.SetValue(this, info.GetValue(se.Name, pi.PropertyType), null);
					}
				}
			}
		}

		/// <summary>
		/// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			PropertyInfo[] infos = GetType().GetProperties();
			foreach (PropertyInfo pi in infos)
			{
				bool isAssociation = false;
				foreach (object obj in pi.GetCustomAttributes(true))
				{
					if (obj.GetType() == typeof(System.Data.Linq.Mapping.AssociationAttribute))
					{
						isAssociation = true;
						break;
					}
				}
				if (!isAssociation)
				{
					if (pi.GetValue(this, null) != null)
					{
						info.AddValue(pi.Name, pi.GetValue(this, null));
					}
				}
			}
		}

		#endregion
	}
}