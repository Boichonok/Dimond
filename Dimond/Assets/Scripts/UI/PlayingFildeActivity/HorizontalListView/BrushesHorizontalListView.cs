using UnityEngine;
using System.Collections.Generic;

public class BrushesHorizontalListView : HorizontaListView
{

    private IBuyMaterialHorizontalListView buyMaterialListener;

    protected override string ContainerTag
    {
        get
        {
            return "brushes_list_container";
        }
            
    }
    protected override void initItem(GameObject item, int index)
    {
        var sectionItem = item.GetComponent<Brush>();
        var listItem = item.GetComponent<ListItemButton>();
        sectionItem.InitBrush(items[index] as BrushesData);
        listItem.InitList(this as HorizontaListView);
        listItem.transform.GetChild(1).GetComponent<ListItemButton>().InitList(this as HorizontaListView);
        listItem.transform.GetChild(1).GetComponent<ListItemButton>().GetComponentInChildren<TextMesh>().text = sectionItem.GetBrushPrice().ToString();
    }

    public void SetBrushListItems(BrushesData[] items)
    {
        this.items = items;
        drawListItem1xN(items.Length);
    }

    public void InitBuyMaterialListener(IBuyMaterialHorizontalListView listener)
    {
        this.buyMaterialListener = listener;
    }

    public void SetBrushesColor(Color color)
    {
        var brushes = ItemsContainerPref.GetComponentsInChildren<Brush>();
        for (int i = 0; i < items.Length; i ++)
        {
            brushes[i].SetBrushColor(color);
        }
    }

    public Brush GetItemByID(int brushId)
    {
        var brush = ItemsContainerPref.transform.GetComponentsInChildren<Brush>();
        for (int i = 0; i < brush.Length; i++)
        {
            if(brush[i].GetBrushId() == brushId)
            {
                return brush[i];
            }
        }
        return null;
    }

    public override void SelectItem(GameObject selectedUI)
    {
        base.SelectItem(selectedUI);
        if (selectedUI != null && selectedUI.tag == Utils.TAGS.BUY_BUTTON)
        {
            var brush = selectedUI.transform.parent.GetComponent<Brush>();
            if (brush.BuyBrush(true))
            {
                selectedUI.SetActive(false);
                buyMaterialListener.OnBuyMaterial(brush);
            }
        }
    }

}
