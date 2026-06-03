using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace ArchiverWPF
{
    public partial class MainWindow : Window
    {
        private List<string> selectedFiles = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAddFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Оберіть файли для архівації",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
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

        private void btnRemoveFile_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxFiles.SelectedItem != null)
            {
                string selectedFile = listBoxFiles.SelectedItem.ToString();
                selectedFiles.Remove(selectedFile);
                listBoxFiles.Items.Remove(selectedFile);
            }
            else
            {
                MessageBox.Show("Оберіть файл для видалення.", "Увага",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnClearList_Click(object sender, RoutedEventArgs e)
        {
            selectedFiles.Clear();
            listBoxFiles.Items.Clear();
        }

        private void btnChooseArchivePath_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Збереження архіву",
                Filter = "ZIP archive (*.zip)|*.zip",
                DefaultExt = "zip",
                FileName = "archive.zip"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                txtArchivePath.Text = saveFileDialog.FileName;
            }
        }

        private void btnCreateArchive_Click(object sender, RoutedEventArgs e)
        {
            if (selectedFiles.Count == 0)
            {
                MessageBox.Show("Список файлів порожній. Додайте файли для архівації.",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtArchivePath.Text))
            {
                MessageBox.Show("Оберіть шлях для збереження архіву.",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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

                MessageBox.Show("Архів успішно створено!",
                    "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася помилка при створенні архіву:\n" + ex.Message,
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}