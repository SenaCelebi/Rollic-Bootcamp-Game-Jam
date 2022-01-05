using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;

public class ItemSelectUI : MonoBehaviour
{
    public static ItemSelectUI Instance { get; private set; }

    public event EventHandler<OnHairColorChangedEventArgs> OnHairColorChanged;
    public event EventHandler<OnDressColorChangedEventArgs> OnDressColorChanged;
    public event EventHandler<OnBodyColorChangedEventArgs> OnBodyColorChanged;
    public event EventHandler<OnLipColorChangedEventArgs> OnLipColorChanged;
    public event EventHandler<OnEyeColorChangedEventArgs> OnEyeColorChanged;
    public class OnLipColorChangedEventArgs : EventArgs
    {
       public string str;
    }
    public class OnHairColorChangedEventArgs : EventArgs
    {
        public string str;
    }
    public class OnDressColorChangedEventArgs : EventArgs
    {
        public string str;
    }
    public class OnBodyColorChangedEventArgs : EventArgs
    {
        public string str;
    }
    public class OnEyeColorChangedEventArgs : EventArgs
    {
        public string str;
    }
    [SerializeField] private List<Transform> itemList; // UI'daki 3 buton

    private HairTypeListSO hairList;
    private DressTypeListSO dressList;
    private BodyTypeListSO bodyList;
    private EyeListSO eyeList;
    private LipListSO lipList;

    [ShowNonSerializedField] private const int maxItemCount = 3;

    private int taskPartOne, taskPartTwo, taskPartThree;
    private int stage = 1;

