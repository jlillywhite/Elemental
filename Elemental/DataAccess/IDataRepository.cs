using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elemental.DataAccess
{
	public interface IDataRepository
	{
		T GetOne<T>(string Id) where T : IModel;
		T GetOne<T>(Func<T, bool> filterFunction) where T : IModel;

		IEnumerable<T> GetAll<T>() where T : IModel;

		IEnumerable<T> Get<T>(Func<T, bool> filterFunction) where T: IModel;

		void Save<T>(T modelToSave) where T : IModel;

		void Delete<T>(T modelToDelete) where T : IModel;

		event BeforeLoadEventHandler BeforeLoad;
		event LoadEventHandler Loaded;
		event BeforeSaveEventHandler BeforeSave;
		event SaveEventHandler Saved;
		event BeforeDeleteEventHandler BeforeDelete;
		event DeleteEventHandler Deleted;
		event RepositoryInitHandler Initialized;

	}
}
