using UnityEngine;

public class LocalCommunicationAdapter:ICommunicationAdapter
{
    public void SendMessage(string message, Player recipient, Player sender)
    {
        recipient.ReceiveMessage(message, sender);
    }

    public void ReceiveMessage(string message, Player sender)
    {
        Debug.Log($"Local message received: {message}");
    }
}
