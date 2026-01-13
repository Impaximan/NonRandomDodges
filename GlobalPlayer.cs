using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace NonRandomDodges
{
    public class NCPlayer : ModPlayer
    {
        public bool newBoC = false;
        public bool newBlackBelt = false;

        public int blackBeltCooldown = 0;
        public int brainCooldown = 0;

        public override void ResetEffects()
        {
            newBoC = false;
            newBlackBelt = false;
        }

        public override void PostUpdateEquips()
        {
            if (Player.brainOfConfusionItem != null || (Player.brainOfConfusionItem != null && !Player.brainOfConfusionItem.IsAir))
            {
                Player.brainOfConfusionItem = null;
                newBoC = true;
            }

            if (Player.blackBelt)
            {
                newBlackBelt = true;
                Player.blackBelt = false;
            }

            if (brainCooldown > 0)
            {
                brainCooldown--;

                if (brainCooldown <= 0)
                {
                    CombatText.NewText(Player.getRect(), Color.Pink, "Brain dodge regained!", true);
                    SoundEngine.PlaySound(SoundID.Item77, Player.Center);
                }
            }

            if (blackBeltCooldown > 0)
            {
                blackBeltCooldown--;

                if (blackBeltCooldown <= 0)
                {
                    CombatText.NewText(Player.getRect(), Color.Gray, "Black belt dodge regained!", true);
                    SoundEngine.PlaySound(SoundID.Item90, Player.Center);
                }
            }
        }
    }
}
