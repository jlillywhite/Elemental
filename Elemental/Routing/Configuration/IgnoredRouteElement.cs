using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Reflection;

namespace Elemental.Routing.Configuration
{
	public class IgnoredRouteElement : ConfigurationElement
	{
		[ConfigurationProperty("url", IsRequired = true, IsKey = true)]
		public string Url
		{
			get
			{
				return (string)this["url"];
			}
			set
			{
				this["url"] = value;
			}
		}

		public IgnoredRoute Route
		{
			get
			{
				return new IgnoredRoute(Url);
			}
		}
	}
}