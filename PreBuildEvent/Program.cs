using System.Xml;
using System.Linq;

namespace PreBuildEvent
{
	/// <summary>
	/// In .NET MVC projects, by default, Views, CSS files, javascript files, and images are 
	/// not compiled into the DLL for the project.  In order for widgets to work as discrete libraries,
	/// their DLL's have to include everything that the widget needs to function, including the Views,
	/// CSS files, javascript files, and images.  This application will scan a project file, looking for
	/// references to files that need to be included in the DLL.  If it finds any that are not already included,
	/// it will modify the project so that when the project is compiled, those files are included in the library as
	/// embedded resources.  This application should be called as a pre-build event for all widget projects.
	/// </summary>
	class Program
	{
		private static string[] excludedExtensions = new string[] { ".asax", ".config" };

		/// <summary>
		/// This function will set all content files to be embedded resources
		/// </summary>
		/// <param name="args">
		///Command Line Arguments:
		/// 0 - Path to project file.
		/// </param>
		static void Main(string[] args)
		{
			string pathToProject = args[0];
			if (System.IO.File.Exists(pathToProject))
			{
				XmlDocument projectFile = new XmlDocument();
				projectFile.Load(pathToProject);
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(projectFile.NameTable);
				nsMgr.AddNamespace("build", "http://schemas.microsoft.com/developer/msbuild/2003");
				XmlNodeList contentNodes = projectFile.SelectNodes("build:Project/build:ItemGroup/build:Content", nsMgr);
				bool wasProjectModified = false;
				foreach (XmlNode contentNode in contentNodes)
				{
					if (contentNode.Attributes["Include"] != null)
					{
						string extension = System.IO.Path.GetExtension(contentNode.Attributes["Include"].Value);
						if (!excludedExtensions.Contains(extension))
						{
							XmlNode resourceNode = projectFile.CreateElement("EmbeddedResource", nsMgr.LookupNamespace("build"));
							XmlAttribute includeAttribute = projectFile.CreateAttribute("Include");
							includeAttribute.Value = contentNode.Attributes["Include"].Value;
							resourceNode.Attributes.Append(includeAttribute);
							XmlNode parentNode = contentNode.ParentNode;
							parentNode.ReplaceChild(resourceNode, contentNode);
							wasProjectModified = true;
						}
					}
				}
				if (wasProjectModified)
				{
					projectFile.Save(pathToProject);
				}
			}
		}
	}
}
