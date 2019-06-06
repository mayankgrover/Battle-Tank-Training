﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTScriptableObject;
using Common;
using System;
using UI;
using SaveLoad;
using BTManager;

namespace AchievementM
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        [SerializeField]
        private AchievementScriptable achievementScriptable;

        private List<Achievement> achievementList;

        public List<Achievement> AchievementList { get { return achievementList; }}

        public event Action<int> AchievementCheck;
        public event Action<string> AchievementUnlocked;

        bool initialized = false;

        public void AchievmentInitialize()
        {
            if (initialized == false)
            {
                initialized = true;
                BTManager.GameManager.Instance.GameStarted += InvokeDefaultEvents;
                if (achievementScriptable != null)
                {
                    achievementList = new List<Achievement>();

                    for (int i = 0; i < achievementScriptable.achievementList.Count; i++)
                    {
                        Achievement achievement = new Achievement();
                        achievement = achievementScriptable.achievementList[i];
                        string dataString = "Achievement_" + i;
                        bool unlocked = SaveLoadManager.Instance.GetAchievementProgress(dataString, i);
                        if (unlocked == true)
                            achievement.achievementInfo.achievementStatus = AchievementStatus.Unlocked;

                        achievementList.Add(achievement);
                    }
                }
                else
                {
                    Debug.Log("[Achievement Manager] missing scriptable object reference");
                }
            }
        }

        private void InvokeDefaultEvents()
        {
            UIManager.Instance.ScoreIncreased += ScoreIncreased;
            Enemy.EnemyManager.Instance.EnemyDestroyed += EnemyKilled;
            BTManager.GameManager.Instance.GameStarted += GamesPlayed;
        }

        private void ScoreIncreased()
        {
            CheckForAchievement(AchievementType.hiScore, UIManager.Instance.playerScore);
        }

        private void EnemyKilled()
        {
            CheckForAchievement(AchievementType.enemyKilled, Enemy.EnemyManager.Instance.enemiesKilled);
        }

        private void GamesPlayed()
        {
            CheckForAchievement(AchievementType.gamesPlayed, BTManager.GameManager.Instance.gamesPlayed);
        }

        void CheckForAchievement(AchievementType achievementType, int achievedVal)
        {
            for (int i = 0; i < achievementList.Count; i++)
            {
                if (CheckAchievementType(achievementType, i))
                {
                    if (AchievementLock(i) && AchievementThreshHold(achievedVal, i))
                    {
                        string value = "Unlocked " + achievementType.ToString() + " Achievement. Title " +
                                              achievementList[i].achievementInfo.achievementTitle;

                        Achievement achievement = new Achievement();
                        achievement = achievementList[i];
                        achievement.achievementInfo.achievementStatus = AchievementStatus.Unlocked;

                        achievementList[i] = achievement;
                        AchievementUnlocked?.Invoke(value);
                        AchievementCheck?.Invoke(i);
                    }
                }
            }
        }

        private bool CheckAchievementType(AchievementType achievementType, int i)
        {
            return achievementList[i].AchievementType == achievementType;
        }

        private bool AchievementThreshHold(int achievedVal, int i)
        {
            return achievedVal >= achievementList[i].achievementInfo.achievementRequirement;
        }

        private bool AchievementLock(int i)
        {
            return achievementList[i].achievementInfo.achievementStatus == AchievementStatus.Locked;
        }

        public string GetAchievementName(int rewardIndex)
        {
            return achievementList[rewardIndex].AchievementType.ToString();
        }

        public string GetAchievementThreshHolder(int rewardIndex)
        {
            return achievementList[rewardIndex].achievementInfo.achievementRequirement.ToString();
        }


    }
}