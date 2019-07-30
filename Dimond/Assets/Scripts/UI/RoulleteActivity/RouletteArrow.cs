using UnityEngine;
using System.Collections;

public class RouletteArrow : MonoBehaviour
{
    private GameObject bonus;

    public GameObject GetBonus()
    {
        return bonus;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision!= null)
        {
            bonus = collision.gameObject;
        }
    }
}
