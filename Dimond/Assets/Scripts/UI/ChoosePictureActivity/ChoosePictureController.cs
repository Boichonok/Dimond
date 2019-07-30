using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePictureController : MonoBehaviour, IClickVListChPictureActivityItem
{
    private PicturesVerticalListView verticalListView;

    private ScreenUIController screenUIController;

    [SerializeField]
    private UIPictureData[] uiContent = null;

    [SerializeField]
    private PlayPictureData[] playPictureContent = null;

    private int contentListLenght;

    private List<UIPictureController> UIPicturesList;

    private bool isListUpdate = false;

    private int updatePictureCount = 0;

    public PictureTable pictureTable { get; set; }

    public void OnClickBuyPicture(UIPictureController selectedUI)
    {
        selectedUI.OnClickBuyPicture();
    }

    public void OnClickPicture(UIPictureController selectedUI)
    {
        if (selectedUI.OnClickSelectPicture())
        {
            if (selectedUI.isFirstOpen)
            {
                var chooseMaterialController = screenUIController.ShowMaterialSelectActivtiy();
                chooseMaterialController.SelectedPicture = selectedUI;
                pictureTable.UpdateIsBuyingInRow(selectedUI.pictureID, true);
                pictureTable.UpdateIsBuyingInRow(selectedUI.pictureID, false);
            }
            else
            {
                for (int i = 0; i < playPictureContent.Length; i++)
                {
                    if (playPictureContent[i].PitureId == selectedUI.pictureID 
                        && pictureTable.GetRowById(selectedUI.pictureID, playPictureContent[i].Material) != null)
                    {
                        var picture = pictureTable.GetRowById(selectedUI.pictureID, playPictureContent[i].Material);
                        if (picture.drawingProgress < 100)
                        {
                            var playFildeActivity = screenUIController.ShowPlayingFildActivity();
                            playFildeActivity.PlayPictureData = playPictureContent[i];
                            playFildeActivity.UIPictureData = selectedUI.Data;
                            if (playFildeActivity.isActiveAndEnabled)
                                playFildeActivity.OnStart();
                            break;
                        }
                        else
                        {
                            screenUIController.ShowGalleryActivity().OnStart();
                        }
                    }

                }

            }

        }
    }

    public void OnClickResetPicture(UIPictureController selectedUI)
    {
        var isReset = screenUIController.DataBase.ResetRowFromPictureTableByID(selectedUI.pictureID,
                                                                               selectedUI.Data.Image.texture.GetRawTextureData(),
                                                                               selectedUI.Data.Image.texture.format);
        if (isReset)
        {
            selectedUI.OnClickResetPicture(isReset, selectedUI.Data);
        }
    }

    void Start()
    {
        verticalListView.drawItems2xN(uiContent.Length);
        verticalListView.InitIClickVerticalListViewItem(this as IClickVListChPictureActivityItem);
        verticalListView.AddContentToItemsByScrObj(uiContent);
        verticalListView.ResetContainerPos();
        contentListLenght = verticalListView.GetAllContent().Count;
        UIPicturesList = verticalListView.GetAllContent();
        pictureTable = screenUIController.DataBase.GetPictureTableFromFile();
    }
    private void Awake()
    {
        isListUpdate = false;
        screenUIController = FindObjectOfType<ScreenUIController>();
        verticalListView = GetComponentInChildren<PicturesVerticalListView>();
    }
    private void LateUpdate()
    {
        UpdateListItem();
    }

    public void ResetVerticalList()
    {
        isListUpdate = false;
        updatePictureCount = 0;
    }

    private void UpdateListItem()
    {
        if (!isListUpdate)
        {
            if (updatePictureCount < pictureTable.GetTableSize())
            {
                for (int i = 0; i < contentListLenght; i++)
                {

                    for (int j = 0; j < pictureTable.GetTableSize(); j++)
                    {

                        if (UIPicturesList[i].pictureID == pictureTable.GetRowByIndex(j).id)
                        {
                            UIPicturesList[i].isFirstOpen = pictureTable.GetRowByIndex(j).IsFirstSelect;
                            UIPicturesList[i].isBuying = pictureTable.GetRowByIndex(j).IsBuying;
                            UIPicturesList[i].UpdatePictureView(pictureTable.GetRowByIndex(j).pictureInBytes, pictureTable.GetRowByIndex(j).pictureFormat);
                            UIPicturesList[i].UpdatePictureState();
                        }
                    }

                }
                updatePictureCount++;
            }
            else
            {
                isListUpdate = true;
            }
        }
    }
}
