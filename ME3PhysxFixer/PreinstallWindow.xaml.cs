using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Path = System.Windows.Shapes.Path;

namespace ME3PhysxFixer
{
    /// <summary>
    /// Interaction logic for PreinstallWindow.xaml
    /// </summary>
    public partial class PreinstallWindow : Window
    {
        public PreinstallWindow()
        {
            InitializeComponent();
        }

        private void Install_Click(object sender, RoutedEventArgs e)
        {
            //Install

            // Get game EXE
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select game executable";
            string filter = $@"Mass Effect 3 Executable|MassEffect3.exe"; //only partially localizable.
            ofd.Filter = filter;
            if (ofd.ShowDialog() == true)
            {
                var licstream = ExtractInternalFileToStream("ME3PhysxFixer.physx.physx_license.txt");
                StreamReader reader = new StreamReader(licstream);
                string lictext = reader.ReadToEnd();
                var result = MessageBox.Show(this, "You must accept the following license to install these files:\n\n" + lictext, "PhysX license", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (result == MessageBoxResult.Yes)
                {
                    var exedir = Directory.GetParent(ofd.FileName).FullName;
                    string[] files = { "cudart32_41_4.dll", "PhysXCooking.dll", "PhysXCore.dll", "PhysXDevice.dll", "PhysXLoader.dll" };
                    foreach (var f in files)
                    {
                        var source = "ME3PhysxFixer.physx." + f;
                        var destpath = System.IO.Path.Combine(exedir, f);
                        ExtractInternalFile(source, destpath, true);
                    }
                    //done. Show dialog
                    new MainWindow().Show();
                    Close();
                    return;
                }

            }
            Environment.Exit(0);
        }

        public static Stream GetResourceStream(string assemblyResource)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var res = assembly.GetManifestResourceNames();
            return assembly.GetManifestResourceStream(assemblyResource);
        }

        internal static string ExtractInternalFile(string internalResourceName, string destination, bool overwrite)
        {
            Debug.WriteLine("Extracting embedded file: " + internalResourceName + " to " + destination);
#if DEBUG
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
#endif
            if (!File.Exists(destination) || overwrite || new FileInfo(destination).Length == 0)
            {

                using (Stream stream = GetResourceStream(internalResourceName))
                {
                    if (File.Exists(destination))
                    {
                        FileInfo fi = new FileInfo(destination);
                        if (fi.IsReadOnly)
                        {
                            fi.IsReadOnly = false; //clear read only. might happen on some binkw32 in archives, maybe
                        }
                    }
                    using (var file = new FileStream(destination, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(file);
                    }
                }
            }
            else
            {
                Debug.WriteLine("File already exists. Not overwriting file.");
            }
            return destination;
        }

        internal static MemoryStream ExtractInternalFileToStream(string internalResourceName)
        {
            Debug.WriteLine("Extracting embedded file: " + internalResourceName + " to memory");
#if DEBUG
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
#endif


            using (Stream stream = GetResourceStream(internalResourceName))
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Position = 0;
                return ms;
            }
        }
    }
}
