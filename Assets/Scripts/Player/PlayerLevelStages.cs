using System.Collections.Generic;

namespace Player
{
    public static class PlayerLevelStages
    {
        public static readonly Dictionary<CultivationRealm, int> RealmExperienceThresholds = new Dictionary<CultivationRealm, int>
        {
            { CultivationRealm.BodyRefinement, 100 },
            { CultivationRealm.QiCondensation, 500 },
            { CultivationRealm.FoundationBuilding, 1000 },
            { CultivationRealm.CoreFormation, 2500 },
            { CultivationRealm.NascentSoul, 5000 },
            { CultivationRealm.SpiritTransformation, 10000 },
            { CultivationRealm.ImmortalAscension, 100000 }
        };

        public static readonly Dictionary<CulitvationStage, int> StageExperienceThresholds = new Dictionary<CulitvationStage, int>
        {
            { CulitvationStage.First, 50 },
            { CulitvationStage.Second, 100 },
            { CulitvationStage.Third, 150 }
        };
    }

    public enum CultivationRealm
    {
        BodyRefinement,
        QiCondensation,
        FoundationBuilding,
        CoreFormation,
        NascentSoul,
        SpiritTransformation,
        ImmortalAscension
    }

    public enum CulitvationStage
    {
        First,
        Second,
        Third
    }
}