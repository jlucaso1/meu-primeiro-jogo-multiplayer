using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Mirror;
using UnityEngine.UI;
/*
	Documentation: https://mirror-networking.com/docs/Components/NetworkManager.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class MainNetworkManager : NetworkManager
{
    float minX = 0.5f;
    float maxX = 9.5f;
    float minY = -4.5f;
    float maxY = 4.5f;
    float variation = 0.5f;


    public RectTransform playersPanel;
    internal readonly List<Player> playersList = new List<Player>();
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //base.OnServerAddPlayer(conn);
        GameObject player = Instantiate(playerPrefab, new Vector3(MinMaxStep(minX, maxX), MinMaxStep(minY, maxY)), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);
        ResetPlayerNumbers();
    }
    

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        ResetPlayerNumbers();
    }

    void ResetPlayerNumbers()
    {
        int playerNumber = 1;
        foreach (Player player in playersList)
        {
            player.playerNumber = playerNumber;
            playerNumber++;
        }
    }

    float MinMaxStep(float min, float max)
    {
        return ((int)Random.Range(min, max)) + variation;
    }
}
