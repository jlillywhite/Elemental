using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Reflection;

namespace Elemental.DataAccess.Configuration
{
	public class RepositoryElement : ConfigurationElement
	{

		/// <summary>
		/// This attribute designates what the handler is supposed to handle.
		/// For example, the cache handler for the site would have a "handles" attribute equal to "Cache"
		/// </summary>
		[ConfigurationProperty("provides", IsRequired = true, IsKey = true)]
		public string Provides
		{
			get
			{
				return (string)this["provides"];
			}
			set
			{
				this["provides"] = value;
			}
		}


		[ConfigurationProperty("type", IsRequired = true)]
		public string TypeName
		{
			get
			{
				return (string)this["type"];
			}
			set
			{
				this["type"] = value;
			}
		}

		public Type Type
		{
			get
			{
				Type result;
				try
				{
					result = Type.GetType((string)this["type"]);
				}
				catch (TypeLoadException ex)
				{
					throw new ArgumentException(string.Format(Resources.ErrorMessages.UnableToLoadHandlerType, this["type"].ToString()), ex);
				}
				return result;
			}
			set
			{
				this["type"] = value.Name;
			}
		}
	}
}