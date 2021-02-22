﻿using Radial.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Radial.Data.Entities
{
    public class CharacterEffect
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public TimeSpan Duration { get; init; }
        public DateTimeOffset StartTime { get; init; }
        public long StatChange { get; init; }
        public CharacterEffectStat TargetStat { get; init; }
        public EffectType Type { get; init; }
    }
}
