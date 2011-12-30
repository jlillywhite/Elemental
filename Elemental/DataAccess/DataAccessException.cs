using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elemental.DataAccess
{
	public class DataAccessException: System.ApplicationException
	{
		public DataAccessException(): base()
		{
		}

		public DataAccessException(string message): base(message)
		{
		}

		public DataAccessException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}
