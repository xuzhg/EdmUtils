// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmUtil
{
    /// <summary>
    /// Constants for the writer.
    /// </summary>
    internal static class WriterConstants
    {
        /// <summary>
        /// Character which starts the object scope.
        /// </summary>
        internal const string StartObjectScope = "{";

        /// <summary>
        /// Character which ends the object scope.
        /// </summary>
        internal const string EndObjectScope = "}";

        /// <summary>
        /// Character which starts the array scope.
        /// </summary>
        internal const string StartArrayScope = "[";

        /// <summary>
        /// Character which ends the array scope.
        /// </summary>
        internal const string EndArrayScope = "]";

        /// <summary>
        /// The separator between array elements.
        /// </summary>
        internal const string ArrayElementSeparator = ",";

        /// <summary>
        /// The separator between object members.
        /// </summary>
        internal const string ObjectMemberSeparator = ",";

        /// <summary>
        /// The separator between the name and the value.
        /// </summary>
        internal const string NameValueSeparator = ": ";

        /// <summary>
        /// The white space for empty object
        /// </summary>
        internal const string WhiteSpaceForEmptyObject = " ";

        /// <summary>
        /// The white space for empty array
        /// </summary>
        internal const string WhiteSpaceForEmptyArray = " ";

        /// <summary>
        /// The prefix of array item
        /// </summary>
        internal const string PrefixOfArrayItem = "- ";

        /// <summary>
        /// Empty object
        /// </summary>
        /// <remarks>To indicate empty object in YAML.</remarks>
        internal const string EmptyObject = "{ }";

        /// <summary>
        /// Empty array
        /// </summary>
        /// <remarks>To indicate empty array in YAML.</remarks>
        internal const string EmptyArray = "[ ]";
    }
}
