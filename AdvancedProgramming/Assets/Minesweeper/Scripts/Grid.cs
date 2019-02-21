using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper
{
    public class Grid : MonoBehaviour
    {
        public GameObject tilePrefab;
        public int width = 10, height = 10;
        public float spacing = 0.155f;

        private Tile[,] tiles;

        Tile SpawnTile(Vector3 pos)
        {
            GameObject clone = Instantiate(tilePrefab); // Clone tile prefab
            clone.transform.position = pos; // Edit it's properties
            Tile currentTile = clone.GetComponent<Tile>();
            return currentTile; // Return it
        }


        void GenerateTiles() //  Spawns tiles in a grid like pattern
        {
            tiles = new Tile[width, height]; // Create a new 2D array of size width x height
            for (int x = 0; x < width; x++) // Loop through the entire tile list
            {
                for (int y = 0; x < height; y++)
                {
                    Vector2 halfSize = new Vector2(width * 0.5f, height * 0.5f); // Store half size for later use

                    Vector2 pos = new Vector2(x - halfSize.x, y - halfSize.y); // pivot tiles around grid

                    Vector2 offset = new Vector2(0.5f, 0.5f); // Pivot tiles around grid

                    pos += offset;

                    pos *= spacing; // Apply spacing

                    Tile tile = SpawnTile(pos); // Spawn the tile using spawn function made earlier
                    tile.transform.SetParent(transform); // Attach newly spawned tile to self (transform)
                    tile.x = x; // Store it's array coordinates within itself for future reference
                    tile.y = y;
                    tiles[x, y] = tile; // Store tile in array at those coordinates
                }
            }
        }

        // Use this for initialization
        void Start()
        {
            GenerateTiles();
        }

        public int GetAdjacentMineCount(Tile tile)
        {           
            int count = 0; // Set count to 0            
            for (int x = -1; x <= 1; x++) // Loop through all the adjacent tiles on the X
            {                
                for (int y = -1; y <= 1; y++)// Loop through all the adjacent tiles on the Y
                {                   
                    int desiredX = tile.x + x;  // Calculate which adjacent tile to look at
                    int desiredY = tile.y + y;                   
                    if (desiredX < 0 || desiredX >= width || desiredY < 0 || desiredY >= height) // Check if the desired x & y is outside bounds
                    {
                        continue;
                    }                    
                    Tile currentTile = tiles[desiredX, desiredY]; // Select current tile                   
                    if (currentTile.isMine)  // Check if that tile is a mine
                    {                       
                        count++;  // Increment count by 1
                    }
                }
            }           
            return count;
        }

        void SelectATile()
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); // Generate a ray from the camera with mouse position

            RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction); // Perform raycast

            if(hit.collider != null) // If the mouse hit something
            {
                Tile hitTile = hit.collider.GetComponent<Tile>(); // Try getting a tile component from the thing we hit
                if(hitTile != null) // Check if the thing it hit was a tile
                {
                    int adjacentMines = GetAdjacentMineCount(hitTile); // Get a count of mines around the hit tile
                    hitTile.Reveal(adjacentMines); // Reveal what the hit tile is
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0)) // Check if mouse button is pressed
            {
                SelectATile(); // Run the method for selecting tiles
            }
        }
    }
}





