﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tank.Service;
using Tank.Controller;
using Tank.View;
using Scriptables;

namespace Tank.Model
{
	public class TankModel
	{
		public TankModel(TankScriptableObject tankScriptableObject)
		{
			TankType = tankScriptableObject.tankType;
			Speed = tankScriptableObject.speed;
			Turn = tankScriptableObject.turn;
			Health = tankScriptableObject.health;
		}
		public TankModel(TankType tankType, float speed, float turn, float health)
		{
			TankType = tankType;
			Speed = speed;
			Turn = turn;
			Health = health;
		}

		public TankType TankType { get; }
		public float Speed { get; }
		public float Turn { get; }
		public float Health { get; }
	}
}