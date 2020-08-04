// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    public abstract class MetaConstantExpression : MetaAnnotationExpression
    {
        public override ExpressionKind Kind => ExpressionKind.Constant;
    }

    public class MetaStringExpression : MetaConstantExpression
    {
        public string Value { get; }
    }

    public class MetaBooleanExpression : MetaConstantExpression
    {
        public bool Value { get; }
    }

    public class MetaDateTimeOffsetExpression : MetaConstantExpression
    {
        public DateTimeOffset Value { get; }
    }
}