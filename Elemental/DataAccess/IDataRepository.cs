using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elemental.DataAccess
{
	public interface IDataRepository
	{
		T GetOne<T>(string Id)
			where T : class, IModel;
		T GetOne<T>(Func<T, bool> filterFunction)
			where T : class, IModel;

		IEnumerable<T> GetAll<T>()
			where T : class, IModel;

		IEnumerable<T> Get<T>(Func<T, bool> filterFunction)
			where T : class, IModel;

		string Save<T>(T modelToSave)
			where T : class, IModel;

		void Delete<T>(T modelToDelete)
			where T : class, IModel;

		event BeforeLoadEventHandler BeforeLoad;
		event LoadEventHandler Loaded;
		event BeforeSaveEventHandler BeforeSave;
		event SaveEventHandler Saved;
		event BeforeDeleteEventHandler BeforeDelete;
		event DeleteEventHandler Deleted;
		event RepositoryInitHandler Initialized;

	}
}
