﻿using Assets.GamePlay.Scripts.BulletEffects;
using Assets.GamePlay.Scripts.Enemies;
using Assets.GamePlay.Scripts.Other.ObjectPull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GamePlay.Scripts.Ammo
{
    public class BulletRaw  : Bullet
    {
        public BulletRaw ()
        {
        }
        protected Enemy currentTarget;
        
        public override void Move ()
        {
            OnFly(transform.position, transform.rotation);
            GetComponent<Rigidbody2D>().velocity = (directionOfMoving * speedOfMoving);
            float angleOfFlying = Mathf.Sign(Mathf.Asin(directionOfMoving.y)) * Mathf.Acos(directionOfMoving.x);
            angleOfFlying *= 180 / Mathf.PI;
            angleOfFlying -= 90;
            transform.rotation = Quaternion.Euler(0, 0, angleOfFlying);
        }
        public override void SetPosition (Vector2 pos)
        {
            transform.position = pos;
        }
        public override void SetRotation (Quaternion pos)
        {
            transform.rotation = pos;
        }

        private void Start()
        {
            //CreateEffects();
        }

        public override void Delete()
        {
            base.Delete();
            //GetComponent<BulletForPull>().ReturnToPull();
        }
        


        //attaching to this bullet effects. For test
        private void CreateEffects()
        {
            ListOfEffects = new List<BulletEffects.BulletEffect>();
            //ListOfEffects.Add(new BulletEffectSlowingRaw(intensity));
            //ListOfEffects.Add(new BulletEffectImmidiateDamageRaw(intensity, Damage.DamageManager.EKindOfDamage.blue));
            //ListOfEffects.Add(new BulletEffectPeriodicDamageRaw(intensity, Damage.DamageManager.EKindOfDamage.blue));
        }

        public override void Clone(Bullet t)
        {
            this.ListOfEffects = t.ListOfEffects;
        }
    }
}
