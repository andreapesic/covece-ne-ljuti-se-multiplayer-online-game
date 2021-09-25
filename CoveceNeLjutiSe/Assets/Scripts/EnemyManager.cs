using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyManager : MonoBehaviour
{
   
    public Dictionary<int, List<Piece>> enemies = new Dictionary<int, List<Piece>>();
    public List<Piece> list = new List<Piece>();
    public void AddEnemie(Piece o, int id, int i, colorType color)
    {
        o.color = color;
        o.id = i;
        list.Add(o);



      

        if (list.Count == 4) {

           
            enemies.Add(id, list.Select(x=>x).ToList());
        
            list.Clear();

        }
       
     
    }

      
    
    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else if (instance != this)
        //{
        //    Debug.Log("Instance already exists, destroying object!");
        //    Destroy(this);
        //}
    }
    public void IzadjiIzKucice(int enemyId, int pieceId, Vector2 where)
    {
     
        MainThreadManager.ExecuteOnMainThread(() =>
        {

           Piece p= enemies[enemyId][pieceId];

            GameManager.instance.IzvediIgraca(p, where);
        });
       
       

    }
    public void Move(int enemyId,int pieceId,Vector2 where)
    {
      
        Piece p = enemies[enemyId][pieceId];

        GameManager.instance.IdiKaPolju(p, where);
    }

    internal void RemoveEmeny(int id, int piece_id, Vector2 pos)
    {
        Debug.Log("Pojeoga ga je ");
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            enemies[id][piece_id].DestoryMe(false, pos);          
            enemies[id][piece_id].transform.position = pos;
        });
    }
}
