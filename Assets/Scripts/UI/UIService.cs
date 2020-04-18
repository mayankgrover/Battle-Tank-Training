﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TankGame.Spawner;

namespace TankGame.UI
{
    public class UIService : MonoSingletonGeneric<UIService>
    {
        public Button[] buttons;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                int buttonIndex = i; //(important) for closure error, first capture the data
                buttons[buttonIndex].onClick.AddListener(() => generateTank(buttonIndex));
            }
        }

        private void generateTank(int tankSerialNumber)
        {
            SpawnerService.Instance.SpawnTanks(tankSerialNumber);  // passing button number for arrays of tank details
        }
    }
}