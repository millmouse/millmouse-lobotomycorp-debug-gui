using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMod.Util.data
{
    public class StatStats
    {
        public RwbpType RwbpType { get; set; }
        public string StatName { get; set; }
        public float StatValue { get; set; }
        public int CurrentStatLevel { get; set; }
        public int PrimaryValue { get; set; }
        public int PrimaryWithExpModifier { get; set; }
        public int NextLevel { get; set; }
        public int MinExpForNextLevel { get; set; }

        public StatStats(RwbpType rwbpType, string statName, float statValue, int currentStatLevel, int primaryValue, int primaryWithExpModifier, int nextLevel, int minExpForNextLevel)
        {
            RwbpType = rwbpType;
            StatName = statName;
            StatValue = statValue;
            CurrentStatLevel = currentStatLevel;
            PrimaryValue = primaryValue;
            PrimaryWithExpModifier = primaryWithExpModifier;
            NextLevel = nextLevel;
            MinExpForNextLevel = minExpForNextLevel;
        }
    }
}
