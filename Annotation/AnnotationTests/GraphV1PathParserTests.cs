// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData.EdmUtils;
using Microsoft.OData.EdmUtils.Segments;
using Xunit;

namespace AnnotationGenerator.Tests
{
    public class GraphV1PathParserTests
    {
        private IEdmModel _edmModel;

        public GraphV1PathParserTests()
        {
            _edmModel = EdmModelHelper.LoadEdmModel("graphV1.0.xml");
            Assert.NotNull(_edmModel);
        }

        [Fact]
        public void ParseEntitySetWorks()
        {
            string requestUri = "users";
            var path = PathParser.ParsePath(requestUri, _edmModel);

            Assert.NotNull(path);
            var pathSegment = Assert.Single<PathSegment>(path.Segments);
            var entitySetSegment = Assert.IsType<EntitySetSegment>(pathSegment);

            //         Assert.Same(entitySetSegment.EntitySet, _users);
        }

        [Theory]
        [InlineData("/users/{user-id}", "/users/{Id|userPrincipalName}")]
        public void ParseAndCompareTwoRequestUriSame(string requestUri1, string requestUri2)
        {
            var path1 = PathParser.ParsePath(requestUri1, _edmModel);
            var path2 = PathParser.ParsePath(requestUri2, _edmModel);

            Assert.NotSame(path1, path2);
            Assert.False(path1.EqualsTo(null));
            Assert.False(path2.EqualsTo(null));

            Assert.True(path1.EqualsTo(path1));
            Assert.True(path2.EqualsTo(path2));
            Assert.True(path1.EqualsTo(path2));

            Assert.Equal("~/users/{id}", path1.ToString());
            Assert.Equal("~/users/{id}", path2.ToString());
        }

        [Fact]
        public void StartsWithTwoRequestUriSame()
        {
            var path1 = PathParser.ParsePath("/users/{user-id}", _edmModel);
            var path2 = PathParser.ParsePath("/users/{user-id}/photo", _edmModel);

            Assert.False(path1.StartsWith(path2));
            Assert.True(path2.StartsWith(path1));
            Assert.True(path1.StartsWith(path1));

            Assert.Equal("~/users/{id}", path1.ToString());
            Assert.Equal("~/users/{id}/photo", path2.ToString());
        }

        [Theory]
        [InlineData("/me/contacts", 2)]
        [InlineData("/users/{id | userPrincipalName}/contacts", 3)]
        [InlineData("/users/{id|userPrincipalName}/mailboxSettings/timeZone", 4)]
        [InlineData("/users/{id|userPrincipalName}/outlook/supportedLanguages", 4)]
        [InlineData("/me/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'{timezone_format}')", 3)]
        [InlineData("/users/{id|userPrincipalName}/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'{timezone_format}')", 4)]
        [InlineData("/drives/{drive-id}/items/{item-id}/permissions/{perm-id}", 6)]
        public void ParseGraphV1RequestUriWorks(string requestUri, int count)
        {
            var segments = PathParser.ParsePath(requestUri, _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(count, segments.Count);
        }

        [Fact]
        public void ParseGraphV1RequestUriWithKeysWorks()
        {
            var segments = PathParser.ParsePath("/drives/{drive-id}/items/{item-id}/permissions/{perm-id}", _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(6, segments.Count);
        }

        [Fact]
        public void ParseGraphV1RequestUriWithKeysWorks2()
        {
            var segments = PathParser.ParsePath("/me/calendar/{id}/events/{id}/attachments/{id}", _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(6, segments.Count);
        }

        [Fact]
        public void ParseGraphV1RequestUriWithKeysWorks3()
        {
            var segments = PathParser.ParsePath("/me/calendar", _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(2, segments.Count);
            Assert.Equal(PathKind.SingleNavigation, segments.Kind);
        }

        [Fact]
        public void ParseGraphV1RequestUriWithKeysWorks4()
        {
            var segments = PathParser.ParsePath("/me/drive/root/workbook/worksheets/{id}/range/resizedRange(deltaRows={n}, deltaColumns={n})", _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(8, segments.Count);
            Assert.Equal(PathKind.Operation, segments.Kind);
        }

        [Fact]
        public void ParseGraphV1RequestUriWithKeysWorks5()
        {
            var segments = PathParser.ParsePath("/users/{id | userPrincipalName}/reminderView(startDateTime=startDateTime-value,endDateTime=endDateTime-value)", _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(3, segments.Count);
            Assert.Equal(PathKind.Operation, segments.Kind);
        }

        [Fact]
        public void ParseGraphV1RequestUriWithKeysWorks6()
        {
            var segments = PathParser.ParsePath("me/drive/root/workbook/worksheets/{id}/range(address={address})/visibleView/rows", _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(9, segments.Count);
            Assert.Equal(PathKind.CollectionNavigation, segments.Kind);
        }
    }
}