    public void SetTaskParts(List<int> indexList)
    {
        //indeksler 0'dan 4'e kadar
        taskPartOne = indexList[0]; // �rn. sa� indeksi neyse o olur
        taskPartTwo = indexList[1]; // �rn. el indeksi neyse o olur
        taskPartThree = indexList[2]; // �rn. v�cut indeksi neyse o olur
    }

    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        GetFirstStage();
    }

    [Button]
    private void adff()
    {
        //ModelController.Instance.ChangeHairColor(RecipeManager.Instance.GetRecipedHair().color);
    }
    private void GetFirstStage()
    {
        switch (taskPartOne)
        {
            case 0:
                hairList = Resources.Load<HairTypeListSO>(typeof(HairTypeListSO).Name);
                GetHairs(hairList);
                break;
            case 1:
                eyeList = Resources.Load<EyeListSO>(typeof(EyeListSO).Name);
                GetEyes(eyeList);
                break;
            case 2:
                dressList = Resources.Load<DressTypeListSO>(typeof(DressTypeListSO).Name);
                GetDresses(dressList);
                break;
            case 3:
                bodyList = Resources.Load<BodyTypeListSO>(typeof(BodyTypeListSO).Name);
                GetBodies(bodyList);
                break;
            case 4:
                lipList = Resources.Load<LipListSO>(typeof(LipListSO).Name);
                GetLips(lipList);
                break;
        }
    }
    [Button]
    private void GetSecondStage()
    {
        switch (taskPartTwo)
        {
            case 0:
                hairList = Resources.Load<HairTypeListSO>(typeof(HairTypeListSO).Name);
                GetHairs(hairList);
                break;
            case 1:
                eyeList = Resources.Load<EyeListSO>(typeof(EyeListSO).Name);
                GetEyes(eyeList);
                break;
            case 2:
                dressList = Resources.Load<DressTypeListSO>(typeof(DressTypeListSO).Name);
                GetDresses(dressList);
                break;
            case 3:
                bodyList = Resources.Load<BodyTypeListSO>(typeof(BodyTypeListSO).Name);
                GetBodies(bodyList);
                break;
            case 4:
                lipList = Resources.Load<LipListSO>(typeof(LipListSO).Name);
                GetLips(lipList);
                break;
        }
    }
    [Button]
    private void GetThirdStage()
    {
        switch (taskPartThree)
        {
            case 0:
                hairList = Resources.Load<HairTypeListSO>(typeof(HairTypeListSO).Name);
                GetHairs(hairList);
                break;
            case 1:
                eyeList = Resources.Load<EyeListSO>(typeof(EyeListSO).Name);
                GetEyes(eyeList);
                break;
            case 2:
                dressList = Resources.Load<DressTypeListSO>(typeof(DressTypeListSO).Name);
                GetDresses(dressList);
                break;
            case 3:
                bodyList = Resources.Load<BodyTypeListSO>(typeof(BodyTypeListSO).Name);
                GetBodies(bodyList);
                break;
            case 4:
                lipList = Resources.Load<LipListSO>(typeof(LipListSO).Name);
                GetLips(lipList);
                break;
        }
    }

    private void CheckStage()
    {
        //UI'da t�klad�ktan sonraki check i�lemi
        switch (stage)
        {
            case 1:
                stage++;
                GetSecondStage();
                break;
            case 2:
                stage++;
                GetThirdStage();
                break;
            case 3:
                AnimationManager.Instance.DeactivateInGameUI();
                break;
        }
    }

    private void GetHairs(HairTypeListSO hairList)
    {
        List<int> tempList = new List<int>();
        foreach (Transform transform in itemList)
        {
            Image image = transform.Find(StringData.IMAGE).GetComponent<Image>();
            bool isAllItemSlotsFilled = maxItemCount == tempList.Count;

            // Farkl� sa�lar�n farkl� s�rada olacak �ekilde UI'a dizilmesi. 
            //3'ten fazla sa� varsa sadece 3 tanesinin al�nmas�. ��ld�rd�m yazarken
            while (!isAllItemSlotsFilled)
            {
                int randomIndex = UnityEngine.Random.Range(0, hairList.list.Count);

                if (!tempList.Contains(randomIndex))
                {
                    image.sprite = hairList.list[randomIndex].sprite;
                    image.color = UtilsClass.GetColorFromString(hairList.list[randomIndex].colorHex);
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        Debug.Log("yanl��");
                        //ModelController.Instance.ChangeHairColor(UtilsClass.GetStringFromColor(image.color));
                        OnHairColorChanged?.Invoke(this, new OnHairColorChangedEventArgs { str = hairList.list[randomIndex].colorHex });
                        //ModelController.Instance.ChangeHairColor(UtilsClass.GetColorFromString(hairList.list[randomIndex].colorHex));
                        CheckStage();

                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }

        tempList.Clear();


        //arad���m renk olu�mu� mu
        bool shouldI = true;
        foreach (Transform transform in itemList)
        {
            if (RecipeManager.Instance.GetRecipedHair().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {
                shouldI = false;
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log("doru");
                    //ModelController.Instance.ChangeHairColor(str);
                    OnHairColorChanged?.Invoke(this, new OnHairColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedHair().colorHex });
                    //ModelController.Instance.ChangeHairColor(UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedHair().colorHex));
                    ProgressBarUI.Instance.OneTaskDone();
                    CheckStage();
                });
            }
        }
        if (shouldI)
        {
            //arad���m renk olu�mam��sa biz olu�tururuz
            //basmam�z gereken renk.
            int itemListIndex = UnityEngine.Random.Range(0, 3);
            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedHair().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedHair().colorHex);

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("doru");
                ProgressBarUI.Instance.OneTaskDone();
                CheckStage();
            });
        }

    }
    private void GetEyes(EyeListSO eyeList)
    {
        List<int> tempList = new List<int>();
        foreach (Transform transform in itemList)
        {

            Image image = transform.Find(StringData.IMAGE).GetComponent<Image>();
            bool isAllItemSlotsFilled = maxItemCount == tempList.Count;

            while (!isAllItemSlotsFilled)
            {
                int randomIndex = UnityEngine.Random.Range(0, eyeList.list.Count);

                if (!tempList.Contains(randomIndex))
                {
                    image.sprite = eyeList.list[randomIndex].sprite;
                    image.color = UtilsClass.GetColorFromString(eyeList.list[randomIndex].colorHex);
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        Debug.Log("yanl��");
                        OnEyeColorChanged?.Invoke(this, new OnEyeColorChangedEventArgs { str = eyeList.list[randomIndex].colorHex });
                        CheckStage();
                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }

        tempList.Clear();

        bool shouldI = true;

        foreach (Transform transform in itemList)
        {
            if (RecipeManager.Instance.GetRecipedEye().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {
                shouldI = false;
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log("doru");
                    OnEyeColorChanged?.Invoke(this, new OnEyeColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedEye().colorHex });
                    ProgressBarUI.Instance.OneTaskDone();
                    CheckStage();
                });
            }
        }
        if (shouldI)
        {
            int itemListIndex = UnityEngine.Random.Range(0, 3);
            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedEye().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedEye().colorHex);

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("doru");
                ProgressBarUI.Instance.OneTaskDone();
                CheckStage();
            });
        }
    }
    private void GetDresses(DressTypeListSO dressList)
    {
        List<int> tempList = new List<int>();
        foreach (Transform transform in itemList)
        {
            Image image = transform.Find(StringData.IMAGE).GetComponent<Image>();
            bool isAllItemSlotsFilled = maxItemCount == tempList.Count;

            while (!isAllItemSlotsFilled)
            {
                int randomIndex = UnityEngine.Random.Range(0, dressList.list.Count);

                if (!tempList.Contains(randomIndex))
                {
                    image.sprite = dressList.list[randomIndex].sprite;
                    image.color = UtilsClass.GetColorFromString(dressList.list[randomIndex].colorHex);
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        Debug.Log("yanl��");
                        OnDressColorChanged?.Invoke(this, new OnDressColorChangedEventArgs { str = dressList.list[randomIndex].colorHex });
                        CheckStage();
                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }

        }

        tempList.Clear();

        bool shouldI = true;
        foreach (Transform transform in itemList)
        {
            if (RecipeManager.Instance.GetRecipedDress().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {
                shouldI = false;
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log("doru");
                    OnDressColorChanged?.Invoke(this, new OnDressColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedDress().colorHex });
                    ProgressBarUI.Instance.OneTaskDone();
                    CheckStage();
                });
            }
        }
        if (shouldI)
        {
            int itemListIndex = UnityEngine.Random.Range(0, 3);
            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedDress().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedDress().colorHex);

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("doru");
                ProgressBarUI.Instance.OneTaskDone();
                CheckStage();
            });
        }
    }
    private void GetBodies(BodyTypeListSO bodyList)
    {
        List<int> tempList = new List<int>();
        foreach (Transform transform in itemList)
        {
            Image image = transform.Find(StringData.IMAGE).GetComponent<Image>();
            bool isAllItemSlotsFilled = maxItemCount == tempList.Count;

            while (!isAllItemSlotsFilled)
            {
                int randomIndex = UnityEngine.Random.Range(0, bodyList.list.Count);

                if (!tempList.Contains(randomIndex))
                {
                    image.sprite = bodyList.list[randomIndex].sprite;
                    image.color = UtilsClass.GetColorFromString(bodyList.list[randomIndex].colorHex);
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        Debug.Log("yanl��");
                        OnBodyColorChanged?.Invoke(this, new OnBodyColorChangedEventArgs { str = bodyList.list[randomIndex].colorHex });
                        CheckStage();
                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }

        tempList.Clear();

        bool shouldI = true;
        foreach (Transform transform in itemList)
        {
            if (RecipeManager.Instance.GetRecipedBody().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {
                shouldI = false;
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log("doru");
                    OnBodyColorChanged?.Invoke(this, new OnBodyColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedBody().colorHex });
                    ProgressBarUI.Instance.OneTaskDone();
                    CheckStage();
                });
            }
        }
        if (shouldI)
        {
            int itemListIndex = UnityEngine.Random.Range(0, 3);
            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedBody().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedBody().colorHex);

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("doru");
                ProgressBarUI.Instance.OneTaskDone();
                CheckStage();
            });
        }
    }
    private void GetLips(LipListSO lipList)
    {
        List<int> tempList = new List<int>();
        foreach (Transform transform in itemList)
        {
            Image image = transform.Find(StringData.IMAGE).GetComponent<Image>();
            bool isAllItemSlotsFilled = maxItemCount == tempList.Count;

            while (!isAllItemSlotsFilled)
            {
                int randomIndex = UnityEngine.Random.Range(0, lipList.list.Count);

                if (!tempList.Contains(randomIndex))
                {
                    image.sprite = lipList.list[randomIndex].sprite;
                    image.color = UtilsClass.GetColorFromString(lipList.list[randomIndex].colorHex);
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        Debug.Log("yanl��");
                        OnLipColorChanged?.Invoke(this, new OnLipColorChangedEventArgs { str = lipList.list[randomIndex].colorHex });
                        CheckStage();
                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }

        tempList.Clear();

        bool shouldI = true;
        foreach (Transform transform in itemList)
        {
            if (RecipeManager.Instance.GetRecipedLips().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {
                shouldI = false;
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log("doru");
                    OnLipColorChanged?.Invoke(this, new OnLipColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedLips().colorHex });
                    ProgressBarUI.Instance.OneTaskDone();
                    CheckStage();
                });
            }
        }
        if (shouldI)
        {
            int itemListIndex = UnityEngine.Random.Range(0, 3);
            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedLips().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedLips().colorHex);

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("doru");
                ProgressBarUI.Instance.OneTaskDone();
                CheckStage();
            });
        }
    }
}
