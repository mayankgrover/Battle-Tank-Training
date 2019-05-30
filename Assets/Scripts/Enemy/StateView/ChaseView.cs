﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Manager;
using Interfaces;

namespace Enemy
{
    public class ChaseView : EnemyBaseStateView
    {

        [SerializeField] private EnemyView enemyView;

        public event Action<EnemyState> StateUpdate;

        private IGameManager gameManager;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (gameManager == null)
                gameManager = StartService.Instance.GetService<IGameManager>();

            enemyView.PetrolState.enabled = false;
            transform.LookAt(enemyView.targetPos);

            if (Vector3.Distance(enemyView.targetPos,transform.position) > 15f)
            {
                enemyView.StateChangedMethod(EnemyState.petrol);
                this.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (gameManager.GetCurrentState().gameStateType == StateMachine.GameStateType.Pause)
            {
                if (enemyView.Agent.isStopped == false)
                    enemyView.Agent.isStopped = true;

                return;
            }
            else if (gameManager.GetCurrentState().gameStateType == StateMachine.GameStateType.Game)
            {
                if (enemyView.Agent.isStopped == true)
                    enemyView.Agent.isStopped = false;
            }

            if (enemyView.Agent.remainingDistance <= 0.5f)
            {
                enemyView.StateChangedMethod(EnemyState.petrol);
                this.enabled = false;
            }
        }
    }
}