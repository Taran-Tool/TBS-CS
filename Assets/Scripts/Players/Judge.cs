using UnityEngine;

public class Judge :Player
{
    public override void Init(string name, ICommunicationAdapter adp, GameObject go)
    {
        base.Init(name, adp, go);
    }
    protected override void ReactToMessage(string message, Player sender)
    {
        if (message.Length == 1)
        {
            Debug.Log($"{sender.playerName} uses {GetSpellName(int.Parse(message))} spell!");
        }
        else if (message.Length == 2)
        {
            Debug.Log($"{sender.playerName} is {GetStatusName(int.Parse(message))}!");
        }
    }

    public override void PerformAction()
    {
        Debug.Log($"{playerName} makes a judgment.");
    }
}
