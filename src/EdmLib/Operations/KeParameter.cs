// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the parameter.
    /// </summary>
    public class KeParameter : KeNamedElement
    {
        public KeParameter(KeOperation declaringOperation, string name, KeTypeReference type)
            : base(name)
        {
            DeclaringOperation = declaringOperation;
            Type = type;
        }

        public override KeElementKind Kind => KeElementKind.Parameter;

        /// <summary>
        /// Gets the type of this parameter.
        /// </summary>
        public KeTypeReference Type { get;}

        /// <summary>
        /// Gets the operation that declared this parameter.
        /// </summary>
        public KeOperation DeclaringOperation { get; }
    }
}
