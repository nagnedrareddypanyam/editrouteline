using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
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
using System.Xml.Linq;

namespace EWLDitital.PresentationLayer.Views
{
    /// <summary>
    /// Interaction logic for UpdateFiles.xaml
    /// </summary>
    public partial class UpdateFiles : Window
    {
        public string extractPath = AppDomain.CurrentDomain.BaseDirectory + "\\Extracted";
        public string fpath = "";
        public string zipPath = "";
        public UpdateFiles()
        {
            InitializeComponent();
            string fullPath = System.IO.Directory.GetCurrentDirectory();
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] str= path.Split('\\');
            string finalpath = str[0] + "\\" + str[1] +"\\" + str[2];
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
        private void StartStopWait()
        {
            LoadingAdorner.IsAdornerVisible = !LoadingAdorner.IsAdornerVisible;
        }
        private void update_Click(object sender, RoutedEventArgs e)
        {
            
           
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(extractPath);
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }

                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                // openFileDialog.Filter = "All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    zipPath = openFileDialog.FileName;
                    textbox.Text = zipPath;
                    finish.IsEnabled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartStopWait();
               
                //fpath = System.IO.Path.GetExtension(openFileDialog.FileName);
                fpath = System.IO.Path.GetFileNameWithoutExtension(zipPath);
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\XMLFiles\\";

                if (File.Exists(extractPath + "\\" + fpath + "\\DELTA_avcs_catalogue_folio.xml"))
                {
                    Func<XElement, string, string> getAttributeValue = (xElement, name) => xElement.Element(name).Value;
                    Func<XElement, string, string> getAttributeValue1 = (xElement, name) => xElement.Value;

                    XDocument xmlOld = XDocument.Load(filepath + "avcs_catalogue_folio.xml");
                    XDocument xmlNew = XDocument.Load(extractPath + "\\" + fpath + "\\DELTA_avcs_catalogue_folio.xml");

                    var ModifidNodes = new List<string>();

                    var nodeName = "UnitOfSale";
                    //var oldNodes = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    // id's of all new nodes

                    var sameNodes = new List<string>();

                    // sameNodes = xmlNew.Descendants("ShortName").ToList();


                    //var oldNodes1 = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    //// id's of all new nodes
                    var newNodes1 = xmlNew.Descendants("ID").Select(node => getAttributeValue1(node, "ID")).ToList();
                    // find new nodes that are not present in the old collection
                    //  var nodeIdsToAdd = newNodes1.Join(oldNodes);

                    // var customer = oldNodes1.FirstOrDefault(cus => newNodes1.Contains("ShortName"));
                    xmlOld.Descendants(nodeName).ToList().ForEach(item =>
                    {
                        var currentElementId = getAttributeValue(item, "ID");

                    });
                    // find new nodes that are not present in the old collection
                    //var nodeIdsToAdd = newNodes.Except(oldNodes);

                    var newNodes11 = xmlOld.Descendants("ID").Select(node => getAttributeValue1(node, "ID")).ToList();
                    //// remove unchanged nodes
                    foreach (var oldNodeId in newNodes1)
                    {
                        var customer = newNodes11.FirstOrDefault(cus => newNodes11.Contains(oldNodeId));
                        if (customer != null)
                        {
                            xmlOld.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ID") == oldNodeId).Remove();
                        }

                        //
                    }
                    foreach (var newNodeId1 in newNodes1)
                    {
                        var newNode = xmlNew.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ID") == newNodeId1);
                        if (newNode != null)
                        {
                            xmlOld.Element("Products").Add(newNode);
                        }
                    }

                    xmlOld.Save(filepath + "avcs_catalogue_folio.xml");
                }

