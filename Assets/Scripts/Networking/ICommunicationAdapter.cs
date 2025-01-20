using UnityEngine;
public interface ICommunicationAdapter
{
    void SendMessage(string message, Player recipient, Player sender);
    void ReceiveMessage(string message, Player sender);
}
