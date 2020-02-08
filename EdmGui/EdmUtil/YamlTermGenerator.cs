// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.IO;

namespace EdmUtil
{
    class YamlTermGenerator : JsonYamlTermGeneratorBase
    {
        protected YamlWriter yamlWriter;

        public YamlTermGenerator(Stream stream)
        {
            yamlWriter = new YamlWriter(new StreamWriter(stream));
            writer = yamlWriter;
        }
    }
}
