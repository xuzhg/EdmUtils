// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace EdmLib
{
    public class KeModel : KeElement
    {
        public override KeElementKind Kind { get; } = KeElementKind.Model;

        public IList<KeSchema> Schemas { get; set; } = new List<KeSchema>();

        public IList<KeModel> ReferencedModels { get; set; } = new List<KeModel>();

        public void Save(string filePath)
        {
        }

        public static KePrimitiveType GetPrimitiveType(string qualifiedName)
        {
            if (!primitiveTypeKinds.TryGetValue(qualifiedName, out KePrimitiveTypeKind kind))
            {
                return null;
            }

            return primitiveTypesByKind[kind];
        }

        public static KePrimitiveType GetPrimitiveType(KePrimitiveTypeKind kind)
        {
            if (kind == KePrimitiveTypeKind.None)
            {
                return null;
            }

            return primitiveTypesByKind[kind];
        }

        private static IDictionary<string, KePrimitiveTypeKind> primitiveTypeKinds;
        private static IDictionary<KePrimitiveTypeKind, KePrimitiveType> primitiveTypesByKind;

        static KeModel()
        {
            BuildPrimitives();
        }

        private static void BuildPrimitives()
        {
            primitiveTypeKinds = new Dictionary<string, KePrimitiveTypeKind>();
            primitiveTypesByKind = new Dictionary<KePrimitiveTypeKind, KePrimitiveType>();

            IList<KePrimitiveType> primitiveTypes = new List<KePrimitiveType>
            {
                new KePrimitiveType(KePrimitiveTypeKind.Double),
                new KePrimitiveType(KePrimitiveTypeKind.Single),
                new KePrimitiveType(KePrimitiveTypeKind.Int64),
                new KePrimitiveType(KePrimitiveTypeKind.Int32),
                new KePrimitiveType(KePrimitiveTypeKind.Int16),
                new KePrimitiveType(KePrimitiveTypeKind.SByte),
                new KePrimitiveType(KePrimitiveTypeKind.Byte),
                new KePrimitiveType(KePrimitiveTypeKind.Boolean),
                new KePrimitiveType(KePrimitiveTypeKind.Guid),
                new KePrimitiveType(KePrimitiveTypeKind.Duration),
                new KePrimitiveType(KePrimitiveTypeKind.TimeOfDay),
                new KePrimitiveType(KePrimitiveTypeKind.DateTimeOffset),
                new KePrimitiveType(KePrimitiveTypeKind.Date),
                new KePrimitiveType(KePrimitiveTypeKind.Decimal),
                new KePrimitiveType(KePrimitiveTypeKind.Binary),
                new KePrimitiveType(KePrimitiveTypeKind.String),
                new KePrimitiveType(KePrimitiveTypeKind.Stream),

                new KePrimitiveType(KePrimitiveTypeKind.Geography),
                new KePrimitiveType(KePrimitiveTypeKind.GeographyPoint),
                new KePrimitiveType(KePrimitiveTypeKind.GeographyLineString),
                new KePrimitiveType(KePrimitiveTypeKind.GeographyPolygon),
                new KePrimitiveType(KePrimitiveTypeKind.GeographyCollection),
                new KePrimitiveType(KePrimitiveTypeKind.GeographyMultiPolygon),
                new KePrimitiveType(KePrimitiveTypeKind.GeographyMultiLineString),
                new KePrimitiveType(KePrimitiveTypeKind.GeographyMultiPoint),
                new KePrimitiveType(KePrimitiveTypeKind.Geometry),
                new KePrimitiveType(KePrimitiveTypeKind.GeometryPoint),
                new KePrimitiveType(KePrimitiveTypeKind.GeometryLineString),
                new KePrimitiveType(KePrimitiveTypeKind.GeometryPolygon),
                new KePrimitiveType(KePrimitiveTypeKind.GeometryCollection),
                new KePrimitiveType(KePrimitiveTypeKind.GeometryMultiPolygon),
                new KePrimitiveType(KePrimitiveTypeKind.GeometryMultiLineString),
                new KePrimitiveType(KePrimitiveTypeKind.GeometryMultiPoint),
                new KePrimitiveType(KePrimitiveTypeKind.PrimitiveType),
            };

            foreach (var primitive in primitiveTypes)
            {
                primitiveTypeKinds[primitive.Name] = primitive.PrimitiveKind;
                primitiveTypeKinds[primitive.Namespace + '.' + primitive.Name] = primitive.PrimitiveKind;
                primitiveTypesByKind[primitive.PrimitiveKind] = primitive;
            }
        }
    }

    public static class KeModelHelper
    {
        private static KeModel _model;

        public static KeModel GetSampleModel()
        {
            if (_model != null)
            {
                return _model;
            }

            _model = new KeModel();

            _model.Schemas.Add(new KeSchema { Namespace = "Microsoft.Graph", DelaringModel = _model });
            _model.Schemas.Add(new KeSchema { Namespace = "Test.Namespace", DelaringModel = _model });

            return _model;
        }
    }
}
