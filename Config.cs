using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace NonRandomDodges
{
    internal class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header($"ConfigHeader")]

        [Increment(0.5f)]
        [Range(6f, 60f)]
        [DefaultValue(15f)]
        public float bocCooldown = 15f;

        [Increment(0.5f)]
        [Range(6f, 60f)]
        [DefaultValue(22f)]
        public float blackBeltCooldown = 22;

        [DefaultValue(true)]
        public bool cooldownResetWhenHit = true;

        public override void OnChanged()
        {
            NonRandomDodges.brainCooldown = bocCooldown;
            NonRandomDodges.beltCooldown = blackBeltCooldown;
            NonRandomDodges.hitCooldownReset = cooldownResetWhenHit;
        }
    }
}
