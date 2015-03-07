using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.Util;
using Raconteur.Helpers;
using JetBrains.Application;
using JetBrains.ReSharper.Psi.Search;

namespace Raconteur.Resharper
{
    public class RenameStepWorkflow : RenameWorkflow
    {
        public RenameStepWorkflow(
            IShellLocks locks, 
            SearchDomainFactory searchDomainFactory, 
            RenameRefactoringService renameRefactoringService, 
            ISolution solution, 
            string actionId) 
            : base(locks, searchDomainFactory, renameRefactoringService, solution, actionId)
        {
        }

        public override bool PostExecute(IProgressIndicator pi) 
        {
            try 
            {
                if (!base.PostExecute(pi)) return false;

                RunnerFileWatcher.Path = Solution.SolutionFilePath.Directory.FullPath;

                RunnerFileWatcher.OnFileChange(f =>
                    ObjectFactory.NewRenameStep
                    (
                        f.FeatureFileFromRunner(), 
                        InitialName, 
                        InitialStageExecuter.NewName
                    )
                    .Execute());

            } 
            catch (Exception e)
            {
                MessageBox.ShowInfo(e.ToString());
            }

            return true;
        }
    }
}