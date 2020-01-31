// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;

namespace Microsoft.OData.EdmUtils.Terms
{
    [Flags]
    public enum EdmTermApplyToKind
    {
        Action = 0, // Action

        ActionImport = 2 ^ 0, // Action Import

        Annotation = 2 ^ 1, // Annotation

        Apply = 2 ^ 2, // Application of a client-side function in an annotation

        Cast = 2 ^ 3, // Type Cast annotation expression

        Collection = 2 ^ 4, // Entity Set or collection-valued Property or Navigation Property

        ComplexType = 2 ^ 5, // Complex Type

        EntityContainer = 2 ^ 6, // Entity Container

        EntitySet = 2 ^ 7, // Entity Set

        EntityType = 2 ^ 8, // Entity Type

        EnumType = 2 ^ 9, // Enumeration Type

        Function = 2 ^ 10, // Function

        FunctionImport = 2 ^ 11, // Function Import

        If = 2 ^ 12, // Conditional annotation expression

        Include = 2 ^ 13, // Reference to an Included Schema

        IsOf = 2 ^ 14, // Type Check annotation expression

        LabeledElement = 2 ^ 15, // Labeled Element expression

        Member = 2 ^ 16, // Enumeration Member

        NavigationProperty = 2 ^ 17, // Navigation Property

        Null = 2 ^ 18, // Null annotation expression

        OnDelete = 2 ^ 19, // On-Delete Action of a navigation property

        Parameter = 2 ^ 20, // Action of Function Parameter

        Property = 2 ^ 21, // Property of a structured type

        PropertyValue = 2 ^ 22, // Property value of a Record annotation expression

        Record = 2 ^ 23, // Record annotation expression

        Reference = 2 ^ 24, // Reference to another CSDL document

        ReferentialConstraint = 2 ^ 25, // Referential Constraint of a navigation property

        ReturnType = 2 ^ 26, // Return Type of an Action or Function

        Schema = 2 ^ 27, // Schema

        Singleton = 2 ^ 28, // Singleton

        Term = 2 ^ 29, // Term

        TypeDefinition = 2 ^ 30, // Type Definition

        UrlRef = 2 ^ 31 // UrlRef annotation expression
    }
}
