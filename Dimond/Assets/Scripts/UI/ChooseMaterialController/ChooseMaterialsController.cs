using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseMaterialsController : BaseActivityController, IClickButtonListener
{
    [SerializeField]
    private PlayPictureData[] playPictureDataList = null;

    private Dictionary<PlayPictureDatasKey, PlayPictureData> playPictureDatas = new Dictionary<PlayPictureDatasKey, PlayPictureData>();
    private ArrayList keys = new ArrayList();

    private UIPictureController seletedPicture;
    public UIPictureController SelectedPicture { private get { return seletedPicture; } set { seletedPicture = value; } }
    [SerializeField]
    private ScreenUIController screenUIController;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < playPictureDataList.Length; i++)
        {
            var newKey = new PlayPictureDatasKey(playPictureDataList[i].name, playPictureDataList[i].Material);
            keys.Add(newKey);
            playPictureDatas.Add(newKey, playPictureDataList[i]);
        }
    }

   

    public override void OnClick(GameObject selectedUI)
    {
        base.OnClick(selectedUI);
        if (selectedUI != null)
            _OnClick(selectedUI);
    }

    public void _OnClick(GameObject selectedButton)
    {

        var keysLength = keys.Count;
        PlayPictureData picture = null;
        switch (selectedButton.tag)
        {
            case "stones_button":
                {
                    for (int i = 0; i < keysLength; i++)
                    {
                        var key = keys[i] as PlayPictureDatasKey;
                        if (key.name == seletedPicture.PictureName && key.material == PictureMaterial.Stounes)
                        {
                            picture = playPictureDatas[key];
                            break;
                        }
                    }
                }
                break;
            case "strands_button":
                {
                    for (int i = 0; i < keysLength; i++)
                    {
                        var key = keys[i] as PlayPictureDatasKey;
                        if (key.name == seletedPicture.PictureName && key.material == PictureMaterial.Strands)
                        {
                            picture = playPictureDatas[key];
                            break;
                        }
                    }
                }
                break;
            case "colors_button":
                {
                    for (int i = 0; i < keysLength; i++)
                    {
                        var key = keys[i] as PlayPictureDatasKey;
                        if (key.name == seletedPicture.PictureName && key.material == PictureMaterial.Colors)
                        {
                            picture = playPictureDatas[key];
                            break;
                        }
                    }
                }
                break;
        }
        if (picture != null)
        {
            var playingFIldeActivity = screenUIController.ShowPlayingFildActivity();
            playingFIldeActivity.PlayPictureData = picture;
            playingFIldeActivity.UIPictureData = SelectedPicture.Data;
            if (playingFIldeActivity.isActiveAndEnabled)
                playingFIldeActivity.OnStart();
        }

    }

   

    private class PlayPictureDatasKey
    {
        public string name { get; private set; }
        public PictureMaterial material { get; private set; }

        public PlayPictureDatasKey(string name, PictureMaterial material)
        {
            this.name = name;
            this.material = material;
        }
    }
}
