// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Utils.Meta;
using Xunit;

namespace Microsoft.OData.Utils.Tests.Meta
{
    public class MetadataTests
    {
        [Fact]
        public void CanGetMetadataUsingProductAndCategoryModel()
        {
            IEdmModel model = EdmModelHelper.GetProductCategoryModel();

            IMetadata metadata = model.GetMetadata();

            Assert.NotNull(metadata);
        }
    }
}
