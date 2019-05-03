namespace EdmGui.Dialog
{
    partial class AddEnumTypeDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Abc");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("xyz");
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.enumTypeNamespace = new System.Windows.Forms.TextBox();
            this.enumTypeName = new System.Windows.Forms.TextBox();
            this.isFlagCheckBox = new System.Windows.Forms.CheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.String = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AddEnumOkButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.enumUnderlingTypeCombox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enum Namespace:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enum Name:";
            // 
            // enumTypeNamespace
            // 
            this.enumTypeNamespace.Location = new System.Drawing.Point(121, 20);
            this.enumTypeNamespace.Name = "enumTypeNamespace";
            this.enumTypeNamespace.Size = new System.Drawing.Size(186, 20);
            this.enumTypeNamespace.TabIndex = 2;
            // 
            // enumTypeName
            // 
            this.enumTypeName.Location = new System.Drawing.Point(121, 57);
            this.enumTypeName.Name = "enumTypeName";
            this.enumTypeName.Size = new System.Drawing.Size(186, 20);
            this.enumTypeName.TabIndex = 2;
            // 
            // isFlagCheckBox
            // 
            this.isFlagCheckBox.AutoSize = true;
            this.isFlagCheckBox.Location = new System.Drawing.Point(250, 160);
            this.isFlagCheckBox.Name = "isFlagCheckBox";
            this.isFlagCheckBox.Size = new System.Drawing.Size(63, 17);
            this.isFlagCheckBox.TabIndex = 3;
            this.isFlagCheckBox.Text = "Is Flag?";
            this.toolTip1.SetToolTip(this.isFlagCheckBox, "the enumeration type allows multiple members to be selected simultaneously.");
            this.isFlagCheckBox.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.String,
            this.columnHeader1});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem3,
            listViewItem4});
            this.listView1.Location = new System.Drawing.Point(3, 16);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(138, 144);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // String
            // 
            this.String.Text = "String";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Value";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 163);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enum members";
            // 
            // AddEnumOkButton
            // 
            this.AddEnumOkButton.Location = new System.Drawing.Point(244, 204);
            this.AddEnumOkButton.Name = "AddEnumOkButton";
            this.AddEnumOkButton.Size = new System.Drawing.Size(75, 23);
            this.AddEnumOkButton.TabIndex = 6;
            this.AddEnumOkButton.Text = "Ok";
            this.AddEnumOkButton.UseVisualStyleBackColor = true;
            this.AddEnumOkButton.Click += new System.EventHandler(this.AddEnumOkButton_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(244, 243);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(190, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Underlying Integer Type:";
            // 
            // enumUnderlingTypeCombox
            // 
            this.enumUnderlingTypeCombox.FormattingEnabled = true;
            this.enumUnderlingTypeCombox.Items.AddRange(new object[] {
            "Edm.Byte",
            "Edm.SByte",
            "Edm.Int16",
            "Edm.Int32",
            "Edm.Int64"});
            this.enumUnderlingTypeCombox.Location = new System.Drawing.Point(193, 122);
            this.enumUnderlingTypeCombox.Name = "enumUnderlingTypeCombox";
            this.enumUnderlingTypeCombox.Size = new System.Drawing.Size(121, 21);
            this.enumUnderlingTypeCombox.TabIndex = 8;
            // 
            // AddEnumTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 284);
            this.Controls.Add(this.enumUnderlingTypeCombox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.AddEnumOkButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.isFlagCheckBox);
            this.Controls.Add(this.enumTypeName);
            this.Controls.Add(this.enumTypeNamespace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEnumTypeDialog";
            this.Text = "Add a new Edm Enum type";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox enumTypeNamespace;
        private System.Windows.Forms.TextBox enumTypeName;
        private System.Windows.Forms.CheckBox isFlagCheckBox;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader String;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button AddEnumOkButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox enumUnderlingTypeCombox;
    }
}