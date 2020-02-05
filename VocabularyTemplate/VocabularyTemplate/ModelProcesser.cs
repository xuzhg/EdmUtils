using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using VocabularyTemplate.Writer;

namespace VocabularyTemplate
{
    public class ModelProcesser
    {
        private IEdmModel _model;

        private IDictionary<string, IEdmModel> _namespacesModels = new Dictionary<string, IEdmModel>();

        public ModelProcesser(IEdmModel model)
        {
            Initialize(model);
        }

        public string ShowAndGetNamespaceMenu()
        {
            string[] nses = _namespacesModels.Keys.ToArray();
            int maxRetry = 3;
            int retry = 1;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nSelect namespace:\n");

                for (int i = 1; i <= nses.Length; i++)
                {
                    Console.WriteLine($"\t[{i}]: {nses[i-1]}");
                }

                Console.Write("\nPlease select a namespace number or [Q/q] for quit:");

                string input = Console.ReadLine().Trim();
                if (input == "q" || input == "Q")
                {
                    // Exit
                    Environment.Exit(0);
                }

                bool bOk = Int32.TryParse(input, out int index);
                if (bOk)
                {
                    if (index >= 1 && index <= nses.Length)
                    {
                        return nses[index - 1];
                    }
                }

                ConsoleColor foreColor = Console.ForegroundColor;
                if (retry > maxRetry)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[Error:] Max try reach, Close now. 88");
                    Console.ForegroundColor = foreColor;
                    Environment.Exit(0);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error:] Wrong input, do you want to retry {retry}/{maxRetry} [y]?");
                Console.ForegroundColor = foreColor;

                input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    retry++;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        public IEdmTerm ShowAndGetTermMenu(string nameSpace)
        {
            _namespacesModels.TryGetValue(nameSpace, out IEdmModel model);
            if (model == null)
            {
                Environment.Exit(0);
            }

            var terms = model.SchemaElements.OfType<IEdmTerm>().ToArray();
            int length = terms.Length;
            int maxRetry = 3;
            int retry = 1;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"\nSelect term under namespace \"{nameSpace}\":\n");

                if (length > 10)
                {
                    for (int i = 1; i <= length; i++)
                    {
                        Console.Write(String.Format("\t{0,02}: {1,-35}", i, terms[i - 1].Name));
                        i++;
                        if (i <= length)
                        {
                            Console.WriteLine(String.Format("\t{0,02}: {1,-35}", i, terms[i - 1].Name));
                        }
                    }

                    if (length % 2 != 0)
                    {
                        Console.WriteLine();
                    }
                }
                else
                {
                    for (int i = 1; i <= length; i++)
                    {
                        Console.WriteLine(String.Format("\t{0,02}: {1,-35}", i, terms[i - 1].Name));
                    }
                }

                Console.Write("\nPlease select a term number or [Q/q] for quit or [B/b] for back:");

                string input = Console.ReadLine().Trim();
                if (input == "q" || input == "Q")
                {
                    // Exit
                    Environment.Exit(0);
                }

                if (input == "B" || input == "b")
                {
                    nameSpace = ShowAndGetNamespaceMenu();
                    return ShowAndGetTermMenu(nameSpace);
                }

                bool bOk = Int32.TryParse(input, out int index);
                if (bOk)
                {
                    if (index >= 1 && index <= terms.Length)
                    {
                        return terms[index - 1];
                    }
                }

                ConsoleColor foreColor = Console.ForegroundColor;
                if (retry > maxRetry)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[Error:] Max try reach, Close now. 88");
                    Console.ForegroundColor = foreColor;
                    Environment.Exit(0);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error:] Wrong input, do you want to retry {retry}/{maxRetry} [y]?");
                Console.ForegroundColor = foreColor;

                input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    retry++;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        public string ShowAndGetResultMenu(IEdmTerm term)
        {
            int maxRetry = 3;
            int retry = 1;
            string[] types = { "xml", "json", "yaml" };
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"\nSelect output format for \"{term.FullName()}\":\n");

                for (int i = 1; i <= types.Length; i++)
                {
                    Console.WriteLine($"\t[{i}]: {types[i - 1]}");
                }

                Console.Write("\nPlease select the output index or [Q/q] for quit:");

                string input = Console.ReadLine().Trim();
                if (input == "q" || input == "Q")
                {
                    // Exit
                    Environment.Exit(0);
                }

                bool bOk = Int32.TryParse(input, out int index);
                if (bOk)
                {
                    if (index >= 1 && index <= types.Length)
                    {
                        return types[index - 1];
                    }
                }

                ConsoleColor foreColor = Console.ForegroundColor;
                if (retry > maxRetry)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[Error:] Max try reach, Close now. 88");
                    Console.ForegroundColor = foreColor;
                    Environment.Exit(0);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error:] Wrong input, do you want to retry {retry}/{maxRetry} [y]?");
                Console.ForegroundColor = foreColor;

                input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    retry++;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        private void Initialize(IEdmModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _model = model;

            ISet<IEdmModel> visited = new HashSet<IEdmModel>();
            Browse(model, visited);

            foreach(var m in visited)
            {
                foreach (var ns in m.DeclaredNamespaces)
                {
                    _namespacesModels[ns] = m;
                }
            }
        }

        private void Browse(IEdmModel model, ISet<IEdmModel> visited)
        {
            if (visited.Contains(model))
            {
                return;
            }

            visited.Add(model);

            foreach(var subModel in model.ReferencedModels)
            {
                Browse(subModel, visited);
            }
        }

    }
}
