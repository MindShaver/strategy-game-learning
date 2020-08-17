using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Turn Based Functionality
    GameMaster Master;
    public int PlayerNumber;

    // Movement Functionality
    public bool IsSelected;
    public int TileSpeed;
    public bool UnitHasMoved;
    public float MoveSpeed;

    // Attack Functionality
    public int AttackRange;
    List<Unit> EnemiesInRange = new List<Unit>();
    public bool UnitHasAttacked;
    public int Health;
    public int AttackDamage;
    //public int DefenseDamage;
    public int Armor;

    public DamageIcon DIcon;

    public GameObject AttackIcon;

    private void Start()
    {
        Master = FindObjectOfType<GameMaster>();
    }

    private void OnMouseDown()
    {
        ResetAttackIcons();

        if (IsSelected == true)
        {
            IsSelected = false;
            Master.SelectedUnit = null;
            Master.ResetTiles();
        }
        else
        {
            if(PlayerNumber == Master.PlayerTurn)
            {
                if (Master.SelectedUnit != null)
                {
                    Master.SelectedUnit.IsSelected = false;
                }

                IsSelected = true;
                Master.SelectedUnit = this;

                Master.ResetTiles();
                GetEnemies();
                GetWalkableTiles();
            }
        }

        Collider2D collider = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        Unit unit = collider.GetComponent<Unit>();
        if(Master.SelectedUnit != null)
        {
            if(Master.SelectedUnit.EnemiesInRange.Contains(unit) && Master.SelectedUnit.UnitHasAttacked == false)
            {
                Master.SelectedUnit.Attack(unit);
            }
        }

    }

    void Attack(Unit enemy)
    {
        UnitHasAttacked = true;
        int enemyDamage = AttackDamage - enemy.Armor;
        //int unitDamage = enemy.DefenseDamage - Armor;
        Vector2 enemyPosition = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 0.5f);

        if(enemyDamage >= 1)
        {
           DamageIcon instance = Instantiate(DIcon, enemyPosition, Quaternion.identity);
            instance.Setup(enemyDamage);
            enemy.Health -= enemyDamage;
        }

        //if(unitDamage >= 1)
        //{
        //    Health -= unitDamage;
        //}

        if(enemy.Health <= 0)
        {
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }

        //if(Health <= 0)
        //{
        //    Master.ResetTiles();
        //    Destroy(this.gameObject);
        //}
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

    void GetEnemies()
    {
        EnemiesInRange.Clear();

        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            if(CanAttack(unit))
            {
                if (unit.PlayerNumber != Master.PlayerTurn && UnitHasAttacked == false)
                {
                    EnemiesInRange.Add(unit);
                    unit.AttackIcon.SetActive(true);
                }
            }
        }
    }

    public void ResetAttackIcons()
    {
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.AttackIcon.SetActive(false);
        }
    }

    private bool CanMove(Tile tile)
    {
        // Basic X Y math for horizontal and vertical movement.
        // TODO: Implement an A* algorthim.
        return (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y)) <= TileSpeed;
    }

    private bool CanAttack(Unit unit)
    {
        return (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y)) <= AttackRange;

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
        ResetAttackIcons();
        GetEnemies();
    }
}
