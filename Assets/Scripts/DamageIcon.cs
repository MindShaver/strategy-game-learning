using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] DamageSprites;
    public GameObject Effect;

    public float Lifetime;

    private void Start()
    {
        Invoke("Destruction", Lifetime);
    }

    public void Setup(int damage)
    {
        GetComponent<SpriteRenderer>().sprite = DamageSprites[damage - 1];
    }

    void Destruction()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
