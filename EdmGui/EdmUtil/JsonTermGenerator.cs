// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.IO;

namespace EdmUtil
{
    class JsonTermGenerator : JsonYamlTermGeneratorBase
    {
        protected JsonWriter jsonWriter;

        public JsonTermGenerator(Stream stream)
        {
            jsonWriter = new JsonWriter(new StreamWriter(stream));
            writer = jsonWriter;
        }
    }
}
