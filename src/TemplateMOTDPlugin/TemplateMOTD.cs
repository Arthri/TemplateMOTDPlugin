using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TemplateMOTDPlugin.Configuration;
using TemplateMOTDPlugin.Templating;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

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
            // Create all important folders
            Directory.CreateDirectory(Paths.SavePath);

            var motdPath = Paths.MOTDPath;

            if (!File.Exists(motdPath))
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream($"{nameof(TemplateMOTDPlugin)}.{nameof(Resources)}.DefaultMOTD.txt"))
                {
                    using (var file = new FileStream(motdPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    {
                        stream.CopyTo(file);
                    }
                }
            }

            RawMOTD = new MOTDTemplate(motdPath);

            ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreetPlayer);

            // Find TShock's MOTD command and replace it
            var motdCommand = Commands.TShockCommands.FirstOrDefault(c => c.HasAlias("motd"));

            if (motdCommand == null)
            {
                TShock.Log.Error("Command replacement aborted: TShock's MOTD command was not found");
            }
            else
            {
                motdCommand.CommandDelegate = (e) => e.Player.SendMessage(MOTD, Color.White);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, OnGreetPlayer);
            }
            base.Dispose(disposing);
        }

        private void OnGreetPlayer(GreetPlayerEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            var tsplayer = TShock.Players[e.Who];

            if (tsplayer == null)
            {
                var netPlayer = Netplay.Clients[e.Who];
                TShock.Log.Error($"Unable to send message to {netPlayer.Socket.GetRemoteAddress()}(joined as {netPlayer.Name}): TShock's equivalent player is null");
                return;
            }

            tsplayer.SendMessage(MOTD, Color.White);
        }
    }
}
