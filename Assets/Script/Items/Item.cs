using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] PowerUp powerUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IBuffable buffable;
        if(collision.gameObject.TryGetComponent<IBuffable>(out buffable))
        {
            buffable.Accept(powerUp);
            Destroy(gameObject);
        }
    }
}
