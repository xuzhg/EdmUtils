// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Xml;

namespace Microsoft.OData.EdmUtils.Terms
{
    public interface IXmlWritable
    {
        void Write(XmlWriter writer);
    }

    public class VocabularyAnnotations
    {
        public IDictionary<string, IList<VocabularyAnnotation>> Annotations { get; } = new Dictionary<string, IList<VocabularyAnnotation>>();

        public void Append(VocabularyAnnotation annotation)
        {
            if (Annotations.TryGetValue(annotation.Target, out IList<VocabularyAnnotation> values))
            {
                values.Add(annotation);
            }
            else
            {
                IList<VocabularyAnnotation> newValues = new List<VocabularyAnnotation>();
                newValues.Add(annotation);
                Annotations[annotation.Target] = newValues;
            }
        }
    }

    public class VocabularyAnnotation
    {
        public string Target { get; set; }

        public IEdmTerm Term { get; set; }

        public IEdmExpression Value;

        public object ValueObj { get; set; }

        public void Write(XmlWriter writer)
        {
            // <Annotation>
            writer.WriteStartElement("Annotation");

            writer.WriteAttributeString("Term", Term.FullName());

            WriteValue(writer, ValueObj, false);

            // </Annotation>
            writer.WriteEndElement();
        }

        private static void WriteValue(XmlWriter writer, object value, bool inCollection = false)
        {
            if (value == null)
            {
                return;
            }

            IRecord record = value as IRecord;
            if (record != null)
            {
                //        writer.WriteRecordValue(record);
                return;
            }

            IEnumerable enumerable = value as IEnumerable;
            if (enumerable != null)
            {
                if (inCollection)
                {
                    throw new Exception("A vocabulary annotation has collection in a collection. It's not valid.");
                }

                foreach (var item in enumerable)
                {
                    WriteValue(writer, item, true);
                }

                return;
            }

            // string
            string str = value as string;
            if (str != null)
            {
                if (inCollection)
                {
                    writer.WriteStartElement("String");
                    writer.WriteValue(str);
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteAttributeString("String", str);
                }

                return;
            }
        }
    }

    public interface IAnnotationValue
    {
        void Write(XmlWriter writer);
    }


    public interface IRecordExpression
    {
        string FullTypeName { get; }
    }

    public interface ITermValue
    {
        // AnnotationValueKind Kind { get; }

        EdmExpressionKind ExpressionKind { get; }

        void Write(XmlWriter writer);
    }

    public interface IEdmRecordType
    {
    }

    public abstract class TermRecordValue : ITermValue
    {
        public EdmExpressionKind ExpressionKind => EdmExpressionKind.Record;

        public void Write(XmlWriter writer)
        {
            // <Record>
            writer.WriteStartElement("Record");

            WriteObject(writer);

            // </Record>
            writer.WriteEndElement();
        }

        public abstract void WriteObject(XmlWriter writer);
    }

    public class TermCollectionValue : ITermValue
    {
        public EdmExpressionKind ExpressionKind => EdmExpressionKind.Collection;

        public IList<ITermValue> Items { get; set; }

        public void Write(XmlWriter writer)
        {
            // <Collection>
            writer.WriteStartElement("Collection");

            foreach (ITermValue item in Items)
            {
                item.Write(writer);
            }

            // </Collection>
            writer.WriteEndElement();
        }
    }

    public class TermCollectionValue<T> : IList<T>, ITermValue
        where T : ITermValue
    {
        public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public EdmExpressionKind ExpressionKind => EdmExpressionKind.Collection;

        public IList<ITermValue> Items { get; set; }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Write(XmlWriter writer)
        {
            // <Collection>
            writer.WriteStartElement("Collection");

            foreach (ITermValue item in Items)
            {
                item.Write(writer);
            }

            // </Collection>
            writer.WriteEndElement();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
