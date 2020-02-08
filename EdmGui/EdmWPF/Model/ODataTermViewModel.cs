
using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EdmWPF.Model
{
    public class ODataTermViewModel
    {
        private IEdmModel edmModel;
        private IDictionary<string, IEdmModel> _namespacesModels = new Dictionary<string, IEdmModel>();

        public ODataTermViewModel()
        {

            TermNames = new List<string>
            {
                "123",
                "2,34"
            };
        }

        public IList<string> Namespaces
        {
            get
            {
                return _namespacesModels.Keys.ToList();
            }
        }

        public IList<string> TermNames { get; set; }

        public string Result { get; set; } = "Sample";

        public void SetEdmModel(IEdmModel model)
        {
            edmModel = model;
            Initialize(model);
        }

        public void SetOutputType(string typeName)
        {

        }

        private void Initialize(IEdmModel model)
        {
            Namespaces.Clear();
            TermNames.Clear();
            _namespacesModels.Clear();

            ISet<IEdmModel> visited = new HashSet<IEdmModel>();
            Browse(model, visited);

            foreach (var m in visited)
            {
                foreach (var ns in m.DeclaredNamespaces)
                {
                    _namespacesModels[ns] = m;
                }
            }
        }

        private void Browse(IEdmModel model, ISet<IEdmModel> visited)
        {
            if (visited.Contains(model))
            {
                return;
            }

            visited.Add(model);

            foreach (var subModel in model.ReferencedModels)
            {
                Browse(subModel, visited);
            }
        }
    }
}
