using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public float HoverAmount;

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * HoverAmount;
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * HoverAmount;
    }
}
