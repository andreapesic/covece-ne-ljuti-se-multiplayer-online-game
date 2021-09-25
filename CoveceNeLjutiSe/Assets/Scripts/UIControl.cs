using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Text t;
    public void UpdateBacanja(int brojBacanja)
    {
        t.text = brojBacanja.ToString();
    }
}
