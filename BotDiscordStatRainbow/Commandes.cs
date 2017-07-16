using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BotDiscordStatRainbow
{
    public class Commandes
    {
        [Command("issou")] // let's define this method as a command
        [Description("mdr")] // this will be displayed to tell users what this command does when they invoke help
        public async Task Issou(CommandContext ctx) // this command takes no arguments
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            // let's make the message a bit more colourful
            var emoji = DiscordEmoji.FromName(ctx.Client, ":dongle:");

            // respond with ping
            await ctx.RespondAsync($"{emoji} lol");
        }

        [Command("stats")] // let's define this method as a command
        [Description("Récupérer les statistiques générales")] // this will be displayed to tell users what this command does when they invoke help
        public async Task Stats(CommandContext ctx, [Description("Nom du joueur")] string username, [Description("Plateforme de jeu : uplay,xone,ps4")] string plateforme)
        {
            await ctx.TriggerTypingAsync();

            Statistiques stats = new Statistiques();

            

            await ctx.RespondAsync("",embed:stats.GetStats(username,plateforme));
        }

    }
}
//stats.GetStats(username, plateforme)