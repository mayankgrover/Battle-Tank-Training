﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankBattle.Tank
{
    public class TankService : GenericMonoSingleton<TankService>
    {
        public TankView tankPrefab;

        void Start()
        {
            //creating a tank
            TankController _tankController = new TankController(tankPrefab);
        }

        void Update()
        {
        
        }
    }
}
