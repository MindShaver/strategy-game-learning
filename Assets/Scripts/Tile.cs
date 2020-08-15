using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public Sprite[] TileGraphics;
    public float HoverAmount;
    public LayerMask ObstacleLayer;
    public Color HighlightedColor;
    public bool IsWalkable;
    public GameMaster Master;

    private void Start()
    {
        int randTile = Random.Range(0, TileGraphics.Length);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = TileGraphics[randTile];

        Master = FindObjectOfType<GameMaster>();
    }

    private void OnMouseEnter()
    {
        // Don't scale tiles with Obstacles.
        if(IsTileClear())
        {
            // Scale up our tile when mouse enters tile.
            transform.localScale += Vector3.one * HoverAmount;
        }
    }

    private void OnMouseExit()
    {
        // Don't Descale Tiles with Obstacles.
        if(IsTileClear())
        {
            // Scale down our tile when mouse leaves tile.
            transform.localScale -= Vector3.one * HoverAmount;
        }
    }

    public bool IsTileClear()
    {
        // Grab the obstacle if it is there.
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, ObstacleLayer);

        if (obstacle != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Highlight()
    {
        _spriteRenderer.color = HighlightedColor;
        IsWalkable = true;
    }

    public void Reset()
    {
        _spriteRenderer.color = Color.white;
        IsWalkable = false;
    }
}
