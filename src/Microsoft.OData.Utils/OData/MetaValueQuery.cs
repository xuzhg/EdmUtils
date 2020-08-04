// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.OData.Utils.Parser
{
    public static class MetaValueQuery
    {

        public static MetaResource Select(this MetaResource metaResource, string selectPath)
        {
            return null;

        }

        public static MetaCollect Select(this MetaCollect metaCollection, string selectPath)
        {
            return null;

        }

        public static MetaResource Expand(this MetaResource metaResource, string expandPath)
        {
            return null;

        }

        public static MetaCollect Expand(this MetaCollect metaCollection, string expandPath)
        {
            return null;

        }

        public static MetaCollect OrderBy(this MetaCollect metaCollection, string orderBy)
        {
            return null;
        }

        public static MetaCollect Top(this MetaCollect metaCollection, int top)
        {
            MetaCollect newCollection = new MetaCollect();
            foreach (var item in metaCollection.Take(top))
            {
                newCollection.Add(item);
            }

            return newCollection;
        }

        public static MetaCollect Skip(this MetaCollect metaCollection, int skip)
        {
            MetaCollect newCollection = new MetaCollect();
            foreach (var item in metaCollection.Skip(skip))
            {
                newCollection.Add(item);
            }

            return newCollection;
        }
    }
}