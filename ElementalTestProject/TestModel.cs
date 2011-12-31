using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elemental.DataAccess;

namespace ElementalTestProject
{
	internal class TestModel : IModel
	{
		#region IModel Members

		public string Id { get; set; }

		#endregion
		public string Name { get; set; }
	}
}
