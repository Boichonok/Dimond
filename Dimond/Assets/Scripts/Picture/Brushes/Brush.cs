using UnityEngine;
using System.Collections;

public class Brush : MonoBehaviour
{
    private BrushesData data;

    private SpriteRenderer view;
    private Sprite viewSprite;
    private SpriteRenderer mask;
    private Sprite maskSprite;
    private Color currentColor;
    private Color oldColor;
    private bool isUpdateColor = false;
    private bool isFreeBrush;


    private bool isInitBrush = false;
    public void InitBrush(BrushesData data_)
    {
        data = data_;
        viewSprite = data.BrushView;
        maskSprite = data.BrushMask;
        isFreeBrush = data_.IsFreeBrush;
    }

    private void initBrushView()
    {
        view = GetComponent<SpriteRenderer>();
        view.sprite = viewSprite;
        mask = transform.GetChild(0).GetComponent<SpriteRenderer>();
        mask.sprite = maskSprite;
        if (data.IsFreeBrush)
            transform.GetChild(1).gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isInitBrush)
        {
            initBrushView();
            isInitBrush = true;
        }
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (currentColor != mask.color && isUpdateColor)
        {
            mask.color = currentColor;
            view.color = currentColor;
            if (data.IsFreeBrush)
                transform.GetChild(1).gameObject.SetActive(false);
            isUpdateColor = false;
        }
    }


    public void SetBrushColor(Color color)
    {
        currentColor = color;
        isUpdateColor = true;
    }

    public Color GetBrushCurrentColor()
    {
        return currentColor;
    }

    public Sprite GetBrushSpriteView()
    {
        return viewSprite;
    }

    public Sprite GetBrushSpriteMask()
    {
        return maskSprite;
    }

    public int GetBrushId()
    {
        return data.ID;
    }

    public bool IsFreeBrush()
    {
        return this.isFreeBrush;
    }

    public int GetBrushPrice()
    {
        return data.BrushPrice;
    }

    public BrushesType GetBrushMaterial()
    {
        return data.Type;
    }

    public bool BuyBrush(bool isFreeBrush)
    {
        if (!this.isFreeBrush && isFreeBrush)
        {
            this.isFreeBrush = isFreeBrush;
            return true;
        }
        return false;
    }
}
