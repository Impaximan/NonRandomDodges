using MonoMod;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ID;

namespace NonRandomDodges
{
	public class NonRandomDodges : Mod
    {
        public static float brainCooldown = 15f;
        public static float beltCooldown = 12f;
        public static bool hitCooldownReset = true;


        private double On_Hurt(On_Player.orig_Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float orig, Player self, PlayerDeathReason damageSource, int Damage, int hitDirection, out Player.HurtInfo info, bool pvp, bool quiet, int cooldownCounter, bool dodgeable, float armorPenetration, float scalingArmorPenetration, float knockback)
        {
            if (self.TryGetModPlayer(out NCPlayer p))
            {
                if (p.newBoC)
                {
                    info = default;
                    double num13 = info.Damage;
                    if (Main.myPlayer == self.whoAmI)
                    {
                        for (int num14 = 0; num14 < 200; num14++)
                        {
                            if (!Main.npc[num14].active || Main.npc[num14].friendly)
                            {
                                continue;
                            }
                            int num10 = 300;
                            num10 += (int)num13 * 2;
                            if (Main.rand.Next(500) < num10)
                            {
                                Vector2 val = Main.npc[num14].Center - self.Center;
                                float num15 = val.Length();
                                float num11 = Main.rand.Next(200 + (int)num13 / 2, 301 + (int)num13 * 2);
                                if (num11 > 500f)
                                {
                                    num11 = 500f + (num11 - 500f) * 0.75f;
                                }
                                if (num11 > 700f)
                                {
                                    num11 = 700f + (num11 - 700f) * 0.5f;
                                }
                                if (num11 > 900f)
                                {
                                    num11 = 900f + (num11 - 900f) * 0.25f;
                                }
                                if (num15 < num11)
                                {
                                    float num12 = Main.rand.Next(90 + (int)num13 / 3, 300 + (int)num13 / 2);
                                    Main.npc[num14].AddBuff(31, (int)num12);
                                }
                            }
                        }
                        Projectile.NewProjectile(new EntitySource_OnHurt(self, null), self.Center.X + (float)Main.rand.Next(-40, 40), self.Center.Y - (float)Main.rand.Next(20, 60), self.velocity.X * 0.3f, self.velocity.Y * 0.3f, 565, 0, 0f, self.whoAmI);
                    }


                    if (p.brainCooldown <= 0)
                    {
                        self.BrainOfConfusionDodge();
                        p.brainCooldown = (int)(brainCooldown * 60);
                        return 0.0;
                    }
                    else if (hitCooldownReset)
                    {
                        p.brainCooldown = (int)(brainCooldown * 60);
                    }
                }

                if (p.newBlackBelt)
                {
                    if (p.blackBeltCooldown <= 0)
                    {
                        info = default;
                        self.NinjaDodge();
                        p.blackBeltCooldown = (int)(60 * beltCooldown);
                        return 0.0;
                    }
                    else if (hitCooldownReset)
                    {
                        p.blackBeltCooldown = (int)(60 * beltCooldown);
                    }
                }
            }

            return orig(self, damageSource, Damage, hitDirection, out info, pvp, quiet, cooldownCounter, dodgeable, armorPenetration, scalingArmorPenetration, knockback);
        }

        public override void Load()
        {
            base.Load();
            On_Player.Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float += On_Hurt;
        }

        public override void Unload()
        {
            base.Unload();
            On_Player.Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float -= On_Hurt;
        }
    }

    public class ModifyTooltipItem : GlobalItem
    {
        public static LocalizedText bocTooltip;
        public static LocalizedText bbTooltip;

        public override bool InstancePerEntity => true;

        public override void SetStaticDefaults()
        {
            bocTooltip = Mod.GetLocalization($"BrainTooltip");
            bbTooltip = Mod.GetLocalization($"BlackBeltTooltip");
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.BrainOfConfusion)
            {
                int i = tooltips.FindIndex(x => x.Name.Contains("Tooltip"));

                tooltips.RemoveAt(i);

                tooltips.Insert(i, new TooltipLine(Mod, "Tooltip", bocTooltip.Value));
            }

            if (item.type == ItemID.BlackBelt)
            {
                int i = tooltips.FindIndex(x => x.Name.Contains("Tooltip"));

                tooltips.RemoveAt(i);

                tooltips.Insert(i, new TooltipLine(Mod, "Tooltip", bbTooltip.Value));
            }

            if (item.type == ItemID.MasterNinjaGear)
            {
                int i = tooltips.FindLastIndex(x => x.Name.Contains("Tooltip"));

                tooltips.RemoveAt(i);

                tooltips.Insert(i, new TooltipLine(Mod, "Tooltip", bbTooltip.Value));
            }
        }
    }
}
