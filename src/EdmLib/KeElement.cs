// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    public enum KeElementKind
    {
        Model,
        Primitive,
        Schema,
        Enum,
        Complex,
        Entity,
        EntitySet,
        Singleton,
        Property,
        Parameter,
        Return,
        Container,
        Function,
        Action,
        FunctionImport,
        ActionImport,
        Term
    }

    /// <summary>
    /// Top level for the Edm element.
    /// </summary>
    public abstract class KeElement
    {
        public abstract KeElementKind Kind { get;}
    }
}
