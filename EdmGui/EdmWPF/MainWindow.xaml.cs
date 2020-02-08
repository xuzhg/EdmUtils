
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using EdmUtil;
using EdmWPF.Model;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Vocabularies;
using Microsoft.Win32;
using Microsoft.OData.Edm.Validation;
using System.Text;

namespace EdmWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       // private ODataTermViewModel _termModel = new ODataTermViewModel();
        private IDictionary<string, IEdmModel> _namespacesModels = new Dictionary<string, IEdmModel>();

        private static EdmModel _defaultEdmModel = new EdmModel();
        private bool _isInUsingDefaultModel = false;
        private TemplateFormat format;

        public MainWindow()
        {
            InitializeComponent();

            // this.DataContext = _termModel;

            defaultModelButton.IsChecked = true;
            EdmFileNameTextBox.Text = "Using default OData CSDL...";
            _isInUsingDefaultModel = true;
            format = TemplateFormat.Xml;
            xmlRadioBtn.IsChecked = true;
            TermOutputTextBox.Text = "<< Please select Namespace and TermName >>";
            ResetModel(_defaultEdmModel);

            //   ResetModel(_defaultEdmModel);
        }

        private void DefaultEdmModelRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isInUsingDefaultModel)
            {
                return;
            }

            _isInUsingDefaultModel = true;
            EdmFileNameTextBox.Text = "Using default OData CSDL...";
            ResetModel(_defaultEdmModel);
        }

        private void EdmModelRadioButton_Click(object sender, RoutedEventArgs e)
        {
            _isInUsingDefaultModel = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "OData CSDL files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                IEdmModel model = LoadEdmModel(openFileDialog.FileName);
                if (model != null)
                {
                    EdmFileNameTextBox.Text = openFileDialog.FileName;
                    ResetModel(model);

                    return;
                }
            }

            EdmFileNameTextBox.Text = "No OData CSDL Loaded.";
            ResetModel(null);
        }

        private void ResetModel(IEdmModel model)
        {
            _namespacesModels.Clear();
            NamesapceComboBox.Items.Clear();
            TermNameComboBox.Items.Clear();
            
            TermOutputTextBox.Text = "";

            if (model == null)
            {
                return;
            }

            ISet<IEdmModel> visited = new HashSet<IEdmModel>();
            Browse(model, visited);

            foreach (var m in visited)
            {
                foreach (var ns in m.DeclaredNamespaces)
                {
                    _namespacesModels[ns] = m;
                    NamesapceComboBox.Items.Add(ns);
                }
            }

            // ResetNamespaceComboBox();
        }

        private void OutputTypeRadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch(radioButton.Tag as String)
            {
                case "json":
                    format = TemplateFormat.Json;
                    break;

                case "yaml":
                    format = TemplateFormat.Yaml;
                    break;

                case "xml":
                    format = TemplateFormat.Xml;
                    break;

            }

            string nameSpace = null;
            if (NamesapceComboBox.SelectedIndex != -1)
            {
                nameSpace = NamesapceComboBox.Text;
            }

            string termName = null;
            if (TermNameComboBox.SelectedIndex != -1)
            {
                termName = TermNameComboBox.Text;
            }

            ResetTemplateForamt(nameSpace, termName);
        }

        private void ResetTemplateForamt(string nameSpace, string termName)
        {
            if (nameSpace == null || termName == null)
            {
                TermOutputTextBox.Text = "<< Please select Namespace and TermName >>";
                return;
            }

            IEdmModel model = _namespacesModels[nameSpace];

            IEdmTerm term = model.FindTerm(nameSpace + "." + termName);
            if (term == null)
            {
                MessageBox.Show("Cannot find the Term", "Weird", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (term.IsBad() && term.Errors().Any())
            {
                StringBuilder sb = new StringBuilder();
                foreach (var err in term.Errors())
                {
                    sb.Append(err.ToString()).Append("\n");
                }

                MessageBox.Show(sb.ToString(), "Bad Term defined", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            TermOutputTextBox.Text = GeneratorFactory.GenerateTermTemplate(model, term, format);
        }

        private void Browse(IEdmModel model, ISet<IEdmModel> visited)
        {
            if (visited.Contains(model))
            {
                return;
            }

            visited.Add(model);

            foreach (var subModel in model.ReferencedModels)
            {
                Browse(subModel, visited);
            }
        }

        private void NamesapceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_namespacesModels.Any())
            {
                TermNameComboBox.Items.Clear();

                string ns = (sender as ComboBox).SelectedItem as string;
                IEdmModel model = _namespacesModels[ns];
                var terms = model.SchemaElements.OfType<IEdmTerm>().ToArray().OrderBy(e => e.Name);

                foreach (var term in terms)
                {
                    TermNameComboBox.Items.Add(term.Name);
                }
            }
        }

        private void TermNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_namespacesModels.Any())
            {
                string termName = (sender as ComboBox).SelectedItem as string;

                string nameSpace = null;
                if (NamesapceComboBox.SelectedIndex != -1)
                {
                    nameSpace = NamesapceComboBox.Text;
                }

                ResetTemplateForamt(nameSpace, termName);
            }
        }

        private static IEdmModel LoadEdmModel(string fileName)
        {
            IEdmModel edmModel;
            try
            {
                string csdl = File.ReadAllText(fileName);
                edmModel = CsdlReader.Parse(XElement.Parse(csdl).CreateReader());
            }
            catch
            {
                MessageBox.Show($"Cann't load CSDL from '{fileName}'", "Load Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return edmModel;
        }
    }
}
