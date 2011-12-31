using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Elemental.DataAccess.Configuration
{
	public class RepositorySection : ConfigurationSection
	{
		[ConfigurationProperty("repositories")]
		public RepositoryElementCollection Repositories
		{
			get
			{
				return (RepositoryElementCollection)this["repositories"] ??
					new RepositoryElementCollection();
			}
			set
			{
				this["repositories"] = value;
			}
		}
	}
}