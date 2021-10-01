using System.Linq;
using Terraria;
using TShockAPI;
using TShockAPI.Configuration;

namespace TemplateMOTDPlugin.Templating
{
    /// <summary>
    /// Provides default variables for a MOTD template
    /// </summary>
    public class DefaultModel
    {
        #region TShock Default

        /// <inheritdoc cref="ServerName"/>
        public string Map => ServerName;

        /// <summary>
        /// Represents a comma-separated list of online players
        /// </summary>
        public virtual string Players => string.Join(
            ", ",
            Main.player.Where(p => p.active)
                       .Select(p => p.name));

        /// <inheritdoc cref="CommandSpecifier"/>
        public string Specifier => CommandSpecifier;

        /// <summary>
        /// Represents the number of online players.
        /// </summary>
        public virtual int OnlinePlayers => Main.player.Count(p => p.active);

        /// <inheritdoc cref="TShockSettings.MaxSlots"/>
        public virtual int ServerSlots => TShock.Config.Settings.MaxSlots;

        #endregion

        /// <summary>
        /// Represents the display name of the server
        /// </summary>
        public virtual string ServerName =>
            TShock.Config.Settings.UseServerName
                ? TShock.Config.Settings.ServerName
                : Main.worldName;

        /// <summary>
        /// Represents an array of TShock players.
        /// </summary>
        public virtual TSPlayer[] PlayersArray => TShock.Players;

        /// <inheritdoc cref="TShockSettings.CommandSpecifier"/>
        public virtual string CommandSpecifier => TShock.Config.Settings.CommandSpecifier;

        /// <inheritdoc cref="TShockSettings.CommandSilentSpecifier"/>
        public virtual string CommandSilentSpecifier => TShock.Config.Settings.CommandSilentSpecifier;
    }
}
