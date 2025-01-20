using UnityEngine;
using UnityEngine.UIElements;
public class StatusIcons : MonoBehaviour
{
    public Camera mainCamera;
    private VisualElement rootVisualElement;
    private VisualElement playerIconContainer;
    private VisualElement aiIconContainer;
    private VisualElement HPPlayerContainer;
    private VisualElement HPAIContainer;
    private ProgressBar HPPlayer;
    private ProgressBar HPAI;
    private GameObject playerLocal;
    private GameObject playerAI;
    public float iconsSize = 4f;
    public float barSize = 8f;



    private void Awake()
    {
        rootVisualElement = gameObject.GetComponent<UIDocument>().rootVisualElement;
    }

    private void Start()
    {
        playerIconContainer = rootVisualElement.Q<VisualElement>("IconsPlayer");
        aiIconContainer = rootVisualElement.Q<VisualElement>("IconsAI");
        HPPlayerContainer = rootVisualElement.Q<VisualElement>("HPPlayer");
        HPAIContainer = rootVisualElement.Q<VisualElement>("HPAI");
        HPPlayer = HPPlayerContainer.Q<ProgressBar>("ProgressBar");
        HPAI = HPAIContainer.Q<ProgressBar>("ProgressBar");
    }

    public void SetPlayers(GameObject player, GameObject ai)
    {
        playerLocal = player;
        playerAI = ai;
    }

    private void Update()
    {
        if (playerLocal != null && playerAI != null)
        {
            MoveToPosition(playerIconContainer, playerLocal.transform.position + Vector3.up * 1.1f, Camera.main, Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)), iconsSize);
            MoveToPosition(aiIconContainer, playerAI.transform.position + Vector3.up * 1.1f, Camera.main, Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)), iconsSize);

            MoveToPosition(HPPlayerContainer, playerLocal.transform.position + Vector3.up * 1.5f , Camera.main, Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)), barSize);
            MoveToPosition(HPAIContainer, playerAI.transform.position + Vector3.up * 1.5f , Camera.main, Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)), barSize);
        }
    }

    private void MoveToPosition(VisualElement element, Vector3 worldPosition, Camera cam, Vector3 screenCenter, float mult)
    {
        Vector3 screen = cam.WorldToScreenPoint(worldPosition);
        screen.y = cam.pixelHeight - screen.y;
        float depth = screen.z;
        screen.z = 0f;
        ITransform tform = element.transform;
        tform.position = screen - screenCenter;
        tform.scale = (depth <= 0f ? Vector3.zero : Vector3.one / depth)* mult;
    }

    public void ShowStatus(int spellNumber, Player player)
    {
        if (player is PlayerAI)
        {
            aiIconContainer.Q<VisualElement>(spellNumber.ToString()).visible = true;
        }
        if (player is PlayerLocal)
        {
            playerIconContainer.Q<VisualElement>(spellNumber.ToString()).visible = true;
        }
    }

    public void HideStatus(int spellNumber, Player player)
    {
        if (player is PlayerAI)
        {
            aiIconContainer.Q<VisualElement>(spellNumber.ToString()).visible = false;
        }
        if (player is PlayerLocal)
        {
            playerIconContainer.Q<VisualElement>(spellNumber.ToString()).visible = false;
        }
    }

    public void SetPlayerHP(Player player)
    {
        if (HPPlayerContainer.visible == false && HPAIContainer.visible == false)
        {
            HPPlayerContainer.visible = true;
            HPAIContainer.visible = true;
        }
        if (player is PlayerAI)
        {
            ChangeHP(player, HPAI);
        }
        if (player is PlayerLocal)
        {
            ChangeHP(player, HPPlayer);
        }
    }

    private void ChangeHP(Player player, ProgressBar bar)
    {
        bar.lowValue = 0f;
        bar.highValue = 50f;
        bar.value = player.GetHealth();
        bar.title = player.playerName + ": " + player.GetHealth();
    }
}
