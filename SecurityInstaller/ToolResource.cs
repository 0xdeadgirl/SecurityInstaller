using System;
using System.IO;

public class Tool {
    public Uri ToolUrl { get; private set; }

    public string ToolName { get; private set; }

    public string ToolLocation { get; private set; }

    public string ToolCliSwitch { get; private set; }

    public string ShortcutLocation { get; private set; }

    public int PercentageComplete { get; set; }

    public Tool(string url, string name, string location, string cliSwitch, string shortcutLoc = null) {
        ToolUrl = new Uri(url);
        ToolName = name;
        ToolLocation = location;
        ToolCliSwitch = cliSwitch;
        ShortcutLocation = shortcutLoc;
        PercentageComplete = 0;
    }
}

public class ToolResource {
    public Tool Malwarebytes { get; private set; } = new Tool("https://downloads.malwarebytes.com/file/mb5_offline", "MBSetup.exe", @"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe", "/verysilent /noreboot");
    public Tool Glary { get; private set; } = new Tool("https://www.glarysoft.com/aff/download.php?s=GU", "GUSetup.exe", @"C:\Program Files (x86)\Glary Utilities\OneClickMaintenance.exe", "/S", @"C:\Program Files (x86)\Glary Utilities\Integrator.exe");
    public Tool Support { get; private set; } = new Tool("https://s3-us-west-2.amazonaws.com/nerdtools/remote.msi", "Remote.msi", $@"""{Path.Combine(Directory.GetCurrentDirectory(), "Remote.msi")}""", "/qn");
    public Tool Adw { get; private set; } = new Tool("https://adwcleaner.malwarebytes.com/adwcleaner?channel=release", "ADWCleaner.exe", @"cmd.exe", $@"/k ""{Path.Combine(Directory.GetCurrentDirectory(), "ADWCleaner.exe")}"" /eula /clean /noreboot");
    public Tool bloatkiller { get; private set; } = new Tool("https://raw.githubusercontent.com/0xdeadgirl/batch-scripts/refs/heads/main/bloatkiller.bat", "bloatkiller.bat", @"cmd.exe", $@"/c ""{Path.Combine(Directory.GetCurrentDirectory(), "bloatkiller.bat")}""");
    public Tool Macrium { get; private set; } = new Tool("https://download.macrium.com/reflect/v7/v7.3.6391/reflect_setup_free_x64.exe", "MacriumReflect7_setup.exe", @"C:\Program Files\Macrium\Reflect\Reflect.exe", "/qn /norestart NODESKTOPSHORTCUT=YES");
}