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

        [Command("stats")] // statistiques d'un joueur
        [Description("Récupérer les statistiques générales")]
        public async Task Stats(CommandContext ctx, [Description("Nom du joueur")] string username, [Description("Plateforme de jeu : uplay,xone,ps4")] string plateforme)
        {
            await ctx.TriggerTypingAsync();

            Statistiques stats = new Statistiques();

            

            await ctx.RespondAsync("",embed:stats.GetStats(username,plateforme));
        }

        [Command("comparer")]
        [Description("Comparer deux joueurs")]

        public async Task Comparatif(CommandContext ctx, [Description("Joueur 1")] string username1, [Description("Joueur 2")] string username2, [Description("Plateforme de jeu : uplay,xone,ps4")] string plateforme)
        {
            await ctx.TriggerTypingAsync();

            Statistiques stats = new Statistiques();



            await ctx.RespondAsync("", embed: stats.GetComparatif(username1,username2,plateforme));
        }
    }
}
//stats.GetStats(username, plateforme)