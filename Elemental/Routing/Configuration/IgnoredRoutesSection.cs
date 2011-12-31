using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Elemental.Routing.Configuration
{
	public class IgnoredRoutesSection : ConfigurationSection
	{
		[ConfigurationProperty("routes")]
		public IgnoredRouteElementCollection Routes
		{
			get
			{
				return (IgnoredRouteElementCollection)this["routes"] ??
					new IgnoredRouteElementCollection();
			}
			set
			{
				this["routes"] = value;
			}
		}
	}
}