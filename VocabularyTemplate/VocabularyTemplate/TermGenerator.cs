using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace VocabularyTemplate
{
    public abstract class TermGenerator : IDisposable
    {
        protected Stream stream;

        public TermGenerator()
        {
            stream = new MemoryStream();
        }

        public void Dispose()
        {
            Dispose(true);
            stream.Dispose();
            stream = null;
        }

        public virtual void Dispose(bool disposing)
        {

        }

        public string Run(string termName, IEdmModel model)
        {
            IEdmTerm term = model.FindTerm(termName);
            if (term == null)
            {
                Console.WriteLine($"Cannot find the term {termName}");
                return null;
            }

            WriteTermStart(termName);

            IEdmType termType = term.Type.Definition;
            ProcessType(termType, false);

            WriteTermEnd();

            stream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);

            // Removing xml header to make the baseline's more compact and focused on the test at hand.
            return reader.ReadToEnd();
        }

        public string Run(IEdmTerm term, IEdmModel model)
        {
            if (term == null)
            {
                Console.WriteLine($"Cannot find the term {term.FullName()}");
                return null;
            }

            WriteTermStart(term.FullName());

            IEdmType termType = term.Type.Definition;
            ProcessType(termType, false);

            WriteTermEnd();

            stream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);

            // Removing xml header to make the baseline's more compact and focused on the test at hand.
            return reader.ReadToEnd();
        }

        public void ProcessType(IEdmType edmType, bool inCollection)
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

                case EdmTypeKind.EntityReference:
                    // IEdmEntityTypeReference
                default:

                    throw new Exception($"Not supported Edm type kind as {edmType.TypeKind}");
            }
        }

        public void ProcessType(IEdmCollectionType collectionType)
        {
            WriteCollectionStart();

            IEdmType elementType = collectionType.ElementType.Definition;
            ProcessType(elementType, true);

            // Get all derived type

            WriteCollectionEnd();
        }

        public void ProcessType(IEdmTypeDefinition typeDefinition, bool inCollection)
        {
            ProcessType(typeDefinition.UnderlyingType, inCollection);
        }

        public void ProcessType(IEdmPathType pathType, bool inCollection)
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

        public void ProcessType(IEdmStructuredType structuredType)
        {
            WriteStructuredStart();

            foreach(var property in structuredType.Properties())
            {
                WritePropertyStart(property);

                ProcessType(property.Type.Definition, false);

                WritePropertyEnd(property);
            }

            WriteStructuredEnd();
        }

        public void ProcessType(IEdmEnumType enumType, bool inCollection)
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

        public void ProcessType(IEdmPrimitiveType primitiveType, bool inCollection)
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

        public abstract void WriteTermStart(string termName);
        public abstract void WriteTermEnd();
        public abstract void WriteCollectionStart();
        public abstract void WriteCollectionEnd();
        public abstract void WriteStructuredStart();
        public abstract void WriteStructuredEnd();

        public abstract void WritePropertyStart(IEdmProperty property);

        public abstract void WritePropertyEnd(IEdmProperty property);

        protected abstract void WriteStringValueTemplate(string kind, string primitiveTemplate, bool inCollection);
    }
}
