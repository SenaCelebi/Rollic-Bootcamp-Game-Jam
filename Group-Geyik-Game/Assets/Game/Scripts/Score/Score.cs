using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }
    
    public static int stars = 0;
    public static int totalCoin = 0;

    private void Awake()
    {
        Instance = this;
    } 
  
    /*
     * Nerede player yanlýþ seçim yaparsa star sayýsýný azalt.
     * Score.stars--
     */

}
