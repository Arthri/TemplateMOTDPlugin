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
using TShockAPI.Hooks;

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
            // Create important folders
            Directory.CreateDirectory(Paths.SavePath);


            // Create default MOTD if one doesn't exist
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


            LoadConfig();


            // Attach hooks
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreetPlayer, int.MinValue);
            GeneralHooks.ReloadEvent += OnReload;


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

        public void LoadConfig()
        {
            // Check if TShock's MOTD file is empty, if not, move it
            var tshockMOTDPath = Path.Combine(TShock.SavePath, "motd.txt");
            FileInfo tshockMOTDFile;
            try
            {
                tshockMOTDFile = new FileInfo(tshockMOTDPath);
            }
            catch (Exception e)
            {
                TShock.Log.Error($"Aborted TShock MOTD check:\n{e}");
                goto PostTShockMOTDCheck;
            }

            if (tshockMOTDFile.Length > 0)
            {
                try
                {
                    var fileCreationTimestamp = ((DateTimeOffset)tshockMOTDFile.CreationTimeUtc).ToUnixTimeMilliseconds();
                    var destFile = $"motd.old.tshock.{fileCreationTimestamp}.txt";
                    var dest = Path.Combine(Paths.SavePath, destFile);

                    // Backup old MOTD
                    tshockMOTDFile.MoveTo(dest);

                    // Create an empty file
                    using (File.CreateText(tshockMOTDPath)) { }

                    TShock.Log.Info($"Moved and replaced TShock MOTD file. Destination: {dest}");
                }
                catch (Exception e)
                {
                    TShock.Log.Error($"TShock MOTD backup aborted:\n{e}");
                }
            }

        PostTShockMOTDCheck:
            RawMOTD = new MOTDTemplate(Paths.MOTDPath);
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
            var tsplayer = TShock.Players[e.Who];

            if (tsplayer == null)
            {
                var netPlayer = Netplay.Clients[e.Who];
                TShock.Log.Error($"Unable to send message to {netPlayer.Socket.GetRemoteAddress()}(joined as {netPlayer.Name}): TShock's equivalent player is null");
                return;
            }

            tsplayer.SendMessage(MOTD, Color.White);
        }

        private void OnReload(ReloadEventArgs e)
        {
            LoadConfig();
        }
    }
}
