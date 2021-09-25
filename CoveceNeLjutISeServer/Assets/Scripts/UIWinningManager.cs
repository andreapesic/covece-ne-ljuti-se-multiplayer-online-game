using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinningManager : MonoBehaviour
{
    public Text t;

    public void SetWinningText(string s)
    {
        t.text = s.ToString();
    }

}
