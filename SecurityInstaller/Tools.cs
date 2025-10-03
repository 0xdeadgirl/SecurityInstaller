﻿using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

public class Tools 
{
    // Start DISM/SFC
    public static bool FileChecker() {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.UseShellExecute = true;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "pause | /k dism /online /cleanup-image /restorehealth&sfc /scannow";
        process.StartInfo = startInfo;
        process.Start();

        return true;
    }

    // Start Updates to third party applications
    public static bool ThirdPartyUpdater() {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.UseShellExecute = true;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "pause | /k winget upgrade --all";
        process.StartInfo = startInfo;
        process.Start();

        return true;
    }

    // Create Shortcuts
    private static void Shortcut(string shortcutName, string targetFileLocation) {
        // Initialize shortcuts
        string shortcutLocation = Path.Combine(@"C:\Users\Public\Desktop\Nerds On Call 800-919-6373", shortcutName + ".lnk");
        WshShell shell = new WshShell();
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

        shortcut.TargetPath = targetFileLocation; // The path of the file that will launch when the shortcut is run
        shortcut.Save();
    }

    /*
     * Create shortcut for Calling Card in NOC folder.
     * 
     * If there's a Windows Installer process running "Remote.msi," we will wait until that's finished executing, then we will attempt to
     * create the Calling Card shortcut in the NOC folder.
     * 
     * @author Lukas Lynch
     */
    public static void MakeSupportShortcut() {
        const string dir = @"C:\Users\Public\Desktop\Nerds On Call 800-919-6373";

        // Since there are multiple possible channel IDs, we'll iterate through them. We need to break up the directory and executable since
        // we need to combine them with the ID in the middle.
        const string callingCardDirectory = @"C:\Program Files (x86)\LogMeIn Rescue Calling Card\";
        const string callingCardExecutable = @"\CallingCard.exe";
        string[] channelIds = { "6gqmpb", "eost6i", "58pq3u" };

        // No point in continuing if the NOC Folder isn't present.
        if (!Directory.Exists(dir))
            return;

        // We check for all Windows Installer processes, and if they're running on "Remote.msi" (as indicated by its CommandLine arguments),
        // we wait for it to exit, then break out of the loop and attempt to create the shortcut in the NOC folder.
        foreach (Process process in Process.GetProcessesByName("msiexec")) {
            using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id)) {
                var managementObject = searcher.Get().Cast<System.Management.ManagementObject>().FirstOrDefault();
                if (managementObject != null && managementObject["CommandLine"]?.ToString().Contains("Remote.msi") == true) {
                    process.WaitForExit();
                    break;
                }
            }
        }

        foreach (string id in channelIds) {
            string path = callingCardDirectory + id + callingCardExecutable;
            if (System.IO.File.Exists(path)) {
                Shortcut("Nerds On Call Support", path);
                break;
            }
        }
    }

    // Make NOC Folder
    public static async Task<bool> MakeNOC(Tool Mb, Tool Cc, Tool Gl) {
        const string dir = @"C:\Users\Public\Desktop\Nerds On Call 800-919-6373";
        const string oldDir = @"C:\Users\Public\Desktop\Nerds On Call 800-919NERD";

        // If directory does not exist, create it
        if (Directory.Exists(oldDir))
            Directory.Move(oldDir, dir);
        else if (!Directory.Exists(dir)) {
            DirectoryInfo folder = Directory.CreateDirectory(dir);

                // Create desktop.ini file
                string deskIni = @"C:\Users\Public\Desktop\Nerds On Call 800-919-6373\desktop.ini";
            using (StreamWriter sw = new StreamWriter(deskIni)) {
                sw.WriteLine("[.ShellClassInfo]");
                sw.WriteLine("ConfirmFileOp=0");
                sw.WriteLine("IconFile=nerd.ico");
                sw.WriteLine("IconIndex=0");
                sw.WriteLine("InfoTip=Contains the Nerds On Call Security Suite");
                sw.Close();
            }

            string place = @"C:\Users\Public\Desktop\Nerds On Call 800-919-6373\nerd.ico";
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("/nerd.ico", UriKind.Relative));

            if (sri != null) {
                using (Stream stream = sri.Stream) {
                    using (var file = System.IO.File.Create(place)) {
                        await stream.CopyToAsync(file);
                    }
                }
            }


            // Copy Nerds icon then set Attributes
            System.IO.File.SetAttributes(place, FileAttributes.Hidden);

            // Hide icon and desktop.ini then set folder as a system folder
            System.IO.File.SetAttributes(deskIni, FileAttributes.Hidden);
            folder.Attributes |= FileAttributes.System;
            folder.Attributes |= FileAttributes.ReadOnly;
            folder.Attributes |= FileAttributes.Directory;
        }

        // Create Shortcutsss

        // MB Shortcut
        if (System.IO.File.Exists(Mb.ToolLocation))
            Shortcut("Malwarebytes", Mb.ToolLocation);

        // CC Shortcut
        if (System.IO.File.Exists(Cc.ToolLocation))
            Shortcut("CCleaner", Cc.ToolLocation);

        // GU Shortcut
        if (System.IO.File.Exists(Gl.ShortcutLocation))
            Shortcut("Glary Utilities", Gl.ShortcutLocation);

        // ADW Shortcut
        if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ADWCleaner.exe")))
            Shortcut("ADWCleaner", Path.Combine(Directory.GetCurrentDirectory(), "ADWCleaner.exe"));

        // Calling card Shortcut
        MakeSupportShortcut();

        return true;
    }

    // Adds Registry keys so next time Chrome or Edge opens, it updates or asks to install UBlock Origin
    public static async Task<bool> InstallUB() {
        string valueName = "update_url";

        var GC = await Task.Run<bool>(() => {
            // Write to Google Chrome
            string value = "https://clients2.google.com/service/update2/crx";
            string key = @"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Google\Chrome\Extensions\ddkjiahejlhfcafbddmgiahcphecmpfh";
            Registry.SetValue(key, valueName, value, RegistryValueKind.String);
            return true;
        });

        var edge = await Task.Run<bool>(() => {
            /// Write to MS Edge
            string eValue = "https://edge.microsoft.com/extensionwebstorebase/v1/crx";
            string eKey = @"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Microsoft\Edge\Extensions\cimighlppcgcoapaliogpjjdehbnofhn";
            Registry.SetValue(eKey, valueName, eValue, RegistryValueKind.String);
            return true;
        });

        return true;
    }
}