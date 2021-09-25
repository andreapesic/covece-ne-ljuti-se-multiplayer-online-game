using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public static Dice instance;
    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite four;
    public Sprite five;
    public Sprite six;
    Animator a;
    bool isPlaying;
  
    Button b;
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
        b = GetComponent<Button>();
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
        a = GetComponent<Animator>();
        a.enabled = false;
        
    }

    public void  StartRoll()
    {
        if (!isPlaying)
        {
            a.enabled = true;
          
            isPlaying = true;
           
            PacketSender.SendDiceRollStarted();
        }
      
    }
    public void StopRoll(int num)
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {       
            StartCoroutine(SetSprite(num));
        });
    }

    public void disableButton()
    {
        
            b.enabled = false;
    }
    public void enableButton()
    {
        
            b.enabled = true;
    }
   
    
    
    private IEnumerator SetSprite(int num)
    {
        yield return new WaitForSeconds(1);

        a.enabled = false;
        isPlaying = false;
 
      
       
      
        GameManager.instance.ThrowDice(num);
     
        switch (num)
        {

            case 1: b.image.sprite = one; break;
            case 2: b.image.sprite = two; break;
            case 3: b.image.sprite = three; break;
            case 4: b.image.sprite = four; break;
            case 5: b.image.sprite = five; break;
            case 6: b.image.sprite = six; break;
        }
    }
    
}