                if (File.Exists(extractPath + "\\" + fpath + "\\DELTA_lol_digital_catalogue.xml"))
                {
                    Func<XElement, string, string> getAttributeValue = (xElement, name) => xElement.Element(name).Value;
                    Func<XElement, string, string> getAttributeValue1 = (xElement, name) => xElement.Value;

                    XDocument xmlOld = XDocument.Load(filepath + "lol_digital_catalogue.xml");
                    XDocument xmlNew = XDocument.Load(extractPath + "\\" + fpath + "\\DELTA_lol_digital_catalogue.xml");

                    var ModifidNodes = new List<string>();

                    var nodeName = "ListOfLights";
                    //var oldNodes = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    // id's of all new nodes

                    var sameNodes = new List<string>();

                    // sameNodes = xmlNew.Descendants("ShortName").ToList();


                    //var oldNodes1 = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    //// id's of all new nodes
                    var newNodes1 = xmlNew.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    // find new nodes that are not present in the old collection
                    //  var nodeIdsToAdd = newNodes1.Join(oldNodes);

                    // var customer = oldNodes1.FirstOrDefault(cus => newNodes1.Contains("ShortName"));
                    xmlOld.Descendants(nodeName).ToList().ForEach(item =>
                    {
                        var currentElementId = getAttributeValue(item, "ShortName");

                    });
                    // find new nodes that are not present in the old collection
                    //var nodeIdsToAdd = newNodes.Except(oldNodes);

                    var newNodes11 = xmlOld.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    //// remove unchanged nodes
                    foreach (var oldNodeId in newNodes1)
                    {
                        var customer = newNodes11.FirstOrDefault(cus => newNodes11.Contains(oldNodeId));
                        if (customer != null)
                        {
                            xmlOld.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == oldNodeId).Remove();
                        }

                        //
                    }
                    foreach (var newNodeId1 in newNodes1)
                    {
                        var newNode = xmlNew.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == newNodeId1);
                        if (newNode != null)
                        {
                            xmlOld.Element("Products").Add(newNode);
                        }
                    }

