// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.IO;

namespace EdmUtil
{
    /// <summary>
    /// Base class for Open API writer.
    /// </summary>
    public abstract class WriterBase : ITermWriter
    {
        /// <summary>
        /// The indentation string to prepand to each line for each indentation level.
        /// </summary>
        private const string IndentationString = "  ";

        /// <summary>
        /// Scope of the Open API element - object, array, property.
        /// </summary>
        protected readonly Stack<Scope> Scopes;

        /// <summary>
        /// Number which specifies the level of indentation.
        /// </summary>
        private int _indentLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiWriterBase"/> class.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        public WriterBase(TextWriter textWriter)
        {
            Writer = textWriter;
            Writer.NewLine = "\n";

            Scopes = new Stack<Scope>();
        }

        /// <summary>
        /// Base Indentation Level.
        /// This denotes how many indentations are needed for the property in the base object.
        /// </summary>
        protected abstract int BaseIndentation { get; }

        /// <summary>
        /// The text writer.
        /// </summary>
        protected TextWriter Writer { get; }

        /// <summary>
        /// Write start object.
        /// </summary>
        public abstract void StartObjectScope();

        /// <summary>
        /// Write end object.
        /// </summary>
        public abstract void EndObjectScope();

        /// <summary>
        /// Write start array.
        /// </summary>
        public abstract void StartArrayScope();

        /// <summary>
        /// Write end array.
        /// </summary>
        public abstract void EndArrayScope();

        /// <summary>
        /// Write the start property.
        /// </summary>
        public abstract void WritePropertyName(string name);

        /// <summary>
        /// Writes a separator of a value if it's needed for the next value to be written.
        /// </summary>
        protected abstract void WriteValueSeparator();

        /// <summary>
        /// Flush the writer.
        /// </summary>
        public void Flush()
        {
            Writer.Flush();
        }

        /// <summary>
        /// Write string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public abstract void WriteValue(string value);

        /// <summary>
        /// Increases the level of indentation applied to the output.
        /// </summary>
        public virtual void IncreaseIndentation()
        {
            _indentLevel++;
        }

        /// <summary>
        /// Decreases the level of indentation applied to the output.
        /// </summary>
        public virtual void DecreaseIndentation()
        {
            if (_indentLevel == 0)
            {
                throw new Exception("The indentation is already at Zero");
            }

            if (_indentLevel < 1)
            {
                _indentLevel = 0;
            }
            else
            {
                _indentLevel--;
            }
        }

        /// <summary>
        /// Write the indentation.
        /// </summary>
        public virtual void WriteIndentation()
        {
            for (var i = 0; i < (BaseIndentation + _indentLevel - 1); i++)
            {
                Writer.Write(IndentationString);
            }
        }

        /// <summary>
        /// Get current scope.
        /// </summary>
        /// <returns></returns>
        protected Scope CurrentScope()
        {
            return Scopes.Count == 0 ? null : Scopes.Peek();
        }

        /// <summary>
        /// Start the scope given the scope type.
        /// </summary>
        /// <param name="type">The scope type to start.</param>
        protected Scope StartScope(ScopeType type)
        {
            if (Scopes.Count != 0)
            {
                var currentScope = Scopes.Peek();

                currentScope.ObjectCount++;
            }

            var scope = new Scope(type);
            Scopes.Push(scope);
            return scope;
        }

        /// <summary>
        /// End the scope of the given scope type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected Scope EndScope(ScopeType type)
        {
            if (Scopes.Count == 0)
            {
                throw new Exception("No scope to end");
            }

            if (Scopes.Peek().Type != type)
            {
                throw new Exception("The ending scope is not match to the starting scope");
            }

            return Scopes.Pop();
        }

        /// <summary>
        /// Whether the current scope is the top level (outermost) scope.
        /// </summary>
        protected bool IsTopLevelScope()
        {
            return Scopes.Count == 1;
        }

        /// <summary>
        /// Whether the current scope is an object scope.
        /// </summary>
        protected bool IsObjectScope()
        {
            return IsScopeType(ScopeType.Object);
        }

        /// <summary>
        /// Whether the current scope is an array scope.
        /// </summary>
        /// <returns></returns>
        protected bool IsArrayScope()
        {
            return IsScopeType(ScopeType.Array);
        }

        private bool IsScopeType(ScopeType type)
        {
            if (Scopes.Count == 0)
            {
                return false;
            }

            return Scopes.Peek().Type == type;
        }

        public void Dispose()
        {
            Writer.Dispose();
        }
    }
}
