using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryController : BaseActivityController, IClickVListView
{
    [SerializeField]
    private GameObject buttonsPanel = null;

    private PicturesVerticalListView verticalListView;

    private ScreenUIController screenUIController;

    private List<PictureTable.PictureProxy> allPicturesForGalerryFromDB;

    private PictureTable.PictureProxy[] allPicturesForGalleryFromDBArray;

    private int amountPicturesForGalerry;

    private PictureTable pictureTable;

    private bool isContentAdded = true;

    private bool isPictureScaled = false;

    private UIPictureController currentSelectedUI = null;

    public void OnClickPicture(UIPictureController selectedUI)
    {
        if (!isPictureScaled)
        {
            selectedUI.transform.localScale = new Vector2(selectedUI.transform.localScale.x + 0.5f, selectedUI.transform.localScale.y + 0.5f);
            buttonsPanel.SetActive(true);
            isPictureScaled = true;
            currentSelectedUI = selectedUI;

        }
        else
        {
            selectedUI.transform.localScale = new Vector2(selectedUI.transform.localScale.x - 0.5f, selectedUI.transform.localScale.y - 0.5f);
            buttonsPanel.SetActive(false);
            isPictureScaled = false;
            currentSelectedUI = null;
        }
    }

    // Use this for initialization

    public void OnStart()
    {
        verticalListView.drawItems1xN(amountPicturesForGalerry);
        verticalListView.InitIClickGalleryListViewItem(this);
        allPicturesForGalleryFromDBArray = allPicturesForGalerryFromDB.ToArray();
        if (allPicturesForGalleryFromDBArray.Length != 0)
            verticalListView.AddContentToItemsByPictProxy(allPicturesForGalleryFromDBArray);
        verticalListView.ResetContainerPos();
        isContentAdded = false;
        isPictureScaled = false;
        buttonsPanel.SetActive(false);
    }
    private void Update()
    {
        if (!isContentAdded)
        {
            var content = verticalListView.GetAllContent();
            var contentSize = content.Count;
            for (int i = 0; i < contentSize; i++)
            {
                content[i].UpdatePictureView(allPicturesForGalleryFromDBArray[i].pictureInBytes, allPicturesForGalleryFromDBArray[i].pictureFormat);
                content[i].UpdatePictureState();
            }
            isContentAdded = true;
        }
    }

    public void ResetVerticalList()
    {
        verticalListView.ClearList();
    }

    public override void OnClick(GameObject selectedUI)
    {
        base.OnClick(selectedUI);
        if (selectedUI != null)
        {
            switch (selectedUI.tag)
            {
                case Utils.TAGS.VIBER_BUTTON:
                    {

                    }
                    break;
                case Utils.TAGS.FACEBOOK_BUTTON:
                    {

                    }
                    break;
                case Utils.TAGS.INSTAGRAMM_BUTTON:
                    {

                    }
                    break;
                case Utils.TAGS.RESET_BUTTON:
                    {
                        if (currentSelectedUI != null)
                        {
                            var defaulImage = allPicturesForGalerryFromDB.Find((PictureTable.PictureProxy obj) => obj.id == currentSelectedUI.pictureID);
                            var isReset = screenUIController.DataBase.ResetRowFromPictureTableByID(currentSelectedUI.pictureID,
                                                                                                   defaulImage.defoultPictureInBytes,
                                                                                                   defaulImage.defaultPictureFormat);
                            if (isReset)
                            {
                                currentSelectedUI.OnClickResetPicture(isReset, currentSelectedUI.Data);
                                var content = verticalListView.GetAllContent();
                                var parent = currentSelectedUI.transform.parent;
                                content.Remove(currentSelectedUI);
                                Destroy(currentSelectedUI.gameObject);
                                Destroy(parent.gameObject);
                                buttonsPanel.SetActive(false);
                            }
                        }
                    }
                    break;
            }
        }
    }

    private void OnEnable()
    {
        screenUIController = FindObjectOfType<ScreenUIController>();
        verticalListView = GetComponentInChildren<PicturesVerticalListView>();
        pictureTable = screenUIController.DataBase.GetPictureTableFromFile();
        allPicturesForGalerryFromDB = pictureTable.GetPicturesForGallery();
        amountPicturesForGalerry = allPicturesForGalerryFromDB.Count;
    }

    private void OnDisable()
    {

    }
}
