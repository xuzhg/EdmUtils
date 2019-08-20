
using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Annotation.EdmUtil
{
    public static class OperationHelper
    {
        public static IEdmOperation ResolveOperations(string identifer, IList<string> parameterNames, IEdmType bindingType, IEdmModel model)
        {
            IEnumerable<IEdmOperation> candidates = model.FindBoundOperations(identifer, bindingType);
            if (!candidates.Any())
            {
                return null;
            }

            bool hasParameters = parameterNames.Count() > 0;
            if (hasParameters)
            {
                candidates = candidates.Where(o => o.IsFunction()).FilterOperationsByParameterNames(parameterNames); // remove actions
            }
            else if (bindingType != null)
            {
                // Filter out functions with more than one parameter. Actions should not be filtered as the parameters are in the payload not the uri
                candidates = candidates.Where(o =>
                (o.IsFunction() && (o.Parameters.Count() == 1 || o.Parameters.Skip(1).All(p => p is IEdmOptionalParameter))) || o.IsAction());
            }
            else
            {
                // Filter out functions with any parameters
                candidates = candidates.Where(o => (o.IsFunction() && !o.Parameters.Any()) || o.IsAction());
            }

            // Only filter if there is more than one and its needed.
            if (candidates.Count() > 1)
            {
                // candidates = candidates.FilterBoundOperationsWithSameTypeHierarchyToTypeClosestToBindingType(bindingType);
            }

            if (candidates.Any(f => f.IsAction()))
            {
                return candidates.Single();
            }

            // If more than one overload matches, try to select based on optional parameters
            if (candidates.Count() > 1)
            {
                // candidates = candidates.FindBestOverloadBasedOnParameters(parameterNames);
            }

            if (candidates.Count() > 1)
            {
                throw new Exception("TODO:");
            }

            return candidates.First();
        }

        public static IEdmOperationImport ResolveOperationImports(string identifer, IList<string> parameterNames, IEdmModel model)
        {
            IEnumerable<IEdmOperationImport> candidates = model.FindDeclaredOperationImports(identifer);
            if (!candidates.Any())
            {
                return null;
            }

            // only action import, without (...)
            if (parameterNames == null)
            {
                candidates = candidates.Where(i => i.IsActionImport());
                if (candidates.Count() > 1)
                {
                    throw new Exception($"Multiple action import overloads for '{identifer}' were found.");
                }

                return candidates.Single();
            }
            else
            {
                candidates = candidates.Where(i => i.IsFunctionImport()).FilterOperationImportsByParameterNames(parameterNames);
            }

            // If parameter count is zero and there is one function import whoese parameter count is zero, return this function import.
            if (candidates.Count() > 1 && parameterNames.Count == 0)
            {
                candidates = candidates.Where(operationImport => operationImport.Operation.Parameters.Count() == 0);
            }

            if (!candidates.Any())
            {
                return null;
            }

            if (candidates.Count() > 1)
            {
                throw new Exception($"Multiple function import overloads for '{identifer}' were found.");
            }

            return candidates.First();
        }

        internal static IEnumerable<IEdmOperation> FilterOperationsByParameterNames(this IEnumerable<IEdmOperation> operations, IEnumerable<string> parameters)
        {
            IList<string> parameterNameList = parameters.ToList();

            // TODO: update code that is duplicate between operation and operation import, add more tests.
            foreach (IEdmOperation operation in operations)
            {
                if (!ParametersSatisfyFunction(operation, parameterNameList))
                {
                    continue;
                }

                yield return operation;
            }
        }

        internal static IEnumerable<IEdmOperationImport> FilterOperationImportsByParameterNames(this IEnumerable<IEdmOperationImport> operationImports,
            IEnumerable<string> parameterNames)
        {
            IList<string> parameterNameList = parameterNames.ToList();

            foreach (IEdmOperationImport operationImport in operationImports)
            {
                if (!ParametersSatisfyFunction(operationImport.Operation, parameterNameList))
                {
                    continue;
                }

                yield return operationImport;
            }
        }

        private static bool ParametersSatisfyFunction(IEdmOperation operation, IList<string> parameterNameList)
        {
            IEnumerable<IEdmOperationParameter> parametersToMatch = operation.Parameters;

            // bindable functions don't require the first parameter be specified, since its already implied in the path.
            if (operation.IsBound)
            {
                parametersToMatch = parametersToMatch.Skip(1);
            }

            List<IEdmOperationParameter> functionParameters = parametersToMatch.ToList();

            // if any required parameters are missing, don't consider it a match.
            if (functionParameters.Where(
                p => !(p is IEdmOptionalParameter)).Any(p => parameterNameList.All(k => !string.Equals(k, p.Name))))
            {
                return false;
            }

            // if any specified parameters don't match, don't consider it a match.
            if (parameterNameList.Any(k => functionParameters.All(p => !string.Equals(k, p.Name))))
            {
                return false;
            }

            return true;
        }
    }
}
