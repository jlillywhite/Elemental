using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Elemental.DataAccess;
using System.Configuration;
using Elemental.DataAccess.Configuration;

namespace Elemental
{
	public class PluginManager
	{
		#region Static Members
		private static PluginManager instance = new PluginManager();
		public static PluginManager Instance
		{
			get
			{
				return instance;
			}
		}
		#endregion

		#region Instance Members
		private PluginManager()
		{
			RepositorySection section = ((RepositorySection)ConfigurationManager.GetSection("dataRepositories"));
			Type dataRepositoryType = section.Repositories["Database"].Type;
			DataRepository = Activator.CreateInstance(dataRepositoryType) as IDataRepository;
		}

		#region Properties
		public IDataRepository DataRepository { get; private set; }
		#endregion
		#endregion
	}
}
