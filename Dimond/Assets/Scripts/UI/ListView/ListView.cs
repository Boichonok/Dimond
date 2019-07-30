using UnityEngine;
using System.Collections;

public abstract class ListView : MonoBehaviour
{
    [SerializeField]
    public GameObject ItemsContainerPref = null;
    [SerializeField]
    protected GameObject itemsPref = null;

    protected float scroll;
    protected float lastScroll;
    [SerializeField]
    protected float drugSpeed;

    public abstract IEnumerator InercionList();
    public abstract IEnumerator TouchFirst(GameObject containerList);
    public abstract IEnumerator TouchSecond();
    public abstract IEnumerator TouchDragList(GameObject containerList);
    public abstract IEnumerator UpdateColorNearSides();
    public abstract void SelectItem(GameObject selectedUI);

    public void ResetContainerPos()
    {
        scroll = 0.0f;
        lastScroll = 0.0f;
    }
}
