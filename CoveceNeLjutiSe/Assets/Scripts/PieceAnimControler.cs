using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAnimControler
{
    List<Animator> a;
   


   internal  PieceAnimControler(List<Piece> player)
    {
        a = new List<Animator>();
        foreach(Piece p in player)
        {
            Animator anim = p.gameObject.GetComponent<Animator>();
            anim.enabled = false;
            a.Add(anim);

            
        }
    }
    public bool StartAnimForNotHause(int num )
    {
        bool can = false;
        foreach (Animator anim in a)
        {
            Piece p = anim.gameObject.GetComponent<Piece>();

      
            if( p.isFinished && p.currentField + num <= 4 && !p.inFronOf) {
                anim.enabled = true;
                p.enabled = true;
               
                anim.SetBool("canSelect", true);
                can= true;
            }
           else  if (!p.InHouse && !p.inFronOf && !p.isFinished )
            {
                if (p.currentField + num > 39)
                {
                    int trebaDaIdeDoKrajaMape = 39 - p.currentField;
                    int uKuciciFinalDaIde = Math.Abs(trebaDaIdeDoKrajaMape - num);
                    if (uKuciciFinalDaIde <= 4)
                    {
                        anim.enabled = true;
                        p.enabled = true;
                        anim.SetBool("canSelect", true);
                        can = true;
                    }
                    else
                    {
                        anim.enabled = false;
                        p.enabled = false;
                       
                    }

                }
                else
                {
                    Debug.Log("Ovde");
                    anim.enabled = true;
                    p.enabled = true;
                    anim.SetBool("canSelect", true);
                    can = true;
                }





            }
            else
            {
                    anim.enabled = false;
                    p.enabled = false;
                
            }
        }
        return can;
    }
    public bool StartAnim()
    {
        bool can=false;
        foreach(Animator anim in a)
        {
            Piece p = anim.gameObject.GetComponent<Piece>();
            if (p.isFinished || p.inFronOf)
            {
                p.enabled = false;
                anim.enabled = false;
                
            }
           else  if ( p.currentField+6 <=43  ){
                anim.enabled = true;
                p.enabled = true;
                anim.SetBool("canSelect", true);
                can=  true;
            }
        }
        return can;
    }
    public void StopAnim()
    {
        foreach (Animator anim in a)
        {
            anim.SetBool("canSelect", false);
            anim.enabled = false;
            Piece p = anim.gameObject.GetComponent<Piece>();
            p.enabled = false;
            p.GetComponent<SpriteRenderer>().color =Color.white;

        }
    }
  
    
}
