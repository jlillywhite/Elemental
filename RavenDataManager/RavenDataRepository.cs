using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elemental.DataAccess;
using Raven.Client.Embedded;
using Raven.Client;
using System.Web.Caching;
using System.Web;
using Raven.Client.Document;
using System.Configuration;

namespace RavenDataManager
{
	public sealed class RavenDataRepository : IDataRepository
	{
		IDocumentSession session;
		Cache cache;
		int cacheTime = 20;

		public RavenDataRepository()
		{
			session = RavenDocumentStore.Instance.DocStore.OpenSession();
			cache = HttpRuntime.Cache;
			OnInit();
		}

		#region Public Members
		#region IDataRepository Members

		public T GetOne<T>(string id) where T : class, IModel
		{
			string message;
			T result;
			if (!OnBeforeLoad<T>(id, out message))
			{
				throw new DataAccessException(message);
			}
			lock (cache)
			{
				object cacheObject = cache[id];
				result = cacheObject as T;
				if (result == null)
				{
					if (cacheObject != null)
					{
						cache.Remove(id);
					}
					result = session.Query<T>().SingleOrDefault(t => t.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
					AddToCache(result);
				}
			}
			OnLoad(result);
			return result;
		}


		public T GetOne<T>(Func<T, bool> filterFunction) where T : class, IModel
		{
			string message;
			T result = session.Query<T>().SingleOrDefault(filterFunction);
			if (!OnBeforeLoad<T>(result.Id, out message))
			{
				throw new DataAccessException(message);
			}
			AddToCache(result);
			OnLoad(result);
			return result;
		}

		public IEnumerable<T> GetAll<T>() where T : class, IModel
		{
			string message;
			IEnumerable<T> results = session.Query<T>();
			foreach (T result in results)
			{
				if (OnBeforeLoad<T>(result.Id, out message))
				{
					AddToCache(result);
					OnLoad(result);
					yield return result;
				}
			}
		}

		public IEnumerable<T> Get<T>(Func<T, bool> filterFunction) where T : class, IModel
		{
			string message;
			IEnumerable<T> results = session.Query<T>().Where(filterFunction);
			foreach (T result in results)
			{
				if (OnBeforeLoad<T>(result.Id, out message))
				{
					AddToCache(result);
					OnLoad(result);
					yield return result;
				}
			}
		}

		public string Save<T>(T modelToSave) where T : class, IModel
		{
			string message;
			if (!OnBeforeSave(modelToSave, out message))
			{
				throw new DataAccessException(message);
			}
			session.Store(modelToSave);
			session.SaveChanges();
			AddToCache(modelToSave);
			OnSave(modelToSave);
			return modelToSave.Id;
		}

		public void Delete<T>(T modelToDelete) where T : class, IModel
		{
			string message;
			if (!OnBeforeDelete(modelToDelete, out message))
			{
				throw new DataAccessException(message);
			}
			session.Delete<T>(modelToDelete);
			session.SaveChanges();
			lock (cache)
			{
				if (cache[modelToDelete.Id] != null)
				{
					cache.Remove(modelToDelete.Id);
				}
			}
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
		private void AddToCache<T>(T result) where T : class, IModel
		{
			lock (cache)
			{
				if (cache[result.Id] != null)
				{
					cache[result.Id] = result;
				}
				else
				{
					cache.Add(result.Id, result, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheTime, 0), CacheItemPriority.Low, null);
				}
			}
		}

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
				BeforeLoadEventArgs args = new BeforeLoadEventArgs
				{
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

		private class RavenDocumentStore : IDisposable
		{
			private static object lockObj = new object();
			private static volatile RavenDocumentStore instance;

			public static RavenDocumentStore Instance
			{
				get
				{
					if (instance == null)
					{
						lock (lockObj)
						{
							if (instance == null)
							{
								instance = new RavenDocumentStore();
							}
						}
					}
					return instance;
				}
			}

			private RavenDocumentStore()
			{
				string dataDirectory = ConfigurationManager.ConnectionStrings["Elemental"].ConnectionString;
				DocStore = new EmbeddableDocumentStore
				{
					DataDirectory = dataDirectory
				};
				DocStore.Initialize();

			}

			public EmbeddableDocumentStore DocStore { get; private set; }



			#region IDisposable Members

			public void Dispose()
			{
				DocStore.Dispose();
			}

			#endregion
		}
		#endregion
	}
}
