using UnityEngine;
using System.Collections;

public class RouletteController : BaseActivityController, IRouletteBonusListener, IDialogListener
{
    [SerializeField] RotationRoulette rotationRoulette = null;
    [SerializeField] ScreenUIController screenUIController = null;
    [SerializeField] PlayingFildeController playingFildeController = null;
    [SerializeField] UIPictureData[] NotFreeUiPictureDatas;
    [SerializeField] BrushesData[] NotFreeBrushesDimondDatas;
    [SerializeField] BrushesData[] NotFreeBrushesColoringDatas;
    [SerializeField] BrushesData[] NotFreeBrushesStrandDatas;
    [SerializeField] RoulleteInfoDialogController dialogInfo;
    [SerializeField] Button go_button;

    private Coins coinsController;
    private DataBase dataBase;
    private BrushTable brushTable;
    private PictureTable pictureTable;

    public void Bonus(GameObject bonus)
    {
        Debug.Log("Bonus: " + bonus.tag);
        StopAllCoroutines();
        switch (bonus.tag)
        {
            case Utils.TAGS.BONUS_X4:
                {
                    StartCoroutine(BonusCoinX_N(4, Utils.TAGS.BONUS_X4));
                }
                break;
            case Utils.TAGS.BONUS_x3:
                {
                    StartCoroutine(BonusCoinX_N(3, Utils.TAGS.BONUS_x3));
                }
                break;
            case Utils.TAGS.BONUS_Dimond:
                {
                    StartCoroutine(BonusBrush(NotFreeBrushesDimondDatas, Utils.TAGS.BONUS_Dimond));
                }
                break;
            case Utils.TAGS.BONUS_Coloring:
                {
                    StartCoroutine(BonusBrush(NotFreeBrushesColoringDatas, Utils.TAGS.BONUS_Coloring));
                }
                break;
            case Utils.TAGS.BONUS_Strands:
                {
                    StartCoroutine(BonusBrush(NotFreeBrushesStrandDatas, Utils.TAGS.BONUS_Strands));
                }
                break;
            case Utils.TAGS.BONUS_Picture:
                {
                    StartCoroutine(BonusPicture(Utils.TAGS.BONUS_Picture));
                }
                break;
        }
    }

    public void InitRouletteController()
    {
        StopAllCoroutines();
        StartCoroutine("IEInitRouletteController");

    }

    private IEnumerator IEInitRouletteController()
    {
        rotationRoulette.InitBonusListener(this as IRouletteBonusListener);
        coinsController = screenUIController.GetCoinController();
        dataBase = screenUIController.DataBase;
        brushTable = dataBase.GetBrushTableFromFile();
        pictureTable = dataBase.GetPictureTableFromFile();
        go_button.enabled = true;
        go_button.GetComponent<SpriteRenderer>().color = new Color(go_button.GetComponent<SpriteRenderer>().color.r,
                                                                                   go_button.GetComponent<SpriteRenderer>().color.g,
                                                                                   go_button.GetComponent<SpriteRenderer>().color.b,
                                                                                    go_button.GetComponent<SpriteRenderer>().color.a * 2);
        dialogInfo.DissmisDialog();
        yield return null;
    }

    public override void OnClick(GameObject selectedUI)
    {
        base.OnClick(selectedUI);
        Debug.Log("Selected Button:  " + selectedUI.tag);
        if (selectedUI != null)
            _OnClick(selectedUI);
    }

    private void _OnClick(GameObject selectedUI)
    {
        switch (selectedUI.tag)
        {
            case Utils.TAGS.START_ROULETTE:
                {
                    if (selectedUI.GetComponent<Button>().enabled)
                    {
                        rotationRoulette.StartRotation();
                        selectedUI.GetComponent<Button>().enabled = false;
                        selectedUI.GetComponent<SpriteRenderer>().color = new Color(selectedUI.GetComponent<SpriteRenderer>().color.r,
                                                                                   selectedUI.GetComponent<SpriteRenderer>().color.g,
                                                                                   selectedUI.GetComponent<SpriteRenderer>().color.b,
                                                                                    selectedUI.GetComponent<SpriteRenderer>().color.a / 2);
                    }
                }
                break;
        }
    }

    private IEnumerator BonusCoinX_N(int N, string bonusTag)
    {
        coinsController.AddCoin(playingFildeController.Reward * N);
        dialogInfo.SetBonusTag(bonusTag)
                  .SetTitle("Greate!")
                  .SetText(string.Format("x{0}\n reward's Coins!", N))
                  .InitDialog(this as IDialogListener);
        dialogInfo.StartCoroutine("ShowDialog");

        yield return null;
    }

