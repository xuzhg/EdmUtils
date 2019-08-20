
using AnnotationGenerator.Edm;
using Microsoft.OData.Edm;
//using Xunit;

namespace AnnotationGenerator.Tests
{
    public class UriParserTests
    {
        private EdmModel _edmModel;
        private IEdmEntitySet _users;

        public UriParserTests()
        {
            EdmModel model = new EdmModel();
            var entityType = new EdmEntityType("NS", "User");

            model.AddElement(entityType);

            var container = new EdmEntityContainer("NS", "Container");
            _users = container.AddEntitySet("Users", entityType);
            model.AddElement(container);

            _edmModel = model;
        }
        /*
        //[Fact]
        public void ParseEntitySetWorks()
        {
            string requestUri = "users";
            var segments = UriParser.Parse(requestUri, _edmModel);

            Assert.NotNull(segments);
            var pathSegment = Assert.Single<PathSegment>(segments);
            var entitySetSegment = Assert.IsType<EntitySetSegment>(pathSegment);

            Assert.Same(entitySetSegment.EntitySet, _users);
        }*/
    }
}
