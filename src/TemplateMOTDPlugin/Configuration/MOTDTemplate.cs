using Scriban;
using System.IO;

namespace TemplateMOTDPlugin.Configuration
{
    /// <summary>
    /// Represents and provides methods, for reading and evaluating template files.
    /// </summary>
    public class MOTDTemplate
    {
        private readonly string _motdPath;
        private readonly bool _isLiquidTemplate;

        /// <summary>
        /// Represents whether or not the template has initialized
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Represents the path to the MOTD file.
        /// </summary>
        public string MOTDPath => _motdPath;

        /// <summary>
        /// Represents whether the template is parsed as a Liquid template. <see langword="true"/> if it's parsed as a Liquid template, otherwise <see langword="false"/>.
        /// </summary>
        public bool IsLiquidTemplate => _isLiquidTemplate;

        /// <summary>
        /// Represents the parsed, evaluable template.
        /// </summary>
        public Template ParsedTemplate { get; private set; }

        /// <summary>
        /// Initializes a new proto-template with only metadata.
        /// </summary>
        /// <param name="motdPath"><inheritdoc cref="MOTDPath" path="/summary"/></param>
        /// <param name="isLiquidTemplate"><inheritdoc cref="IsLiquidTemplate" path="/summary"/></param>
        /// <param name="initialize"><see langword="true"/> to initialize the template in constructor, otherwise <see langword="false"/>.</param>
        public MOTDTemplate(string motdPath, bool isLiquidTemplate = false, bool initialize = true)
        {
            _motdPath = motdPath;
            _isLiquidTemplate = isLiquidTemplate;

            if (initialize)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Initializes the template.
        /// </summary>
        /// <returns><see langword="true"/> if successfully initialized, otherwise <see langword="false"/>.</returns>
        public bool Initialize()
        {
            if (IsInitialized)
            {
                return false;
            }

            if (File.Exists(_motdPath))
            {
                var rawTemplate = File.ReadAllText(_motdPath);
                ParsedTemplate = IsLiquidTemplate
                    ? Template.ParseLiquid(rawTemplate)
                    : Template.Parse(rawTemplate);
                IsInitialized = true;
                return true;
            }
            return false;
        }
    }
}
