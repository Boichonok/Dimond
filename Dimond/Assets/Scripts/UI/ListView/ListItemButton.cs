using UnityEngine;
using System.Collections;

public class ListItemButton : MonoBehaviour
{
    [SerializeField] private ListView listView;

    public bool used;

    public void InitList(ListView list)
    {
        listView = list;
    }

    private void OnMouseDown()
    {
        StopAllCoroutines();
        used = true;
        listView.StartCoroutine("TouchFirst",listView.ItemsContainerPref);
    }

    private void OnMouseUp()
    {
        if (used)
            listView.SelectItem(gameObject);
        if (listView.gameObject.active)
        {
            listView.StartCoroutine("TouchSecond");
            listView.StartCoroutine("InercionList");
        }
        used = false;
    }

    private void OnMouseExit()
    {
        used = false;
    }

   

    private void OnMouseDrag()
    {
        if (!used)
        {
            listView.StopCoroutine("InercionList");
            listView.StartCoroutine("UpdateColorNearSides");
            listView.StartCoroutine("TouchDragList",listView.ItemsContainerPref);
        }
    }
}
