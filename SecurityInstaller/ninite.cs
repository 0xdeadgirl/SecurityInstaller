using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/**
 * For the "Ninite Installers" tab.
 * 
 * We'll populate the tab with checkboxes for every executable found in a local "ninite" folder.
 * They can be run by just clicking the 'Start' button. It's independent of the 'Download,'
 * 'Install,' and 'Run' checkboxes.
 */
public static class Ninite {

    // We need to associate the checkboxes with the paths to the executables.
    public struct ninite {
        public CheckBox box;
        public String path;

        public ninite(CheckBox box,String path) {
            this.box = box;
            this.path = path;
        }
    }

    public static List<ninite> ninite_installers = new List<ninite>();

    /**
     * Searches for and in a local folder named 'ninite', and for every executable it finds,
     * it creates a checkbox in the 'Ninite Installers' tab.
     */
    public static void FindNiniteInstallers(StackPanel parent) {
        string niniteFolder = Path.Combine(Directory.GetCurrentDirectory(),"ninite");

        if(!Directory.Exists(niniteFolder))
            return;

        foreach(string file in Directory.EnumerateFiles(niniteFolder)) {
            if(file.EndsWith(".exe")) {
                // This is the actual CheckBox XAML element
                CheckBox installerCB = new CheckBox {
                    Content = Path.GetFileNameWithoutExtension(file),
                    Foreground = (SolidColorBrush) new BrushConverter().ConvertFrom("#E8E9ED"),
                    Margin = (Thickness) new ThicknessConverter().ConvertFromString("20,5,0,5"),
                    FontWeight = FontWeights.SemiBold
                };

                // See 'Ninite' structure below, which tracks the CheckBox and executable path
                ninite installer = new ninite(installerCB, file);

                parent.Children.Add(installerCB);
                ninite_installers.Add(installer);
            }
        }
    }

    /**
     * This just spawns a new process and attempts to run whatever path is passed to it.
     */
    public static async Task<bool> RunNinite(string path, IProgress<string> results) {
        try {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo { FileName = path };
            process.Start();
            results.Report($"Launched: {Path.GetFileName(path)}");

            return true;
        } catch(Exception ex) {
            results.Report($"\nError Opening:{path}\n{ex.Message}");
            return false;
        }
    }
}