using EdmGui.Extensions;
using EdmLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdmGui
{
    static class KeModelExtensions
    {
        public static void FillTreeView(this KeModel model, TreeView modelTreeView)
        {
            modelTreeView.Nodes.Clear();

            TreeNode topLevelNode = modelTreeView.Nodes.Add("OData-Edm (v4)"); // Top level
            topLevelNode.Tag = model;

            foreach (KeSchema schema in model.Schemas)
            {
                TreeNode schemaNode = topLevelNode.Nodes.Add("Schema - " + schema.Namespace);
                schemaNode.Tag = schema;
                
                schema.FillNode(schemaNode);
            }
        }

        public static void FillNode(this KeSchema schema, TreeNode schemaNode)
        {
            // Enum types
            TreeNode enumTypesNode = schemaNode.Nodes.Add("Enum Types");
            enumTypesNode.Tag = "Enum Types";
            enumTypesNode.ContextMenuStrip = ContextMenuEnumTypes.CreateContextMenuForEnumType();

            foreach (KeEnumType enumType in schema.EnumTypes)
            {

            }

            // Complex types
            TreeNode complexTypesNode = schemaNode.Nodes.Add("Complex Types");
            complexTypesNode.Tag = "";

            foreach (KeComplexType complexType in schema.ComplexTypes)
            {

            }

            // Entity types
            TreeNode entityTypesNode = schemaNode.Nodes.Add("Entity Types");
            entityTypesNode.Tag = "";

            foreach (KeEntityType entityType in schema.EntityTypes)
            {

            }
        }

        public static ContextMenuStrip CreateContextMenuForEnumType()
        {
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

            return contextMenuStrip1;
        }
    }
}
