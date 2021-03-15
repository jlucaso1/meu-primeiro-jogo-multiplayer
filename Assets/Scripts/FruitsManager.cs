using UnityEngine;
using Mirror;
/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class FruitsManager : NetworkBehaviour
{
    private float minX = 0.5f;
    private float maxX = 9.5f;
    private float minY = -4.5f;
    private float maxY = 4.5f;
    private float variation = 0.5f;
    public GameObject fruitPrefab;
    public static FruitsManager instance;

    public override void OnStartServer()
    {
        GenerateFruit();
    }
    private void Awake()
    {
        instance = this;
    }
    public void GenerateFruit()
    {
        var x = MinMaxStep(minX, maxX, variation);
        var y = MinMaxStep(minY, maxY, variation);
        var position = new Vector3(x, y, 0f);
        var fruit = Instantiate(fruitPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(fruit);
    }
    float MinMaxStep(float min, float max, float variation)
    {
        return ((int)Random.Range(min, max)) + variation;
    }
}
