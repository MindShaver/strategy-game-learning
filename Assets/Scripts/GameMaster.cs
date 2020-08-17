using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Unit SelectedUnit;
    public int PlayerTurn = 1;
    public GameObject SelectedUnitSquare;

    public void ResetTiles()
    {
        foreach(Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        // TODO: Add a button to the UI called "END TURN"
        // For now the spacebar will end the turn with no prompts
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }

        if(SelectedUnit != null)
        {
            SelectedUnitSquare.SetActive(true);
            SelectedUnitSquare.transform.position = SelectedUnit.transform.position;
        } else
        {
            SelectedUnitSquare.SetActive(false);
        }
    }

    void EndTurn()
    {
        // TODO: Randomize the starting turn
        if (PlayerTurn == 1)
        {
            PlayerTurn = 2;
        } else if( PlayerTurn == 2)
        {
            PlayerTurn = 1;
        }

        if(SelectedUnit != null)
        {
            SelectedUnit.IsSelected = false;
            SelectedUnit = null;
        }

        ResetTiles();

        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.UnitHasMoved = false;
        }
    }
}
