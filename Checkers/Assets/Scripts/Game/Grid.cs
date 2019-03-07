using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject redPiecePrefab, whitePiecePrefab;
    public Vector3 boardOffset = new Vector3(-4.0f, 0.0f, -4.0f);
    public Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);
    public Piece[,] pieces = new Piece[8, 8];

    // For Drag and Drop
    // Grid coordinated the mouse is over
    private Vector2Int mouseOver;
    // Piece that has been clicked and dragged
    private Piece selectedPiece;

    // Returns a piece at a given cell
    Piece GetPiece(Vector2Int cell)
    {
        return pieces[cell.x, cell.y];
    }

    // Checks if given coordinates are out of the board range
    bool IsOutOfBounds(Vector2Int cell)
    {
        return cell.x < 0 || cell.x >= 8 || cell.y < 0 || cell.y >= 8;
    }

    // Selects a piece on the 2D grid and returns it
    Piece SelectedPiece(Vector2Int cell)
    {
        // Check if X and Y is out of bounds
        if (IsOutOfBounds(cell))
        {
            // Return result early
            return null;
        }

        // Get the piece at X and Y location
        Piece piece = GetPiece(cell);

        // Check that it isn't null
        if(piece)
        {
            return piece;
        }

        return null;
    }

    void MouseOver()
    {
        // Perform Raycast from mouseposition
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // If the ray hit the board
        if (Physics.Raycast(camRay, out hit))
        {
            // Convert mouse coordinates to 2D array coordinates
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else // Otherwie
        {
            // Default to error (-1)
            mouseOver = new Vector2Int(-1, -1);
        }
    }
    
    // Drags the selected piece using Raycast location
    void DragPiece(Piece selected)
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Detects mouse ray hit point
        if (Physics.Raycast(camRay, out hit))
        {
            // Updates position of selected pice to hit point + offset
            selected.transform.position = hit.point + Vector3.up;
        }
    }


    Vector3 GetWorldPosition(Vector2Int cell)
    {
        return new Vector3(cell.x, 0, cell.y) + boardOffset + pieceOffset;
    }

    void MovePiece(Piece piece, Vector2Int newCell)
    {
        Vector2Int oldCell = piece.cell;
        // Update array
        pieces[oldCell.x, oldCell.y] = null;
        pieces[newCell.x, newCell.y] = piece;
        // Update data on piece
        piece.oldCell = oldCell;
        piece.cell = newCell;
        // Translate the piece to another location
        piece.transform.localPosition = GetWorldPosition(newCell);
    }

    void GeneratePiece(GameObject prefab, Vector2Int desiredCell)
    {
        // Generate Instance of prefab
        GameObject clone = Instantiate(prefab, transform);
        // Get the piece component
        Piece piece = clone.GetComponent<Piece>();
        // Set the cell data for the first time
        piece.oldCell = desiredCell;
        piece.cell = desiredCell;
        // Reposition clone
        MovePiece(piece, desiredCell);
    }

    void GenerateBoard()
    {
        Vector2Int desiredCell = Vector2Int.zero;
        // Generate White Team
        for (int y = 0; y < 3; y++)
        {
            bool oddRow = y % 2 == 0;
            // Loop through columns
            for (int x = 0; x < 8; x += 2)
            {
                desiredCell.x = oddRow ? x : x + 1;
                desiredCell.y = y;
                // Generate Piece
                GeneratePiece(whitePiecePrefab, desiredCell);
            }
        }
        // Generate Red Team
        for(int y = 5; y < 8; y++)
        {
            bool oddRow = y % 2 == 0;
            // Loop through columns
            for ( int x = 0; x < 8; x += 2)
            {
                desiredCell.x = oddRow ? x : x + 1;
                desiredCell.y = y;
                // Generate Piece
                GeneratePiece(redPiecePrefab, desiredCell);
            }
        }
    }

    // Checks if selected pice can move to desired cell based on game rules
    bool TryMove(Piece selected, Vector2Int desiredCell)
    {
        // Get the selected piece's cell
        Vector2Int startCell = selected.cell;
        // Is desired cell out of bounds?
        if(IsOutOfBounds(desiredCell))
        {
            // Move it back to original
            MovePiece(selected, startCell);
            Debug.Log("<colour=red>Invalid - You cannot move out of the map </colour>");
            return false;
        }

        // Replace end coordinates with our selected piece
        MovePiece(selected, desiredCell);
        // Valid move detected
        return true;
    }

    void Start()
    {
        GenerateBoard();
    }

    void Update()
    {
        // Update the mouse over information
        MouseOver();
        // If the mouse is pressed
        if(Input.GetMouseButtonDown(0))
        {
            // Try selecting piece
            selectedPiece = SelectedPiece(mouseOver);
        }
        // if there is a selected piece
        if(selectedPiece)
        {
            // Move piece to end position
            TryMove(selectedPiece, mouseOver);
            // Let go of the piece
            selectedPiece = null;
        }
    }
}
