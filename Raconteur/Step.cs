using System.Collections.Generic;
using System.Reflection;

namespace Raconteur
{
    public class Step
    {
        public Feature Feature { get; set; }

        public string Name { get; set; }
        public List<string> Args { get; set; }

        public int ArgsCount
        {
            get
            {
                return 
                    Args.Count +
                    (HasTable && Table.HasHeader ?
                        Table.Header.Count : 0);
            }
        }

        public bool IsImplemented { get { return Implementation != null; } }
        public bool IsImplementedInFeatureSteps
        {
            get
            {
                return Implementation != null && Feature != null
                    && Implementation.DeclaringType.Name == Feature.Name;
            }
        }
        public MethodInfo Implementation { get; set; }
        
        public bool HasTable { get { return Table != null; } }
        public Table Table { get; set; }

        public Step()
        {
            Args = new List<string>();
        }

        public string Call
        {
            get
            {
                return !IsImplemented || IsImplementedInFeatureSteps ? Name :
                    Implementation.DeclaringType.Name + "." + Name;
            } 
        }

        public void AddRow(List<string> Row)
        {
            if (!HasTable) Table = new Table();

            if (Table.IsSingleColumn) 
                Table.Rows[0].Add(Row[0]);
            else Table.Add(Row);
        }
    }
}