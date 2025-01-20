using UnityEngine;

public class PlayerLocal : Player
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
            
            if (sender is PlayerAI)
            {                
                ApplySpell(int.Parse(message), this);
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
        Debug.Log($"{playerName} performs action as PlayerLocal.");
    }
    public void CompleteTurn()
    {        
        EndTurn();
    }
}

