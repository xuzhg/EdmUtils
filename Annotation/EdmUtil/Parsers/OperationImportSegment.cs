
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    public class OperationImportSegment : PathSegment
    {
        public OperationImportSegment(IEdmOperationImport import)
            : base(import.Name)
        {
            OperationImport = import;
           // EdmType = entitySet.EntityType();
        }

        public IEdmOperationImport OperationImport { get; }

        public override bool IsSingle => false;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get; }
    }
}
