using EdmGui.Dialog;
using EdmLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdmGui
{
    public partial class MainForm : Form
    {
        private KeModel _keModel;
        private bool _changed;

        public MainForm()
        {
            _keModel = KeModelHelper.GetSampleModel();
            _changed = false;

            InitializeComponent();

            _keModel.FillTreeView(this.edmModeltreeView);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Click the New menu
            if (_keModel != null && _changed)
            {
                DialogResult dlgResult = MessageBox.Show("Do you want to save changes to Untitled", "EdmGui", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                switch(dlgResult)
                {
                    case DialogResult.Yes:
                        break;

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        return;
                }
            }

            _keModel = new KeModel();
            _keModel.FillTreeView(this.edmModeltreeView);
            _changed = true;
        }
       
        private void edmModeltreeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point clickPoint = new Point(e.X, e.Y);
                TreeNode clickNode = edmModeltreeView.GetNodeAt(clickPoint);
                if (clickNode == null)
                {
                    return;
                }

//                MessageBox.Show("aaa" + clickNode.Text);
            }
        }

        private void edmModeltreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Text == "Enum Types")
                {
                    e.Node.ContextMenuStrip.Tag = e.Node;
                    /*
                    AddEnumTypeDialog newEnumTypeDialog = new AddEnumTypeDialog();
                    if (newEnumTypeDialog.ShowDialog() == DialogResult.OK)
                    {
                    //    MessageBox.Show(e.Node.Text + "OK" );
                    }*/
                }
            }
        }
    }
}
