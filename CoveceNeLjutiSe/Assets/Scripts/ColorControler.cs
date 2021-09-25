using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControler : MonoBehaviour
{
    colorType color=colorType.NONE;
   // List<colorType> DisabledColors;
    public void RedPressed()
    {
        color = colorType.RED;
    }
    public void BluePressed()
    {
        color = colorType.BLUE;
    }
    public void YellowPressed()
    {
        color = colorType.YELLOW;
    }
    public void GreenPressed()
    {
        color = colorType.GREEN;
    }
    public void Close()
    {
        if (color != colorType.NONE)
        {
            Client.instance.user.colorType = color;
            PacketSender.ColorChoosed(((int)color));
        }
        this.gameObject.SetActive(false);
    }
}
