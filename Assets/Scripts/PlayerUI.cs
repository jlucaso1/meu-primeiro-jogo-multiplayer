using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [Header("Player Components")]
    public Image image;

    [Header("Child Text Objects")]
    public Text playerNameText;
    public Text playerPointsText;

    public Player player;
    public void SetPlayer(Player player, bool isLocalPlayer)
    {
        // cache reference to the player that controls this UI object
        this.player = player;
        // subscribe to the events raised by SyncVar Hooks on the Player object
        player.OnPlayerNumberChanged += OnPlayerNumberChanged;
        player.OnPlayerScoreChanged += OnPlayerScoreChanged;

        // add a visual background for the local player in the UI
        if (isLocalPlayer)
            image.color = new Color(0, 255, 0, 0.1f);
    }

    void OnDisable()
    {
        player.OnPlayerNumberChanged -= OnPlayerNumberChanged;
    }

    // This value can change as clients leave and join
    void OnPlayerScoreChanged(int newPlayerPoints)
    {
        playerPointsText.text = $"Points: {newPlayerPoints}";
    }
    void OnPlayerNumberChanged(int newPlayerNumber)
    {
        playerNameText.text = $"Player {newPlayerNumber}";
    }
}
