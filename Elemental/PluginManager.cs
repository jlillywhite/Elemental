using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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

		}

		#region Properties
		public List<Assembly> PluginAssemblies { get; private set; }
		#endregion
		#endregion
	}
}
