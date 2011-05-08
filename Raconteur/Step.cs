using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Raconteur.Compilers;

namespace Raconteur
{
    public enum StepType
    {
        Simple,
        Table,
        HeaderTable,
        ObjectTable
    }

    public class Step
    {
        public StepType Type;

        public Feature Feature { get; set; }

        public string Name { get; set; }
        public List<string> Args { get; set; }
        public List<string> OriginalArgs { get; set; }

        public bool HasArgs { get { return Args.Count > 0; } }

        public MethodInfo Implementation { get; set; }
        
        public bool IsImplemented { get { return Implementation != null; } }

        public ParameterInfo[] ArgDefinitions { get { return Implementation.GetParameters(); } }

        public Step()
        {
            Args = new List<string>();
        }

        public string Call
        {
            get
            {
                if (!IsImplemented || Feature == null || !Feature.HasStepDefinitions) 
                    return Name;

                return  IsImplementedInFeatureSteps ? Name :
                    StepDefinitions.Name + "." + Name;
            } 
        }

        bool IsImplementedInFeatureSteps
        {
            get
            {
                return StepDefinitions == null || 
                    StepDefinitions == Feature.DefaultStepDefinitions;
            }
        }

        Type stepDefinitions;
        Type StepDefinitions
        {
            get
            {
                return stepDefinitions ?? (stepDefinitions = 
                    Feature.StepDefinitions.Where(ImplementsStep).FirstOrDefault());
            } 
        }
        
        bool ImplementsStep(Type StepDefinition)
        {
            return StepDefinition == Implementation.DeclaringType
                || StepDefinition.IsSubclassOf(Implementation.DeclaringType);
        }

        public bool HasTable { get { return Table != null; } }

        Table table;
        public Table Table
        {
            get { return table; } 
            set
            {
                table = value;

                Type = table.HasHeader? 
                    StepType.HeaderTable : 
                    StepType.Table;
            }
        }

        public void AddRow(List<string> Row)
        {
            if (!HasTable) Table = new Table();

            Table.Add(Row);
        }

        Type objectArg;
        public Type ObjectArg
        {
            get { return objectArg ?? (objectArg = Implementation.LastArg().ElementType()); }
        }
    }
}