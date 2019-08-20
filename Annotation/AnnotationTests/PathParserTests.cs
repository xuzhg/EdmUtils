
using System;
using System.Collections.Generic;
using Annotation.EdmUtil;
using Microsoft.OData.Edm;
using Xunit;

namespace AnnotationGenerator.Tests
{
    public class PathParserTests
    {
        private EdmModel _edmModel;
        private IEdmEntitySet _users;
        private IEdmSingleton _me;

        public PathParserTests()
        {
            EdmModel model = new EdmModel();
            var entityType = new EdmEntityType("NS", "user");
            entityType.AddKeys(entityType.AddStructuralProperty("id", EdmPrimitiveTypeKind.String));
            model.AddElement(entityType);

            var container = new EdmEntityContainer("NS", "Container");
            _users = container.AddEntitySet("users", entityType);
            _me = container.AddSingleton("me", entityType);
            model.AddElement(container);

            _edmModel = model;
        }

        [Fact]
        public void ParseEntitySetWorks()
        {
            string requestUri = "users";
            var segments = PathParser.Parse(requestUri, _edmModel);

            Assert.NotNull(segments);
            var pathSegment = Assert.Single<PathSegment>(segments);
            var entitySetSegment = Assert.IsType<EntitySetSegment>(pathSegment);

            Assert.Same(entitySetSegment.EntitySet, _users);
        }

        #region CreateFirstSegment
        [Theory]
        [InlineData("users", true)]
        [InlineData("UsErs", true)]
        [InlineData("users", false)]
        public void CreateFirstSegmentForEntitySetWorks(string identifier, bool enableCaseInsensitive)
        {
            IList<PathSegment> path = new List<PathSegment>();
            PathParser.CreateFirstSegment(identifier, _edmModel, path, enableCaseInsensitive);

            Assert.NotEmpty(path);
            var pathSegment = Assert.Single(path);
            var entitySetSegment = Assert.IsType<EntitySetSegment>(pathSegment);
            Assert.Same(entitySetSegment.EntitySet, _users);
        }

        [Fact]
        public void CreateFirstSegmentForEntitySetDisableCaseInsensitiveThrows()
        {
            IList<PathSegment> path = new List<PathSegment>();
            Action test = () => PathParser.CreateFirstSegment("UsErs", _edmModel, path, enableCaseInsensitive: false);
            var exception = Assert.Throws<Exception>(test);
            Assert.Equal($"Unknow kind of first segment: 'UsErs'", exception.Message);
        }

        [Fact]
        public void CreateFirstSegmentForEntitySetWithKeyWorks()
        {
            IList<PathSegment> path = new List<PathSegment>();
            PathParser.CreateFirstSegment("users({id | userPrincipalName})", _edmModel, path);
            Assert.NotEmpty(path);
            Assert.Equal(2, path.Count);
            var entitySetSegment = Assert.IsType<EntitySetSegment>(path[0]);
            Assert.Same(entitySetSegment.EntitySet, _users);
            var keySegment = Assert.IsType<KeySegment>(path[1]);
            Assert.Equal("{id | userPrincipalName}", keySegment.Identifier);
            var value = Assert.Single(keySegment.Values);
            Assert.Equal(String.Empty, value.Key);
            Assert.Equal("{id | userPrincipalName}", value.Value);
        }

        [Theory]
        [InlineData("me", true)]
        [InlineData("mE", true)]
        [InlineData("me", false)]
        public void CreateFirstSegmentForSingletonWorks(string identifier, bool enableCaseInsensitive)
        {
            IList<PathSegment> path = new List<PathSegment>();
            PathParser.CreateFirstSegment(identifier, _edmModel, path, enableCaseInsensitive);
            Assert.NotEmpty(path);
            var pathSegment = Assert.Single(path);
            var singletonSegment = Assert.IsType<SingletonSegment>(pathSegment);
            Assert.Same(singletonSegment.Singleton, _me);
        }

        [Fact]
        public void CreateFirstSegmentForSingletonDisableCaseInsensitiveThrows()
        {
            IList<PathSegment> path = new List<PathSegment>();
            Action test = () => PathParser.CreateFirstSegment("mE", _edmModel, path, enableCaseInsensitive: false);
            var exception = Assert.Throws<Exception>(test);
            Assert.Equal($"Unknow kind of first segment: 'mE'", exception.Message);
        }

        [Fact]
        public void CreateFirstSegmentForSingletonWithParenthesisThrows()
        {
            IList<PathSegment> path = new List<PathSegment>();
            Action test = () => PathParser.CreateFirstSegment("me({id})", _edmModel, path, enableCaseInsensitive: false);
            var exception = Assert.Throws<Exception>(test);
            Assert.Equal("Unknown parentheis '({id})' after a singleton 'me'.", exception.Message);
        }

        #endregion

        #region CreateNextSegment

        [Fact]
        public void CreateNextSegmentForEntitySetWithKeyWorks()
        {
            IList<PathSegment> path = new List<PathSegment>();

            PathParser.CreateFirstSegment("users", _edmModel, path);

            Assert.NotEmpty(path);
            var pathSegment = Assert.Single(path);

            var entitySetSegment = Assert.IsType<EntitySetSegment>(pathSegment);

            Assert.Same(entitySetSegment.EntitySet, _users);

            PathParser.CreateNextSegment("{id | userPrincipalName}", _edmModel, path);

            Assert.Equal(2, path.Count);

            var keySegment = Assert.IsType<KeySegment>(path[1]);
        }

        #endregion
    }
}
