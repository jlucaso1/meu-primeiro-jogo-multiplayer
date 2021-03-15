using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Fruit : NetworkBehaviour
{
    private float minX = 0.5f;
    private float maxX = 9.5f;
    private float minY = -4.5f;
    private float maxY = 4.5f;
    private float variation = 0.5f;

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            player.playerScore += 1;
            ChangePosition();
        }
    }
    float MinMaxStep(float min, float max)
    {
        return ((int)Random.Range(min, max)) + variation;
    }
    [Server]
    public void ChangePosition()
    {
        var x = MinMaxStep(minX, maxX);
        var y = MinMaxStep(minY, maxY);
        var position = new Vector2(x, y);
        transform.position = position;
    }
}
