using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    [SerializeField] private Animator loadingGameUI;
    [SerializeField] private Animator introGameUI;
    [SerializeField] private Animator inGameUI;
    [SerializeField] private Animator failGameUI;
    [SerializeField] private Animator winGameUI;
    [SerializeField] private Animator wheelUI;
    [SerializeField] private Animator model;


    private void Awake()
    {
        Instance = this;
    }

    [Button]
    public void ActivateLoadingGameUI()
    {
        introGameUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateLoadingGameUI()
    {
        introGameUI.SetBool(StringData.ISACTIVE, false);
    }

    [Button]
    public void ActivateIntroGameUI()
    {
        introGameUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateIntroGameUI()
    {
        introGameUI.SetBool(StringData.ISACTIVE, false);
    }


    [Button]
    public void ActivateInGameUI()
    {
        inGameUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateInGameUI()
    {
        inGameUI.SetBool(StringData.ISACTIVE, false);
    }
    [Button]
    public void ActivateDanceFemale()
    {
        model.SetBool(StringData.ISDANCING, true);
    }
    [Button]
    public void DeactivateDanceFemale()
    {
        model.SetBool(StringData.ISDANCING, false);
    }

    [Button]
    public void ActivateFailGameUI()
    {
        failGameUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateFailGameUI()
    {
        failGameUI.SetBool(StringData.ISACTIVE, false);
    }

    [Button]
    public void ActivateWinGameUI()
    {
        winGameUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateWinGameUI()
    {
        winGameUI.SetBool(StringData.ISACTIVE, false);
    }
    [Button]
    public void ActivateWheelUI()
    {
        wheelUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateWheelUI()
    {
        wheelUI.SetBool(StringData.ISACTIVE, false);
    }

}
