using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// ��� �÷��̾�
    /// </summary>
    GameObject deadPlayer;
    /// <summary>
    /// ���� ui
    /// </summary>
    Score scoreUI;
    /// <summary>
    /// Ÿ�̸� ��ũ��Ʈ
    /// </summary>
    Timer timer;
    /// <summary>
    /// ����
    /// </summary>
    public int score = 0;
    /// <summary>
    /// ī�Ŷ� ��ġ
    /// </summary>
    [HideInInspector] public Transform cameraPos=null;
    /// <summary>
    /// ���� ������ ���� ui
    /// </summary>
    LifeCountUI lifeCountUI;
    /// <summary>
    /// ���� ũ������ ���� ui
    /// </summary>
    CreditUI creditsUI;
    /// <summary>
    /// ���� �Ѿ��� ���� ui
    /// </summary>
    AmmoUI ammoUI;

    public int prisonerCount = 0;
    private void Awake()
    {
        scoreUI = FindAnyObjectByType<Score>();
        lifeCountUI= FindAnyObjectByType<LifeCountUI>();
        ammoUI = FindAnyObjectByType<AmmoUI>();
        timer = FindAnyObjectByType<Timer>();
        creditsUI = FindAnyObjectByType<CreditUI>();
    }
    public int Score
    {
        get=>score;
        set
        {
            score = value;
            scoreUI.GetScore(score);
            //Debug.Log(score);
        }
    }

    public int lifeScore = 2;
    public int LifeScore
    {
        get => lifeScore;
        set
        {
            lifeScore --;
            if(lifeScore < 0)
            {
                InsertCoin();
            }
            lifeCountUI.Count(lifeScore);
            //Debug.Log($"Life={lifeScore},Coin={coin}");
        }
    }

    [SerializeField] int coin = 100;
    public int Coin
    {
        get => coin;
        set
        {
            if (coin != 0)
            {
                coin--;
                creditsUI.CreditUse(coin);
                lifeScore = 3;
            }
        }
    }
    /// <summary>
    /// �÷��̾ �װ�ٽ� ������ �ɶ� ����Ǵ� �Լ�
    /// </summary>
    public void ReSpawn(GameObject player)
    {
        prisonerCount = 0;
        if(player != null)
            deadPlayer = player;
        LifeScore--;
        if(player != null)
        {
            PlayerHealth playerHP= deadPlayer.GetComponent<PlayerHealth>();
            playerHP.ReadyToResapwn();
        }
        timer.OnRespawn();
    }
    /// <summary>
    /// �÷��̾ ������ ��ŭ ������ ������ �ϳ� �����ϰ� �������� 3�� ä�� �ִ� �Լ�
    /// </summary>
     void InsertCoin()
    {
        Coin--;
        
        ReSpawn(null);
    }
    /// <summary>
    /// ī�޶��� ��ġ�� �޴��Լ�
    /// </summary>
    /// <param name="transform"></param>
    public void CameraPos(Transform transform)
    {
        cameraPos = transform;
    }

    public void ChangeWeapon()
    {
        ammoUI.MachainGunAmmoUI();
    }


}
