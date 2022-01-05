using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    [SerializeField] private Animator inGameUI;

    private void Awake()
    {
        Instance = this;
    }

    [Button]
    public void DeactivateInGameUI()
    {
        inGameUI.SetBool(StringData.ISACTIVE, false);
    }

    [Button]
    public void ActivateInGameUI()
    {
        inGameUI.SetBool(StringData.ISACTIVE, true);
    }

}