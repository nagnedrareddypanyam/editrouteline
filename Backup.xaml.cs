using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;
using System.IO.Compression;
using System.Security.AccessControl;
using System.Security.Principal;

namespace EWLDitital.PresentationLayer.Views
{
    /// <summary>
    /// Interaction logic for Backup.xaml
    /// </summary>
    public partial class Backup : Window
    {
        public Backup()
        {
            InitializeComponent();

            string fullPath = System.IO.Directory.GetCurrentDirectory();
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] str = path.Split('\\');
            string finalpath = str[0] + "\\" + str[1] + "\\" + str[2];
            DirectoryInfo dInfo = new DirectoryInfo(finalpath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

            //string fullPath1 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            DirectoryInfo dInfo1 = new DirectoryInfo(finalpath);
            DirectorySecurity dSecurity1 = dInfo1.GetAccessControl();
            dSecurity1.AddAccessRule(new FileSystemAccessRule("everyone", FileSystemRights.FullControl,
                                                             InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                                             PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
        }
        public string extractPath = AppDomain.CurrentDomain.BaseDirectory + "\\XMLFiles\\";
        public string extractPath1 = @"E:\\Alacrty Doc\\";
        public string fpath = "";
        public string zipPath = "";
        private void finish_Click(object sender, RoutedEventArgs e)
        {
            fpath = System.IO.Path.GetFileNameWithoutExtension(zipPath);
            ZipFile.CreateFromDirectory(extractPath, zipPath +".zip", CompressionLevel.Fastest, true);
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.InitialDirectory = textbox.Text; // Use current value for initial dir
                dialog.Title = "Select a Directory"; // instead of default "Save As"
                dialog.Filter = "Directory|*"; // Prevents displaying files
                dialog.FileName = "backup_" + DateTime.Now.ToString("dd-MM-yyyy");
                if (dialog.ShowDialog() == true)
                {
                    string path = dialog.FileName;
                    // Remove fake filename from resulting path
                    path = path.Replace("\\select.this.directory", "");
                    path = path.Replace(".this.directory", "");
                    // If user has changed the filename, create the new directory
                    //if (!System.IO.Directory.Exists(path))
                    //{
                    //    System.IO.Directory.CreateDirectory(path);
                    //}
                    // Our final value is in path
                    zipPath = dialog.FileName;
                    textbox.Text = zipPath;
                    finish.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void update2_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            // openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                zipPath = openFileDialog.FileName;
                textbox.Text = zipPath;
                finish2.IsEnabled = true;
            }
        }
        

        private void finish2_Click(object sender, RoutedEventArgs e)
        {
            //fpath = System.IO.Path.GetFileNameWithoutExtension(zipPath);
            //ZipFile.ExtractToDirectory(zipPath, extractPath1);

            using (var strm = File.OpenRead(zipPath))
            using (ZipArchive a = new ZipArchive(strm))
            {
                a.Entries.Where(o => o.Name == string.Empty && !Directory.Exists(System.IO.Path.Combine(extractPath1, o.FullName))).ToList().ForEach(o => Directory.CreateDirectory(System.IO.Path.Combine(extractPath1, o.FullName)));
                a.Entries.Where(o => o.Name != string.Empty).ToList().ForEach(i => i.ExtractToFile(System.IO.Path.Combine(extractPath1, i.FullName), true));
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
