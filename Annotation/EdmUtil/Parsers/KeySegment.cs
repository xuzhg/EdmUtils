
using System.Collections.Generic;
using Annotation.EdmUtil.Commons;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    public class KeySegment : PathSegment
    {
        public KeySegment(IDictionary<string, string> keys,
            IEdmEntityType edmEntityType,
            IEdmNavigationSource navigationSource)
            : base(keys.ToKeyValueString())
        {
            Values = keys;
            EdmType = edmEntityType;
            NavigationSource = navigationSource;
        }

        public IDictionary<string, string> Values { get; }

        // Key is always single value.
        public override bool IsSingle => true;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get;}
    }
}
