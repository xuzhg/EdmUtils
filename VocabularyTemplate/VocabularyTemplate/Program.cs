using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace VocabularyTemplate
{
    class Program
    {
        static int Main(string[] args)
        {
            IEdmModel model;
            if (args.Length == 1)
            {
                // string csdlName = ""; // empty means use the built-in vacabulary
                model = LoadEdmModel(args[0]);
            }
            else
            {
                model = new EdmModel();
            }

            if (model == null)
            {
                return -1;
            }

            /*
            IEdmTerm term = model.FindTerm(termName);
            if (term == null)
            {
                Console.WriteLine($"Cannot find the term {termName}");
                return 1;
            }

            Console.WriteLine("\n======> xml\n");
            TermGenerator g = GeneartorFactor.Create("xml");
            string result = g.Run(termName, model);
            Console.WriteLine(result);

            Console.WriteLine("\n======> json\n");
            g = GeneartorFactor.Create("json");
            result = g.Run(termName, model);
            Console.WriteLine(result);

            Console.WriteLine("\n======> yaml\n");
            g = GeneartorFactor.Create("yaml");
            result = g.Run(termName, model);
            Console.WriteLine(result);
            */
            while (true)
            {
                // ShowTerms(model);
                ModelProcesser processer = new ModelProcesser(model);
                string selected = processer.ShowAndGetNamespaceMenu();

                IEdmTerm term = processer.ShowAndGetTermMenu(selected);

                string format = processer.ShowAndGetResultMenu(term);

                TermGenerator g = GeneartorFactor.Create(format);
                string result = g.Run(term, model);

                Console.Clear();
                Console.WriteLine($"\n ==> Term '{term.FullName()}' has the following '{format}' template:\n");

                ConsoleColor foreColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result);
                Console.ForegroundColor = foreColor;
                Console.WriteLine();

                Console.Write("\nPress any key do it again for other term. Or input [Q/q] for exit...");
                string input = Console.ReadLine().Trim();
                if (input == "q" || input == "Q")
                {
                    // Exit
                    Environment.Exit(0);
                }
            }
        }

        private static IEdmModel LoadEdmModel(string fileName)
        {
            IEdmModel edmModel = null;
            try
            {
                string csdl = File.ReadAllText(fileName);
                edmModel = CsdlReader.Parse(XElement.Parse(csdl).CreateReader());
            }
            catch
            {
                Console.WriteLine($"Cann't load CSDL from '{fileName}'");
                Environment.Exit(-1);
            }

            return edmModel;
        }

        static void ShowTerms(IEdmModel model)
        {
            ShowNamespace(model.DeclaredNamespaces);

            foreach(var subModel in model.ReferencedModels)
            {
                ShowNamespace(subModel.DeclaredNamespaces);
            }
        }

        static void ShowNamespace(IEnumerable<string> namespaces)
        {
            foreach(var str in namespaces)
            {
                Console.WriteLine(str);
            }
        }
    }
}
