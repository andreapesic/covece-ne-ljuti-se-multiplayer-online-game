using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SpawnerInfo")]
public class SpawnerInfo :ScriptableObject
{
    public colorType color;
    public Vector2[] pos;
    public Vector2 IzlazPos;
    public List<Transform> movingPoints;

}
