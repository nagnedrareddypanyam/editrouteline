using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EWLDitital.PresentationLayer.Views
{
    /// <summary>
    /// Interaction logic for BackupRestore.xaml
    /// </summary>
    public partial class BackupRestore : UserControl
    {

        public string backupPath = AppDomain.CurrentDomain.BaseDirectory + "\\XMLFiles\\";
        public string extractPath = AppDomain.CurrentDomain.BaseDirectory;
        public string fpath = "";
        public string zipPath = "";

        public BackupRestore()
        {
            InitializeComponent();
        }

        private void btnSelectPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.InitialDirectory = lblBackupPath.Text; // Use current value for initial dir
                dialog.Title = "Select a Directory"; // instead of default "Save As"
                dialog.Filter = "Directory|*"; // Prevents displaying files
                dialog.FileName = "backup_" + DateTime.Now.ToString("dd-MM-yyyy");
                if (dialog.ShowDialog() == true)
                {
                    string path = dialog.FileName;
                    // Remove fake filename from resulting path
                    path = path.Replace("\\select.this.directory", "");
                    path = path.Replace(".this.directory", "");
                    
                    zipPath = dialog.FileName;
                    lblBackupPath.Text = zipPath;
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSelectPath1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            // openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                zipPath = openFileDialog.FileName;
                lblRestorPath.Text = zipPath;
            }
           
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            if (lblBackupPath.Text != "")
            {
                fpath = System.IO.Path.GetFileNameWithoutExtension(zipPath);
                ZipFile.CreateFromDirectory(backupPath, zipPath + ".zip", CompressionLevel.Fastest, true);

                MessageBox.Show("Successfully Backup All Data", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("Please Select Backuped Path First", "Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (lblRestorPath.Text != "")
            {
                using (var strm = File.OpenRead(zipPath))
                using (ZipArchive a = new ZipArchive(strm))
                {
                    a.Entries.Where(o => o.Name == string.Empty && !Directory.Exists(System.IO.Path.Combine(extractPath, o.FullName))).ToList().ForEach(o => Directory.CreateDirectory(System.IO.Path.Combine(extractPath, o.FullName)));
                    a.Entries.Where(o => o.Name != string.Empty).ToList().ForEach(i => i.ExtractToFile(System.IO.Path.Combine(extractPath, i.FullName), true));
                }

                MessageBox.Show("Successfully Restored All Data", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please Select Restore Path First", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
