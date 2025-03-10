﻿using System.Collections.Generic;

namespace Project._Scripts.Entities.Player
{
    public static class PlayerLevelTable
    {
        public static int GetExperienceNeededForLevel(int level)
        {
            return LevelToExpRequired[level];
        }

        private static readonly Dictionary<int, int> LevelToExpRequired = new()
        {
            { 1, 72 },
            { 2, 140 },
            { 3, 208 },
            { 4, 276 },
            { 5, 344 },
            { 6, 412 },
            { 7, 480 },
            { 8, 548 },
            { 9, 616 },
            { 10, 684 },
            { 11, 752 },
            { 12, 820 },
            { 13, 888 },
            { 14, 956 },
            { 15, 1024 },
            { 16, 1092 },
            { 17, 1160 },
            { 18, 1228 },
            { 19, 1296 },
            { 20, 1364 },
            { 21, 1432 },
            { 22, 1500 },
            { 23, 1568 },
            { 24, 1636 },
            { 25, 1704 },
            { 26, 1772 },
            { 27, 1840 },
            { 28, 1908 },
            { 29, 1976 },
            { 30, 2044 },
            { 31, 2112 },
            { 32, 2180 },
            { 33, 2248 },
            { 34, 2316 },
            { 35, 2384 },
            { 36, 2452 },
            { 37, 2520 },
            { 38, 2588 },
            { 39, 2656 },
            { 40, 2724 },
            { 41, 2792 },
            { 42, 2860 },
            { 43, 2928 },
            { 44, 2996 },
            { 45, 3064 },
            { 46, 3132 },
            { 47, 3200 },
            { 48, 3268 },
            { 49, 3336 },
            { 50, 3404 },
            { 51, 3472 },
            { 52, 3540 },
            { 53, 3608 },
            { 54, 3676 },
            { 55, 3744 },
            { 56, 3812 },
            { 57, 3880 },
            { 58, 3948 },
            { 59, 4016 },
            { 60, 4084 },
            { 61, 4152 },
            { 62, 4220 },
            { 63, 4288 },
            { 64, 4356 },
            { 65, 4424 },
        };
    }
}