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

        public AddEnumTypeDialog()
            : this(null)
        { }

        public AddEnumTypeDialog(string schemeNamespace)
        {
            InputNamespace = schemeNamespace;
            InitializeComponent();
        }
    }
}
