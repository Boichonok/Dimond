using UnityEngine;
using System.Collections;

public class RoulleteInfoDialogController : BaseDialogController
{
    [SerializeField] private Sprite[] bonusIcons;

    private string bonusTag;

    public RoulleteInfoDialogController SetBonusTag(string bonusTag)
    {
        this.bonusTag = bonusTag;
        return this;
    }

    public override BaseDialogController InitDialog(IDialogListener dialogListener)
    {
        base.InitDialog(dialogListener);
        StopAllCoroutines();
        StartCoroutine("IEInitDialog");
        return this;
    }

    private IEnumerator IEInitDialog()
    {
        switch (bonusTag)
        {
            case Utils.TAGS.BONUS_X4:
                {
                    SetIcon(bonusIcons[0]);
                }
                break;
            case Utils.TAGS.BONUS_x3:
                {
                    SetIcon(bonusIcons[0]);
                }
                break;
            case Utils.TAGS.BONUS_Dimond:
                {
                    SetIcon(bonusIcons[1]);
                }
                break;
            case Utils.TAGS.BONUS_Coloring:
                {
                    SetIcon(bonusIcons[2]);
                }
                break;
            case Utils.TAGS.BONUS_Strands:
                {
                    SetIcon(bonusIcons[3]);
                }
                break;
            case Utils.TAGS.BONUS_Picture:
                {
                    SetIcon(bonusIcons[4]);
                }
                break;
        }
        yield return null;
    }

    protected override void _OnClick(GameObject selectedUI)
    {
        switch (selectedUI.tag)
        {
            case Utils.TAGS.OK_DIALOG_BUTTON:
                {
                    StartCoroutine("DissmisDialog");
                    dialogListener.OnClickOkButton(this);
                }
                break;
        }
    }

   
}
