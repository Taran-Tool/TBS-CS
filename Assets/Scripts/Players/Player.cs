using System;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public string playerName;
    public ICommunicationAdapter communicationAdapter;
    protected GameObject playerObject;
    protected int health;
    public bool isAlive => health > 0;

    protected bool isBarrierActive;
    protected bool isRegenerating;
    protected bool isBurning;

    protected int barrierCooldown;
    protected int regenerationCooldown;
    protected int fireballCooldown;
    protected int cleanseCooldown;

    protected int barrierTurnsRemaining;
    protected int regenerationTurnsRemaining;
    protected int burnTurnsRemaining;

    public event Action OnTurnEnd;

    protected Core core;

    public virtual void Init(string name, ICommunicationAdapter adp, GameObject go)
    {
        playerName = name;
        communicationAdapter = adp;
        playerObject = go;
        go.name = name;
        health = 50;
        isBarrierActive = false;
        isRegenerating = false;
        isBurning = false;
        barrierCooldown = 0;
        regenerationCooldown = 0;
        fireballCooldown = 0;
        cleanseCooldown = 0;
        barrierTurnsRemaining = 0;
        regenerationTurnsRemaining = 0;
        burnTurnsRemaining = 0;
    }

    private void Awake()
    {
        core = GameObject.Find("Arena").GetComponent<Core>();
    }

    public void EndTurn()
    {
        OnTurnEnd?.Invoke();
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetToBase()
    {
        health = 50;
        isBarrierActive = false;
        isRegenerating = false;
        isBurning = false;
        barrierCooldown = 0;
        regenerationCooldown = 0;
        fireballCooldown = 0;
        cleanseCooldown = 0;
        barrierTurnsRemaining = 0;
        regenerationTurnsRemaining = 0;
        burnTurnsRemaining = 0;
    }

    public void ApplyDamage(int damage)
    {
        if (isBarrierActive && barrierTurnsRemaining>0)
        {
            damage = Mathf.Max(damage - 5, 0);
        }
        health -= damage;
    }

    public void Regenerate()
    {
        if (isRegenerating && regenerationTurnsRemaining > 0)
        {
            health += 2;
            regenerationTurnsRemaining--;
            if (regenerationTurnsRemaining==0)
            {
                core.StatusIsExpired(3, this);
            }
        }
        else
        {
            isRegenerating = false;
            core.StatusIsExpired(3, this);
        }
    }

    public void StartBurnEffect()
    {
        if (isBurning && burnTurnsRemaining > 0)
        {
            ApplyDamage(1);
            burnTurnsRemaining--;
            if (burnTurnsRemaining==0)
            {
                core.StatusIsExpired(4, this);
            }
        }
        else
        {
            isBurning = false;
            core.StatusIsExpired(4, this);
        }
    }

    public void Shielded()
    {
        if (isBarrierActive && barrierTurnsRemaining > 0)
        {
            barrierTurnsRemaining--;
            if (barrierTurnsRemaining == 0)
            {
                core.StatusIsExpired(2, this);
            }
        }
        else
        {
            isBarrierActive = false;
            core.StatusIsExpired(2, this);
        }
    }

    public void ApplyFireball()
    {
        if (fireballCooldown<=0)
        {
            isBurning = true;
            burnTurnsRemaining = 5;
            ApplyDamage(5);
        }        
    }

    public void SetFireballUsed()
    {
        fireballCooldown = 6;
    }

    public void ApplyCleanse()
    {
        if (cleanseCooldown<=0)
        {
            isBurning = false;
            burnTurnsRemaining = 0;
            cleanseCooldown = 5;
        }
    }

    public void ApplyBarrier()
    {
        if (barrierCooldown==0)
        {
            isBarrierActive = true;
            barrierTurnsRemaining = 2;
            barrierCooldown = 4;
        }
    }

    public void ApplyRegeneration()
    {
        if (regenerationCooldown<=0)
        {
            isRegenerating = true;
            regenerationTurnsRemaining = 3;
            regenerationCooldown = 5;
        }        
    }

    public void StartCooldowns()
    {
        if (barrierCooldown>0) 
        {
            barrierCooldown--;
        }
        if (regenerationCooldown > 0)
        {
            regenerationCooldown--;
        }
        if (fireballCooldown > 0)
        {
            fireballCooldown--;
        }
        if (cleanseCooldown > 0)
        {
            cleanseCooldown--;
        }
    }

    public void StatusEffects()
    {
        Regenerate();        
        StartBurnEffect();
        Shielded();
    }

    public void ApplySpell(int spellnumber, Player player)
    {
        switch (spellnumber)
        {
            case 1:
            ApplyDamage(8);
            break;
            case 2:
            ApplyBarrier();
            core.GotStatus(22, player);
            break;
            case 3:
            ApplyRegeneration();
            core.GotStatus(33, player);
            break;
            case 4:
            ApplyFireball();
            core.GotStatus(44, player);
            break;
            case 5:
            ApplyCleanse();
            break;
        }
    }
    public string GetSpellName(int spellNumber)
    {
        string name = "";
        switch (spellNumber)
        {
            case 1:
            name = "Attack";
            break;
            case 2:
            name = "Barrier";
            break;
            case 3:
            name = "Regeneration";
            break;
            case 4:
            name = "Fireball";
            break;
            case 5:
            name = "Cleanse";
            break;
        }
        return name;
    }

    public string GetStatusName(int statusNumber)
    {
        string name = "";
        switch (statusNumber)
        {
            case 22:
            name = "shielded with a Barrier";
            break;
            case 33:
            name = "Regenerating";
            break;
            case 44:
            name = "Burning";
            break;
        }
        return name;
    }

    public bool CheckCooldown(int spellNumber)
    {
        switch (spellNumber)
        {
            case 1:
            return true;
            case 2:
            if (barrierCooldown==0) 
            {
                return true;
            } 
            break;
            case 3:
            if (regenerationCooldown == 0)
            {
                return true;
            }
            break;
            case 4:
            if (fireballCooldown == 0)
            {
                return true;
            }
            break;
            case 5:
            if (cleanseCooldown == 0)
            {
                return true;
            }
            break;
            default:
            return false;
        }
        return false;
    }

    public int GetCooldownValue(int spellNumber)
    {
        int value = 0;
        switch (spellNumber)
        {
            case 2:
            value = barrierCooldown;
            break;
            case 3:
            value = regenerationCooldown;
            break;
            case 4:
            value = fireballCooldown;
            break;
            case 5:
            value = cleanseCooldown;
            break;
        }
        return value;
    }

    //Общаюсь через адаптер
    public void SendMessage(string message, Player recipient)
    {
        communicationAdapter.SendMessage(message, recipient, this);
    }
    public virtual void ReceiveMessage(string message, Player sender)
    {
            ReactToMessage(message, sender);        
    }

    protected abstract void ReactToMessage(string message, Player sender);

    public abstract void PerformAction();
}
