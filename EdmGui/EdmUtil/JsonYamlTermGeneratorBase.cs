// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.OData.Edm;

namespace EdmUtil
{
    public abstract class JsonYamlTermGeneratorBase : EdmTermGenerator, IDisposable
    {
        protected ITermWriter writer;

        protected override void WriteTermStart(string termName)
        {
            writer.StartObjectScope();

            writer.WritePropertyName("@" + termName);
        }

        protected override void WriteTermEnd()
        {
            writer.EndObjectScope();
            writer.Flush();
        }

        protected override void WriteCollectionStart()
        {
            writer.StartArrayScope();
        }

        protected override void WriteCollectionEnd()
        {
            writer.EndArrayScope();
        }

        protected override void WriteStructuredStart()
        {
            writer.StartObjectScope();
        }

        protected override void WriteStructuredEnd()
        {
            writer.EndObjectScope();
        }

        protected override void WritePropertyStart(IEdmProperty property)
        {
            // <PropertyValue Property="" >
            writer.WritePropertyName(property.Name);
        }

        protected override void WritePropertyEnd(IEdmProperty property)
        {
        }

        protected override void WriteStringValueTemplate(string kind, string primitiveTemplate, bool inCollection)
        {
            if (inCollection)
            {
                writer.WriteValue(primitiveTemplate + "_01");
                writer.WriteValue(primitiveTemplate + "_02");
                writer.WriteValue(primitiveTemplate + "_03");
            }
            else
            {
                writer.WriteValue(primitiveTemplate);
            }
        }

        public void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
            }
        }
    }
}
