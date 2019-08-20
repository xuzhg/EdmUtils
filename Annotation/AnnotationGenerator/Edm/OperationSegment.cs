
using Microsoft.OData.Edm;

namespace AnnotationGenerator.Edm
{
    public class OperationSegment : PathSegment
    {
        public OperationSegment(IEdmOperation operation)
            : base(operation.Name)
        {
            Operation = operation;
           // EdmType = entitySet.EntityType();
        }

        public IEdmOperation Operation { get; }

        public override bool IsSingle => false;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get; }
    }
}
