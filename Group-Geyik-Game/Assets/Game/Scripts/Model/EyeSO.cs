using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Scriptable Objects/Model/Eye")]
public class EyeSO : ScriptableObject
{
    public Color color; // g�z rengi
    public string colorHex; // g�z rengi
    public string colorName; // "red", "green", "blue"

    [ShowAssetPreview] public Sprite sprite;   // UI ekran� i�in 2d resmi
    [ShowAssetPreview] public Sprite taskSprite;   // Task List i�in 2d resmi

    public string GetRecipeText()
    {
        return $" <color=#{colorHex}>{colorName}</color> Eye";
    }
}
