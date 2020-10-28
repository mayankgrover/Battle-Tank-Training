﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController
{
    public TankController(TankModel tankModel, TankView tankPrefab) 
    {
        TankModel = tankModel;
        
        //GameObject go = GameObject.Instantiate(tankPrefab);
        //TankView = go.GetComponent<TankView>();

        TankView = GameObject.Instantiate<TankView>(tankPrefab);

    }

    public TankModel TankModel { get; }
    public TankView TankView { get; }
}
