// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace EdmLib
{
    /// <summary>
    /// Represents the operation.
    /// </summary>
    public abstract class KeOperation : KeNamedElement
    {
        private KeReturnType _returnType;

        public KeOperation(KeSchema schema, string name, bool isBound, KePathExpression entitySetPathExpression)
            : base(name)
        {
            DeclaringSchema = schema;
            IsBound = isBound;
            EntitySetPath = entitySetPathExpression;
        }

        public bool IsBound { get; private set; }

        public KePathExpression EntitySetPath { get; private set; }
        public KeSchema DeclaringSchema { get; }

        public IList<KeParameter> Parameters { get; }

        public void SetReturnType(KeTypeReference typeReference)
        {
            if (typeReference == null)
            {
                _returnType = null;
            }
            else
            {
                _returnType = new KeReturnType(typeReference);
            }
        }

        public KeReturnType GetReturnType()
        {
            return _returnType;
        }

        public KeParameter AddParameter(string name, KeTypeReference type)
        {
            KeParameter parameter = new KeParameter(this, name, type);
            this.Parameters.Add(parameter);
            return parameter;
        }
    }
}
