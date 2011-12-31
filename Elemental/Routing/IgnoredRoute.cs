using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Elemental.Routing
{
	public struct IgnoredRoute
	{
		public object Constraints { get; private set; }
		public string Url { get; private set; }

		public IgnoredRoute(string url, object constraints): this()
		{
			this.Constraints = constraints;
			this.Url = url;
		}

		public IgnoredRoute(string url): this()
		{
			this.Constraints = null;
			this.Url = url;
		}
	}
}