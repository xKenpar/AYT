using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    SpriteRenderer renderer;
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.sortingOrder = (int)(-100 * transform.transform.position.y);
    }
}
