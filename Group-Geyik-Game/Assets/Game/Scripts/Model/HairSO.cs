using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Model/Hair")]
public class HairSO : ScriptableObject
{
    public Color color; //sa� rengi
    public string colorHex; //sa� rengi
    public string colorName; // "red", "green", "blue"

    public Sprite sprite;   // UI ekran� i�in 2d resmi

    //? sa� uzunlu�u koycak m�y�z belli de�il
    public float hairLength;

    public string GetRecipeText()
    {
        return $" <color=#{colorHex}>{colorName}</color> Hair";
    }

}
