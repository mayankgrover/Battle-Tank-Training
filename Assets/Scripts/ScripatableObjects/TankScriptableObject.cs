﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankBattle.Tank;

[CreateAssetMenu(fileName = "TankScriptableObject", menuName = "ScriptableObject/TankScriptableObject", order = 0)]
public class TankScriptableObject : ScriptableObject {
    public string tankName;
    public float speed;
    public float turningTorque;
    public int health;
    public TankType tankType;
    public TankView tankPrefab;
}
