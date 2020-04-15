﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.Bullet;
using TankGame.Spawner;

namespace TankGame.Tank
{
    public class TankService : MonoSingletonGeneric<TankService>
    {
        public TankView tankView;
        public TankScriptableObjectList tankList;

        //public Color[] tankColor;
        //public float[] tankHealth;
        //public float[] bulletDamage;

        protected override void Awake()
        {
            base.Awake();
        }

        public void fire(Transform bulletSpawn, float bulletDamange)
        {
            BulletService.Instance.spawnBullet(bulletSpawn, bulletDamange);
        }
        public void DestroyView(TankView tankView)
        {
            ParticleService.Instance.CreateTankExplosion(tankView.transform.position, tankView.transform.rotation);

            Destroy(tankView.gameObject, 0.1f);
            StartCoroutine(RestartTank());
        }

        IEnumerator RestartTank()
        {
            yield return new WaitForSeconds(3f);
            SpawnerService.Instance.SpawnTanks(0);
        }
        public TankController SpawnTankPrefab(Transform spawner, int tankSerial)
        {
            TankModel model = new TankModel(tankList.tankScriptableObject[0]);
            TankController tank = new TankController(model, tankView, spawner);
            return tank;
        }
    }
}