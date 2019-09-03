// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

namespace AnnotationGenerator.MD
{
    /// <summary>
    /// {
    ///     "Name": "AccessReview.Read.All",
    ///     "Description": "Allows the app to read access reviews on behalf of the signed-in user.",
    ///     "Grant": "admin"
    /// }
    /// </summary>
    public class ApiPermissionsBySchemeType
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Grant { get; set; }
    }
}
