﻿using Assets.GamePlay.Scripts.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GamePlay.Scripts.BulletEffects
{
    [Serializable]
    public class BulletEffectSlowingRaw : BulletEffect
    {
        public BulletEffectSlowingRaw(float intensity) : base(intensity)
        {
            typeOfEffect = ETypeOfBulletEffect.slowing;
            effectName = "slowing";
        }

        public override void AffectOnce(Enemy enemy)
        {
            enemy.speed /= effectivity;
        }

        public override BulletEffect CloneEffectWithOtherIntensity(float Intensity)
        {
            return new BulletEffectSlowingRaw(Intensity);
        }

        public override void RemoveEffect(Enemy enemy)
        {
            enemy.speed *= effectivity;
        }

        public override BulletEffect Clone()
        {
            return new BulletEffectSlowingRaw(effectivity);
        }
    }
}