                    xmlOld.Save(filepath + "lol_digital_catalogue.xml");
                }

                if (File.Exists(extractPath + "\\" + fpath + "\\DELTA_alrsdigital_catalogue.xml"))
                {
                    Func<XElement, string, string> getAttributeValue = (xElement, name) => xElement.Element(name).Value;
                    Func<XElement, string, string> getAttributeValue1 = (xElement, name) => xElement.Value;

                    XDocument xmlOld = XDocument.Load(filepath + "alrsdigital_catalogue.xml");
                    XDocument xmlNew = XDocument.Load(extractPath + "\\" + fpath + "\\DELTA_alrsdigital_catalogue.xml");

                    var ModifidNodes = new List<string>();

                    var nodeName = "RadioSignals";
                    //var oldNodes = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    // id's of all new nodes

                    var sameNodes = new List<string>();

                    // sameNodes = xmlNew.Descendants("ShortName").ToList();


                    //var oldNodes1 = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    //// id's of all new nodes
                    var newNodes1 = xmlNew.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    // find new nodes that are not present in the old collection
                    //  var nodeIdsToAdd = newNodes1.Join(oldNodes);

                    // var customer = oldNodes1.FirstOrDefault(cus => newNodes1.Contains("ShortName"));
                    xmlOld.Descendants(nodeName).ToList().ForEach(item =>
                    {
                        var currentElementId = getAttributeValue(item, "ShortName");

                    });
                    // find new nodes that are not present in the old collection
                    //var nodeIdsToAdd = newNodes.Except(oldNodes);

                    var newNodes11 = xmlOld.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    //// remove unchanged nodes
                    foreach (var oldNodeId in newNodes1)
                    {
                        var customer = newNodes11.FirstOrDefault(cus => newNodes11.Contains(oldNodeId));
                        if (customer != null)
                        {
                            xmlOld.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == oldNodeId).Remove();
                        }

                        //
                    }
                    foreach (var newNodeId1 in newNodes1)
                    {
                        var newNode = xmlNew.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == newNodeId1);
                        if (newNode != null)
                        {
                            xmlOld.Element("Products").Add(newNode);
                        }
                    }

                    xmlOld.Save(filepath + "alrsdigital_catalogue.xml");
                }

                if (File.Exists(extractPath + "\\" + fpath + "\\DELTA_enp_catalogue.xml"))
                {
                    Func<XElement, string, string> getAttributeValue = (xElement, name) => xElement.Element(name).Value;
                    Func<XElement, string, string> getAttributeValue1 = (xElement, name) => xElement.Value;

                    XDocument xmlOld = XDocument.Load(filepath + "enp_catalogue.xml");
                    XDocument xmlNew = XDocument.Load(extractPath + "\\" + fpath + "\\DELTA_enp_catalogue.xml");

                    var ModifidNodes = new List<string>();

                    var nodeName = "ElectronicPublications";
                    //var oldNodes = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    // id's of all new nodes

                    var sameNodes = new List<string>();

                    // sameNodes = xmlNew.Descendants("ShortName").ToList();


                    //var oldNodes1 = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    //// id's of all new nodes
                    var newNodes1 = xmlNew.Descendants("ID").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    // find new nodes that are not present in the old collection
                    //  var nodeIdsToAdd = newNodes1.Join(oldNodes);

                    // var customer = oldNodes1.FirstOrDefault(cus => newNodes1.Contains("ShortName"));
                    xmlOld.Descendants(nodeName).ToList().ForEach(item =>
                    {
                        var currentElementId = getAttributeValue(item, "ShortName");

                    });
                    // find new nodes that are not present in the old collection
                    //var nodeIdsToAdd = newNodes.Except(oldNodes);

                    var newNodes11 = xmlOld.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    //// remove unchanged nodes
                    foreach (var oldNodeId in newNodes1)
                    {
                        var customer = newNodes11.FirstOrDefault(cus => newNodes11.Contains(oldNodeId));
                        if (customer != null)
                        {
                            xmlOld.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == oldNodeId).Remove();
                        }

                        //
                    }
                    foreach (var newNodeId1 in newNodes1)
                    {
                        var newNode = xmlNew.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == newNodeId1);
                        if (newNode != null)
                        {
                            xmlOld.Element("Products").Add(newNode);
                        }
                    }

                    xmlOld.Save(filepath + "enp_catalogue.xml");
                }

                if (File.Exists(extractPath + "\\" + fpath + "\\DELTA_miscellaneous_catalogue.xml"))
                {
                    Func<XElement, string, string> getAttributeValue = (xElement, name) => xElement.Element(name).Value;
                    Func<XElement, string, string> getAttributeValue1 = (xElement, name) => xElement.Value;

                    XDocument xmlOld = XDocument.Load(filepath + "miscellaneous_catalogue.xml");
                    XDocument xmlNew = XDocument.Load(extractPath + "\\" + fpath + "\\DELTA_miscellaneous_catalogue.xml");

                    var ModifidNodes = new List<string>();

                    var nodeName = "MiscPublications";
                    //var oldNodes = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    // id's of all new nodes

                    var sameNodes = new List<string>();

                    // sameNodes = xmlNew.Descendants("ShortName").ToList();


                    //var oldNodes1 = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    //// id's of all new nodes
                    var newNodes1 = xmlNew.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    // find new nodes that are not present in the old collection
                    //  var nodeIdsToAdd = newNodes1.Join(oldNodes);

                    // var customer = oldNodes1.FirstOrDefault(cus => newNodes1.Contains("ShortName"));
                    xmlOld.Descendants(nodeName).ToList().ForEach(item =>
                    {
                        var currentElementId = getAttributeValue(item, "ShortName");

                    });
                    // find new nodes that are not present in the old collection
                    //var nodeIdsToAdd = newNodes.Except(oldNodes);

                    var newNodes11 = xmlOld.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    //// remove unchanged nodes
                    foreach (var oldNodeId in newNodes1)
                    {
                        var customer = newNodes11.FirstOrDefault(cus => newNodes11.Contains(oldNodeId));
                        if (customer != null)
                        {
                            xmlOld.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == oldNodeId).Remove();
                        }

                        //
                    }
                    foreach (var newNodeId1 in newNodes1)
                    {
                        var newNode = xmlNew.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == newNodeId1);
                        if (newNode != null)
                        {
                            xmlOld.Element("Products").Add(newNode);
                        }
                    }

                    xmlOld.Save(filepath + "miscellaneous_catalogue.xml");
                }

                if (File.Exists(extractPath + "\\" + fpath + "\\DELTA_totaltide_catalogue.xml"))
                {
                    Func<XElement, string, string> getAttributeValue = (xElement, name) => xElement.Element(name).Value;
                    Func<XElement, string, string> getAttributeValue1 = (xElement, name) => xElement.Value;

                    XDocument xmlOld = XDocument.Load(filepath + "totaltide_catalogue.xml");
                    XDocument xmlNew = XDocument.Load(extractPath + "\\" + fpath + "\\DELTA_totaltide_catalogue.xml");

                    var ModifidNodes = new List<string>();

                    var nodeName = "TotalTide";
                    //var oldNodes = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    // id's of all new nodes

                    var sameNodes = new List<string>();

                    // sameNodes = xmlNew.Descendants("ShortName").ToList();


                    //var oldNodes1 = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    //// id's of all new nodes
                    var newNodes1 = xmlNew.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    // find new nodes that are not present in the old collection
                    //  var nodeIdsToAdd = newNodes1.Join(oldNodes);

                    // var customer = oldNodes1.FirstOrDefault(cus => newNodes1.Contains("ShortName"));
                    xmlOld.Descendants(nodeName).ToList().ForEach(item =>
                    {
                        var currentElementId = getAttributeValue(item, "ShortName");

                    });
                    // find new nodes that are not present in the old collection
                    //var nodeIdsToAdd = newNodes.Except(oldNodes);

                    var newNodes11 = xmlOld.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    //// remove unchanged nodes
                    foreach (var oldNodeId in newNodes1)
                    {
                        var customer = newNodes11.FirstOrDefault(cus => newNodes11.Contains(oldNodeId));
                        if (customer != null)
                        {
                            xmlOld.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == oldNodeId).Remove();
                        }

                        //
                    }
                    foreach (var newNodeId1 in newNodes1)
                    {
                        var newNode = xmlNew.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == newNodeId1);
                        if (newNode != null)
                        {
                            xmlOld.Element("Products").Add(newNode);
                        }
                    }

                    xmlOld.Save(filepath + "totaltide_catalogue.xml");
                }

                if (File.Exists(extractPath + "\\" + fpath + "\\DELTA_avcs_catalogue.xml"))
                {
                    Func<XElement, string, string> getAttributeValue = (xElement, name) => xElement.Element(name).Value;
                    Func<XElement, string, string> getAttributeValue1 = (xElement, name) => xElement.Value;

                    XDocument xmlOld = XDocument.Load(filepath + "avcs_catalogue.xml");
                    XDocument xmlNew = XDocument.Load(extractPath + "\\" + fpath + "\\DELTA_avcs_catalogue.xml");

                    var ModifidNodes = new List<string>();

                    var nodeName = "ENC";
                    //var oldNodes = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    // id's of all new nodes

                    var sameNodes = new List<string>();

                    // sameNodes = xmlNew.Descendants("ShortName").ToList();


                    //var oldNodes1 = xmlOld.Descendants(nodeName).Select(node => getAttributeValue(node, "ShortName")).ToList();
                    //// id's of all new nodes
                    var newNodes1 = xmlNew.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    // find new nodes that are not present in the old collection
                    //  var nodeIdsToAdd = newNodes1.Join(oldNodes);

                    // var customer = oldNodes1.FirstOrDefault(cus => newNodes1.Contains("ShortName"));
                    xmlOld.Descendants(nodeName).ToList().ForEach(item =>
                    {
                        var currentElementId = getAttributeValue(item, "ShortName");

                    });
                    // find new nodes that are not present in the old collection
                    //var nodeIdsToAdd = newNodes.Except(oldNodes);

                    var newNodes11 = xmlOld.Descendants("ShortName").Select(node => getAttributeValue1(node, "ShortName")).ToList();
                    //// remove unchanged nodes
                    foreach (var oldNodeId in newNodes1)
                    {
                        var customer = newNodes11.FirstOrDefault(cus => newNodes11.Contains(oldNodeId));
                        if (customer != null)
                        {
                            xmlOld.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == oldNodeId).Remove();
                        }

                        //
                    }
                    foreach (var newNodeId1 in newNodes1)
                    {
                        var newNode = xmlNew.Descendants(nodeName).FirstOrDefault(node => getAttributeValue(node, "ShortName") == newNodeId1);
                        if (newNode != null)
                        {
                            xmlOld.Element("Products").Add(newNode);
                        }
                    }

                    xmlOld.Save(filepath + "avcs_catalogue.xml");
                }

                System.IO.DirectoryInfo di = new DirectoryInfo(extractPath);
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
                StartStopWait();
                MessageBox.Show("Your Database is Up to Date Now");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
}
