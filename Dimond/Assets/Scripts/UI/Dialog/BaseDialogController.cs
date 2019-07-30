using UnityEngine;
using System.Collections;

public abstract class BaseDialogController : BaseActivityController
{
    [SerializeField] protected TextMesh title;
    [SerializeField] protected TextMesh message;
    [SerializeField] protected SpriteRenderer icon;
    [SerializeField] protected Button ok_button;
    [SerializeField] protected AnimationCurve animation;

    protected IDialogListener dialogListener;

    public override void OnClick(GameObject selectedUI)
    {
        base.OnClick(selectedUI);
        Debug.Log("Selected Button:  " + selectedUI.tag);
        if (selectedUI != null)
            _OnClick(selectedUI);
    }

    protected abstract void _OnClick(GameObject selectedUI);
    public virtual BaseDialogController InitDialog(IDialogListener dialogListener)
    {
        this.dialogListener = dialogListener;
        return this;
    }

    public BaseDialogController SetTitle(string title)
    {
        this.title.text = title;
        return this;
    }

    public BaseDialogController SetText(string text)
    {
        this.message.text = text;
        return this;
    }

    protected void SetIcon(Sprite icon)
    {
        this.icon.sprite = icon;
    }

    public IEnumerator ShowDialog()
    {
        //ok_button.transform.localScale = new Vector3(11.5f, 12.5f, 17.3f);
        float step = 0;
        while(step < 1)
        {
            transform.position = new Vector3(animation.Evaluate(step), transform.position.y, transform.position.z);
            step += Time.deltaTime * 1.2f;
            yield return null;
        }
     
    }

    public IEnumerator DissmisDialog()
    {
       //ok_button.transform.localScale = new Vector3(11.5f, 12.5f, 17.3f);
        float step = 1;
        while (step > 0)
        {
            transform.position = new Vector3(animation.Evaluate(step), transform.position.y, transform.position.z);
            step -= Time.deltaTime * 1.2f;
            yield return null;
        }
    }

}
