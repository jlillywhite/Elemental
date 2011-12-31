using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Elemental.Routing.Configuration
{
	[ConfigurationCollection(typeof(IgnoredRouteElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class IgnoredRouteElementCollection : ConfigurationElementCollection
	{
		public IgnoredRouteElement this[int index]
		{
			get
			{
				return (IgnoredRouteElement)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				base.BaseAdd(index, value);
			}
		}

		new public IgnoredRouteElement this[string url]
		{
			get
			{
				return (IgnoredRouteElement)base.BaseGet(url);
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new IgnoredRouteElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			IgnoredRouteElement result = element as IgnoredRouteElement;
			return result == null ? null : result.Url;
		}
	}
}