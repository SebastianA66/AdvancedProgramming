using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        public int x, y;
        public bool isMine = false; // Is the current tile a mine
        public bool isRevealed = false; // Has the tile been revealed
        [Header("References")]
        public Sprite[] emptySprites; // List of empty sprites
        public Sprite[] mineSprites; // Mine sprites
        private SpriteRenderer rend; // Sprite renderer

        void Awake()
        {
            rend = GetComponent<SpriteRenderer>(); // Reference to sprite renderer
        }

        // Use this for initialization
        void Start()
        {
            isMine = Random.value < 0.5f;
        }

        public void Reveal(int adjacentMines, int mineState = 0)
        {
            isRevealed = true; // Flags the tile as being revealed
            if(isMine) // Checks if tile is a mine
            {
                rend.sprite = mineSprites[mineState]; // Sets sprite to mine sprite
            }
            else
            {
                rend.sprite = emptySprites[adjacentMines]; // Sets sprite to appropriate texture based on adjacent mines
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


