// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Common base class for all named EDM elements.
    /// </summary>
    public abstract class KeNamedElement : KeElement
    {
        public KeNamedElement(string name)
        {
            Name = name;
        }

        public string Name { get;}
    }
}
