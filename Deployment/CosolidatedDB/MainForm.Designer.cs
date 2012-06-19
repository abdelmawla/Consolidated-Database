using System.Diagnostics;
using System.Windows.Forms;

namespace CosolidatedDB
{
    partial class mainForm
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
            this.txtNewDatabaseName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartDataTransfer = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblSelectedFileName = new System.Windows.Forms.LinkLabel();
            this.loadingPic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadingPic)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNewDatabaseName
            // 
            this.txtNewDatabaseName.Location = new System.Drawing.Point(147, 12);
            this.txtNewDatabaseName.MaxLength = 20;
            this.txtNewDatabaseName.Name = "txtNewDatabaseName";
            this.txtNewDatabaseName.Size = new System.Drawing.Size(135, 20);
            this.txtNewDatabaseName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "New DataBase Name";
            // 
            // btnStartDataTransfer
            // 
            this.btnStartDataTransfer.Location = new System.Drawing.Point(116, 244);
            this.btnStartDataTransfer.Name = "btnStartDataTransfer";
            this.btnStartDataTransfer.Size = new System.Drawing.Size(123, 23);
            this.btnStartDataTransfer.TabIndex = 2;
            this.btnStartDataTransfer.Text = "Start Data Transfer";
            this.btnStartDataTransfer.UseVisualStyleBackColor = true;
            this.btnStartDataTransfer.Click += new System.EventHandler(this.BtnStartDataTransferClick);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(15, 46);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(125, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse For Connection Strings";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowseClick);
            // 
            // lblSelectedFileName
            // 
            this.lblSelectedFileName.AutoEllipsis = true;
            this.lblSelectedFileName.Location = new System.Drawing.Point(149, 51);
            this.lblSelectedFileName.Name = "lblSelectedFileName";
            this.lblSelectedFileName.Size = new System.Drawing.Size(182, 23);
            this.lblSelectedFileName.TabIndex = 4;
            this.lblSelectedFileName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LblSelectedFileNameLinkClicked);
            // 
            // loadingPic
            // 
            this.loadingPic.Image = global::CosolidatedDB.Properties.Resources.Loading;
            this.loadingPic.Location = new System.Drawing.Point(116, 94);
            this.loadingPic.Name = "loadingPic";
            this.loadingPic.Size = new System.Drawing.Size(95, 95);
            this.loadingPic.TabIndex = 5;
            this.loadingPic.TabStop = false;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 306);
            this.Controls.Add(this.loadingPic);
            this.Controls.Add(this.lblSelectedFileName);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnStartDataTransfer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNewDatabaseName);
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.MainFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.loadingPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNewDatabaseName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartDataTransfer;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.LinkLabel lblSelectedFileName;
        private System.Windows.Forms.PictureBox loadingPic;
    }
}

