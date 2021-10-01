using System.IO;

namespace TemplateMOTDPlugin.Configuration
{
    /// <summary>
    /// Provides constant paths that point to important folders or files.
    /// </summary>
    public static class Paths
    {
        /// <summary>
        /// Represents the shared folder where plugins store their configs.
        /// </summary>
        public static readonly string GlobalConfigPath = Path.GetFullPath("config");

        /// <summary>
        /// Represents the folder where this plugin stores its config.
        /// </summary>
        public static readonly string SavePath = Path.Combine(GlobalConfigPath, "templatemotd");

        /// <summary>
        /// Represents the path to the MOTD template.
        /// </summary>
        public static readonly string MOTDPath = Path.Combine(SavePath, "motd.txt");
    }
}
