using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>   
{
    //Player player;                                      //플레이어 받기
    //public Player Player
    //{
    //    get
    //    {
    //        if(player == null)
    //        {
    //            player=FindAnyObjectByType<Player>();
    //        }
    //        return player;
    //    }
    //}
    PlayerHealth playerHealth;
    public PlayerHealth PlayerHealth
    {
        get
        {
            if(playerHealth == null)
            {
                playerHealth=FindAnyObjectByType<PlayerHealth>();
            }
            return playerHealth;
        }
    }


    AmmoCounter ammoCounter;
    public AmmoCounter AmmoCounter
    {
        get
        {
            if(ammoCounter == null)
            {
                ammoCounter=FindAnyObjectByType<AmmoCounter>();
            }
            return ammoCounter;
        }
    }
    ScoreManager scoreManager;
    public ScoreManager ScoreManager
    {
        get
        {
            if(scoreManager == null)
            {
                scoreManager=FindAnyObjectByType<ScoreManager>();
            }
            return scoreManager;
        }
    }

    GrenadeCountUI grenadeCountUI;
    public GrenadeCountUI GrenadeCountUI
    {
        get
        {
            if(grenadeCountUI == null)
            {
                grenadeCountUI=FindFirstObjectByType<GrenadeCountUI>(); 
            }
            return grenadeCountUI;
        }
        
    }
}
