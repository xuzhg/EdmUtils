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
            : this(singleton, singleton?.Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SingletonSegment"/> class.
        /// </summary>
        /// <param name="singleton">The wrapped singleton.</param>
        /// <param name="literal">The literal string in the request uri.</param>
        public SingletonSegment(IEdmSingleton singleton, string literal)
            : base(literal)
        {
            Singleton = singleton ?? throw new ArgumentNullException(nameof(singleton));
            EdmType = singleton.EntityType();
            Target = singleton.Container.Namespace + "/" + singleton.Name;
        }

        public IEdmSingleton Singleton { get; }

        /// <summary>
        /// Singleton is always single value. So IsSingle is always is true.
        /// </summary>
        public override bool IsSingle => true;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource => Singleton;

        public override string Target { get; }
    }
}
