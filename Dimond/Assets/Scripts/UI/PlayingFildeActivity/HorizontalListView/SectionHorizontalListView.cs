using UnityEngine;
using System.Collections;

public class SectionHorizontalListView : HorizontaListView
{
    protected override string ContainerTag
    {
        get
        {
            return "section_list_container";
        }
    }

    protected override void initItem(GameObject item, int index)
    {
        var sectionItem = item.GetComponent<SectionListItem>();
        var listItem = item.GetComponent<ListItemButton>();
        sectionItem.InitSection(items[index] as SectionData);
        listItem.InitList(this as HorizontaListView);
    }

    public void SetSectionListItems(SectionData[] items)
    {
        this.items = items;
        drawListItem1xN(items.Length);
    }
   
}
