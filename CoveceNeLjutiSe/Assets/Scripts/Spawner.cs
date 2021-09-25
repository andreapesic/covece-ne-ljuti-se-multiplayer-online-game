using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject blue;
    public GameObject red;
    public GameObject yellow;
    public GameObject green;

    public SpawnerInfo blueSpawner;
    public SpawnerInfo redSpawner;
    public SpawnerInfo yellowSpawner;
    public SpawnerInfo greenSpawner;

    public GameObject PathBlue;
    public GameObject PathYellow;
    public GameObject PathGreen;
    public GameObject PathRed;

    public void SpawnPlayer(User u)
    {
       
      
        switch (u.colorType)
        {
            case colorType.BLUE:SpawnBlue(u.id); break;
            case colorType.RED:  SpawnRed(u.id); break;
            case colorType.YELLOW: SpawnYellow(u.id); break;
            case colorType.GREEN:SpawnGreen(u.id); break;
        }
    }
    public void SpawnPojedenog(User u, float xspawn, float yspawn)
    {
        Vector2 pos = new Vector2(xspawn, yspawn);
        switch (u.colorType)
        {
            case colorType.BLUE: SpawnBluePiece(u.id, pos); break;
            case colorType.RED: SpawnRedPice(u.id, pos); break;
            case colorType.YELLOW: SpawnYellowPiece(u.id, pos); break;
            case colorType.GREEN: SpawnGreenPiece(u.id, pos); break;
        }
    }

    private void SpawnGreenPiece(int id, Vector2 pos)
    {
        SpawnPiece(green, greenSpawner, id,pos);
    }

    private void SpawnYellowPiece(int id, Vector2 pos)
    {
        SpawnPiece(yellow, yellowSpawner, id, pos);
    }

    private void SpawnRedPice(int id, Vector2 pos)
    {
        SpawnPiece(red, redSpawner, id, pos);
    }

    private void SpawnBluePiece(int id, Vector2 pos)
    {
        SpawnPiece(blue, blueSpawner, id, pos);
    }
    public void SpawnPiece(GameObject p ,SpawnerInfo info,int id, Vector2 pos)
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            GameObject o = Instantiate(p);
            o.transform.position = pos;
        });
        
       
    }

    private void SpawnBlue(int id)
    {
        Spawn(blue,blueSpawner,id);
    }
    private void SpawnRed(int id)
    {
        Spawn(red,redSpawner,id);
    }
    private void SpawnYellow(int id)
    {
        Spawn(yellow,yellowSpawner,id);
    }
    private void SpawnGreen(int id)
    {
        Spawn(green,greenSpawner,id);
    }
    private void Spawn(GameObject p, SpawnerInfo spawnerinfo,int id)
    {
     

        MainThreadManager.ExecuteOnMainThread(() =>
        {
         
            for (int i = 0; i < 4; i++)
            {
                GameObject o = Instantiate(p);

                o.transform.position = spawnerinfo.pos[i];
                Piece piece = o.GetComponent<Piece>();
                if (id == Client.instance.user.id)
                {
                   GameManager.instance.AddPlayer(piece, i, (colorType)spawnerinfo.color);
                }
                else
                {

                    GameManager.instance.AddEnemie(piece, id,i, (colorType)spawnerinfo.color);
                }

            }
        });
        


    }
}
