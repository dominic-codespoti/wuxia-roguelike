using System.Collections.Generic;

public static class PlayerLevelStages
{
    public static readonly Dictionary<CultivationRealm, int> RealmExperienceThresholds = new Dictionary<CultivationRealm, int>
    {
        { CultivationRealm.BodyRefinement, 100 },
        { CultivationRealm.QiCondensation, 200 },
        { CultivationRealm.FoundationBuilding, 400 },
        { CultivationRealm.CoreFormation, 800 },
        { CultivationRealm.NascentSoul, 1600 },
        { CultivationRealm.SpiritTransformation, 3200 },
        { CultivationRealm.ImmortalAscension, 6400 }
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
