using System.Collections;
using UnityEngine;

public class PlayerAI : Player
{
    public override void Init(string name, ICommunicationAdapter adp, GameObject go)
    {
        base.Init(name, adp, go);
    }

    protected override void ReactToMessage(string message, Player sender)
    {
        if (sender is Judge)
        {
            Debug.Log($"{sender.playerName}: {message}");
        }
        if (message.Length == 1)
        {
            int msg = int.Parse(message);
            
            if (sender is PlayerLocal)
            {
                ApplySpell(int.Parse(message), this);
                //если заклинания в дальнейшем станут не событием а объектом - создать свойство "действие заклинания на него, но cooldown мне"
                if (msg == 4)
                {
                    sender.SetFireballUsed();
                }
            }
            CompleteTurn();
        }                
    }
    public override void PerformAction()
    {
        StartCoroutine(MakeDesigion());
    }
    IEnumerator MakeDesigion()
    {
        int randomTime = Random.Range(2, 5);
        yield return new WaitForSeconds(randomTime);
        int randomSpellNumber = 0;
        bool isValidSpell = false;
        while (!isValidSpell)
        {
            randomSpellNumber = Random.Range(1, 6);
            isValidSpell = CheckCooldown(randomSpellNumber);
        }
        core.SendSpell(randomSpellNumber);
    }

    public void CompleteTurn()
    {
        EndTurn();
    }
}
