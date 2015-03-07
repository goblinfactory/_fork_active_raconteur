<Query Kind="Program" />

string root = Environment.CurrentDirectory;

void Main()
{
	InitResharperReferences();
	
}

void InitResharperReferences() 
{
	var resharperFolder = FindResharperLibFolder();
	var dest = Path.Combine(root,@"dotnet\lib\resharper");
	CopyFilesIfNotExist("RESHARPER", resharperFolder, Files_Resharper, dest, false);
}

void CopyFilesIfNotExist(string dependancy, string srcFolder, string[] fileList, string destFolder, bool overwrite)
{
	var missing = new List<string>();
	Console.Write("Checking dependancy : {0} -> ", dependancy);
	if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);
	foreach(var file in fileList) 
	{
		var src = Path.Combine(srcFolder,file);
		var fi =  new FileInfo(src);
		if (fi.Exists)
		{
			var dest = Path.Combine(destFolder, fi.Name);
			if (File.Exists(dest) && overwrite == false) continue;
			Console.WriteLine("		copying {0} to {1}",file, destFolder);
			File.Copy(src,dest,true);		
		}
		else
		{
			Console.WriteLine("Missing assembly, could not find:{0}",file);
			missing.Add(file);
		}
	}
	if (missing.Count ()==0) 
		Console.WriteLine("found all required files.");
	else
		{
			Console.WriteLine("---------------------");
			Console.WriteLine("Some files missing!");
			Console.WriteLine("");
			missing.ForEach(Console.WriteLine);
			Console.WriteLine("---------------------");			
		}
	return;
}

string FindResharperLibFolder() {
	var src = @"C:\Program Files (x86)\JetBrains\ReSharper\v{ver}\Bin";
	var versions = new [] { "9.0", "8.2.3", "8.2.2","8.2.1","8.2","8.1","8.0.2","8.0.1","8.0","7.1.3","7.1.2","7.1.1","7.1","7.0.1","7.0" };
	foreach(var v in versions)
	{
		var folder = src.Replace("{ver}",v);
		if (Directory.Exists(folder)) 
		{
			Console.WriteLine("Found resharper version '{0}' at :{1}",v,folder);
			return folder;
		}
	}
	throw new ApplicationException("Unable to find any Resharper version, aborting.");
}

static string[] Files_Resharper = new string[] {
"jetbrains.annotations.dll",
"jetbrains.decompiler.core.dll",
"jetbrains.platform.commonservices.dll",
"jetbrains.platform.resharper.abstracttreebuilder.dll",
"jetbrains.platform.resharper.actionmanagement.dll",
"jetbrains.platform.resharper.activitytracking.dll",
"jetbrains.platform.resharper.componentmodel.dll",
"jetbrains.platform.resharper.documentmanager.dll",
"jetbrains.platform.resharper.documentmodel.dll",
"jetbrains.platform.resharper.ide.dll",
"jetbrains.platform.resharper.interop.winapi.dll",
"jetbrains.platform.resharper.metadata.dll",
"jetbrains.platform.resharper.projectmodel.dll",
"jetbrains.platform.resharper.shell.dll",
"jetbrains.platform.resharper.symbols.dll",
"jetbrains.platform.resharper.textcontrol.dll",
"jetbrains.platform.resharper.ui.dll",
"jetbrains.platform.resharper.util.dll",
"jetbrains.platform.resharper.vsintegration.dll",
"jetbrains.resharper.externalsources.dll",
"jetbrains.resharper.externalsources.vs.dll",
"jetbrains.resharper.feature.services.dll",
"jetbrains.resharper.feature.services.csharp.dll",
"jetbrains.resharper.feature.services.externalsources.dll",
"jetbrains.resharper.feature.services.externalsources.csharp.dll",
"jetbrains.resharper.feature.services.xml.dll",
"jetbrains.resharper.features.altering.dll",
"jetbrains.resharper.features.browsing.dll",
"jetbrains.resharper.features.common.dll",
"jetbrains.resharper.features.environment.dll",
"jetbrains.resharper.features.finding.dll",
"jetbrains.resharper.features.intellisense.dll",
"jetbrains.resharper.features.internal.dll",
"jetbrains.resharper.i18n.services.dll",
"jetbrains.resharper.i18n.services.csharp.dll",
"jetbrains.resharper.inplacerefactorings.dll",
"jetbrains.resharper.intentions.dll",
"jetbrains.resharper.intentions.csharp.dll",
"jetbrains.resharper.newrefactorings.dll",
"jetbrains.resharper.psi.dll",
"jetbrains.resharper.psi.csharp.dll",
"jetbrains.resharper.psi.services.dll",
"jetbrains.resharper.psi.services.csharp.dll",
"jetbrains.resharper.refactorings.csharp.dll",
"jetbrains.resharper.resources.dll",
"jetbrains.resharper.vs.dll",
"jetbrains.resharper.vsi.dll",
"JetBrains.ReSharper.Feature.Services.Navigation.dll",
"JetBrains.ReSharper.Daemon.dll",
};