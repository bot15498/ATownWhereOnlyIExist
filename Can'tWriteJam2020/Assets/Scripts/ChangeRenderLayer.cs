using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRenderLayer : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer playerSpriteRender;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpriteRender = player.GetComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerSpriteRender = player.GetComponent<SpriteRenderer>();
        }
        else
        {
            if (player.transform.position.y < transform.position.y)
            {
                sr.sortingOrder = playerSpriteRender.sortingOrder - 1;
            }
            else
            {
                sr.sortingOrder = playerSpriteRender.sortingOrder + 1;
            }
        }
    }
}
