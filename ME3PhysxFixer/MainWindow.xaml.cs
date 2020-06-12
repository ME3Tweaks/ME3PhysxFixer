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

namespace ME3PhysxFixer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ModManager_Clicked(object sender, RoutedEventArgs e)
        {
            OpenWebpage("https://me3tweaks.com/modmanager");
        }

        public static void OpenWebpage(string uri)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = uri,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception e)
            {
            }
        }

        private void ALOT_Clicked(object sender, RoutedEventArgs e)
        {
            OpenWebpage("https://www.nexusmods.com/masseffect3/mods/363");
        }

        private void Discord_Clicked(object sender, RoutedEventArgs e)
        {
            OpenWebpage("https://discord.gg/s8HA6dc");
        }

        private void MER_Clicked(object sender, RoutedEventArgs e)
        {
            OpenWebpage("https://me3tweaks.com/masseffectrandomizer");
        }
    }
}
