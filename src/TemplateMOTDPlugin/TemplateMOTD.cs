using Microsoft.Xna.Framework;
using System;
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
            RawMOTD = new MOTDTemplate(Paths.MOTDPath);

            ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreetPlayer);
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
