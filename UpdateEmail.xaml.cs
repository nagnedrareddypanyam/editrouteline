using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for UpdateEmail.xaml
    /// </summary>
    public partial class UpdateEmail : UserControl
    {
        public UpdateEmail()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(lblEmail.Text);
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(lblSubject.Text);
        }

        private void btngen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnrepo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lbloutbox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Outbox\\";
            Process.Start(path);
        }

        private void lblinbox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Inbox\\";
            Process.Start(path);
        }
    }
}
