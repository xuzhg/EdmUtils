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

    public interface IMetadata
    {
        IMetaValue Query(string queryPattern);
    }

    public class Metadata : IMetadata
    {
        private IEdmModel _model;
        private ISet<string> _schemaNamespaces;
        private MetaEntityContainer _entityContainer;
        private IDictionary<string, MetaReference> _references;
        private IDictionary<string, MetaSchema> _schemata;
        private IDictionary<string, MetaType> _types;
        private IDictionary<string, MetaProperty> _properties;
        private IDictionary<string, MetaNavigationProperty> _navProperties;
        private IDictionary<string, MetaTerm> _terms;
        private IDictionary<string, MetaEntitySet> _entitySet;
        private IDictionary<string, MetaSingleton> _singletons;
        private IDictionary<string, MetaActionImport> _actionImports;
        private IDictionary<string, MetaFunctionImport> _functionImports;

        public Metadata(IEdmModel model)
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
            _terms = new Dictionary<string, MetaTerm>();
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

        private void VisitProperties(MetaStructuredType structuredType, IEnumerable<IEdmProperty> properties)
        {
            foreach (var property in properties)
            {
                MetaProperty metaProperty = new MetaProperty();
                metaProperty.Fullname = structuredType.QualifiedName;
                metaProperty.Name = property.Name;
                metaProperty.IsCollection = property.Type.IsCollection();
                metaProperty.Nullable = property.Type.IsNullable;

                metaProperty.Type = GetOrBuildType(property.Type);

                _properties[metaProperty.Fullname] = metaProperty;
            }
        }

        private void VisitNavProperties(MetaStructuredType structuredType, IEnumerable<IEdmNavigationProperty> navProperties)
        {
            foreach (var property in navProperties)
            {
                MetaNavigationProperty metaProperty = new MetaNavigationProperty();
                metaProperty.Fullname = structuredType.QualifiedName;
                metaProperty.Name = property.Name;
                metaProperty.IsCollection = property.Type.IsCollection();
                metaProperty.Nullable = property.Type.IsNullable;

                metaProperty.Type = GetOrBuildType(property.Type);

                _navProperties[metaProperty.Fullname] = metaProperty;
            }
        }


        private MetaType GetOrBuildType(IEdmTypeReference edmTypeReference)
        {
            switch (edmTypeReference.TypeKind())
            {
                case EdmTypeKind.Primitive:
                    VisitPrimitiveTypeReference(edmTypeReference.AsPrimitive());
                    break;
                case EdmTypeKind.Entity:
                    VisitEntityType(edmTypeReference.AsEntity().EntityDefinition());
                    break;
                case EdmTypeKind.Complex:
                    VisitComplexType(edmTypeReference.AsComplex().ComplexDefinition());
                    break;

                case EdmTypeKind.Collection:
                    IEdmCollectionTypeReference collectionTypeRef = edmTypeReference.AsCollection();
                    return GetOrBuildType(collectionTypeRef.ElementType());

                case EdmTypeKind.Enum:
                    VisitEnumType(edmTypeReference.AsEnum().EnumDefinition());
                    break;
                case EdmTypeKind.TypeDefinition:
                    VisitTypeDefinition(edmTypeReference.AsTypeDefinition().TypeDefinition());
                    break;
                case EdmTypeKind.Path:
                    VisitPathType(edmTypeReference.AsPath());
                    break;

                case EdmTypeKind.EntityReference:
                case EdmTypeKind.None:
                case EdmTypeKind.Untyped:
                default:
                    throw new InvalidOperationException($"Found an unknow type kind '{edmTypeReference.TypeKind()}'");
            }

            return _types[edmTypeReference.FullName()];
        }

        private void VisitPathType(IEdmPathTypeReference pathReference)
        {
            string qualifiedName = pathReference.FullName();

            if (_types.ContainsKey(qualifiedName))
            {
                return;
            }

            IEdmPathType primitiveType = (IEdmPathType)(pathReference.Definition);

            MetaPrimitiveType metaPrimitiveType = new MetaPrimitiveType();
            metaPrimitiveType.QualifiedName = qualifiedName;
            metaPrimitiveType.Name = primitiveType.Name;
            _types[qualifiedName] = metaPrimitiveType;
        }

        private void VisitPrimitiveTypeReference(IEdmPrimitiveTypeReference reference)
        {
            string qualifiedName = reference.FullName();
            if (_types.ContainsKey(qualifiedName))
            {
                return;
            }

            IEdmPrimitiveType primitiveType = reference.PrimitiveDefinition();

            MetaPrimitiveType metaPrimitiveType = new MetaPrimitiveType();
            metaPrimitiveType.QualifiedName = qualifiedName;
            metaPrimitiveType.Name = primitiveType.Name;
            _types[qualifiedName] = metaPrimitiveType;
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

        private void VisitEnumType(IEdmEnumType enumType)
        {
            string qualifiedName = enumType.FullTypeName();
            if (_types.ContainsKey(qualifiedName))
            {
                return; // processed
            }

            MetaEnumType metaEnum = new MetaEnumType();
            metaEnum.QualifiedName = qualifiedName;
            metaEnum.Name = enumType.Name;
            metaEnum.IsFlag = enumType.IsFlags;
            _types[qualifiedName] = metaEnum;

        }

        private void VisitTypeDefinition(IEdmTypeDefinition typeDefinition)
        {
            string qualifiedName = typeDefinition.FullTypeName();
            if (_types.ContainsKey(qualifiedName))
            {
                return; // processed
            }

            MetaTypeDefinitionType metaTypeDef = new MetaTypeDefinitionType();
            metaTypeDef.QualifiedName = qualifiedName;
            metaTypeDef.Name = typeDefinition.Name;
            _types[qualifiedName] = metaTypeDef;
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


}