using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Spawner spawner;
    public Dice dice;
    public GameInfo info;
    public PlayerManager pManager;
    public EnemyManager eManager;

    Vector2 pomerajIgraca = Vector2.zero;
    Piece zaPomeraj;

   public void Update() {
        if (Input.GetMouseButtonDown(0) && pManager.p.turn)
        {
           
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
            if( hit.transform!=null && hit.transform.gameObject.name.Equals(pManager.p.name))
            {
                if(hit.transform.gameObject.GetComponent<Piece>().enabled)

                    pManager.p.TryToEndTurn(hit.transform.gameObject.GetComponent<Piece>().id);
            }

        }
        if ( zaPomeraj !=null && (Vector2)zaPomeraj.gameObject.transform.position != pomerajIgraca)
        {
            zaPomeraj.transform.position=Vector2.Lerp(zaPomeraj.transform.position, pomerajIgraca, 0.4f);
        }else if(zaPomeraj !=null && (Vector2)zaPomeraj.gameObject.transform.position == pomerajIgraca)
        {
            pomerajIgraca = Vector2.zero;
            zaPomeraj = null;
        }
    }
    public void IzvediIgraca(Piece p,Vector2 pos)
    {      
            pomerajIgraca = pos;
            
            zaPomeraj = p;
        
    }

    internal void ThrowDice(int num)
    {
        pManager.ThrowDice(num);
    }

    public void IdiKaPolju(Piece p,Vector2 pos)
    {

        pomerajIgraca = pos;

        zaPomeraj = p;
    }
   
    public void Disconect()
    {
        Reset();
        Client.instance.Disconnect();
        
    }

    private void Reset()
    {
        Client.instance.user.restartinfo();
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
    private void Start()
    {
        info = new GameInfo();
       info.SetFirst(Client.instance.user.whoIsFirst);
    
       
        foreach (User c in PlayersInfo.instance.Players.Values)
        {
            SpawnPlayer(c);
        }
        if (Client.instance.user.id != info.WhoisFirst)
        {
            EnemieTurn();
           
        }
        else
        {
            PlayerTurn();
        }
    }

    

    public void PlayerTurn()
    {
       
        StartDice();
        pManager.Play();
    }
    public void EnemieTurn()
    {
        StopDice();
      

    }

    public void StopDice()
    {
       
            dice.disableButton();
    }
    public void StartDice()
    {
        
            dice.enableButton();
    }
   
    private void SpawnPlayer(User c)
    {
        spawner.SpawnPlayer(c);
    }

    internal void AddPlayer(Piece piece, int i, colorType color)
    {
        pManager.AddPlayer(piece, i, color);
    }
    //======
    internal void VratiNaPocetakPlayer( int pieceId,float xspawn, float yspawn )
    {
        pManager.RemoveFromGame(pieceId, new Vector2(xspawn, yspawn));
    }

    internal void AddEnemie(Piece piece, int id, int i, colorType color)
    {
        eManager.AddEnemie(piece, id, i, color);
    }
   
    internal void VratiNaPocetakEnemy(int id, int pieceId, float xspawn, float yspawn)
    {
        eManager.RemoveEmeny(id, pieceId,new Vector2(xspawn,yspawn));
    }

    internal  void Move(int idPi, float x, float y)
    {
        pManager.Move(idPi, x, y);
    }

    internal void IzadjiIzKucice(int id, int idPi, Vector2 vector2)
    {
        eManager.IzadjiIzKucice(id, idPi, vector2);
    }

    internal void UsaoUKucincu(int idPi, int v)
    {
        pManager.UsaoUKucincu(idPi, v);
    }

    internal void MoveForPolje(int idPi, float x, float y)
    {
        pManager.MoveForPolje(idPi, x, y);
    }

    internal void MoveEnemy(int id, int idPi, Vector2 vector2)
    {
        eManager.Move(id, idPi, vector2);
    }

    internal void PromeniVarijableNeprijatelja(int id, int idPi, int v, bool v2)
    {
        eManager.enemies[id][idPi].currentField = v;
        eManager.enemies[id][idPi].isFinished = true;
    }
}

