using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool IsSelected;
    public int TileSpeed;
    public bool UnitHasMoved;
    GameMaster Master;
    public float MoveSpeed;

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
            Master.ResetTiles();
        }
        else
        {
            if (Master.SelectedUnit != null)
            {
                Master.SelectedUnit.IsSelected = false;
            }

            IsSelected = true;
            Master.SelectedUnit = this;

            Master.ResetTiles();
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

    public void Move(Vector2 tilePosition)
    {
        Master.ResetTiles();
        StartCoroutine(StartMovement(tilePosition));
    }

    IEnumerator StartMovement(Vector2 tilePosition)
    {
        while (transform.position.x != tilePosition.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePosition.x, transform.position.y), MoveSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.position.y != tilePosition.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, tilePosition.y), MoveSpeed * Time.deltaTime);
            yield return null;
        }

        UnitHasMoved = true;
    }
}
