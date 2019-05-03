using EdmLib;
using EdmLib.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdmGui.Dialog
{
    public partial class AddEnumTypeDialog : Form
    {
        private string InputNamespace { get; }
        private KeSchema schema { get; }

        public AddEnumTypeDialog()
            : this(schemeNamespace: null)
        { }



        public AddEnumTypeDialog(string schemeNamespace)
        {
            InitializeComponent();
            enumTypeNamespace.Text = schemeNamespace;
            if (schemeNamespace != null)
            {
                enumTypeNamespace.Enabled = false;
            }
        }

        public AddEnumTypeDialog(KeSchema schema)
        {
            InitializeComponent();
            enumTypeNamespace.Text = schema.Namespace;
            enumTypeNamespace.Enabled = false;

            enumUnderlingTypeCombox.SelectedIndex = 2;
            this.schema = schema;
        }

        public bool IsFlag { get; private set; }
        public string EnumName { get; private set; }

        public string UnderlingTypeName { get; private set; }

        private void AddEnumOkButton_Click(object sender, EventArgs e)
        {
            string enumName = enumTypeName.Text;
            if (!KeValidator.VerifySimpleIdentifier(enumName))
            {
                if (MessageBox.Show($"Wrong format of enum name {enumName}, it must follow up the simple identifier rules.", "Invalid name format", MessageBoxButtons.RetryCancel)
                    == DialogResult.Cancel)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
                else
                {
                    enumTypeName.Focus();
                    return;
                }
            }

            if (!KeValidator.IsUnique(this.schema, enumName))
            {
                if (MessageBox.Show($"The num name '{enumName}' is already used in this namespace. Do you want to try another name? ",
                    "Enum name Unique", MessageBoxButtons.RetryCancel)
                    == DialogResult.Cancel)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
                else
                {
                    enumTypeName.Focus();
                    return;
                }
            }

            IsFlag = isFlagCheckBox.Checked;
            EnumName = enumTypeName.Text;
            UnderlingTypeName = enumUnderlingTypeCombox.SelectedText;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
