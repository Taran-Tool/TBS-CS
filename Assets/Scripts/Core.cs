using UnityEngine;

public class Core:MonoBehaviour
{
    private ICommunicationAdapter adapter;
    private PlayerLocal player;
    private PlayerAI player_ai;
    private Judge judge;
    private Player currentPlayer;
    private Player opponent;

    public GameObject playerPrefab;
    public GameObject localEnemyPrefab;
    public GameObject judgePrefab;

    private GuiController gui;
    private StatusIcons icons;

    private enum GameStage
    {
        Hub,
        GameStart,
        PlayerTurn,
        EndGame
    }
    private GameStage currentStage = GameStage.Hub;

    private int turnsNumber = 1;

    void Start()
    {
        adapter = new LocalCommunicationAdapter();
        gui = GameObject.Find("UI").GetComponent<GuiController>();
        icons = GameObject.Find("IconsAndBars").GetComponent<StatusIcons>();

        gui.ShowMainMenu();
        gui.OnPlayButtonClickedEvent += StartLocalGame;
        gui.OnAttackButtonClickedEvent += () => SpellClicked(1);
        gui.OnBarrierButtonClickedEvent += () => SpellClicked(2);
        gui.OnRegenButtonClickedEvent += () => SpellClicked(3);
        gui.OnFireballButtonClickedEvent += () => SpellClicked(4);
        gui.OnCleansingButtonClickedEvent += () => SpellClicked(5);
        gui.OnResultButtonClickedEvent += Restart;

        GameObject po = Instantiate(playerPrefab, new Vector3(-3, 1.25f, -3), Quaternion.identity);
        GameObject ao = Instantiate(localEnemyPrefab, new Vector3(3, 1.25f, 3), Quaternion.identity);
        GameObject jo = Instantiate(judgePrefab, new Vector3(-3, 1f, 3), Quaternion.identity);

        player = po.GetComponent<PlayerLocal>();
        player.Init("User", adapter, po);
        player_ai = ao.GetComponent<PlayerAI>();
        player_ai.Init("AI", adapter, ao);

        icons.SetPlayers(po, ao);

        judge = jo.GetComponent<Judge>();
        judge.Init("Judge", adapter, jo);

        player.OnTurnEnd += HandlePlayerTurn;
        player_ai.OnTurnEnd += HandlePlayerTurn;

        currentStage = GameStage.Hub;
    }

    private void Restart()
    {
        gui.HideAll();
        currentStage = GameStage.Hub;
        player.SetToBase();
        player_ai.SetToBase();
        icons.SetPlayerHP(player);
        icons.SetPlayerHP(player_ai);
        turnsNumber = 1;
        gui.ShowMainMenu();
    }
    void ChangeStage(GameStage newStage)
    {
        switch (newStage)
        {
            case GameStage.GameStart:
            DisplayGameStartMessage();
            break;

            case GameStage.PlayerTurn:
            HandlePlayerTurn();
            break;

            case GameStage.EndGame:
            EndGame();
            break;
        }
    }
    void StartLocalGame()
    {
        ChangeStage(GameStage.GameStart);
    }

    private void DisplayGameStartMessage()
    {
        judge.SendMessage("Game started!", player);
        ChooseFirstTurnPlayer();
    }

    private void ChooseFirstTurnPlayer()
    {
        int firstPlayer = Random.Range(0, 2);
        if (firstPlayer == 0)
        {
            currentPlayer = player;
            opponent = player_ai;
            
        }
        else
        {
            currentPlayer = player_ai;
            opponent = player;
        }
        icons.SetPlayerHP(player_ai);
        icons.SetPlayerHP(player);
        judge.SendMessage("Battle begins! " + currentPlayer.playerName + " goes first!", player);
        ChangeStage(GameStage.PlayerTurn);
    }

    private void SpellClicked(int spellNumber)
    {
        //нужно будет залинания из событий превратить в объекты со свойствами, в последствии...
        player.SendMessage(spellNumber.ToString(), judge);
        //выключаю иконки заклинаний
        if (spellNumber > 1)
        {
            gui.SetAsCooldownSpell(spellNumber);
        }
        //показываю статусы
        if (spellNumber > 1 && spellNumber < 5)
        {
            if (spellNumber == 4)
            {
                icons.ShowStatus(spellNumber, player_ai);
            }
            else
            {
                icons.ShowStatus(spellNumber, player);
            }
        }
        if (spellNumber == 1 || spellNumber == 4)
        {
            player.SendMessage(spellNumber.ToString(), player_ai);
        }
        else
        {
            player.ApplySpell(spellNumber, player);
            player.CompleteTurn();
        }
    }
    public void SendSpell(int spellNumber)
    {
        player_ai.SendMessage(spellNumber.ToString(), judge);
        if (spellNumber > 1 && spellNumber < 5)
        {
            if (spellNumber == 4)
            {
                icons.ShowStatus(spellNumber, player);
            }
            else
            {
                icons.ShowStatus(spellNumber, player_ai);
            }
        }
        if (spellNumber == 1 || spellNumber == 4)
        {
            player_ai.SendMessage(spellNumber.ToString(), player);
        }
        else
        {
            player_ai.ApplySpell(spellNumber, player_ai);
            player_ai.CompleteTurn();
        }
    }
    public void GotStatus(int statusNumber, Player player)
    {
        player.SendMessage(statusNumber.ToString(), judge);
    }

    public void StatusIsExpired(int statusNumber, Player player)
    {
            icons.HideStatus(statusNumber, player);
    }

    private void HandlePlayerTurn()
    {
        judge.SendMessage($"Turn number: {turnsNumber}. Player HP:{player.GetHealth()}, AI HP: {player_ai.GetHealth()}", player);

        if (player.isAlive && player_ai.isAlive)
        {
            icons.SetPlayerHP(player_ai);
            icons.SetPlayerHP(player);
            if (turnsNumber == 1)
            {
                if (currentPlayer == player)
                {
                    RefreshSpells(player);
                    gui.ShowSpellMenu();
                }
                else
                {
                    gui.HideAll();
                    player_ai.PerformAction();
                }
            }
            else
            {
                if (currentPlayer == player)
                {
                    currentPlayer = player_ai;
                    opponent = player;
                    gui.HideAll();
                    player_ai.PerformAction();
                }
                else
                {
                    currentPlayer = player;
                    opponent = player_ai;
                    RefreshSpells(player);
                    gui.ShowSpellMenu();
                }
                currentPlayer.StatusEffects();
                player.StartCooldowns();
                player_ai.StartCooldowns();
            }
        }
        else
        {
            ChangeStage(GameStage.EndGame);
        }
        turnsNumber++;
    }

    private void EndGame()
    {
        string winner = currentPlayer.isAlive ? currentPlayer.name : opponent.name;
        judge.SendMessage($"{winner} is a winner! There was {turnsNumber} turns.", player);
        gui.ResultText.text = winner + " победил! Совершено " + turnsNumber + " ходов!";
        gui.ShowResultMenu();
    }
    void RefreshSpells(Player player)
    {
        for (int i = 2; i < 6; i++)
        {
            bool check = player.CheckCooldown(i);
            if (check == true)
            {
                gui.SetAsReadySpell(i);
                gui.SetCooldownText(i, player);
            }
            else
            {
                gui.SetCooldownText(i, player);
            }
        }        
    }
}
