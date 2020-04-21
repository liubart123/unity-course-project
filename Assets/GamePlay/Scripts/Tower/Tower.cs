﻿using Assets.GamePlay.Scripts.Ammo;
using Assets.GamePlay.Scripts.Enemies;
using Assets.GamePlay.Scripts.Tower.Interfaces;
using Assets.GamePlay.Scripts.TowerClasses;
using Assets.GamePlay.Scripts.TowerClasses.TowerCombinations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.GamePlay.Scripts.Player;
using Assets.GamePlay.Scripts.Tower.Interfaces.ClassesCollection;
using Assets.GamePlay.Scripts.BulletEffects;
using Assets.GamePlay.Scripts.Building.interfaces.HealthContorller;

namespace Assets.GamePlay.Scripts.Tower
{
    public abstract class Tower : Building.Building
    {

        [SerializeField]
        protected Bullet bullet;   //type of bullet, that is used by this tower
        [SerializeField]
        protected float effectivity;   //effectivity of effects

        //AIMING
        public Enemy CurrentTarget { get; protected set; }
        private Vector2 directionOfShooting;
        public TargetChooser TargetChooser{ get; protected set; }
        public virtual void ChooseTarget()
        {
            if (CurrentTarget?.gameObject != null)
            {
                CurrentTarget.eventsWhenThisDie -= ChooseTargetAfterEnemyDeath;
            }
            CurrentTarget = TargetChooser.ChooseTarget(
                new TargetChooserParameters(
                    (transform.eulerAngles.z + 90)/180*Mathf.PI,
                    (Vector2)transform.position));
            AimTaker.ResetAimTaker();
            if (CurrentTarget != null)
            {
                CurrentTarget.eventsWhenThisDie += ChooseTargetAfterEnemyDeath;
            }
        }   //chose target among all possible enemies

        private void ChooseTargetAfterEnemyDeath(Enemy en)
        {
            if (CurrentTarget == en)
                ChooseTarget();
        }
        //пасля смерці тавэра цякучы таргет не будзе аднаўляць тагрет тафэру пасля смерці сябе)
        private void MakeEnemyTakeNewTarget()
        {
            //Debug.Log(CurrentTarget.gameObject.name + " MakeEnemyTakeNewTarget");
            if (CurrentTarget!=null)
                CurrentTarget.eventsWhenThisDie -= ChooseTargetAfterEnemyDeath;
        }
        public AimTaker AimTaker { get; protected set; }    //calculate direction for shooting
        public virtual void TakeAim(bool getActualValue = false)
        {
            if (CurrentTarget == null)
            {
                return;
            }
            directionOfShooting = AimTaker.TakeAim(new AimTakerParameters(CurrentTarget,transform.position, bullet.speedOfMoving, getActualValue));
            bool aimed = TowerRotater.RotateTower(new TowerRotaterParameters(directionOfShooting, transform));
            if (aimed && isLoaded)
            {
                Shoot();
            }
        }   //calculate rigth direction and rotate tower accordingly
        public TowerRotater TowerRotater { get; protected set; }

        //SHOOTING
        public Shooter Shooter { get; protected set; }
        public virtual void Shoot()
        {
            Bullet bullet = BulletFactory.CreateBullet(new BulletFactoryParameters(transform));
            TakeAim(true);
            Shooter.Shoot(new ShooterParameters(CurrentTarget, bullet));
            ResetReloading();
        }

        //RELOADING
        public Reloader Reloader { get; protected set; }
        public Boolean isLoaded;
        public void Reload()
        {
            isLoaded = Reloader.Reload(new ReloaderParameters(ref isLoaded));
        }   //call iteration of reloading, if reloading is finished "isLoaded" will turn true
        protected void ResetReloading()
        {
            Reloader.ResetReloading(new ReloaderParameters(ref isLoaded));
            isLoaded = false;
        }
        public BulletFactory BulletFactory { get; protected set; }
        protected virtual void InitializeBulletFactory()
        {
            InitializeBullet();
            ICollection<BulletEffect> effects = classCollection.GetAllEffects();
            foreach(var ef in effects)
            {
                ef.Effectivity *= effectivity;
            }
            BulletFactory.Initialize(bullet, effects, this);
        }
        protected virtual void InitializeBullet()
        {
            bullet = transform.GetComponentInChildren<Bullet>(true);
        }

        //TOWER_CLASSES
        public ClassCollection classCollection;

        private bool initialized = false;
        public override void Initialize()
        {
            initialized = true;

            healthController = GetComponent<HealthController>();
            owner = FindObjectOfType<Player.Player>();
            TargetChooser = GetComponent<TargetChooser>();
            AimTaker = GetComponent<AimTaker>();
            TowerRotater = GetComponent<TowerRotater>();
            Shooter = GetComponent<Shooter>();
            Reloader = GetComponent<Reloader>();
            BulletFactory = GetComponent<BulletFactory>();
            classCollection = GetComponent<ClassCollection>();


            healthController.Initialize();
            classCollection.Initialize();
            InitializeBulletFactory();



            base.Initialize();
        }
        public void FixedUpdate()
        {
            if (initialized)
            {
                Reload();
                TakeAim();
            }
        }

        public override void Die()
        {
            base.Die();
            MakeEnemyTakeNewTarget();
            BulletFactory.Delete();
        }
    }
}
