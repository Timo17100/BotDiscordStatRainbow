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
        #region Statistique 1 joueur
        public DiscordEmbed GetStats(string username, string platform)
        {
            var client = new RestSharp.RestClient("https://api.r6stats.com");
            var session = new RestRequest("/api/v1/players/" + username, Method.GET);
            session.AddQueryParameter("platform", platform);
            var reponse = client.Execute(session);
            if (reponse.Content.Contains("failed"))
            {
                var erreur = JsonConvert.DeserializeObject<failRoot>(reponse.Content);
                var embed = new DiscordEmbed
                {
                    Title = "Erreur " + erreur.errors[0].code,
                    Description = "Le bot a rencontré une erreur : " + erreur.errors[0].detail,
                    Color = 0x96211D
                };
                return embed;
            }
            else
            {
                var stats = JsonConvert.DeserializeObject<StatsRoot>(reponse.Content);
                DateTime d2 = DateTime.SpecifyKind(DateTime.Parse(stats.player.updated_at, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTimeKind.Utc);
                DateTime dt = d2.ToLocalTime();
                var headshotsRatio = Math.Round(((float)stats.player.stats.overall.headshots / ((float)stats.player.stats.ranked.kills + (float)stats.player.stats.casual.kills)), 3);
                var precision = Math.Round(((float)stats.player.stats.overall.bullets_hit / (float)stats.player.stats.overall.bullets_fired), 3);
                var distance = Math.Round(((stats.player.stats.overall.steps_moved / 1.31233595801) / 1000), 2);
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
        #endregion
        public DiscordEmbed GetComparatif(string username1, string username2, string platform)
        {
            var client = new RestSharp.RestClient("https://api.r6stats.com");
            var sessionUser1 = new RestRequest("/api/v1/players/" + username1, Method.GET);
            sessionUser1.AddQueryParameter("platform", platform);
            var sessionUser2 = new RestRequest("/api/v1/players/" + username2, Method.GET);
            sessionUser2.AddQueryParameter("platform", platform);
            var reponseUser1 = client.Execute(sessionUser1);
            var reponseUser2 = client.Execute(sessionUser2);

            if (reponseUser1.Content.Contains("failed"))
            {
                var erreurUser1 = JsonConvert.DeserializeObject<failRoot>(reponseUser1.Content);
                var embed = new DiscordEmbed
                {
                    Title = "Erreur" + erreurUser1.errors[0].code,
                    Description = "Le bot a rencontré une erreur : " + erreurUser1.errors[0].detail,
                    Color = 0x96211D
                };
                return embed;
            }
            else if (reponseUser2.Content.Contains("failed"))
            {
                var erreurUser2 = JsonConvert.DeserializeObject<failRoot>(reponseUser2.Content);
                var embed = new DiscordEmbed
                {
                    Title = "Erreur" + erreurUser2.errors[0].code,
                    Description = "Le bot a rencontré une erreur : " + erreurUser2.errors[0].detail,
                    Color = 0x96211D
                };
                return embed;
            }
            else
            {
                var statsUser1 = JsonConvert.DeserializeObject<StatsRoot>(reponseUser1.Content);
                var statsUser2 = JsonConvert.DeserializeObject<StatsRoot>(reponseUser2.Content);

                var headshotsRatioUser1 = Math.Round(((float)statsUser1.player.stats.overall.headshots / ((float)statsUser1.player.stats.ranked.kills + (float)statsUser1.player.stats.casual.kills)), 3);
                var precisionUser1 = Math.Round(((float)statsUser1.player.stats.overall.bullets_hit / (float)statsUser1.player.stats.overall.bullets_fired), 3);
                var distanceUser1 = Math.Round(((statsUser1.player.stats.overall.steps_moved / 1.31233595801) / 1000), 2);

                var headshotsRatioUser2 = Math.Round(((float)statsUser2.player.stats.overall.headshots / ((float)statsUser2.player.stats.ranked.kills + (float)statsUser2.player.stats.casual.kills)), 3);
                var precisionUser2 = Math.Round(((float)statsUser2.player.stats.overall.bullets_hit / (float)statsUser2.player.stats.overall.bullets_fired), 3);
                var distanceUser2 = Math.Round(((statsUser2.player.stats.overall.steps_moved / 1.31233595801) / 1000), 2);

                string wlrRanked = "";
                if (statsUser1.player.stats.ranked.wlr == statsUser2.player.stats.ranked.wlr) { wlrRanked = statsUser1.player.username + "(" + "*" + statsUser1.player.stats.ranked.wlr + "*" + ")" + " **=** " + statsUser2.player.username + "(" + "*" + statsUser2.player.stats.ranked.wlr + "*" + ")"; };
                if (statsUser1.player.stats.ranked.wlr > statsUser2.player.stats.ranked.wlr) { wlrRanked = statsUser1.player.username + "(" + "*" + statsUser1.player.stats.ranked.wlr + "*" + ")" + " **>** " + statsUser2.player.username + "(" + "*" + statsUser2.player.stats.ranked.wlr + "*" + ")"; };
                if (statsUser1.player.stats.ranked.wlr < statsUser2.player.stats.ranked.wlr) { wlrRanked = statsUser1.player.username + "(" + "*" + statsUser1.player.stats.ranked.wlr + "*" + ")" + " **<** " + statsUser2.player.username + "(" + "*" + statsUser2.player.stats.ranked.wlr + "*" + ")"; };

                string kdRanked = "";
                if (statsUser1.player.stats.ranked.kd == statsUser2.player.stats.ranked.kd) { kdRanked = statsUser1.player.username + "(" + "*" + statsUser1.player.stats.ranked.kd + "*" + ")" + " **=** " + statsUser2.player.username + "(" + "*" + statsUser2.player.stats.ranked.kd + "*" + ")"; };
                if (statsUser1.player.stats.ranked.kd > statsUser2.player.stats.ranked.kd) { kdRanked = statsUser1.player.username + "(" + "*" + statsUser1.player.stats.ranked.kd + "*" + ")" + " **>** " + statsUser2.player.username + "(" + "*" + statsUser2.player.stats.ranked.kd + "*" + ")"; };
                if (statsUser1.player.stats.ranked.kd < statsUser2.player.stats.ranked.kd) { kdRanked = statsUser1.player.username + "(" + "*" + statsUser1.player.stats.ranked.kd + "*" + ")" + " **<** " + statsUser2.player.username + "(" + "*" + statsUser2.player.stats.ranked.kd + "*" + ")"; };

                var embed = new DiscordEmbed
                {
                    Title = "R6Stats",
                    Description = "Comparatif entre " + statsUser1.player.username + " et " + statsUser2.player.username,
                    Color = 0xB6B623,
                    Fields = new List<DiscordEmbedField>()
                    {
                        new DiscordEmbedField
                        {
                            Name = "Ranked",
                            Value = "__w/l Ratio__ : " + wlrRanked + "   **||**   " + "__k/d Ratio__ : " + kdRanked,
                            Inline = false
                        },
                    }

                };
                return embed;
            }
        }
        #region JSONParser
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
        #endregion
    }
}
