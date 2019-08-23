// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    /// <summary>
    /// A singleton segment, for example: ~/me
    /// </summary>
    public class SingletonSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SingletonSegment"/> class.
        /// </summary>
        /// <param name="singleton">The wrapped singleton.</param>
        public SingletonSegment(IEdmSingleton singleton)
            : base(singleton?.Name)
        {
            Singleton = singleton ?? throw new ArgumentNullException(nameof(singleton));
            EdmType = singleton.EntityType();
            Target = singleton.Container.Namespace + "/" + singleton.Name;
        }

        public IEdmSingleton Singleton { get; }

        public override bool IsSingle => true;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource => Singleton;

        public override string Target { get; }
    }
}
