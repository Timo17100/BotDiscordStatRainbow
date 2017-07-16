using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using DSharpPlus;
using System.Drawing;
using System.Drawing.Printing;
namespace BotDiscordStatRainbow
{
    class Statistiques
    {
        public DiscordEmbed GetStats(string username,string platform)
        {
            var client = new RestSharp.RestClient("https://api.r6stats.com");
            var session = new RestRequest("/api/v1/players/"+username, Method.GET);
            session.AddQueryParameter("platform", platform);
            var reponse = client.Execute(session);
            if (reponse.Content.Contains("failed"))
            {
                var erreur = JsonConvert.DeserializeObject<failRoot>(reponse.Content);
                var embed = new DiscordEmbed
                {
                    Title = "Erreur " + erreur.errors[0].code,
                    Description = "Le bot a rencontré une erreur : test "+ erreur.errors[0].detail,
                    Color = 0x96211D,
                };
                return embed;
            }
            else {
                var stats = JsonConvert.DeserializeObject<StatsRoot>(reponse.Content);
                DateTime d2 = DateTime.SpecifyKind(DateTime.Parse(stats.player.updated_at, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTimeKind.Utc);
                DateTime dt = d2.ToLocalTime();
                var headshotsRatio = Math.Round(((float)stats.player.stats.overall.headshots / ((float)stats.player.stats.ranked.kills + (float)stats.player.stats.casual.kills)),3);
                var precision = Math.Round(((float)stats.player.stats.overall.bullets_hit / (float)stats.player.stats.overall.bullets_fired),3);
                var distance = Math.Round(((stats.player.stats.overall.steps_moved / 1.31233595801) / 1000),2);
                var embed = new DiscordEmbed
                {
                    Title = "R6Stats",
                    Description = "Statistiques de " + stats.player.username,
                    Color = 0x1B1E4F,
                    Fields = new List<DiscordEmbedField>()
                    {
                        new DiscordEmbedField
                        {
                            Name = "Ranked",
                            Value = "__w/l Ratio__ : " + "*" + stats.player.stats.ranked.wlr + "*" + "   **||**   " + "__k/d Ratio__ : " + "*" + stats.player.stats.ranked.kd + "*",
                            Inline = false
                        },

                        new DiscordEmbedField
                        {
                            Name = "Casual",
                            Value = "__w/l Ratio__ : " + "*" + stats.player.stats.casual.wlr + "*" + "   **||**   " + "__k/d Ratio__ : " + "*" + stats.player.stats.casual.kd + "*",
                            Inline = false
                        },

                        new DiscordEmbedField
                        {
                            Name = "Général",
                            Value = "__HeadShots Ratio__ : " + "*" + headshotsRatio + "*" + "   **||**   " + "__Précision__ : " + "*" + precision + "*" + "   **||**   " + "__Distance__ : " + "*" + distance + "km*",
                            Inline = false
                        }
                    },
                    Footer = new DiscordEmbedFooter
                    {
                        IconUrl = "https://cdn4.iconfinder.com/data/icons/tupix-1/30/graph-512.png",
                        Text = "Derniere mise à jour : " + dt

                    }
                };
               
                return embed;
            }
            
        }
    }

    public class failRoot
    {
        public string status { get; set; }

        public List<failErrorsJSON> errors { get; set; }
    }

    public class failErrorsJSON
    {
        public string detail { get; set; }

        public int code { get; set; }
    }

    public class Ranked
    {
        public bool has_played { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public double wlr { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public double kd { get; set; }
        public int playtime { get; set; }
    }

    public class Casual
    {
        public bool has_played { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public double wlr { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public double kd { get; set; }
        public double playtime { get; set; }
    }

    public class Overall
    {
        public int revives { get; set; }
        public int suicides { get; set; }
        public int reinforcements_deployed { get; set; }
        public int barricades_built { get; set; }
        public int steps_moved { get; set; }
        public int bullets_fired { get; set; }
        public int bullets_hit { get; set; }
        public int headshots { get; set; }
        public int melee_kills { get; set; }
        public int penetration_kills { get; set; }
        public int assists { get; set; }
    }

    public class Progression
    {
        public int level { get; set; }
        public int xp { get; set; }
    }

    public class Stats
    {
        public Ranked ranked { get; set; }
        public Casual casual { get; set; }
        public Overall overall { get; set; }
        public Progression progression { get; set; }
    }

    public class Player
    {
        public string username { get; set; }
        public string platform { get; set; }
        public string ubisoft_id { get; set; }
        public string indexed_at { get; set; }
        public string updated_at { get; set; }
        public Stats stats { get; set; }
    }

    public class StatsRoot
    {
        public Player player { get; set; }
    }
}
