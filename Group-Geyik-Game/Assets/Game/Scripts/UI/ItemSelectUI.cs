using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSelectUI : MonoBehaviour
{
    public static ItemSelectUI Instance { get; private set; }

    [SerializeField] private List<Transform> itemList; // UI'daki 5 buton

    [SerializeField] private Transform winUI;
    [SerializeField] private Transform loseUI;
    [SerializeField] private Transform wheelUI;
    [SerializeField] private Transform levelText;

    private Action RefFunction;

    public Transform handTransform;

    private bool isDrag = false;
    private int currentLevel = 1;



    #region Observer
    public event EventHandler<OnHairColorChangedEventArgs> OnHairColorChanged;
    public event EventHandler<OnDressColorChangedEventArgs> OnDressColorChanged;
    public event EventHandler<OnBodyColorChangedEventArgs> OnBodyColorChanged;
    public event EventHandler<OnLipColorChangedEventArgs> OnLipColorChanged;
    public event EventHandler<OnEyeColorChangedEventArgs> OnEyeColorChanged;

    public event EventHandler OnThreeStagesCompleted;
    public event EventHandler OnOnePartChanged;

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

    #endregion

    private HairTypeListSO hairList;
    private DressTypeListSO dressList;
    private BodyTypeListSO bodyList;
    private EyeListSO eyeList;
    private LipListSO lipList;

    [ShowNonSerializedField] private const int maxItemCount = 5;

    private int taskPartOne, taskPartTwo, taskPartThree; //task manager'dan gelen sa� m� g�z m� onlar
    private int stage = 1; //item se�me a�amas� toplam 3 kere

    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        SetTaskParts();
        GetStage(taskPartOne);
    }
    private void Update()
    {
        InputHandler();
    }

    private void InputHandler()
    {
        handTransform.position = Input.mousePosition;
        handTransform.gameObject.SetActive(false);

        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = true;
            if (handTransform.GetComponent<Image>().sprite == null)
            {
                handTransform.gameObject.SetActive(true);
            }

        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            isDrag = false;
            //handTransform.GetComponent<Image>().sprite = null;
        }
        if (isDrag)
        {
            handTransform.gameObject.SetActive(true);
        }
    }

    private void SetTaskParts()
    {
        //indeksler 0'dan 4'e kadar
        taskPartOne = TaskListUI.Instance.GetTempList(0);   //indexList[0]; // �rn. sa� indeksi neyse o olur
        taskPartTwo = TaskListUI.Instance.GetTempList(1);   //indexList[1]; // �rn. el indeksi neyse o olur
        taskPartThree = TaskListUI.Instance.GetTempList(2); //= indexList[2]; // �rn. v�cut indeksi neyse o olur
    }
    private void GetStage(int stage)
    {
        OnOnePartChanged?.Invoke(this, EventArgs.Empty);

        switch (stage)
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
                GetStage(taskPartTwo);
                break;
            case 2:
                stage++;
                GetStage(taskPartThree);
                break;
            case 3:
                AnimationManager.Instance.DeactivateInGameUI();
                StartCoroutine(ThreeStagesCompleted());
                break;
        }
    }

    private void GetHairs(HairTypeListSO hairList)
    {
        List<int> tempList = new List<int>();
        foreach (Transform transform in itemList)
        {
            bool isAllItemSlotsFilled = maxItemCount == tempList.Count;

            // Farkl� sa�lar�n farkl� s�rada olacak �ekilde UI'a dizilmesi. 5'ten fazla sa� varsa sadece 5 tanesinin al�nmas�. ��ld�rd�m yazarken
            while (!isAllItemSlotsFilled)
            {
                int randomIndex = UnityEngine.Random.Range(0, hairList.list.Count);

                if (!tempList.Contains(randomIndex))
                {
                    Image image = transform.Find(StringData.IMAGE).GetComponent<Image>();
                    image.sprite = hairList.list[randomIndex].sprite;
                    image.color = UtilsClass.GetColorFromString(hairList.list[randomIndex].colorHex);

                    //butonun �zerine gelme
                    PointerEvents pointerEvents = transform.GetComponent<PointerEvents>();
                    pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
                    {
                        handTransform.GetComponent<Image>().sprite = image.sprite;
                        handTransform.GetComponent<Image>().color = image.color;
                    };

                    //butona t�klama hepsine false atar�z
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                        {

                            RefFunction = () =>
                            {
                                Debug.Log("yanl��");
                                TaskListUI.Instance.SetActiveXMark(stage - 1);
                                OnHairColorChanged?.Invoke(this, new OnHairColorChangedEventArgs { str = hairList.list[randomIndex].colorHex });
                                CheckStage();
                            };

                            StartCoroutine(WheelTime(RefFunction));
                        }
                        else
                        {
                            Debug.Log("yanl��");
                            OnHairColorChanged?.Invoke(this, new OnHairColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedHair().colorHex });
                            TaskListUI.Instance.SetActiveXMark(stage - 1);
                            CheckStage();
                        }
                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }
        //tempList.Clear();

        bool shouldI = true; //butonlarda, task listteki istenen rengi olu�turmama gerek var m�

        foreach (Transform transform in itemList)
        {
            //arad���m renk zaten varsa butona t�klay�ncaki negatif �zelli�ini kald�r�r�z
            if (RecipeManager.Instance.GetRecipedHair().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {
                shouldI = false;

                //halihaz�rda varolan butona, t�klay�nca nolca��n� eklerim
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                    {
                        RefFunction = () =>
                        {
                            Debug.Log("doru");
                            OnHairColorChanged?.Invoke(this, new OnHairColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedHair().colorHex });
                            ProgressBarUI.Instance.OneTaskDone();
                            TaskListUI.Instance.SetActiveTick(stage - 1);
                            CheckStage();
                        };
                        StartCoroutine(WheelTime(RefFunction));

                    }
                    else
                    {
                        Debug.Log("doru");
                        OnHairColorChanged?.Invoke(this, new OnHairColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedHair().colorHex });
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    }

                });
            }
        }
        //arad���m renk olu�mam��sa biz olu�tururuz.
        if (shouldI)
        {
            int itemListIndex = UnityEngine.Random.Range(0, 3);

            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedHair().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedHair().colorHex);

            //butonun �zerine gelme
            PointerEvents pointerEvents = itemList[itemListIndex].GetComponent<PointerEvents>();
            pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                handTransform.GetComponent<Image>().sprite = RecipeManager.Instance.GetRecipedHair().sprite;
                handTransform.GetComponent<Image>().color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedHair().colorHex);
            };

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                {
                    RefFunction = () =>
                    {

                        Debug.Log("doru");
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    };
                    StartCoroutine(WheelTime(RefFunction));

                }
                else
                {
                    Debug.Log("doru");
                    ProgressBarUI.Instance.OneTaskDone();
                    TaskListUI.Instance.SetActiveTick(stage - 1);
                    CheckStage();
                }

            });
        }
        tempList.Clear();
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

                    //butonun �zerine gelme
                    PointerEvents pointerEvents = transform.GetComponent<PointerEvents>();
                    pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
                    {
                        handTransform.GetComponent<Image>().sprite = image.sprite;
                        handTransform.GetComponent<Image>().color = image.color;
                    };
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                        {
                            RefFunction = () =>
                            {
                                Debug.Log("yanl��");
                                TaskListUI.Instance.SetActiveXMark(stage - 1);
                                OnEyeColorChanged?.Invoke(this, new OnEyeColorChangedEventArgs { str = eyeList.list[randomIndex].colorHex });
                                CheckStage();
                            };
                            StartCoroutine(WheelTime(RefFunction));

                        }
                        else
                        {
                            Debug.Log("yanl��");
                            TaskListUI.Instance.SetActiveXMark(stage - 1);
                            OnEyeColorChanged?.Invoke(this, new OnEyeColorChangedEventArgs { str = eyeList.list[randomIndex].colorHex });
                            CheckStage();
                        }

                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }

        bool shouldI = true;
        foreach (Transform transform in itemList)
        {

            if (RecipeManager.Instance.GetRecipedEye().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {
                shouldI = false;

                //butona t�klama
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                    {
                        RefFunction = () =>
                        {
                            Debug.Log("doru");
                            OnEyeColorChanged?.Invoke(this, new OnEyeColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedEye().colorHex });
                            ProgressBarUI.Instance.OneTaskDone();
                            TaskListUI.Instance.SetActiveTick(stage - 1);
                            CheckStage();
                        };
                        StartCoroutine(WheelTime(RefFunction));
                    }
                    else
                    {
                        Debug.Log("doru");
                        OnEyeColorChanged?.Invoke(this, new OnEyeColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedEye().colorHex });
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    }

                });
            }
        }
        if (shouldI)
        {
            int itemListIndex = UnityEngine.Random.Range(0, 3);

            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedEye().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedEye().colorHex);

            //Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            //imagee.sprite = RecipeManager.Instance.GetRecipedEye().sprite;
            //imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedEye().colorHex);

            //butonun �zerine gelme
            PointerEvents pointerEvents = itemList[itemListIndex].GetComponent<PointerEvents>();
            pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                handTransform.GetComponent<Image>().sprite = RecipeManager.Instance.GetRecipedEye().sprite;
                handTransform.GetComponent<Image>().color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedEye().colorHex);
            };

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                {
                    RefFunction = () =>
                    {
                        Debug.Log("doru");
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    };
                    StartCoroutine(WheelTime(RefFunction));

                }
                else
                {
                    Debug.Log("doru");
                    ProgressBarUI.Instance.OneTaskDone();
                    TaskListUI.Instance.SetActiveTick(stage - 1);
                    CheckStage();
                }


            });
        }
        tempList.Clear();
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

                    //butonun �zerine gelme
                    PointerEvents pointerEvents = transform.GetComponent<PointerEvents>();
                    pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
                    {
                        handTransform.GetComponent<Image>().sprite = image.sprite;
                        handTransform.GetComponent<Image>().color = image.color;
                    };
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                        {
                            RefFunction = () =>
                            {
                                Debug.Log("yanl��");
                                TaskListUI.Instance.SetActiveXMark(stage - 1);
                                OnDressColorChanged?.Invoke(this, new OnDressColorChangedEventArgs { str = dressList.list[randomIndex].colorHex });
                                CheckStage();
                            };
                            StartCoroutine(WheelTime(RefFunction));

                        }
                        else
                        {
                            Debug.Log("yanl��");
                            TaskListUI.Instance.SetActiveXMark(stage - 1);
                            OnDressColorChanged?.Invoke(this, new OnDressColorChangedEventArgs { str = dressList.list[randomIndex].colorHex });
                            CheckStage();
                        }

                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }

        }

        bool shouldI = true;

        foreach (Transform transform in itemList)
        {

            if (RecipeManager.Instance.GetRecipedDress().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {

                shouldI = false;

                //butona t�klama
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                    {
                        RefFunction = () =>
                        {
                            Debug.Log("doru");
                            OnDressColorChanged?.Invoke(this, new OnDressColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedDress().colorHex });
                            ProgressBarUI.Instance.OneTaskDone();
                            TaskListUI.Instance.SetActiveTick(stage - 1);
                            CheckStage();
                        };
                        StartCoroutine(WheelTime(RefFunction));

                    }
                    else
                    {
                        Debug.Log("doru");
                        OnDressColorChanged?.Invoke(this, new OnDressColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedDress().colorHex });
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    }

                });
            }
        }
        if (shouldI)
        {
            int itemListIndex = UnityEngine.Random.Range(0, 3);

            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedDress().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedDress().colorHex);

            //butonun �zerine gelme
            PointerEvents pointerEvents = itemList[itemListIndex].GetComponent<PointerEvents>();
            pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                handTransform.GetComponent<Image>().sprite = RecipeManager.Instance.GetRecipedDress().sprite;
                handTransform.GetComponent<Image>().color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedDress().colorHex);
            };

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                {
                    RefFunction = () =>
                    {
                        Debug.Log("doru");
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    };
                    StartCoroutine(WheelTime(RefFunction));

                }
                else
                {
                    Debug.Log("doru");
                    ProgressBarUI.Instance.OneTaskDone();
                    TaskListUI.Instance.SetActiveTick(stage - 1);
                    CheckStage();
                }

            });
        }

        tempList.Clear();
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

                    //butonun �zerine gelme
                    PointerEvents pointerEvents = transform.GetComponent<PointerEvents>();
                    pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
                    {
                        handTransform.GetComponent<Image>().sprite = image.sprite;
                        handTransform.GetComponent<Image>().color = image.color;
                    };
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                        {
                            RefFunction = () =>
                            {
                                Debug.Log("yanl��");
                                TaskListUI.Instance.SetActiveXMark(stage - 1);
                                OnBodyColorChanged?.Invoke(this, new OnBodyColorChangedEventArgs { str = bodyList.list[randomIndex].colorHex });
                                CheckStage();
                            };
                            StartCoroutine(WheelTime(RefFunction));

                        }
                        else
                        {
                            Debug.Log("yanl��");
                            TaskListUI.Instance.SetActiveXMark(stage - 1);
                            OnBodyColorChanged?.Invoke(this, new OnBodyColorChangedEventArgs { str = bodyList.list[randomIndex].colorHex });
                            CheckStage();
                        }
                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }

        bool shouldI = true;

        foreach (Transform transform in itemList)
        {

            if (RecipeManager.Instance.GetRecipedBody().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {

                shouldI = false;

                //butona t�klama
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                    {
                        RefFunction = () =>
                        {
                            Debug.Log("doru");
                            OnBodyColorChanged?.Invoke(this, new OnBodyColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedBody().colorHex });
                            ProgressBarUI.Instance.OneTaskDone();
                            TaskListUI.Instance.SetActiveTick(stage - 1);
                            CheckStage();
                        };
                        StartCoroutine(WheelTime(RefFunction));

                    }
                    else
                    {
                        Debug.Log("doru");
                        OnBodyColorChanged?.Invoke(this, new OnBodyColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedBody().colorHex });
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    }
                });
            }
        }
        if (shouldI)
        {
            int itemListIndex = UnityEngine.Random.Range(0, 3);

            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedBody().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedBody().colorHex);

            //butonun �zerine gelme
            PointerEvents pointerEvents = itemList[itemListIndex].GetComponent<PointerEvents>();
            pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                handTransform.GetComponent<Image>().sprite = RecipeManager.Instance.GetRecipedBody().sprite;
                handTransform.GetComponent<Image>().color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedBody().colorHex);
            };

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                {
                    RefFunction = () =>
                    {
                        Debug.Log("doru");
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    };
                    StartCoroutine(WheelTime(RefFunction));

                }
                else
                {
                    Debug.Log("doru");
                    ProgressBarUI.Instance.OneTaskDone();
                    TaskListUI.Instance.SetActiveTick(stage - 1);
                    CheckStage();
                }
            });
        }

        tempList.Clear();
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

                    //butonun �zerine gelme
                    PointerEvents pointerEvents = transform.GetComponent<PointerEvents>();
                    pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
                    {
                        handTransform.GetComponent<Image>().sprite = image.sprite;
                        handTransform.GetComponent<Image>().color = image.color;
                    };
                    transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                        {
                            RefFunction = () =>
                            {
                                Debug.Log("yanl��");
                                TaskListUI.Instance.SetActiveXMark(stage - 1);
                                OnLipColorChanged?.Invoke(this, new OnLipColorChangedEventArgs { str = lipList.list[randomIndex].colorHex });
                                CheckStage();
                            };
                            StartCoroutine(WheelTime(RefFunction));

                        }
                        else
                        {
                            Debug.Log("yanl��");
                            TaskListUI.Instance.SetActiveXMark(stage - 1);
                            OnLipColorChanged?.Invoke(this, new OnLipColorChangedEventArgs { str = lipList.list[randomIndex].colorHex });
                            CheckStage();
                        }
                    });
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }



        bool shouldI = true;

        foreach (Transform transform in itemList)
        {
            if (RecipeManager.Instance.GetRecipedLips().colorHex == UtilsClass.GetStringFromColor(transform.Find(StringData.IMAGE).GetComponent<Image>().color))
            {

                shouldI = false;

                //butona t�klama
                transform.GetComponent<Button>().onClick.RemoveAllListeners();
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                    {
                        RefFunction = () =>
                        {
                            Debug.Log("doru");
                            OnLipColorChanged?.Invoke(this, new OnLipColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedLips().colorHex });
                            ProgressBarUI.Instance.OneTaskDone();
                            TaskListUI.Instance.SetActiveTick(stage - 1);
                            CheckStage();
                        };
                        StartCoroutine(WheelTime(RefFunction));

                    }
                    else
                    {
                        Debug.Log("doru");
                        OnLipColorChanged?.Invoke(this, new OnLipColorChangedEventArgs { str = RecipeManager.Instance.GetRecipedLips().colorHex });
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    }
                });
            }

        }
        if (shouldI)
        {
            int itemListIndex = UnityEngine.Random.Range(0, 3);

            Image imagee = itemList[itemListIndex].transform.Find(StringData.IMAGE).GetComponent<Image>();

            imagee.sprite = RecipeManager.Instance.GetRecipedLips().sprite;
            imagee.color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedLips().colorHex);

            //butonun �zerine gelme
            PointerEvents pointerEvents = itemList[itemListIndex].GetComponent<PointerEvents>();
            pointerEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                handTransform.GetComponent<Image>().sprite = RecipeManager.Instance.GetRecipedLips().sprite;
                handTransform.GetComponent<Image>().color = UtilsClass.GetColorFromString(RecipeManager.Instance.GetRecipedLips().colorHex);
            };

            itemList[itemListIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            itemList[itemListIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (TaskListUI.Instance.CheckHaveQuestionMark(stage - 1))
                {
                    RefFunction = () =>
                    {
                        Debug.Log("doru");
                        ProgressBarUI.Instance.OneTaskDone();
                        TaskListUI.Instance.SetActiveTick(stage - 1);
                        CheckStage();
                    };
                    StartCoroutine(WheelTime(RefFunction));

                }
                else
                {
                    Debug.Log("doru");
                    ProgressBarUI.Instance.OneTaskDone();
                    TaskListUI.Instance.SetActiveTick(stage - 1);
                    CheckStage();
                }
            });
        }

        tempList.Clear();
    }

    private IEnumerator WheelTime(Action action)
    {
        wheelUI.gameObject.SetActive(true);
        AnimationManager.Instance.ActivateWheelUI();
        yield return new WaitForSeconds(6.7f);
        wheelUI.gameObject.SetActive(false);

        Action SthImportantFunction = action;
        SthImportantFunction();

    }
    private IEnumerator ThreeStagesCompleted()
    {
        //kazanma ekran� bekleme sekans�
        if (ProgressBarUI.Instance.GetScore() == 1 || ProgressBarUI.Instance.GetScore() == 2)
        {
            winUI.gameObject.SetActive(true);

            winUI.Find("ButtonNext").GetComponent<Button>().onClick.AddListener(() =>
            {
                winUI.transform.gameObject.SetActive(false);
                JustDoIT();
            });
        }
        else if (ProgressBarUI.Instance.GetScore() == 3)
        {
            yield return new WaitForSeconds(6f);
            winUI.gameObject.SetActive(true);
            winUI.Find("ButtonNext").GetComponent<Button>().onClick.AddListener(() =>
            {
                winUI.transform.gameObject.SetActive(false);
                JustDoIT();
                winUI.Find("ButtonNext").GetComponent<Button>().onClick.AddListener(() =>
                {
                    winUI.transform.gameObject.SetActive(false);
                    JustDoIT();
                    AnimationManager.Instance.DeactivateDanceFemale();
                    AnimationManager.Instance.DeactivateDanceMale();
                });

            });
        }
        else
        {
            Debug.Log("loooooserrr");
            loseUI.gameObject.SetActive(true);
            loseUI.Find("ButtonRetry").GetComponent<Button>().onClick.AddListener(() =>
            {
                loseUI.transform.gameObject.SetActive(false);
                JustDoIT();

            });

        }

    }

    private void JustDoIT()
    {
        currentLevel++;
        levelText.GetComponent<TextMeshProUGUI>().SetText("LEVEL: " + currentLevel.ToString());

        //progress bar'� s�f�rla
        ProgressBarUI.Instance.ResetBar();
        //yeni tarif olu�tur
        TaskListUI.Instance.CreatRandomRecipe();
        // 3 a�amal� item se�meyi 1'e geri �ek
        stage = 1;
        //yeni tarif part datalar�n� �ek
        SetTaskParts();
        //�lk task'a g�re UI'� g�ncelle
        GetStage(taskPartOne);
        //UI animasyonu aktive olsun
        AnimationManager.Instance.ActivateInGameUI();
        //event'i
        OnThreeStagesCompleted?.Invoke(this, EventArgs.Empty);

        Debug.Log("end game bitti");
    }
}
