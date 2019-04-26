using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdmGui.Extensions
{
    public class ContextMenuEnumTypes
    {
        private static ContextMenuStrip _enumTypesContextMenu;

        public static ContextMenuStrip CreateContextMenuForEnumType()
        {
            if (_enumTypesContextMenu != null)
            {
                return _enumTypesContextMenu;
            }

            var newEnumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var expandCollapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            var contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(new System.ComponentModel.Container());

            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            newEnumToolStripMenuItem,
            expandCollapseToolStripMenuItem,
            removeAllToolStripMenuItem});
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            // 
            // newEnumToolStripMenuItem
            // 
            newEnumToolStripMenuItem.Name = "newEnumToolStripMenuItem";
            newEnumToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            newEnumToolStripMenuItem.Text = "New Enum ...";
            newEnumToolStripMenuItem.Click += new System.EventHandler(newEnumTypeMenuItem_Click);
            // 
            // expandCollapseToolStripMenuItem
            // 
            expandCollapseToolStripMenuItem.Name = "expandCollapseToolStripMenuItem";
            expandCollapseToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            expandCollapseToolStripMenuItem.Text = "Expand/Collapse";
            // 
            // removeAllToolStripMenuItem
            // 
            removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            removeAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            removeAllToolStripMenuItem.Text = "Remove All";

            _enumTypesContextMenu = contextMenuStrip1;
            return _enumTypesContextMenu;
        }

        public static void newEnumTypeMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ok");
        }
    }
}
