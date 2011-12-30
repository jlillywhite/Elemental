using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Elemental.DataAccess.Configuration
{
	[ConfigurationCollection(typeof(RepositoryElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class RepositoryElementCollection : ConfigurationElementCollection
	{
		public RepositoryElement this[int index]
		{
			get
			{
				return (RepositoryElement)base.BaseGet(index);
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

		new public RepositoryElement this[string provides]
		{
			get
			{
				return (RepositoryElement)base.BaseGet(provides);
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new RepositoryElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			RepositoryElement result = element as RepositoryElement;
			return result == null ? null : result.Provides;
		}
	}
}