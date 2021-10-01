using System;
using System.Reflection;
using TemplateMOTDPlugin.Configuration;
using TemplateMOTDPlugin.Templating;
using Terraria;
using TerrariaApi.Server;

namespace TemplateMOTDPlugin
{
    [ApiVersion(2, 1)]
    public class TemplateMOTD : TerrariaPlugin
    {
        public override string Name => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;

        public override string Description => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public override string Author => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>().Company;

        public static MOTDTemplate RawMOTD { get; private set; }

        public static string MOTD => RawMOTD.ParsedTemplate.Render(new DefaultModel());

        public TemplateMOTD(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
