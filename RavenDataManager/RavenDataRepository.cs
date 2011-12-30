using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elemental.DataAccess;
using Raven.Client.Embedded;
using Raven.Client;

namespace RavenDataManager
{
	public sealed class RavenDataRepository : IDataRepository
	{
		IDocumentSession session;

		public RavenDataRepository()
		{
			session = RavenDocumentStore.Instance.OpenSession();
			OnInit();
		}

		#region Public Members
		#region IDataRepository Members

		public T GetOne<T>(string Id) where T : IModel
		{
			string message;
			if (!OnBeforeLoad<T>(Id, out message))
			{
				throw new DataAccessException(message);
			}
			T result = session.Query<T>().SingleOrDefault(t => t.Id.Equals(Id, StringComparison.InvariantCultureIgnoreCase));
			OnLoad(result);
			return result;
		}

		public T GetOne<T>(Func<T, bool> filterFunction) where T : IModel
		{
			string message;
			T result = session.Query<T>().SingleOrDefault(filterFunction);
			if (!OnBeforeLoad<T>(result.Id, out message))
			{
				throw new DataAccessException(message);
			}
			OnLoad(result);
			return result;
		}

		public IEnumerable<T> GetAll<T>() where T : IModel
		{
			string message;
			IEnumerable<T> results = session.Query<T>();
			foreach(T result in results){
				if (OnBeforeLoad<T>(result.Id, out message))
				{
					OnLoad(result);
					yield return result;
				}
			}
		}

		public IEnumerable<T> Get<T>(Func<T, bool> filterFunction) where T : IModel
		{
			string message;
			IEnumerable<T> results = session.Query<T>().Where(filterFunction);
			foreach(T result in results){
				if (OnBeforeLoad<T>(result.Id, out message))
				{
					OnLoad(result);
					yield return result;
				}
			} 
		}

		public void Save<T>(T modelToSave) where T : IModel
		{
			string message;
			if (!OnBeforeSave(modelToSave, out message))
			{
				throw new DataAccessException(message);
			}
			session.Store(modelToSave);
			session.SaveChanges();
			OnSave(modelToSave);
		}

		public void Delete<T>(T modelToDelete) where T : IModel
		{
			string message;
			if (!OnBeforeDelete(modelToDelete, out message))
			{
				throw new DataAccessException(message);
			}
			session.Delete<T>(modelToDelete);
			session.SaveChanges();
			OnDelete(modelToDelete);
		}
		public event BeforeLoadEventHandler BeforeLoad;

		public event LoadEventHandler Loaded;

		public event BeforeSaveEventHandler BeforeSave;

		public event SaveEventHandler Saved;

		public event BeforeDeleteEventHandler BeforeDelete;

		public event DeleteEventHandler Deleted;

		public event RepositoryInitHandler Initialized;

		#endregion
		#endregion

		#region Private Members

		private void OnInit()
		{
			if (Initialized != null)
			{
				EventArgs args = new EventArgs();
				Initialized(this, args);
			}
		}

		private bool OnBeforeLoad<T>(string objectID, out string message)
		{
			bool result = true;
			message = string.Empty;
			if (BeforeLoad != null)
			{
				BeforeLoadEventArgs  args = new BeforeLoadEventArgs{
					ObjectId = objectID
				};
				result = BeforeLoad(this, args, out message);
			}
			return result;
		}

		private void OnLoad<T>(T loadedObject)
		{
			if (Loaded != null)
			{
				LoadEventArgs args = new LoadEventArgs
				{
					LoadedObject = loadedObject
				};
				Loaded(this, args);
			}
		}

		private bool OnBeforeSave<T>(T objectToSave, out string message)
		{
			bool result = true;
			message = string.Empty;
			if (BeforeSave != null)
			{
				BeforeSaveEventArgs args = new BeforeSaveEventArgs
				{
					ObjectToSave = objectToSave
				};
				result = BeforeSave(this, args, out message);
			}
			return result;
		}

		private void OnSave<T>(T savedObject)
		{
			if (Saved != null)
			{
				SaveEventArgs args = new SaveEventArgs
				{
					SavedObject = savedObject
				};
				Saved(this, args);
			}
		}


		private bool OnBeforeDelete<T>(T objectToDelete, out string message)
		{
			bool result = true;
			message = string.Empty;
			if (BeforeDelete != null)
			{
				BeforeDeleteEventArgs args = new BeforeDeleteEventArgs
				{
					ObjectToDelete = objectToDelete
				};
				result = BeforeDelete(this, args, out message);
			}
			return result;
		}

		private void OnDelete<T>(T DeletedObject)
		{
			if (Deleted != null)
			{
				DeleteEventArgs args = new DeleteEventArgs
				{
					DeletedObject = DeletedObject
				};
				Deleted(this, args);
			}
		}

		private class RavenDocumentStore
		{
			private static EmbeddableDocumentStore instance = new EmbeddableDocumentStore
			{
				DataDirectory = "Elemental"
			};

			public static EmbeddableDocumentStore Instance
			{
				get
				{
					return instance;
				}
			}

		}
		#endregion
	}
}
