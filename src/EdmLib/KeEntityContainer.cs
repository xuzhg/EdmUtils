// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace EdmLib
{
    /// <summary>
    /// Represents the "Edm.Type".
    /// </summary>
    public class KeEntityContainer : KeNamedElement
    {
        public KeEntityContainer(string @namespace, string name)
            : base(name)
        {
            Namespace = @namespace;
        }

        /// <summary>
        /// Gets/sets the namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets/sets the declared schema.
        /// </summary>
        public KeSchema DeclaringSchema { get; set; }

        public override KeElementKind Kind => KeElementKind.Container;

        public IList<KeNavigationSource> NavigationSources { get; set; }
    }
}
