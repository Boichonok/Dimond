using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScreenUIController : BaseActivityController
{

    [Header("Main Camera")]
    [SerializeField]
    private Camera mainCamera = null;

    [SerializeField]
    private Transform leftSide;
    [SerializeField]
    private Transform rightSide;
    [SerializeField]
    private Transform center;

    private Coins coins;

    [Header("Activitys")]
    [SerializeField]
    private ChoosePictureController pictureSelectActivity = null;
    public ChoosePictureController PictureSelectActivity { get { return pictureSelectActivity; } }
    [SerializeField]
    private ChooseMaterialsController materialSelectActivity = null;
    public ChooseMaterialsController MaterialSelectActivity { get { return materialSelectActivity; } }
    [SerializeField]
    private PlayingFildeController playingFildeActivity = null;
    public PlayingFildeController PlayingFildeActivity { get { return playingFildeActivity; } }
    [SerializeField]
    private GalleryController galleryActivity = null;
    public GalleryController GalleryActivity { get { return galleryActivity; } }
    [SerializeField]
    private RouletteController rouletteActivity = null;
    public RouletteController RouletteActivity { get { return rouletteActivity; } }
    [SerializeField]
    private DataBase dataBase;
    public DataBase DataBase { get { return dataBase; } }

    private GameObject lastActivity;

    private void Start()
    {
        InitSides();
        InitFildeCoinUI();
        ShowPictureSelectActivity();
        dataBase.SaveSettings();
    }

    private void Update()
    {
        UpdateCoinFildeValue();
    }

    private void InitSides()
    {
        leftSide.localPosition = new Vector3(Camera.main.ScreenToWorldPoint(Vector2.zero).x, 0, 0);
        rightSide.localPosition = new Vector3(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x, 0, 0);
        center.localPosition = new Vector3(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, 0)).x, 0, 0);
    }

    private void InitFildeCoinUI()
    {
        coins = FindObjectOfType<Coins>();
        if (dataBase.GetSettings().isFirstStartApp)
        {
            coins.AddCoin(1000);
            dataBase.GetSettings().CoinsCount = coins.CurrentCoin;
        }
        else
        {
            coins.AddCoin(dataBase.GetSettings().CoinsCount);
        }
    }

    public override void OnClick(GameObject selectedUI)
    {
        base.OnClick(selectedUI);
        if (selectedUI != null)
        {
            switch (selectedUI.tag)
            {
                case Utils.TAGS.MENU_BUTTONS_LIST:
                    {
                        selectedUI.GetComponent<UIMenuButtonListController>().OnClickMenuButton();
                    }
                    break;
                case Utils.TAGS.EXITE_BUTTON:
                    {
                        if (lastActivity != null)
                            switch (lastActivity.tag)
                            {
                                case Utils.TAGS.CHOOSE_PICTURE_ACTIVITY:
                                    {
                                        Application.Quit();
                                    }
                                    break;
                                case Utils.TAGS.CHOOSE_MATERIAL_ACTIVITY:
                                    {
                                        ShowPictureSelectActivity().ResetVerticalList();
                                        ResetLastActivity();
                                    }
                                    break;
                                case Utils.TAGS.PLAYING_FILD_ACTIVITY:
                                    {
                                        playingFildeActivity.StartCoroutine("BackToMenu");
                                    }
                                    break;
                                case Utils.TAGS.GALLERY_ACTIVITY:
                                    {
                                        galleryActivity.ResetVerticalList();
                                        ShowPictureSelectActivity().ResetVerticalList();
                                    }
                                    break;
                                case Utils.TAGS.ROULETTE_ACTIVITY:
                                    {
                                        ShowPictureSelectActivity().ResetVerticalList();
                                    }
                                    break;
                            }

                    }
                    break;

            }

        }
    }

    private void UpdateCoinFildeValue()
    {
        var coinValueTxt = coins.GetComponentInChildren<TextMesh>();
        coinValueTxt.text = coins.CurrentCoin.ToString();
        dataBase.GetSettings().CoinsCount = coins.CurrentCoin;
    }

    public Coins GetCoinController()
    {
        return coins;
    }

    public ChoosePictureController ShowPictureSelectActivity()
    {
        pictureSelectActivity.transform.parent.gameObject.SetActive(true);
        materialSelectActivity.transform.parent.gameObject.SetActive(false);
        materialSelectActivity.enabled = false;
        playingFildeActivity.transform.parent.gameObject.SetActive(false);
        playingFildeActivity.enabled = false;
        galleryActivity.transform.parent.gameObject.SetActive(false);
        galleryActivity.enabled = false;
        rouletteActivity.transform.parent.gameObject.SetActive(false);
        rouletteActivity.enabled = false;

        lastActivity = pictureSelectActivity.transform.parent.gameObject;
        pictureSelectActivity.enabled = true;
        pictureSelectActivity.pictureTable = DataBase.GetPictureTableFromFile();
        return pictureSelectActivity;
    }

    public ChooseMaterialsController ShowMaterialSelectActivtiy()
    {
        pictureSelectActivity.transform.parent.gameObject.SetActive(false);
        pictureSelectActivity.enabled = false;
        materialSelectActivity.transform.parent.gameObject.SetActive(true);
        playingFildeActivity.transform.parent.gameObject.SetActive(false);
        playingFildeActivity.enabled = false;

        lastActivity = materialSelectActivity.transform.parent.gameObject;
        materialSelectActivity.enabled = true;
        return materialSelectActivity;
    }

    public PlayingFildeController ShowPlayingFildActivity()
    {
        pictureSelectActivity.transform.parent.gameObject.SetActive(false);
        pictureSelectActivity.enabled = false;
        materialSelectActivity.transform.parent.gameObject.SetActive(false);
        materialSelectActivity.enabled = false;
        playingFildeActivity.transform.parent.gameObject.SetActive(true);
        lastActivity = playingFildeActivity.transform.parent.gameObject;
        playingFildeActivity.enabled = true;
        return playingFildeActivity;
    }

    public GalleryController ShowGalleryActivity()
    {
        pictureSelectActivity.transform.parent.gameObject.SetActive(false);
        pictureSelectActivity.enabled = false;

        materialSelectActivity.transform.parent.gameObject.SetActive(false);
        materialSelectActivity.enabled = false;

        playingFildeActivity.transform.parent.gameObject.SetActive(false);
        playingFildeActivity.enabled = false;

        rouletteActivity.transform.parent.gameObject.SetActive(false);
        rouletteActivity.enabled = false;

        galleryActivity.transform.parent.gameObject.SetActive(true);

        lastActivity = galleryActivity.transform.parent.gameObject;
        galleryActivity.enabled = true;
        return galleryActivity;
    }

    public RouletteController ShowRouletteActivity()
    {
        pictureSelectActivity.transform.parent.gameObject.SetActive(false);
        pictureSelectActivity.enabled = false;

        materialSelectActivity.transform.parent.gameObject.SetActive(false);
        materialSelectActivity.enabled = false;

        playingFildeActivity.transform.parent.gameObject.SetActive(false);
        playingFildeActivity.enabled = false;

        galleryActivity.transform.parent.gameObject.SetActive(false);
        galleryActivity.enabled = false;

        rouletteActivity.transform.parent.gameObject.SetActive(true);
        lastActivity = rouletteActivity.transform.parent.gameObject;

        rouletteActivity.enabled = true;
        return rouletteActivity;
    }
    public void ResetLastActivity()
    {
        lastActivity = null;
    }

    private void OnApplicationQuit()
    {
        dataBase.GetSettings().CoinsCount = coins.CurrentCoin;
        dataBase.GetSettings().isFirstStartApp = false;
        dataBase.SaveSettings();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            dataBase.GetSettings().CoinsCount = coins.CurrentCoin;
            dataBase.GetSettings().isFirstStartApp = false;
            dataBase.SaveSettings();
        }
    }

}
