using System.Drawing;

namespace ArchiverApp
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.btnRemoveFile = new System.Windows.Forms.Button();
            this.btnClearList = new System.Windows.Forms.Button();
            this.btnChooseArchivePath = new System.Windows.Forms.Button();
            this.labelArchivePath = new System.Windows.Forms.Label();
            this.txtArchivePath = new System.Windows.Forms.TextBox();
            this.btnCreateArchive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(280, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Архіватор файлів";
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.ItemHeight = 23;
            this.listBoxFiles.Location = new System.Drawing.Point(30, 85);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(480, 280);
            this.listBoxFiles.TabIndex = 1;
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.btnAddFiles.Location = new System.Drawing.Point(550, 85);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(220, 46);
            this.btnAddFiles.TabIndex = 2;
            this.btnAddFiles.Text = "Додати файли";
            this.btnAddFiles.UseVisualStyleBackColor = true;
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
            // 
            // btnRemoveFile
            // 
            this.btnRemoveFile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.btnRemoveFile.Location = new System.Drawing.Point(550, 145);
            this.btnRemoveFile.Name = "btnRemoveFile";
            this.btnRemoveFile.Size = new System.Drawing.Size(220, 46);
            this.btnRemoveFile.TabIndex = 3;
            this.btnRemoveFile.Text = "Видалити файл";
            this.btnRemoveFile.UseVisualStyleBackColor = true;
            this.btnRemoveFile.Click += new System.EventHandler(this.btnRemoveFile_Click);
            // 
            // btnClearList
            // 
            this.btnClearList.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.btnClearList.Location = new System.Drawing.Point(550, 205);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(220, 46);
            this.btnClearList.TabIndex = 4;
            this.btnClearList.Text = "Очистити список";
            this.btnClearList.UseVisualStyleBackColor = true;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // labelArchivePath
            // 
            this.labelArchivePath.AutoSize = true;
            this.labelArchivePath.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelArchivePath.Location = new System.Drawing.Point(30, 392);
            this.labelArchivePath.Name = "labelArchivePath";
            this.labelArchivePath.Size = new System.Drawing.Size(136, 23);
            this.labelArchivePath.TabIndex = 5;
            this.labelArchivePath.Text = "Шлях до архіву:";
            // 
            // txtArchivePath
            // 
            this.txtArchivePath.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtArchivePath.Location = new System.Drawing.Point(180, 389);
            this.txtArchivePath.Name = "txtArchivePath";
            this.txtArchivePath.Size = new System.Drawing.Size(430, 30);
            this.txtArchivePath.TabIndex = 6;
            // 
            // btnChooseArchivePath
            // 
            this.btnChooseArchivePath.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnChooseArchivePath.Location = new System.Drawing.Point(625, 387);
            this.btnChooseArchivePath.Name = "btnChooseArchivePath";
            this.btnChooseArchivePath.Size = new System.Drawing.Size(145, 34);
            this.btnChooseArchivePath.TabIndex = 7;
            this.btnChooseArchivePath.Text = "Обрати...";
            this.btnChooseArchivePath.UseVisualStyleBackColor = true;
            this.btnChooseArchivePath.Click += new System.EventHandler(this.btnChooseArchivePath_Click);
            // 
            // btnCreateArchive
            // 
            this.btnCreateArchive.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCreateArchive.Location = new System.Drawing.Point(285, 452);
            this.btnCreateArchive.Name = "btnCreateArchive";
            this.btnCreateArchive.Size = new System.Drawing.Size(230, 50);
            this.btnCreateArchive.TabIndex = 8;
            this.btnCreateArchive.Text = "Створити архів";
            this.btnCreateArchive.UseVisualStyleBackColor = true;
            this.btnCreateArchive.Click += new System.EventHandler(this.btnCreateArchive_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 540);
            this.Controls.Add(this.btnCreateArchive);
            this.Controls.Add(this.txtArchivePath);
            this.Controls.Add(this.labelArchivePath);
            this.Controls.Add(this.btnChooseArchivePath);
            this.Controls.Add(this.btnClearList);
            this.Controls.Add(this.btnRemoveFile);
            this.Controls.Add(this.btnAddFiles);
            this.Controls.Add(this.listBoxFiles);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Архіватор";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.Button btnAddFiles;
        private System.Windows.Forms.Button btnRemoveFile;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.Button btnChooseArchivePath;
        private System.Windows.Forms.Label labelArchivePath;
        private System.Windows.Forms.TextBox txtArchivePath;
        private System.Windows.Forms.Button btnCreateArchive;
    }
}