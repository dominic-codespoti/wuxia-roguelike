namespace Entities.Player
{
    public static class PlayerLevelTable
    {
        public static int GetExperienceNeededForLevel(int level)
        {
            return level * 10;
        }
    }
}