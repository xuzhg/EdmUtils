// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using Microsoft.OData.Utils.Json;
using Microsoft.OData.Utils.Value;
using Microsoft.OData.Utils.Vocabulary.Capabilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.OData.Utils.Meta
{
    public class MetaModel1
    {
        public string PropertyPath { get; }

        public string Alias { get; }
    }



    public interface IMetadata1
    {
        IMetaValue Query(string queryPattern);
    }

    public class Metadata1 : IMetadata1
    {
        //public static Metadata GetMetadata(IEdmModel model)
        //{
        //    if (model == null)
        //    {
        //        throw new ArgumentNullException(nameof(model));
        //    }

        //    Metadata meta = new Metadata();
        //    meta.Visit(model);
        //    return meta;
        //}

        private IEdmModel _model;
        private ISet<string> _schemaNamespaces;
        private IDictionary<string, MetaType> _types;
        private IDictionary<string, MetaProperty> _properties;
        private IDictionary<string, MetaNavigationProperty> _navProperties;

        public Metadata1(IEdmModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _model = model;
            _schemaNamespaces = new HashSet<string>();
            _types = new Dictionary<string, MetaType>();
            _properties = new Dictionary<string, MetaProperty>();
            _navProperties = new Dictionary<string, MetaNavigationProperty>();
        }

        public void Visit()
        {
            VisitSchemaElements(_model.SchemaElements);

            VisitVocabularyAnnotations(_model.VocabularyAnnotations);
        }

        private void VisitSchemaElements(IEnumerable<IEdmSchemaElement> elements)
        {
            foreach (var element in elements)
            {
                VisitSchemaElement(element);

                switch (element.SchemaElementKind)
                {
                    case EdmSchemaElementKind.Action:
                        VisitAction((IEdmAction)element);
                        break;
                    case EdmSchemaElementKind.Function:
                        VisitFunction((IEdmFunction)element);
                        break;
                    case EdmSchemaElementKind.TypeDefinition:
                        VisitSchemaType((IEdmType)element);
                        break;
                    case EdmSchemaElementKind.Term:
                        VisitTerm((IEdmTerm)element);
                        break;
                    case EdmSchemaElementKind.EntityContainer:
                        VisitEntityContainer((IEdmEntityContainer)element);
                        break;
                    case EdmSchemaElementKind.None:
                    default:
                        throw new InvalidOperationException($"Found an unknow schema element kind '{element.SchemaElementKind}'");
                }
            }
        }

        private void VisitAction(IEdmAction action)
        {

        }

        private void VisitFunction(IEdmFunction action)
        {

        }

        private void VisitSchemaType(IEdmType definition)
        {
            switch (definition.TypeKind)
            {
                case EdmTypeKind.Complex:
                    VisitComplexType((IEdmComplexType)definition);
                    break;
                case EdmTypeKind.Entity:
                    VisitEntityType((IEdmEntityType)definition);
                    break;
                case EdmTypeKind.Enum:
                    VisitEnumType((IEdmEnumType)definition);
                    break;
                case EdmTypeKind.TypeDefinition:
                    VisitTypeDefinition((IEdmTypeDefinition)definition);
                    break;
                case EdmTypeKind.None:
                default:
                    throw new InvalidOperationException($"Found an unknown schema type Kind '{definition.TypeKind}'");
            }
        }


        private void VisitComplexType(IEdmComplexType complex)
        {
            string qualifiedName = complex.FullTypeName();
            if (_types.ContainsKey(qualifiedName))
            {
                return; // processed
            }

            MetaComplexType metaComplex = new MetaComplexType();
            metaComplex.QualifiedName = qualifiedName;
            metaComplex.Name = complex.Name;
            metaComplex.Abstract = complex.IsAbstract;
            _types[qualifiedName] = metaComplex;

            VisitProperties(metaComplex, complex.DeclaredProperties);
            VisitNavProperties(metaComplex, complex.DeclaredNavigationProperties());
        }

        private void VisitEntityType(IEdmEntityType entity)
        {
            string qualifiedName = entity.FullTypeName();
            if (_types.ContainsKey(qualifiedName))
            {
                return; // processed
            }

            MetaEntityType metaEntity = new MetaEntityType();
            metaEntity.QualifiedName = qualifiedName;
            metaEntity.Name = entity.Name;
            metaEntity.Abstract = entity.IsAbstract;
            metaEntity.OpenType = entity.IsOpen;
            metaEntity.HasStream = entity.HasStream;
            _types[qualifiedName] = metaEntity;

            VisitProperties(metaEntity, entity.DeclaredProperties);
            VisitNavProperties(metaEntity, entity.DeclaredNavigationProperties());
        }

        private void VisitProperties(MetaStructuredType structuredType, IEnumerable<IEdmProperty> properties)
        {
            foreach (var property in properties)
            {
                MetaProperty metaProperty = new MetaProperty();
                metaProperty.Fullname = structuredType.QualifiedName;
                metaProperty.Name = property.Name;
                metaProperty.IsCollection = property.Type.IsCollection();
                metaProperty.Nullable = property.Type.IsNullable;
            }
        }

        private void VisitNavProperties(MetaStructuredType structuredType, IEnumerable<IEdmNavigationProperty> navProperties)
        {
            foreach (var property in navProperties)
            {

            }
        }

        private void VisitEnumType(IEdmEnumType enumType)
        {

        }

        private void VisitTypeDefinition(IEdmTypeDefinition typeDefinition)
        {

        }

        private void VisitTerm(IEdmTerm term)
        {

        }

        private void VisitEntityContainer(IEdmEntityContainer container)
        {

        }


        private void VisitSchemaElement(IEdmSchemaElement schemaElement)
        {
            string namespaceName = schemaElement.Namespace;

            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                throw new InvalidOperationException("Found an empty namespace.");
            }

            _schemaNamespaces.Add(namespaceName);
        }

        private void VisitVocabularyAnnotations(IEnumerable<IEdmVocabularyAnnotation> annotations)
        {
            foreach (var annotation in annotations)
            {
                VisitVocabularyAnnotation(annotation);
            }
        }

        private void VisitVocabularyAnnotation(IEdmVocabularyAnnotation vocabularyAnnotation)
        {
            // TODO:
        }

        private void VisitDirectAnnotations(IEdmElement element)
        {
            IEnumerable<IEdmDirectValueAnnotation> annotations = _model.DirectValueAnnotations(element);
            foreach (var annotation in annotations)
            {
                VisitDirectAnnotation(annotation);
            }
        }

        private void VisitDirectAnnotation(IEdmDirectValueAnnotation directAnnotation)
        {
            // TODO:
        }


        public IMetaValue EntitySets()
        {
            return null;
        }

        public IMetaValue Query(string queryPattern)
        {
            return null;
        }
    }


    internal interface IMetaSerializable
    {
        void Serialize(IJsonWriter jsonWriter);
    }
}