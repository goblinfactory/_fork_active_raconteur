﻿using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Raconteur.Generators;

namespace Raconteur.IDEIntegration
{
    [ComVisible(true)]
    [Guid("0509186D-E29A-49C2-A71F-0E530E52D199")]
    public abstract class RaconteurSingleFileGeneratorBase : BaseCodeGeneratorWithSite
    {
        protected override string GetDefaultExtension()
        {
            return "." + GetCodeProvider().FileExtension;
        }

        protected override string GenerateCode(string InputFileContent)
        {
            var Project = DteProjectReader.LoadProjectFrom(CurrentProject);
            Project.DefaultNamespace = Project.DefaultNamespace ?? CodeFileNameSpace;
            var Generator = new RaconteurGenerator(Project);

            using (var Writer = new StringWriter(new StringBuilder()))
            {
                Generator.GenerateTestFile
                (
                    Project.GetOrCreateFeatureFile(CodeFilePath), 
                    GetCodeProvider(),
                    new StringReader(InputFileContent), 
                    Writer
                );

                return Writer.ToString();
            }
        }
    }
}