// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;

namespace EdmUtil
{
    public abstract class EdmTermGenerator
    {
        public void Create(IEdmModel model, IEdmTerm term)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            WriteTermStart(term.FullName());

            IEdmType termType = term.Type.Definition;
            ProcessType(termType, false);

            WriteTermEnd();
        }

        private void ProcessType(IEdmType edmType, bool inCollection)
        {
            switch (edmType.TypeKind)
            {
                case EdmTypeKind.Primitive:
                    ProcessType((IEdmPrimitiveType)edmType, inCollection);
                    break;

                case EdmTypeKind.Entity:
                // IEdmEntityType
                case EdmTypeKind.Complex:
                    // IEdmComplexType
                    ProcessType((IEdmStructuredType)edmType);
                    break;

                case EdmTypeKind.Collection:
                    ProcessType((IEdmCollectionType)edmType);
                    break;

                case EdmTypeKind.Enum:
                    ProcessType((IEdmEnumType)edmType, inCollection);
                    break;

                case EdmTypeKind.TypeDefinition:
                    // IEdmTypeDefinition
                    ProcessType((IEdmTypeDefinition)edmType, inCollection);
                    break;

                case EdmTypeKind.Untyped:
                    break;

                case EdmTypeKind.Path:
                    ProcessType((IEdmPathType)edmType, inCollection);
                    break;

                case EdmTypeKind.None:
                    break;

                case EdmTypeKind.EntityReference:
                // IEdmEntityTypeReference
                default:

                    throw new Exception($"Not supported Edm type kind as {edmType.TypeKind}");
            }
        }

        private void ProcessType(IEdmCollectionType collectionType)
        {
            WriteCollectionStart();

            IEdmType elementType = collectionType.ElementType.Definition;
            ProcessType(elementType, true);

            // Get all derived type

            WriteCollectionEnd();
        }

        private void ProcessType(IEdmTypeDefinition typeDefinition, bool inCollection)
        {
            ProcessType(typeDefinition.UnderlyingType, inCollection);
        }

        private void ProcessType(IEdmPathType pathType, bool inCollection)
        {
            string kind = "";
            string primitiveTemplate = null;
            switch (pathType.PathKind)
            {
                case EdmPathTypeKind.PropertyPath:
                    kind = "PropertyPath";
                    primitiveTemplate = "PropertyPathValue";
                    break;

                case EdmPathTypeKind.AnnotationPath:
                    kind = "AnnotationPath";
                    primitiveTemplate = "Supplier/@Communication.Contact";
                    break;

                case EdmPathTypeKind.NavigationPropertyPath:
                    kind = "NavigationPropertyPath";
                    primitiveTemplate = "NavigationPropertyPathValue";
                    break;
            }

            WriteStringValueTemplate(kind, primitiveTemplate, inCollection);
        }

        private void ProcessType(IEdmStructuredType structuredType)
        {
            WriteStructuredStart();

            foreach (var property in structuredType.Properties())
            {
                WritePropertyStart(property);

                ProcessType(property.Type.Definition, false);

                WritePropertyEnd(property);
            }

            WriteStructuredEnd();
        }

        private void ProcessType(IEdmEnumType enumType, bool inCollection)
        {
            string fullName;
            if (enumType.Members.Any())
            {
                fullName = $"{enumType.FullName()}/{enumType.Members.First().Name}";
            }
            else
            {
                fullName = $"{enumType.FullName()}/?";
            }

            if (enumType.IsFlags && enumType.Members.Count() >= 2)
            {
                fullName += $" {enumType.FullName()}/{enumType.Members.Last().Name}";
            }

            WriteStringValueTemplate("EnumMember", fullName, inCollection);
        }

        private void ProcessType(IEdmPrimitiveType primitiveType, bool inCollection)
        {
            string kind;
            string primitiveTemplate;
            switch (primitiveType.PrimitiveKind)
            {
                case EdmPrimitiveTypeKind.Byte:
                case EdmPrimitiveTypeKind.Int16:
                case EdmPrimitiveTypeKind.Int32:
                case EdmPrimitiveTypeKind.Int64:
                case EdmPrimitiveTypeKind.SByte:
                    kind = "Int";
                    primitiveTemplate = "intValue_as_42";
                    break;

                case EdmPrimitiveTypeKind.Guid:
                    kind = "Guid";
                    primitiveTemplate = "guidValue_as_21EC2020-3AEA-1069-A2DD-08002B30309D";
                    break;

                case EdmPrimitiveTypeKind.Single:
                case EdmPrimitiveTypeKind.Double:
                    kind = "Float";
                    primitiveTemplate = "floatValue_as_3.14";
                    break;

                case EdmPrimitiveTypeKind.Duration:
                    kind = "Duration";
                    primitiveTemplate = "durationValue_as_P7D";
                    break;

                case EdmPrimitiveTypeKind.Decimal:
                    kind = "Decimal";
                    primitiveTemplate = "decimalValue_as_3.14";
                    break;

                case EdmPrimitiveTypeKind.DateTimeOffset:
                    kind = "DateTimeOffset";
                    primitiveTemplate = "dateTimeOffsetValue_as_2000-01-01T16:00:00.000Z";
                    break;

                case EdmPrimitiveTypeKind.Date:
                    kind = "Date";
                    primitiveTemplate = "dateValue_as_2000-01-01";
                    break;

                case EdmPrimitiveTypeKind.TimeOfDay:
                    kind = "TimeOfDay";
                    primitiveTemplate = "timeOfDayValue_as_21:45:00";
                    break;

                case EdmPrimitiveTypeKind.Binary:
                    kind = "Binary";
                    primitiveTemplate = "binaryValue_as_T0RhdGE";
                    break;

                case EdmPrimitiveTypeKind.Boolean:
                    kind = "Boolean";
                    primitiveTemplate = "true_or_false";
                    break;

                case EdmPrimitiveTypeKind.String:
                    kind = "String";
                    primitiveTemplate = "stringValue_as_ProductCatalog";
                    break;

                case EdmPrimitiveTypeKind.PrimitiveType:
                    kind = "PrimitiveType";
                    primitiveTemplate = "anyPrimitiveTypeValue";
                    break;

                case EdmPrimitiveTypeKind.Stream:
                default:
                    throw new Exception($"Doesn't support the Primitve kind as {primitiveType.PrimitiveKind}");
            }

            if (primitiveTemplate != null)
            {
                WriteStringValueTemplate(kind, primitiveTemplate, inCollection);
            }
        }

        protected abstract void WriteTermStart(string termName);
        protected abstract void WriteTermEnd();
        protected abstract void WriteCollectionStart();
        protected abstract void WriteCollectionEnd();
        protected abstract void WriteStructuredStart();
        protected abstract void WriteStructuredEnd();

        protected abstract void WritePropertyStart(IEdmProperty property);

        protected abstract void WritePropertyEnd(IEdmProperty property);

        protected abstract void WriteStringValueTemplate(string kind, string primitiveTemplate, bool inCollection);
    }
}
