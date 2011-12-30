using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Reflection;

namespace Elemental.DataAccess.Configuration
{
	public class RepositoryElement : ConfigurationElement
	{

		/// <summary>
		/// This attribute designates what the handler is supposed to handle.
		/// For example, the cache handler for the site would have a "handles" attribute equal to "Cache"
		/// </summary>
		[ConfigurationProperty("provides", IsRequired = true, IsKey = true)]
		public string Provides
		{
			get
			{
				return (string)this["provides"];
			}
			set
			{
				this["provides"] = value;
			}
		}

		/// <summary>
		/// This attribute designates which assembly contains the implementation of the handler.
		/// The attribute should contain the simple name of the assembly, not the full name.
		/// </summary>
		[ConfigurationProperty("assembly", IsRequired = true)]
		[CallbackValidator(CallbackMethodName = "ValidateAssembly", Type = typeof(RepositoryElement))]
		public string AssemblyName
		{
			get
			{
				return (string)this["assembly"];
			}
			set
			{
				this["assembly"] = value;
			}
		}

		public Assembly Assembly
		{
			get
			{
				return Registry.Current.Assemblies.FirstOrDefault(assembly => assembly.FullName.Split(',')[0] == AssemblyName);
			}
		}

		[ConfigurationProperty("type", IsRequired = true)]
		public string TypeName
		{
			get
			{
				return (string)this["type"];
			}
			set
			{
				this["type"] = value;
			}
		}

		public Type Type
		{
			get
			{
				System.Reflection.Assembly asm = Assembly;
				if (asm != null)
				{
					Type result;
					try
					{
						result = asm.GetType((string)this["type"]);
					}
					catch (TypeLoadException ex)
					{
						throw new ArgumentException(string.Format(Resources.ErrorMessages.UnableToLoadHandlerType, this["type"].ToString()), ex);
					}
					return result;
				}
				else
				{
					throw new ArgumentException(string.Format(Resources.ErrorMessages.UnableToLoadHandlerAssembly, AssemblyName));
				}
			}
			set
			{
				this["type"] = value.Name;
			}
		}

		/// <summary>
		/// This method validates the "assembly" attribute.  If the attribute is invalid, it will throw an ArgumentException.
		/// </summary>
		/// <param name="value"></param>
		public static void ValidateAssembly(object value)
		{
			if (!string.IsNullOrEmpty((string)value))
			{
				if (!Registry.Current.Assemblies.Any(assembly => assembly.FullName.Split(',')[0] == (string)value))
				{
					throw new ArgumentException(Resources.ErrorMessages.InvalidAssembly);
				}
			}
		}
	}
}