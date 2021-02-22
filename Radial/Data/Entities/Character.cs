﻿using Radial.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Radial.Data.Entities
{
    public class Character
    {
        private static readonly long _lowestStatValue = 10;

        public Guid Id { get; init; } = Guid.NewGuid();

        [JsonIgnore]
        public double ActionBonus { get; set; }


        [JsonIgnore]
        public long ChargeCurrent { get; set; }

        [JsonIgnore]
        public long ChargeMax => Math.Max(_lowestStatValue, CoreEnergyCurrent + ChargeMaxMod);

        [JsonIgnore]
        public long ChargeMaxMod => Effects
            .Where(x => x.TargetStat == CharacterEffectStat.ChargeMax)
            .Sum(x => x.StatChange);

        [JsonIgnore]
        public double ChargePercent => (double)ChargeCurrent / ChargeMax;

        [JsonIgnore]
        public long ChargeRate => Math.Max(_lowestStatValue, CoreEnergyCurrent + ChargeRateMod);

        [JsonIgnore]
        public long ChargeRateMod => Effects
            .Where(x => x.TargetStat == CharacterEffectStat.ChargeRate)
            .Sum(x => x.StatChange);


        public long CoreEnergy { get; set; }

        [JsonIgnore]
        public long CoreEnergyCurrent => Math.Max(_lowestStatValue, CoreEnergy + CoreEnergyMod);

        [JsonIgnore]
        public long CoreEnergyMod => Effects
            .Where(x => x.TargetStat == CharacterEffectStat.CoreEnergy)
            .Sum(x => x.StatChange);

        public List<CharacterEffect> Effects { get; init; } = new List<CharacterEffect>();

        public long EnergyCurrent { get; set; }

        [JsonIgnore]
        public long EnergyMax => CoreEnergyCurrent + EnergyMaxMod;

        [JsonIgnore]
        public long EnergyMaxMod => Effects
            .Where(x => x.TargetStat == CharacterEffectStat.EnergyMax)
            .Sum(x => x.StatChange);

        [JsonIgnore]
        public double EnergyPercent => (double)EnergyCurrent / EnergyMax;

        public long Experience { get; set; }

        [JsonIgnore]
        public Interactable Interaction { get; set; }

        [JsonIgnore]
        public DateTimeOffset LastMoveTime { get; set; }

        public string Name { get; init; }

        [JsonIgnore]
        public string PartyId { get; set; }

        [JsonIgnore]
        public CharacterState State { get; set; }

        [JsonIgnore]
        public Character Target { get; set; }

        public CharacterType Type { get; set; }
    }
}