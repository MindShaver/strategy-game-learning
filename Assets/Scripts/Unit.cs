using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool IsSelected;
    public int TileSpeed;
    public bool UnitHasMoved;
    public GameMaster Master;

    private void Start()
    {
        Master = FindObjectOfType<GameMaster>();
    }

    private void OnMouseDown()
    {
        if (IsSelected == true)
        {
            IsSelected = false;
            Master.SelectedUnit = null;
        }
        else
        {
            if (Master.SelectedUnit != null)
            {
                Master.SelectedUnit.IsSelected = false;
            }

            IsSelected = true;
            Master.SelectedUnit = this;

            GetWalkableTiles();
        }
    }

    private void GetWalkableTiles()
    {
        if (UnitHasMoved == true)
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (CanMove(tile))
            {
                if (tile.IsTileClear() == true)
                {
                    tile.Highlight();
                }
            }
        }
    }

    private bool CanMove(Tile tile)
    {
        // Basic X Y math for horizontal and vertical movement.
        // TODO: Implement an A* algorthim.
        return (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y)) <= TileSpeed;
    }
}
