
using Annotation.EdmUtil;
using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace AnnotationGenerator.Tests
{
    public class UriParserTests
    {
        private EdmModel _edmModel;
        private IEdmEntitySet _users;
        private IEdmSingleton _me;

        public UriParserTests()
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
            var segments = UriParser.Parse(requestUri, _edmModel);

            Assert.NotNull(segments);
            var pathSegment = Assert.Single<PathSegment>(segments);
            var entitySetSegment = Assert.IsType<EntitySetSegment>(pathSegment);

            Assert.Same(entitySetSegment.EntitySet, _users);
        }

        [Fact]
        public void CreateFirstSegmentForEntitySetWorks()
        {
            IList<PathSegment> path = new List<PathSegment>();

            UriParser.CreateFirstSegment("users", _edmModel, path);

            Assert.NotEmpty(path);
            var pathSegment = Assert.Single(path);

            var entitySetSegment = Assert.IsType<EntitySetSegment>(pathSegment);

            Assert.Same(entitySetSegment.EntitySet, _users);
        }

        [Fact]
        public void CreateFirstSegmentForEntitySetWithKeyWorks()
        {
            IList<PathSegment> path = new List<PathSegment>();

            UriParser.CreateFirstSegment("users({id | userPrincipalName})", _edmModel, path);

            Assert.NotEmpty(path);
            Assert.Equal(2, path.Count);

            var entitySetSegment = Assert.IsType<EntitySetSegment>(path[0]);

            Assert.Same(entitySetSegment.EntitySet, _users);

            var keySegment = Assert.IsType<KeySegment>(path[1]);
        }

        [Fact]
        public void CreateFirstSegmentForSingletonWorks()
        {
            IList<PathSegment> path = new List<PathSegment>();

            UriParser.CreateFirstSegment("me", _edmModel, path);

            Assert.NotEmpty(path);
            var pathSegment = Assert.Single(path);

            var singletonSegment = Assert.IsType<SingletonSegment>(pathSegment);

            Assert.Same(singletonSegment.Singleton, _me);
        }

        #region CreateNextSegment

        [Fact]
        public void CreateNextSegmentForEntitySetWithKeyWorks()
        {
            IList<PathSegment> path = new List<PathSegment>();

            UriParser.CreateFirstSegment("users", _edmModel, path);

            Assert.NotEmpty(path);
            var pathSegment = Assert.Single(path);

            var entitySetSegment = Assert.IsType<EntitySetSegment>(pathSegment);

            Assert.Same(entitySetSegment.EntitySet, _users);

            UriParser.CreateNextSegment("{id | userPrincipalName}", _edmModel, path);

            Assert.Equal(2, path.Count);

            var keySegment = Assert.IsType<KeySegment>(path[1]);
        }

        #endregion
    }
}
