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

    public GameObject FinalHouseBlue;
    public GameObject FinalHouseRed;
    public GameObject FinalHouseGreen;
    public GameObject FinalHouseYellow;

    Spawner instance;

    void Start()
    {
        DontDestroyOnLoad(this);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(User u)
    {
        switch (u.color)
        {
            case colorType.BLUE:SpawnBlue(u.id); break;
            case colorType.RED:  SpawnRed(u.id); break;
            case colorType.YELLOW: SpawnYellow(u.id); break;
            case colorType.GREEN:SpawnGreen(u.id); break;
        }
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

        Server.clients[id].user.p = new Player();

        for (int i = 0; i < 4; i++)
        {
           
            GameObject o =Instantiate(p);
            o.transform.position = spawnerinfo.pos[i];
            Piece piece= o.GetComponent<Piece>();
            piece.color = (colorType)spawnerinfo.color;
            piece.spawnPos= spawnerinfo.pos[i];
            piece.id = i;
            piece.mainParentId = id;
            Server.clients[id].user.p.player.Add(piece);
            //PlayerManager.instance.AddPlayer(piece,i,(colorType)spawnerinfo.color);
            
            
          

        }


    }

    internal void SpawnPojedenog(User u, int id)
    {
        switch (u.color)
        {
            case colorType.BLUE: SpawnBluePiece(u.id,id); break;
            case colorType.RED: SpawnRedPice(u.id,id); break;
            case colorType.YELLOW: SpawnYellowPiece(u.id,id); break;
            case colorType.GREEN: SpawnGreenPiece(u.id,id); break;
        }
    }
    private void SpawnGreenPiece(int id, int idPiece)
    {
        SpawnPiece(green, greenSpawner, id, idPiece);
    }

    private void SpawnYellowPiece(int id, int idPiece)
    {
        SpawnPiece(yellow, yellowSpawner, id, idPiece);
    }

    private void SpawnRedPice(int id, int idPiece)
    {
        SpawnPiece(red, redSpawner, id, idPiece);
    }

    private void SpawnBluePiece(int id, int idPiece)
    {
        SpawnPiece(blue, blueSpawner, id,idPiece);
    }
    public void SpawnPiece(GameObject p, SpawnerInfo info, int id, int idPiece)
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            GameObject o = Instantiate(p);
            Piece piece = o.GetComponent<Piece>();
            piece.color = (colorType)info.color;
            piece.spawnPos = info.pos[idPiece];
            piece.id = idPiece;
            piece.mainParentId = id;
            o.transform.position = info.pos[idPiece];
        });


    }
}
