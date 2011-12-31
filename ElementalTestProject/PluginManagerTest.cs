using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Elemental;
using Elemental.DataAccess;

namespace ElementalTestProject
{
	[TestClass]
	public class PluginManagerTest
	{
		[TestMethod]
		public void GetDataRepositoryTest()
		{
			IDataRepository repository = PluginManager.Instance.DataRepository;
			Assert.IsNotNull(repository);
		}
	}
}
