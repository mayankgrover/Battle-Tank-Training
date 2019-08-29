﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankBattle.Bullet
{
    public class BulletController
    {
        private BulletView bulletView;
        private BulletModel bulletModel;

        public BulletController(BulletView _prefab, Vector3 _position, Quaternion _rotation)
        {
            bulletModel = new BulletModel();
            bulletView = GameObject.Instantiate<BulletView>(_prefab, _position, _rotation);
            bulletView.SetController(this);
            bulletView.GetComponent<Rigidbody>().AddForce(bulletView.transform.forward * 1000);
        }

        public void DestroyBullet()
        {
            bulletModel = null;
            GameObject.Destroy(bulletView.gameObject);
        }

        public int GetBulletDamagePower()
        {
            return bulletModel.damage;
        }

    }
}
