using UnityEngine;
using Mirror;


public class Player : NetworkBehaviour
{
    public SpriteRenderer sprite;
    public UnityEngine.UI.Text text_name;

    // Events that the UI will subscribe to
    public event System.Action<int> OnPlayerNumberChanged;
    public event System.Action<int> OnPlayerScoreChanged;

    [Header("Player UI")]
    public GameObject playerUIPrefab;
    GameObject playerUI;

    [Header("SyncVars")]

    /// <summary>
    /// This is appended to the player name text, e.g. "Player 01"
    /// </summary>
    [SyncVar(hook = nameof(PlayerNumberChanged))]
    public int playerNumber = 0;

    /// <summary>
    /// This is updated by UpdateData which is called from OnStartServer via InvokeRepeating
    /// </summary>
    [SyncVar(hook = nameof(PlayerScoreChanged))]
    public int playerScore = 0;


    // This is called by the hook of playerNumber SyncVar above
    void PlayerNumberChanged(int _, int newPlayerNumber)
    {
        OnPlayerNumberChanged?.Invoke(newPlayerNumber);
    }

    // This is called by the hook of playerData SyncVar above
    void PlayerScoreChanged(int _, int newPlayerData)
    {
        UpdatePanelOrder();
        OnPlayerScoreChanged?.Invoke(newPlayerData);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        ((MainNetworkManager)NetworkManager.singleton).playersList.Add(this);
    }
    public override void OnStopServer()
    {
        CancelInvoke();
        ((MainNetworkManager)NetworkManager.singleton).playersList.Remove(this);
    }

    private void Start()
    {
        text_name.text = playerNumber.ToString();
        if (isLocalPlayer)
        {
            sprite.color = Color.green;
            sprite.sortingOrder = 2;
            GetComponentInChildren<Canvas>().sortingOrder = 3;
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.W) && transform.position.y < 4.5f)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1);
        }
        if (Input.GetKeyDown(KeyCode.S) && transform.position.y > -4.5f)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        }
        if (Input.GetKeyDown(KeyCode.A) && transform.position.x > 0.5f)
        {
            transform.position = new Vector2(transform.position.x - 1, transform.position.y);
        }
        if (Input.GetKeyDown(KeyCode.D) && transform.position.x < 9.5f)
        {
            transform.position = new Vector2(transform.position.x + 1, transform.position.y);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(isClient + " " + isServer);
            if (isClient && isServer)
            {
                NetworkManager.singleton.StopHost();
            }
            else
            {
                NetworkManager.singleton.StopClient();
            }
        }
    }

    public override void OnStartClient()
    {
        // Instantiate the player UI as child of the Players Panel
        playerUI = Instantiate(playerUIPrefab, ((MainNetworkManager)NetworkManager.singleton).playersPanel);
        
        // Set this player object in PlayerUI to wire up event handlers
        playerUI.GetComponent<PlayerUI>().SetPlayer(this, isLocalPlayer);

        // Invoke all event handlers with the current data

        OnPlayerNumberChanged.Invoke(playerNumber);
        OnPlayerScoreChanged.Invoke(playerScore);
    }
    public override void OnStopClient()
    {
        Destroy(playerUI);
    }

    [Client]
    void UpdatePanelOrder()
    {
        int HighValue = 0;
        foreach (Transform transform in ((MainNetworkManager)NetworkManager.singleton).playersPanel.transform)
        {
            if(transform.gameObject.GetComponent<PlayerUI>().player.playerScore > HighValue)
            {
                HighValue = transform.gameObject.GetComponent<PlayerUI>().player.playerScore;
                transform.SetAsFirstSibling();
            }
        }
    }
}
