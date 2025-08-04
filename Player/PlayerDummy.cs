using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour
{
    [SerializeField]Animator anim;
    /// <summary>
    /// 더미의 이동속도 
    /// </summary>
    [SerializeField] float moveSpeed;

    [SerializeField] float moveTime;
    [SerializeField] GameObject player;
    Player playerScrpit; 
    Vector2 moveforc = new Vector2(1, 0);
    //Player playerScript;
    private void Awake()
    {
        playerScrpit=player.GetComponent<Player>();
        //playerScript =player.GetComponent<Player>();
        
    }
    private void OnEnable()
    {
        StartCoroutine(Movetime());
        //player.SetActive(false);
        //playerScript.InputDisable();
    }
    private void Start()
    {
        
        player.SetActive(false);
    }
    private void Update()
    {
        transform.Translate(moveforc * moveSpeed * Time.deltaTime);
    }

    IEnumerator Movetime()
    {
        //playerScrpit.WaitInputStart(moveTime + 0.4f);
        yield return new WaitForSeconds(moveTime);
        anim.SetTrigger("Stop");
        moveSpeed = 0.0f;
        yield return new WaitForSeconds(0.4f);
        player.transform.position = transform.position;
        player.SetActive(true);
        //player.SetActive(true);
        //playerScript.InputEnable();
        gameObject.SetActive(false);
    }
}
