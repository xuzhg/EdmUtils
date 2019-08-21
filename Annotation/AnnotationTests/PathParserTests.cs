
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
        private IEdmEntitySet _users, _customers;
        private IEdmSingleton _me, _customMe;

        public PathParserTests()
        {
            EdmModel model = new EdmModel();
            var entityType = new EdmEntityType("NS", "user");
            entityType.AddKeys(entityType.AddStructuralProperty("id", EdmPrimitiveTypeKind.String));
            model.AddElement(entityType);

            var customerType = new EdmEntityType("NS", "Customer");
            customerType.AddKeys(customerType.AddStructuralProperty("FirstName", EdmPrimitiveTypeKind.String));
            customerType.AddKeys(customerType.AddStructuralProperty("LastName", EdmPrimitiveTypeKind.String));
            model.AddElement(customerType);

            var vipCustomerType = new EdmEntityType("NS", "VipCustomer", customerType);
            vipCustomerType.AddStructuralProperty("VipName", EdmPrimitiveTypeKind.String);
            model.AddElement(vipCustomerType);

            var container = new EdmEntityContainer("NS", "Container");
            _users = container.AddEntitySet("users", entityType);
            _me = container.AddSingleton("me", entityType);

            _customers = container.AddEntitySet("Customers", customerType);
            _customMe = container.AddSingleton("customMe", customerType);
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

        #region TryBindKeySegment
        [Theory]
        [InlineData("({id})")]
        [InlineData("(id={id})")]
        public void TryBindKeySegmentForSingleKeyValueWorks(string keyString)
        {
            IList<PathSegment> path = new List<PathSegment>
            {
                new EntitySetSegment(_users)
            };

            bool result = PathParser.TryBindKeySegment(keyString, path);
            Assert.True(result);
            Assert.Equal(2, path.Count);
            var keySegment = Assert.IsType<KeySegment>(path[1]);
            Assert.Equal(keyString, "(" + keySegment.Identifier + ")");
            var value = Assert.Single(keySegment.Values);
            Assert.Equal("{id}", value.Value);
        }

        [Fact]
        public void TryBindKeySegmentForCompositeKeyValueWorks()
        {
            IList<PathSegment> path = new List<PathSegment>
            {
                new EntitySetSegment(_customers)
            };

            bool result = PathParser.TryBindKeySegment("(FirstName={a},LastName={b})", path);
            Assert.True(result);
            Assert.Equal(2, path.Count);
            var keySegment = Assert.IsType<KeySegment>(path[1]);
            Assert.Equal("FirstName={a},LastName={b}", keySegment.Identifier);
            Assert.Equal(new[] { "FirstName", "LastName" }, keySegment.Values.Keys);
            Assert.Equal(new[] { "{a}", "{b}" }, keySegment.Values.Values);

        }
        #endregion

        #region TryBindTypeCastSegment
        [Theory]
        [InlineData("NS.VipCustomer", true)]
        [InlineData("nS.VipcusTomer", true)]
        [InlineData("NS.VipCustomer", false)]
        public void TryBindTypeCastSegmentForTypeCastAfterEntitySetWorks(string identifier, bool caseInsensitive)
        {
            IList<PathSegment> path = new List<PathSegment>
            {
                new EntitySetSegment(_customers)
            };

            bool result = PathParser.TryBindTypeCastSegment(identifier, null, _edmModel, path, caseInsensitive);
            Assert.True(result);
            Assert.Equal(2, path.Count);
            var typeSegment = Assert.IsType<TypeSegment>(path[1]);
            Assert.Equal("Collection(NS.VipCustomer)", typeSegment.Identifier);
            Assert.False(typeSegment.IsSingle);
        }

        [Fact]
        public void TryBindTypeCastSegmentForTypeCastAfterEntitySetWithKeyWorks()
        {
            IList<PathSegment> path = new List<PathSegment>
            {
                new EntitySetSegment(_customers)
            };

            bool result = PathParser.TryBindTypeCastSegment("NS.VipCustomer", "(FirstName={a},LastName={b})", _edmModel, path, false);
            Assert.True(result);
            Assert.Equal(3, path.Count);
            var typeSegment = Assert.IsType<TypeSegment>(path[1]);
            Assert.Equal("Collection(NS.VipCustomer)", typeSegment.Identifier);
            Assert.False(typeSegment.IsSingle);

            var keySegment = Assert.IsType<KeySegment>(path[2]);
            Assert.Equal("FirstName={a},LastName={b}", keySegment.Identifier);
            Assert.Equal(new[] { "FirstName", "LastName" }, keySegment.Values.Keys);
            Assert.Equal(new[] { "{a}", "{b}" }, keySegment.Values.Values);
        }

        [Theory]
        [InlineData("NS.VipCustomer", true)]
        [InlineData("nS.VipcusTomer", true)]
        [InlineData("NS.VipCustomer", false)]
        public void TryBindTypeCastSegmentForSingletonWorks(string identifier, bool caseInsensitive)
        {
            IList<PathSegment> path = new List<PathSegment>
            {
                new SingletonSegment(_customMe)
            };

            bool result = PathParser.TryBindTypeCastSegment(identifier, null, _edmModel, path, caseInsensitive);
            Assert.True(result);
            Assert.Equal(2, path.Count);
            var typeSegment = Assert.IsType<TypeSegment>(path[1]);
            Assert.Equal("NS.VipCustomer", typeSegment.Identifier);
            Assert.True(typeSegment.IsSingle);
        }
        #endregion
    }
}
