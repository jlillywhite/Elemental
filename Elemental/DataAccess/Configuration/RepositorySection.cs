using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Elemental.DataAccess.Configuration
{
	public class RepositorySection : ConfigurationSection
	{
		[ConfigurationProperty("repository")]
		public RepositoryElementCollection Handlers
		{
			get
			{
				return (RepositoryElementCollection)this["repository"] ??
					new RepositoryElementCollection();
			}
			set
			{
				this["repository"] = value;
			}
		}
	}
}