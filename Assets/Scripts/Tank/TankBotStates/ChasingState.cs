﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankBattle.Tank
{
    public class ChasingState : TankBotState
    {
        private TankController targetTank;
        public TankController TargetTank {set { targetTank = value; }}
        public override void OnEnterState(TankController _tankController, TankView _tankView)
        {
            base.OnEnterState(_tankController, _tankView);
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public void Update() 
        {
            if (targetTank != null && (Vector3.Distance(targetTank.GetTankPosition(), tankController.GetTankPosition()) < TankService.Instance.BotTankPropScriptableObject.AttackTriggerDistance))
            {
                AttackingState attackingBehaviour = tankView.GetComponent<AttackingState>();
                attackingBehaviour.TargetTank = targetTank;
                tankController.SetTankBotState(attackingBehaviour);
            }
            else if (targetTank != null && (Vector3.Distance(targetTank.GetTankPosition(), tankController.GetTankPosition()) < TankService.Instance.BotTankPropScriptableObject.EnemyDetectionRadius))
            {
                tankController.LookTo(targetTank.GetTankPosition());
                tankController.MoveTo(Vector3.MoveTowards(tankView.transform.position, targetTank.GetTankPosition(), Time.deltaTime));
            }
            else
            {
                tankController.SetTankBotState(tankView.GetComponent<PatrollingState>());
            }
        }
    }
}