    private IEnumerator BonusBrush(BrushesData[] brushesDatas, string bonusTag)
    {
        if (brushesDatas.Length > 0)
        {
            var brush = brushesDatas[Random.Range(0, brushesDatas.Length)];
            if (!brush.IsFreeBrush)//Если эта кисточка не куплена
            {
                if (brushTable.TableSize() > 0 && brushTable.GetRowById(brush.ID) != null)
                {
                    coinsController.AddCoin(brush.BrushPrice);
                    Debug.Log("Take money!\n Coins: " + brush.BrushPrice);
                    //Добавить вызов отображения Диалогового окна.
                    dialogInfo.SetBonusTag(bonusTag)
                              .SetTitle("Greate!")
                              .SetText(string.Format("Take money!\nCoins: {0}", brush.BrushPrice))
                              .InitDialog(this as IDialogListener);
                    dialogInfo.StartCoroutine("ShowDialog");

                }
                else
                {
                    dataBase.UpdateBrushTable(brush.ID, true, brush.Type);
                    //Добавить вызов отображения Диалогового окна.
                    Debug.Log("You goted brush with ID: " + brush.ID);
                    dialogInfo.SetBonusTag(bonusTag)
                              .SetTitle("Greate!")
                              .SetText(string.Format("You goted new brush!"))
                              .InitDialog(this as IDialogListener);
                    dialogInfo.StartCoroutine("ShowDialog");
                }
            }
        }
        else
        {
            dialogInfo.SetBonusTag(bonusTag)
                             .SetTitle("Faile!")
                             .SetText(string.Format("Nothing!"))
                             .InitDialog(this as IDialogListener);
            dialogInfo.StartCoroutine("ShowDialog");
        }
        yield return null;
    }

    private IEnumerator BonusPicture(string bonusTag)
    {
        var picture = NotFreeUiPictureDatas[Random.Range(0, NotFreeUiPictureDatas.Length)];
        if (picture.PictureType != PictureType.FreePicture &&
           picture.PictureType != PictureType.LockedPicture)
        {
            if (pictureTable.GetTableSize() > 0 && pictureTable.GetRowById(picture.PitureId) != null)
            {

                coinsController.AddCoin(picture.Prise);
                //Добавить вызов отображения Диалогового окна.
                if (picture.PictureType == PictureType.AdsPicture)
                {
                    Debug.Log("You already had this ads picture,\ntake Nothing!");
                    dialogInfo.SetBonusTag(bonusTag)
                              .SetTitle("Faile!")
                              .SetText(string.Format("Nothing!"))
                              .InitDialog(this as IDialogListener);
                    dialogInfo.StartCoroutine("ShowDialog");
                }
                else
                {
                    Debug.Log("Take money!\nCoins: " + picture.Prise);
                    dialogInfo.SetBonusTag(bonusTag)
                              .SetTitle("Grate!")
                              .SetText(string.Format("Take money!\nCoins: " + picture.Prise))
                              .InitDialog(this as IDialogListener);
                    dialogInfo.StartCoroutine("ShowDialog");
                }
            }
            else
            {
                dataBase.UpdatePictureTable(picture.PitureId,
                                           playingFildeController.PlayPictureData.Material,
                                          true,
                                          true,
                                          0,
                                          null,
                                           picture.Image.texture.GetRawTextureData(),
                                           picture.Image.texture.GetRawTextureData(),
                                           picture.Image.texture.format,
                                           picture.Image.texture.format, -1);
                //Добавить вызов отображения Диалогового окна.
                Debug.Log("You have new picture. Picture Name: " + picture.name);
                dialogInfo.SetBonusTag(bonusTag).SetTitle("Grate!")
                              .SetText(string.Format("You have new picture.\nPicture Name: " + picture.name))
                              .InitDialog(this as IDialogListener);
                dialogInfo.StartCoroutine("ShowDialog");
            }
        }
        yield return null;
    }

    public void OnClickOkButton(BaseDialogController dialog)
    {
        Debug.Log("OkDialog!Button.");
        StartCoroutine("GoToGallery");
    }

    private IEnumerator GoToGallery()
    {
        yield return new WaitForSeconds(2f);
        screenUIController.ShowGalleryActivity().OnStart();
    }
}
