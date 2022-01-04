using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Model/Body")]
public class BodySO : ScriptableObject
{
    public GameObject bodyType; //kad�n, erkek, uzayl�
    public Color color;  //ten rengi
    public string colorHex;
    public string colorName;

    public string GetRecipeText()
    {
        return $" <color=#{colorHex}>{colorName}</color> Body";
    }
}
