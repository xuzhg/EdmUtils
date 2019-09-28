
using System;
using System.Collections.Generic;
using Annotation.EdmUtil.Commons;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    /// <summary>
    /// A key segment, for example ~/users({id})
    /// or ~/users/{id}
    /// </summary>
    public class KeySegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="KeySegment"/> class.
        /// </summary>
        /// <param name="keys">The key/value pairs for the key segment.</param>
        /// <param name="entityType">The declaring entity type.</param>
        /// <param name="navigationSource">The related navigation source.</param>
        public KeySegment(IDictionary<string, string> keys,
            IEdmEntityType entityType,
            IEdmNavigationSource navigationSource)
            : base(keys.ToKeyValueString())
        {
            Values = keys;
            EdmType = entityType;
            NavigationSource = navigationSource;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="KeySegment"/> class.
        /// </summary>
        /// <param name="keys">The key/value pairs for the key segment.</param>
        /// <param name="edmEntityType">The declaring entity type.</param>
        /// <param name="navigationSource">The related navigation source.</param>
        public KeySegment(string keys, IEdmEntityType entityType, IEdmNavigationSource navigationSource)
            : base(keys)
        {
            keys.ExtractKeyValuePairs(out IDictionary<string, string> values, out _);
            Values = values;
            EdmType = entityType;
            NavigationSource = navigationSource;
        }

        public IDictionary<string, string> Values { get; }

        /// <summary>
        /// KeySegment is always single value. So IsSingle is always is true.
        /// </summary>
        public override bool IsSingle => true;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get;}

        public override string Target => throw new NotImplementedException();
    }
}
