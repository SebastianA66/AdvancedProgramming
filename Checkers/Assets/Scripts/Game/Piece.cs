using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isWhite, isKing;
    // Stores X and Y cell location in grid
    public Vector2Int cell, oldCell;
    // Reference to animator
    private Animator anim;

    void Awake()
    {
        // Get reference to Animator component
        anim = GetComponent<Animator>();
    }

    public void King()
    {
        // The piece is now king
        isKing = true;
        // Trigger king animation
        anim.SetTrigger("King");
    }
}
