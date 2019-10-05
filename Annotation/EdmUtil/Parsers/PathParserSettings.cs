// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

namespace Annotation.EdmUtil
{
    /// <summary>
    /// The setting for Uri path parser
    /// </summary>
    public class PathParserSettings
    {
        internal static PathParserSettings Default = new PathParserSettings();

        /// <summary>
        /// Gets/sets a boolean to indicate the parser supports the case insensitive.
        /// </summary>
        public bool EnableCaseInsensitive { get; set; } = false;
    }
}
