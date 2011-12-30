using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elemental.DataAccess
{
	public delegate void RepositoryInitHandler(object sender, EventArgs e);
	public delegate void LoadEventHandler(object sender, LoadEventArgs e);
	public delegate bool BeforeLoadEventHandler(object sender, BeforeLoadEventArgs e, out string message);
	public delegate void SaveEventHandler (object sender, SaveEventArgs e);
	public delegate bool BeforeSaveEventHandler(object sender, BeforeSaveEventArgs e, out string message);
	public delegate void DeleteEventHandler(object sender, DeleteEventArgs e);
	public delegate bool BeforeDeleteEventHandler(object sender, BeforeDeleteEventArgs e, out string message);

	public class SaveEventArgs : System.EventArgs
	{
		public object SavedObject { get; set; }
	}
	public class DeleteEventArgs : System.EventArgs
	{
		public object DeletedObject { get; set; }
	}
	public class LoadEventArgs : System.EventArgs
	{
		public object LoadedObject { get; set; }
	}
	public class BeforeLoadEventArgs : System.EventArgs
	{
		public string ObjectId { get; set; }
	}
	public class BeforeSaveEventArgs : System.EventArgs
	{
		public object ObjectToSave { get; set; }
	}
	public class BeforeDeleteEventArgs : System.EventArgs
	{
		public object ObjectToDelete { get; set; }
	}
}
