using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Minish Randomizer")]
[assembly: AssemblyDescription("Randomizer for The Legend of Zelda: The Minish Cap")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Team Minish Maker")]
[assembly: AssemblyProduct("Minish Randomizer")]
[assembly: AssemblyCopyright("Copyright ©  2019 Team MinishMaker")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("704dad64-9c28-458b-8980-a945c1159ffa")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.6.0")]
[assembly: AssemblyFileVersion("0.6.0")]

namespace RandomizerCore.Properties;

public static class AssemblyInfo
{
    /// <summary>
    ///     Gets the git hash value from the assembly
    ///     or null if it cannot be found.
    /// </summary>
    public static string GetGitHash()
    {
        var asm = typeof(AssemblyInfo).Assembly;
        var attrs = asm.GetCustomAttributes<AssemblyMetadataAttribute>();
        return attrs.FirstOrDefault(a => a.Key == "GitHash")?.Value;
    }

    public static string GetGitTag()
    {
        var asm = typeof(AssemblyInfo).Assembly;
        var attrs = asm.GetCustomAttributes<AssemblyMetadataAttribute>();
        return attrs.FirstOrDefault(a => a.Key == "GitTag")?.Value;
    }
}
