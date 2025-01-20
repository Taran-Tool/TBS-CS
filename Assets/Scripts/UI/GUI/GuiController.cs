using UnityEngine;
using UnityEngine.UIElements;
using System;
public class GuiController : MonoBehaviour
{
    private VisualElement rootVisualElement;
    private Button playButton;
    private Button attackButton;
    private Button barrierButton;
    private Button regenButton;
    private Button fireballButton;
    private Button cleansingButton;
    public Label ResultText;
    private Button resultButton;

    public delegate void PlayButtonClickedHandler();
    public event PlayButtonClickedHandler OnPlayButtonClickedEvent;
    public delegate void AttackButtonClickedHandler();
    public event AttackButtonClickedHandler OnAttackButtonClickedEvent;
    public delegate void BarrierButtonClickedHandler();
    public event BarrierButtonClickedHandler OnBarrierButtonClickedEvent;
    public delegate void RegenButtonClickedHandler();
    public event RegenButtonClickedHandler OnRegenButtonClickedEvent;
    public delegate void FireballButtonClickedHandler();
    public event FireballButtonClickedHandler OnFireballButtonClickedEvent;
    public delegate void CleansingButtonClickedHandler();
    public event CleansingButtonClickedHandler OnCleansingButtonClickedEvent;
    public delegate void ResultButtonClickedHandler();
    public event ResultButtonClickedHandler OnResultButtonClickedEvent;

    void Start()
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        ResultText = rootVisualElement.Q<VisualElement>("ResultMenu").Q<Label>("ResultText");
        SetButtons();
    }

    void SetButtons()
    {
        playButton = rootVisualElement.Q<VisualElement>("StartMenu").Q<Button>("Startlocal");
        playButton.clicked += OnPlayButtonClicked;

        attackButton = rootVisualElement.Q<VisualElement>("SpellMenu").Q<Button>("AttackSpell");
        attackButton.clicked += OnAttackButtonClicked;

        barrierButton = rootVisualElement.Q<VisualElement>("SpellMenu").Q<Button>("BarrierSpell");
        barrierButton.clicked += OnBarrierButtonClicked;

        regenButton = rootVisualElement.Q<VisualElement>("SpellMenu").Q<Button>("RegenSpell");
        regenButton.clicked += OnRegenButtonClicked;

        fireballButton = rootVisualElement.Q<VisualElement>("SpellMenu").Q<Button>("FireballSpell");
        fireballButton.clicked += OnFireballButtonClicked;

        cleansingButton = rootVisualElement.Q<VisualElement>("SpellMenu").Q<Button>("CleansingSpell");
        cleansingButton.clicked += OnCleansingButtonClicked;

        resultButton = rootVisualElement.Q<VisualElement>("ResultMenu").Q<Button>("Restart");
        resultButton.clicked += OnResultButtonClicked;
    }

    public void SetAsCooldownSpell(int spellNumber)
    {
        Button selectedButton = null;
        switch (spellNumber)
        {
            case 2:
            selectedButton = barrierButton;
            break;
            case 3:
            selectedButton = regenButton;
            break;
            case 4:
            selectedButton = fireballButton;
            break;
            case 5:
            selectedButton = cleansingButton;
            break;
        }
        selectedButton.style.borderTopColor = Color.red;
        selectedButton.style.borderRightColor = Color.red;
        selectedButton.style.borderBottomColor = Color.red;
        selectedButton.style.borderLeftColor = Color.red;
        selectedButton.style.borderTopWidth = 3f;
        selectedButton.style.borderRightWidth = 3f;
        selectedButton.style.borderBottomWidth = 3f;
        selectedButton.style.borderLeftWidth = 3f;
        selectedButton.SetEnabled(false);
        selectedButton.style.opacity = 0.5f;
    }

    public void SetCooldownText(int spellNumber, Player player)
    {
        string cooldownText = player.GetCooldownValue(spellNumber) == 0 ? "" : player.GetCooldownValue(spellNumber).ToString();
        Button buttonToUpdate = null;

        switch (spellNumber)
        {
            case 2:
            buttonToUpdate = barrierButton;
            break;
            case 3:
            buttonToUpdate = regenButton;
            break;
            case 4:
            buttonToUpdate = fireballButton;
            break;
            case 5:
            buttonToUpdate = cleansingButton;
            break;
        }

        if (buttonToUpdate != null)
        {
            buttonToUpdate.Q<Label>("Cooldown").text = cooldownText;
        }
    }

    public void SetAsReadySpell(int spellNumber)
    {
        Button selectedButton = null;
        switch (spellNumber)
        {
            case 2:
            selectedButton = barrierButton;
            break;
            case 3:
            selectedButton = regenButton;
            break;
            case 4:
            selectedButton = fireballButton;
            break;
            case 5:
            selectedButton = cleansingButton;
            break;
        }
        selectedButton.style.borderTopColor = Color.black;
        selectedButton.style.borderRightColor = Color.black;
        selectedButton.style.borderBottomColor = Color.black;
        selectedButton.style.borderLeftColor = Color.black;
        selectedButton.style.borderTopWidth = 1f;
        selectedButton.style.borderRightWidth = 1f;
        selectedButton.style.borderBottomWidth = 1f;
        selectedButton.style.borderLeftWidth = 1f;
        selectedButton.SetEnabled(true);
        selectedButton.style.opacity = 1f;
    }

    private void OnPlayButtonClicked()
    {
        OnPlayButtonClickedEvent?.Invoke();
    }
    private void OnAttackButtonClicked()
    {
        OnAttackButtonClickedEvent?.Invoke();
    }
    private void OnBarrierButtonClicked()
    {
        OnBarrierButtonClickedEvent?.Invoke();
    }
    private void OnRegenButtonClicked()
    {
        OnRegenButtonClickedEvent?.Invoke();
    }
    private void OnFireballButtonClicked()
    {
        OnFireballButtonClickedEvent?.Invoke();
    }
    private void OnCleansingButtonClicked()
    {
        OnCleansingButtonClickedEvent?.Invoke();
    }

    private void OnResultButtonClicked()
    {
        OnResultButtonClickedEvent?.Invoke();
    }

    public void ShowMainMenu()
    {
        rootVisualElement.Q<VisualElement>("StartMenu").visible = true;
        rootVisualElement.Q<VisualElement>("SpellMenu").visible = false;
        rootVisualElement.Q<VisualElement>("ResultMenu").visible = false;
    }
    public void ShowSpellMenu()
    {
        rootVisualElement.Q<VisualElement>("StartMenu").visible = false;
        rootVisualElement.Q<VisualElement>("SpellMenu").visible = true;
        rootVisualElement.Q<VisualElement>("ResultMenu").visible = false;
    }
    public void ShowResultMenu()
    {
        rootVisualElement.Q<VisualElement>("StartMenu").visible = false;
        rootVisualElement.Q<VisualElement>("SpellMenu").visible = false;
        rootVisualElement.Q<VisualElement>("ResultMenu").visible = true;
    }
    public void HideAll()
    {
        rootVisualElement.Q<VisualElement>("StartMenu").visible = false;
        rootVisualElement.Q<VisualElement>("SpellMenu").visible = false;
        rootVisualElement.Q<VisualElement>("ResultMenu").visible = false;
    }
}
