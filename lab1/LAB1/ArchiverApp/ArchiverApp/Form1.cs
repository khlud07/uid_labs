using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace ArchiverApp
{
    public partial class Form1 : Form
    {
        private List<string> selectedFiles = new List<string>();

        private readonly Color addColor = Color.FromArgb(40, 167, 69);
        private readonly Color removeColor = Color.FromArgb(220, 53, 69);
        private readonly Color clearColor = Color.FromArgb(255, 193, 7);
        private readonly Color chooseColor = Color.FromArgb(108, 117, 125);
        private readonly Color createColor = Color.FromArgb(0, 120, 215);

        public Form1()
        {
            InitializeComponent();
            ApplyModernStyle();
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.ForeColor = Color.FromArgb(45, 45, 45);

            StyleButton(btnAddFiles, addColor, Color.White);
            StyleButton(btnRemoveFile, removeColor, Color.White);
            StyleButton(btnClearList, clearColor, Color.Black);
            StyleButton(btnChooseArchivePath, chooseColor, Color.White);
            StyleButton(btnCreateArchive, createColor, Color.White);

            SetRoundedButton(btnAddFiles, 18);
            SetRoundedButton(btnRemoveFile, 18);
            SetRoundedButton(btnClearList, 18);
            SetRoundedButton(btnChooseArchivePath, 14);
            SetRoundedButton(btnCreateArchive, 20);

            AddHoverEffect(btnAddFiles, addColor);
            AddHoverEffect(btnRemoveFile, removeColor);
            AddHoverEffect(btnClearList, clearColor);
            AddHoverEffect(btnChooseArchivePath, chooseColor);
            AddHoverEffect(btnCreateArchive, createColor);

            listBoxFiles.BorderStyle = BorderStyle.FixedSingle;
            listBoxFiles.BackColor = Color.White;
            listBoxFiles.ForeColor = Color.FromArgb(50, 50, 50);

            txtArchivePath.BackColor = Color.White;
            txtArchivePath.ForeColor = Color.FromArgb(50, 50, 50);
            txtArchivePath.BorderStyle = BorderStyle.FixedSingle;

            label1.ForeColor = Color.FromArgb(33, 37, 41);
            labelArchivePath.ForeColor = Color.FromArgb(60, 60, 60);
        }

        private void StyleButton(Button btn, Color backColor, Color foreColor)
        {
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
        }

        private void SetRoundedButton(Button btn, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(btn.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(btn.Width - radius, btn.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, btn.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();

            btn.Region = new Region(path);
        }

        private void AddHoverEffect(Button btn, Color baseColor)
        {
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = ControlPaint.Light(baseColor);
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = baseColor;
            };
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            SetRoundedButton(btnAddFiles, 18);
            SetRoundedButton(btnRemoveFile, 18);
            SetRoundedButton(btnClearList, 18);
            SetRoundedButton(btnChooseArchivePath, 14);
            SetRoundedButton(btnCreateArchive, 20);
        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Оберіть файли для архівації",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if (!selectedFiles.Contains(file))
                    {
                        selectedFiles.Add(file);
                        listBoxFiles.Items.Add(file);
                    }
                }
            }
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem != null)
            {
                string selectedFile = listBoxFiles.SelectedItem.ToString();
                selectedFiles.Remove(selectedFile);
                listBoxFiles.Items.Remove(selectedFile);
            }
            else
            {
                MessageBox.Show(
                    "Оберіть файл для видалення.",
                    "Увага",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            selectedFiles.Clear();
            listBoxFiles.Items.Clear();
        }

        private void btnChooseArchivePath_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Збереження архіву",
                Filter = "ZIP archive (*.zip)|*.zip",
                DefaultExt = "zip",
                FileName = "archive.zip"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtArchivePath.Text = saveFileDialog.FileName;
            }
        }

        private void btnCreateArchive_Click(object sender, EventArgs e)
        {
            if (selectedFiles.Count == 0)
            {
                MessageBox.Show(
                    "Список файлів порожній. Додайте файли для архівації.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(txtArchivePath.Text))
            {
                MessageBox.Show(
                    "Оберіть шлях для збереження архіву.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            string archivePath = txtArchivePath.Text;

            try
            {
                if (File.Exists(archivePath))
                {
                    File.Delete(archivePath);
                }

                using (ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
                {
                    foreach (string filePath in selectedFiles)
                    {
                        if (File.Exists(filePath))
                        {
                            string fileName = Path.GetFileName(filePath);
                            archive.CreateEntryFromFile(filePath, fileName);
                        }
                    }
                }

                MessageBox.Show(
                    "Архів успішно створено!",
                    "Успіх",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Сталася помилка при створенні архіву:\n" + ex.Message,
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}