using UnityEngine;
using System;

public static class PlayerEvents
{
    public static event Action<Player> OnPlayerLevelUp;

    public static void PlayerLeveledUp(Player player)
    {
        OnPlayerLevelUp?.Invoke(player);
    }
}
