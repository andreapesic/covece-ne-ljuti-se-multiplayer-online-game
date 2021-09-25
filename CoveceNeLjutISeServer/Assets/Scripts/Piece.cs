using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool InHouse = true;
    public int mainParentId;
    public bool isFinished;
    public int id;
    public colorType color;
    public int currentField;
    public bool onSpawn;
    public new bool   enabled=false;
    public Vector2 spawnPos;
    private int offset = 10;
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Piece pieceThatWantsToEatYou = collision.gameObject.GetComponent<Piece>();
        if (isFinished || pieceThatWantsToEatYou.isFinished) return;
        if (Server.clients[mainParentId].user.isOnMove) return;
        int pom = 0;
        switch (color)
        {
            case colorType.RED:
                switch (pieceThatWantsToEatYou.color)
                {
                    case colorType.RED: pom = -1; break;
                    case colorType.YELLOW: pom = 3*offset; break;
                    case colorType.BLUE: pom = 2 * offset; break;
                    case colorType.GREEN: pom =  offset; break;
                }
                break;
            case colorType.YELLOW:
                switch (pieceThatWantsToEatYou.color)
                {
                    case colorType.RED: pom = offset; break;
                    case colorType.YELLOW: pom = -1; break;
                    case colorType.BLUE: pom = 3 * offset; break;
                    case colorType.GREEN: pom = 2 * offset; break;
                }
                break;
            case colorType.BLUE:
                switch (pieceThatWantsToEatYou.color)
                {
                    case colorType.RED: pom = 2*offset; break;
                    case colorType.YELLOW: pom = offset; break;
                    case colorType.BLUE: pom = -1; break;
                    case colorType.GREEN: pom = 3* offset; break;
                }
                break;
            case colorType.GREEN:
                switch (pieceThatWantsToEatYou.color)
                {
                    case colorType.RED: pom = 3 * offset; break;
                    case colorType.YELLOW: pom =2* offset; break;
                    case colorType.BLUE: pom = offset; break;
                    case colorType.GREEN: pom = -1; break;
                }
                break;
        }      
        if (pieceThatWantsToEatYou.currentField-pom>=0 &&  pieceThatWantsToEatYou.currentField- pom != currentField) 
            return;
        InHouse = true;
        currentField = 0;
        PacketSender.SendPojeoGa(id, mainParentId,spawnPos);
        transform.position = spawnPos;
        InHouse = true;
        currentField = 0;
        onSpawn = false;
        enabled = false;
        PlayerManager.instance.resetPlayer();
    }
}
