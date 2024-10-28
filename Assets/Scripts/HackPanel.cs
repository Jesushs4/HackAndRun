using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackPanel : MonoBehaviour
{

    private LayerMask playerLayer;
    private Animator animator;

    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }


    void Update()
    {
        if (IsPlayerInside() && Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 0f;
        }
    }

    private bool IsPlayerInside()
    {
        return Physics2D.OverlapCircle(transform.position, 0.2f, playerLayer);
    }

    private void UseAnimation()
    {
        animator.SetBool("isUsing", true);
    }
}
