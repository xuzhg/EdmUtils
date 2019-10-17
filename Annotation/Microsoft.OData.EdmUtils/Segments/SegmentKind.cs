// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.OData.EdmUtils.Segments
{
    public enum SegmentKind
    {
        /// <summary>
        /// EntitySet segment.
        /// </summary>
        EntitySet,

        /// <summary>
        /// Singleton segment.
        /// </summary>
        Singleton,

        /// <summary>
        /// Property segment.
        /// </summary>
        Property,

        /// <summary>
        /// Navigation proprety segment.
        /// </summary>
        Navigation,

        /// <summary>
        /// Operation segment.
        /// </summary>
        Operation,

        /// <summary>
        /// Operation Import segment.
        /// </summary>
        OpertionImport,

        /// <summary>
        /// Key segment.
        /// </summary>
        Key,

        /// <summary>
        /// Type cast segment.
        /// </summary>
        Type
    }
}
