// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    public enum ExpressionKind
    {
        Constant,
        Path,
        Record,
        Collection,
    }

    public abstract class MetaAnnotationExpression : MetaElement
    {
        public abstract ExpressionKind Kind { get; }
    }

    public class MetaRecordExpression : MetaAnnotationExpression
    {
        public override ExpressionKind Kind => ExpressionKind.Record;

        public IDictionary<string, MetaAnnotationExpression> Properties { get; }
    }

    public class MetaCollectionExpression : MetaAnnotationExpression
    {
        public override ExpressionKind Kind => ExpressionKind.Collection;

        public ICollection<MetaAnnotationExpression> Items { get; }
    }


}