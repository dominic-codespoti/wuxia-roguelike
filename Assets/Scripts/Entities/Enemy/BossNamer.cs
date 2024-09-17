using System;
using System.Collections.Generic;

namespace Entities.Enemy
{
    public static class BossNamer
    {
        private static readonly List<string> Names = new List<string>
        {
            "The Unseen", "Harbinger", "Grimshade", "Duskcaller", "Shadowbane", "Ebonmaw"
        };

        private static readonly List<string> Subtitles = new List<string>
        {
            "of the Void", "the Endless", "of Forgotten Souls", "of the Abyss", "the Forsaken", "of the Eclipse"
        };

        private static readonly Random Random = new Random();

        public static (string Name, string Subtitle) GetRandomBossName()
        {
            string name = Names[Random.Next(Names.Count)];
            string subtitle = Subtitles[Random.Next(Subtitles.Count)];
            return (name, subtitle);
        }
    }
}