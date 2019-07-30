using UnityEngine;
using System.Collections.Generic;

public class ContainerListView : MonoBehaviour
{
    [SerializeField] private ListView listView;

 
    private void OnMouseDown()
    {
        StopAllCoroutines();
        if (listView.enabled)
            listView.StartCoroutine("TouchFirst",gameObject);
    }

    private void OnMouseUp()
    {
        if (listView.enabled)
        {
            listView.StartCoroutine("TouchSecond");
            listView.StartCoroutine("InercionList");
            listView.StartCoroutine("UpdateColorNearSides");
        }
    }

    private void OnMouseDrag()
    {
        if (listView.enabled)
        {
            listView.StopCoroutine("InercionList");
            listView.StartCoroutine("UpdateColorNearSides");
            listView.StartCoroutine("TouchDragList", gameObject);
        }
    }




}
