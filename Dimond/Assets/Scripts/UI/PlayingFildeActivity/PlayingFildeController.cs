using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayingFildeController : BaseActivityController,
IClickHorizontalListViewItem,
IBuyMaterialHorizontalListView,
ISwipeListenerHorizontalListView,
IClickButtonListener,
CircleProgressBar.ICircleUpdateProgress,
IPlayPictureCameraListener,
IPlayPictureControllerCalbacks
{
    [SerializeField]
    private GameObject spawnPicturePosition = null;

    [SerializeField]
    private GameObject buttonPanel = null;

    [SerializeField]
    private GameObject progressBar = null;

    [SerializeField]
    private GameObject buttonNumber = null;

    [SerializeField]
    private SectionHorizontalListView sectionHorizontalListView = null;

    private SectionData[] sectionListItems;

    private SectionListItem currentSection;

    private Brush currentBrush;

    [SerializeField]
    private BrushesHorizontalListView brushesHorizontalListView = null;

    [SerializeField]
    private SpriteRenderer background = null;

    [SerializeField]
    private BackgroundData[] backgrounds = null;

    private int backgroundID = 0;

    private int lastBackgroundId = 0;

    public UIPictureData UIPictureData { private get; set; }

    public PlayPictureData PlayPictureData { get; set; }

    private PlayPictureController playPictureController = null;

    private PlayPictureZoomController playPictureZoomController = null;

    private CircleProgressBar circleProgressBar;

    private TextMesh buttonNumberSection;

    [SerializeField]
    private ScreenUIController screenUIController;
    [SerializeField]
    private AdvicesController advicesController;

    private PictureTable pictureTable;

    private GameSettings gameSettings;

    private bool isPictureUpdated = false;

    private bool buttonsPanelVisibility = true;

    private bool canExite = false;

    private CircleCollider2D[] buttonsPanelColliders;

    private int lastColorPointsValue = 0;

    private bool isCoinsAdded = false;

    private int reward = 20;
    public int Reward { get { return reward; } private set { reward = value; } }


    /*
     * Метод который инициализирует все компоненты.
     * Вызывается как при первом старте приложения, тоесть в методе Start(),
     * когда экземпляр еще не был создан, и вызывается из вне, когда повторно,
     * переходим в данную активность, что бы переопределить ссылки на компоненты.
     */
    public bool OnStart()
    {
        StopAllCoroutines();
        isPictureUpdated = false;
        buttonsPanelColliders = buttonPanel.GetComponentsInChildren<CircleCollider2D>();
        pictureTable = screenUIController.DataBase.GetPictureTableFromFile();
        gameSettings = screenUIController.DataBase.GetSettings();
        buttonNumberSection = buttonNumber.transform.GetChild(0).GetComponent<TextMesh>();
        StartCoroutine("InitSectionList");
        StartCoroutine("InitBrushesList");
        if (InitPlayPictureController())
            InitCircleProgressBarComponent();
        advicesController.NextAdvice(AdvicesController.ADVICES.SWIPE_LIST_CHOOSE_SECTION, false);
        return true;
    }

    /*
     * Инициализация горизонтального,
     *прокручивающегося списка с секциями картинки.
     */
    private IEnumerator InitSectionList()
    {
        sectionListItems = PlayPictureData.AmountSectionsType;
        sectionHorizontalListView.SetSectionListItems(sectionListItems);
        sectionHorizontalListView.InitIClickHorizontalListView(this);
        sectionHorizontalListView.InitSwipeListener(this);
        sectionHorizontalListView.ResetContainerPos();
        yield return null;
    }
    /*
     * Инициализацция горизонтального,  
     * прокручивающегося списка с кисточками.
     */
    private IEnumerator InitBrushesList()
    {
        var brushes = PlayPictureData.Brushes;
        brushesHorizontalListView.SetBrushListItems(brushes);
        brushesHorizontalListView.InitIClickHorizontalListView(this);
        brushesHorizontalListView.InitSwipeListener(this);
        brushesHorizontalListView.InitBuyMaterialListener(this);
        brushesHorizontalListView.SetVisiblity(VisibleStates.Gone);
        brushesHorizontalListView.ResetContainerPos();
        var brushesInTable = screenUIController.DataBase.GetBrushTableFromFile();
        if (brushesInTable.TableSize() > 0)
        {
            for (int i = 0; i < brushes.Length; i++)
            {
                if (brushesInTable.GetRowById(brushes[i].ID) != null)
                {
                    var listBrush = brushesHorizontalListView.GetItemByID(brushes[i].ID);
                    listBrush.BuyBrush(true);
                    listBrush.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
        yield return null;
    }
    /*
     * Инициализания картинки для игры.
     * Если картинка загружена из базы данных, тем самым она уже игралась, то мы заполняем 
     * закращеные секции нужными кисточками, выбирая кисточку по ее Ид.
     */
    private bool InitPlayPictureController()
    {
        StartCoroutine("IE_InitPlayPictureController");
        return true;
    }
    private IEnumerator IE_InitPlayPictureController()
    {
        playPictureController = Instantiate(PlayPictureData.PicturePlayObject).GetComponent<PlayPictureController>();
        playPictureController.transform.SetParent(spawnPicturePosition.transform);
        playPictureController.transform.localPosition = PlayPictureData.PicturePlayObject.transform.position;
        playPictureController.InitPlayPicture(PlayPictureData, playPictureCamera);
        playPictureController.InitCallbacks(this);
        playPictureZoomController = playPictureCamera.GetComponent<PlayPictureZoomController>();
        playPictureZoomController.InitIPlayPicrtureZoomListener(this);
        yield return null;
    }
    /*
     * Инициализация круглого прогресс бара,
     * для отоброжения процента заполняемости картинки.
     */
    private void InitCircleProgressBarComponent()
    {
        circleProgressBar = progressBar.GetComponent<CircleProgressBar>();
        circleProgressBar.SetProgressValue(0);
        circleProgressBar.SetOnCircleUpddateProgressListener(this);
        isPictureUpdated = false;

    }


    /*
     * Метод Update, используэм для вызова
     * методов обновления компонентов.
     */
    private void Update()
    {
        UpdatePictureColoringProgress();
        UpdateButtonNumberSectionText();
        UpdatePictureIfItLoadeFromDB();
        UpdateBackground();
        IsVisibleButtonsPanel(buttonsPanelVisibility);
    }



    private void UpdateBackground()
    {
        if (lastBackgroundId != backgroundID)
        {
            if (backgroundID < backgrounds.Length)
            {
                background.sprite = backgrounds[backgroundID].Sprite;
                lastBackgroundId = backgroundID;
            }
        }
    }

    /*
     * Обновления прогресс бара, 
     * для отоброжения процента заполняемости картинки.
     */
    private void UpdatePictureColoringProgress()
    {
        var colorPoints = playPictureController.CountColorPointsInPercents();
        if (lastColorPointsValue != colorPoints)
        {
            circleProgressBar.SetProgressValue(colorPoints);
        }
    }

    /*
     * Обновляем текст, на кнопке заполнения последней выбраной секции.
     */
    private void UpdateButtonNumberSectionText()
    {
        if (currentSection != null)
            if (currentSection.ToString() != buttonNumberSection.text)
                buttonNumberSection.text = currentSection.SectionNumber().ToString();
    }
    /*
     * Обновляет картинку всего раз, 
     * в том случае если процес рисования на ней продолжается.
     */
    private void UpdatePictureIfItLoadeFromDB()
    {
        if (!isPictureUpdated)
        {
            if (pictureTable.GetRowById(PlayPictureData.PitureId) != null)
            {
                if (!pictureTable.GetRowById(PlayPictureData.PitureId).IsFirstSelect)
                {
                    var allPlaySectionFromDB = pictureTable.GetRowById(PlayPictureData.PitureId).picturePoints;
                    for (int i = 0; i < allPlaySectionFromDB.Length; i++)
                    {
                        for (int j = 0; j < PlayPictureData.Brushes.Length; j++)
                        {
                            if (PlayPictureData.Brushes[j].ID == allPlaySectionFromDB[i].brushId)
                            {
                                var brush = new Brush();
                                brush.InitBrush(PlayPictureData.Brushes[j]);
                                brush.SetBrushColor(allPlaySectionFromDB[i].color);
                                playPictureController.Brush = brush;
                                playPictureController.ReColorSectionPoint(i);
                                /*var listBrush = brushesHorizontalListView.GetItemByID(allPlaySectionFromDB[i].brushId);
                                listBrush.CheckBrushStatus(true);
                                listBrush.transform.GetChild(1).gameObject.SetActive(false);*/
                            }
                        }
                    }
                    backgroundID = pictureTable.GetRowById(PlayPictureData.PitureId).backgroundID;
                    lastBackgroundId = backgroundID;
                    background.sprite = backgrounds[backgroundID].Sprite;
                }
            }
            isPictureUpdated = true;
        }
    }

    public IEnumerator BackToMenu()
    {
        canExite = true;
        while (canExite)
        {
            SavePicture();
            Destroy(playPictureController.gameObject);
            brushesHorizontalListView.ClearList();
            sectionHorizontalListView.ClearList();
            buttonsPanelVisibility = true;
            screenUIController.ShowPictureSelectActivity().ResetVerticalList();
            canExite = false;
            yield return null;
        }
    }

    /*
     * Машина выбора Юи елемента по которому нажали.
     */
    public override void OnClick(GameObject selectedUI)
    {
        base.OnClick(selectedUI);
        if (selectedUI != null)
            _OnClick(selectedUI);
    }

    /*
     * Обработчик нажатий на ЮИ.
     */
    public void _OnClick(GameObject selectedButton)
    {
        if (selectedButton != null)
            switch (selectedButton.tag)
            {
                case Utils.TAGS.BUTTON_BRUSHS:
                    {
                        brushesHorizontalListView.SetVisiblity(VisibleStates.Visible);
                        buttonsPanelVisibility = false;
                        if (brushesHorizontalListView.IsVisivle == VisibleStates.Visible && currentSection != null)
                            brushesHorizontalListView.SetBrushesColor(currentSection.GetColor());
                        advicesController.NextAdvice(AdvicesController.ADVICES.SWIPE_LIST_CHOOSE_BRUSHS, false);
                    }
                    break;
                case Utils.TAGS.BUTTON_NUMBER:
                    {
                        if (currentBrush != null && currentSection != null && buttonsPanelVisibility == true)
                        {
                            playPictureController.ColorFullLastSectionPoints();
                        }
                    }
                    break;
                case Utils.TAGS.CHOOSE_BG_BUTTON:
                    {
                        if (buttonsPanelVisibility == true)
                        {
                            if (backgroundID < backgrounds.Length)
                            {
                                backgroundID++;
                            }
                            else
                            {
                                backgroundID = 0;
                            }

                        }
                    }
                    break;
            }
    }

    private void SavePicture()
    {
        var allSections = playPictureController.GetAllPlaySectionsPoints();
        PictureTable.SectionPlayObjectProxy[] sections = new PictureTable.SectionPlayObjectProxy[allSections.Length];
        for (int i = 0; i < allSections.Length; i++)
        {
            sections[i] = new PictureTable.SectionPlayObjectProxy(allSections[i].BrushId, allSections[i].IsPainted, allSections[i].SectionColor);
        }
        var sShoot = Utils.CaptureSimple(playPictureCamera);
        playPictureCamera.enabled = true;
        var resizeSShoot = Utils.Resize(sShoot, Utils.ImageFilterMode.Biliner, 329f, 400f);
        var ShootBytes = resizeSShoot.GetRawTextureData();
        var defaultePicture = UIPictureData.Image.texture.GetRawTextureData();
        var defaultPictureFormat = UIPictureData.Image.texture.format;
        screenUIController.DataBase.UpdatePictureTable(PlayPictureData.PitureId,
                                                       PlayPictureData.Material,
                                                           true,
                                                           false,
                                                           playPictureController.CountColorPointsInPercents(),
                                                           sections,
                                                       ShootBytes,
                                                       defaultePicture,
                                                       resizeSShoot.format,
                                                       defaultPictureFormat,
                                                       backgroundID);
    }



    /*
     * Обработчик нажатия кнопок на горизонтальных
     * скролл списках.
     */
    public void OnClickItem(GameObject item)
    {
        /*
         * Выбран елемент из списка секций.
         */
        if (item.GetComponent<SectionListItem>() != null)
        {
            var section = item.GetComponent<SectionListItem>();
            currentSection = section;
            playPictureController.SelectSection(section.SectionNumber());
            brushesHorizontalListView.SetBrushesColor(section.GetColor());
            advicesController.NextAdvice(AdvicesController.ADVICES.OPEN_BRUSHS_LIST, false);
        }
        /*
         * Выбран елемент из списка кисточек.
         */
        if (item.GetComponent<Brush>() != null)
        {
            var brush = item.GetComponent<Brush>();
            currentBrush = brush;
            if (currentSection != null && brush.IsFreeBrush())
            {
                playPictureController.Brush = brush;
                buttonsPanelVisibility = true;
                brushesHorizontalListView.SetVisiblity(VisibleStates.Gone);
                advicesController.NextAdvice(AdvicesController.ADVICES.COLORING_ONE_POINT, false);
            }
        }
    }

    public void OnBuyMaterial(Brush brush)
    {
        Debug.Log("Brush was buying!: " + brush.GetBrushId());
        screenUIController.GetCoinController().RemoveCoin(brush.GetBrushPrice());
        screenUIController.DataBase.UpdateBrushTable(brush.GetBrushId(), brush.IsFreeBrush(), brush.GetBrushMaterial());
    }

    private bool IsVisibleButtonsPanel(bool visibility)
    {
        for (int i = 0; i < buttonPanel.transform.childCount; i++)
        {
            buttonPanel.transform.GetChild(i).gameObject.SetActive(visibility);
        }
        for (int i = 0; i < buttonsPanelColliders.Length; i++)
        {
            buttonsPanelColliders[i].gameObject.SetActive(visibility);
        }
        return true;
    }

    void CircleProgressBar.ICircleUpdateProgress.UpdateProgress(int progressValue)
    {
        GoToRoulette(progressValue);
    }

    private void GoToRoulette(int progressValue)
    {
        lastColorPointsValue = progressValue;
        if (progressValue == 100 && !isCoinsAdded)
        {
            isCoinsAdded = true;
            screenUIController.GetCoinController().AddCoin(reward);
            SavePicture();
            Destroy(playPictureController.gameObject);
            brushesHorizontalListView.ClearList();
            sectionHorizontalListView.ClearList();
            buttonsPanelVisibility = true;
            Debug.Log("Before ROulette showed!");
            screenUIController.ShowRouletteActivity().InitRouletteController();
            Debug.Log("ROulet ShoweD!");
            isCoinsAdded = false;
        }
    }

    public void UpdateZoome(float currentZoom, bool isZooming)
    {
        if (isZooming)
        {
            advicesController.NextAdvice(AdvicesController.ADVICES.ZOOMING_PICTURE_MINUS, false);

        }
        else
        {
            advicesController.NextAdvice(AdvicesController.ADVICES.SWIPE_PICTURE_LEFT, false);
        }
    }

    public void UpdateSwipe(bool rightSwipe, bool leftSwipe, bool upSwipe, bool bottomSwipe)
    {
        if (rightSwipe && !leftSwipe && !upSwipe && !bottomSwipe)
        {
            rightSwipe = false;
            advicesController.NextAdvice(AdvicesController.ADVICES.SWIPE_PICTURE_UP, false);
        }
        else
        if (!rightSwipe && leftSwipe && !upSwipe && !bottomSwipe)
        {
            leftSwipe = false;
            advicesController.NextAdvice(AdvicesController.ADVICES.SWIPE_PICTURE_RIGHT, false);
        }
        else
        if (!rightSwipe && !leftSwipe && upSwipe && !bottomSwipe)
        {
            upSwipe = false;
            advicesController.NextAdvice(AdvicesController.ADVICES.SWIPE_PICTURE_DOWN, false);
        }
        else
        if (!rightSwipe && !leftSwipe && !upSwipe && bottomSwipe)
        {
            bottomSwipe = false;
            advicesController.NextAdvice(AdvicesController.ADVICES.FINISHED, false);
        }
    }

    public void OnTupColorFinished()
    {
        advicesController.NextAdvice(AdvicesController.ADVICES.COLORIMG_MORE_POINT, false);
    }

    public void OnHoldColorFinished()
    {
        advicesController.NextAdvice(AdvicesController.ADVICES.ZOOMING_PICTURE_PLUS, false);
    }

    public void UpdateSwipeList(HorizontaListView listView, float swipeTime, float swipeSpeed)
    {
        if (listView is BrushesHorizontalListView)
        {
            advicesController.NextAdvice(AdvicesController.ADVICES.CHOOSE_LIST_ITEM_BRUSHS, false);
        }

        if (listView is SectionHorizontalListView)
        {
            advicesController.NextAdvice(AdvicesController.ADVICES.CHOOSE_LIST_ITEM_CHOOSE_SECTION, false);
        }
    }


}
