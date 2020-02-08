// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmUtil
{
    /// <summary>
    /// Various scope types for Open API writer.
    /// </summary>
    public enum ScopeType
    {
        /// <summary>
        /// Object scope.
        /// </summary>
        Object = 0,

        /// <summary>
        /// Array scope.
        /// </summary>
        Array = 1,
    }

    /// <summary>
    /// Class representing scope information.
    /// </summary>
    public sealed class Scope
    {
        /// <summary>
        /// The type of the scope.
        /// </summary>
        private readonly ScopeType _type;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">The type of the scope.</param>
        public Scope(ScopeType type)
        {
            this._type = type;
        }

        public int ObjectCount { get; set; }

        public ScopeType Type
        {
            get
            {
                return _type;
            }
        }

        public bool IsInArray { get; set; } = false;
    }
}
