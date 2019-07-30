using UnityEngine;
using System.Collections;

public enum VisibleStates
{
    Visible,
    Gone
}

public abstract class HorizontaListView : ListView
{


    protected abstract string ContainerTag { get; }

    private VisibleStates visible = VisibleStates.Visible;
    public VisibleStates IsVisivle { get { return visible; } }


    [SerializeField]
    private GameObject leftSide = null;
    [SerializeField]
    private GameObject rightSide = null;

    protected ScriptableObject[] items;

    [SerializeField]
    private float startItemPosX = 0.0f;
    [SerializeField]
    private float startItemPosY = 0.0f;
    [SerializeField]
    private float stepItemPosX = 0.0f;

    private Vector2 scrollPosition;
    private Vector2 firstScrollPosition;
    private Vector2 firstItemFirstPos;
    private Vector2 firstItemSecondPos;
    private Vector2 secondScrollPosition;
    private float container_posX;
    private GameObject containerList;
    private float scrollTime = 1.3f;
    private float scrollSpeed;
    private GameObject[] itemsGO;
    private bool canTouchItem = false;

    private IClickHorizontalListViewItem listener;
    public void InitIClickHorizontalListView(IClickHorizontalListViewItem listener_)
    {
        listener = listener_;
    }

    private ISwipeListenerHorizontalListView swipeListener;
    public void InitSwipeListener(ISwipeListenerHorizontalListView listener)
    {
        swipeListener = listener;
    }

    private void Start()
    {
        scroll = 0.0f;
        lastScroll = 0.0f;
    }



    public override IEnumerator TouchFirst(GameObject containerList)
    {
        canTouchItem = false;
        this.containerList = containerList;
        firstScrollPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        container_posX = containerList.transform.localPosition.x;
        scrollPosition = Vector2.zero;
        scrollTime = 0.1f;
        scrollSpeed = 2.1f;
        if (containerList.tag == ContainerTag)
            FindObjectOfType<PlayPictureZoomController>().CanSwipe = false;

        yield return null;
    }

    public override IEnumerator TouchSecond()
    {

        secondScrollPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        scrollPosition = secondScrollPosition - firstScrollPosition;
        scrollPosition.y = 0.0f;
        scrollTime = 1.3f;
        FindObjectOfType<PlayPictureZoomController>().CanSwipe = true;
        if (Vector2.Distance(secondScrollPosition, firstScrollPosition) > 0.0f)
        {
            canTouchItem = false;
        }
        else
        {
            canTouchItem = true;
        }
        yield return null;
    }

    public override void SelectItem(GameObject selectedUI)
    {
        if (selectedUI != null && selectedUI.tag == Utils.TAGS.SCROLL_LIST_COL)
        {
            listener.OnClickItem(selectedUI.gameObject);
        }
        if (selectedUI != null)
        {
            selectedUI = null;
        }
    }



    public override IEnumerator TouchDragList(GameObject containerList)
    {

        if (containerList != null && containerList.tag == ContainerTag)
        {
            canTouchItem = false;
            container_posX += Input.GetAxis("Mouse X") * Time.fixedDeltaTime / drugSpeed;
            scroll = container_posX;
            containerList.transform.localPosition = new Vector3(scroll, 0.0f, -0.1f);
            LeftLimitSide();
            RightLimitSide();
        }
        yield return null;
    }

    public override IEnumerator InercionList()
    {
        if (containerList != null && containerList.tag == ContainerTag)
        {
            while (scrollTime > 0.0f && scrollSpeed > 0.0f)
            {
                if (Vector2.Distance(secondScrollPosition, firstScrollPosition) > 0.2f)
                {
                    canTouchItem = false;
                    firstItemFirstPos = itemsGO[0].transform.localPosition;
                    scroll += scrollPosition.x * Time.deltaTime * scrollSpeed;
                    containerList.transform.localPosition = new Vector3(scroll, 0.0f, -0.1f);
                    scrollTime -= Time.deltaTime * 2.1f;
                    scrollSpeed -= Time.deltaTime * 2.1f;
                    firstItemSecondPos = itemsGO[0].transform.localPosition;
                    LeftLimitSide();
                    RightLimitSide();
                    if (scrollTime < 0.1f)
                        swipeListener.UpdateSwipeList(this, scrollTime, scrollSpeed);

                }
                yield return null;
            }
            /* else
             {
                 canTouchItem = true;
             }*/
        }
    }




    private void LeftLimitSide()
    {
        if (containerList != null)
        {
            var distance = itemsGO[0].transform.position.x - leftSide.transform.position.x;
            if (distance >= 0.2f && distance < 0.3f)
            {
                lastScroll = scroll;
            }
            if (distance > 0.3f)
            {
                scroll = Mathf.Clamp(scroll, lastScroll - 0.02f, lastScroll);
                containerList.transform.localPosition = new Vector3(scroll, 0.0f, -0.1f);
            }
        }
    }

    private void RightLimitSide()
    {
        if (containerList != null)
        {
            var distance = itemsGO[itemsGO.Length - 1].transform.position.x - rightSide.transform.position.x;
            if (distance <= -0.2f && distance > -0.3f)
            {
                lastScroll = scroll;
            }
            if (distance < -0.3f)
            {
                scroll = Mathf.Clamp(scroll, lastScroll - 0.02f, lastScroll);
                containerList.transform.localPosition = new Vector3(scroll, 0.0f, -0.1f);
            }
        }
    }


    protected void drawListItem1xN(int N)
    {
        itemsGO = new GameObject[N];
        for (int i = 0; i < N; i++)
        {
            var item = Instantiate(itemsPref);
            item.transform.SetParent(ItemsContainerPref.transform);
            item.transform.localPosition = new Vector3(startItemPosX + i * stepItemPosX, startItemPosY, -0.5f);
            itemsGO[i] = item;
            initItem(item, i);
        }
    }

    public void ClearList()
    {

        for (int i = 0; i < items.Length; i++)
        {
            //items[i] = null;
            Destroy(ItemsContainerPref.transform.GetChild(i).gameObject);
        }
    }

    public override IEnumerator UpdateColorNearSides()
    {
        while (scrollTime > 0.0f && scrollSpeed > 0.0f)
        {
            if (itemsGO.Length != 0)
            {
                for (int i = 0; i < itemsGO.Length; i++)
                {
                    var distanceL = Vector2.Distance(itemsGO[i].transform.position, leftSide.transform.position);
                    var distanceR = Vector2.Distance(itemsGO[i].transform.position, rightSide.transform.position);
                    SpriteRenderer view = null;
                    if (itemsGO[i].GetComponent<SpriteRenderer>() != null)
                        view = itemsGO[i].GetComponent<SpriteRenderer>();
                    SpriteRenderer mask = null;
                    if (itemsGO[i].transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
                        mask = itemsGO[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
                    if (distanceL < 0.21f)
                    {
                        if (view != null)
                            view.color = new Color(view.color.r, view.color.g, view.color.b, distanceL);
                        if (mask != null)
                            mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, distanceL);

                    }
                    else if (distanceR < 0.21f)
                    {
                        if (view != null)
                            view.color = new Color(view.color.r, view.color.g, view.color.b, distanceR);
                        if (mask != null)
                            mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, distanceR);

                    }
                    else
                    {
                        if (view != null)
                            view.color = new Color(view.color.r, view.color.g, view.color.b, 1);
                        if (mask != null)
                            mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 1);
                    }

                }
            }
            yield return null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == Utils.TAGS.SCROLL_LIST_COL)
        {


            //disable item's text
            if (collision.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>() != null)
                collision.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == Utils.TAGS.SCROLL_LIST_COL)
        {

            //enable item's text
            if (collision.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>() != null)
                collision.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        }
    }



    public void SetVisiblity(VisibleStates state)
    {
        if (visible == VisibleStates.Visible && state == VisibleStates.Gone)
        {
            ItemsContainerPref.transform.parent.gameObject.SetActive(false);
            visible = VisibleStates.Gone;
        }
        else if (visible == VisibleStates.Gone && state == VisibleStates.Visible)
        {
            ItemsContainerPref.transform.parent.gameObject.SetActive(true);
            visible = VisibleStates.Visible;
        }
        else if (visible == VisibleStates.Visible && state == VisibleStates.Visible)
        {
            ItemsContainerPref.transform.parent.gameObject.SetActive(false);
            visible = VisibleStates.Gone;
        }
    }

    protected abstract void initItem(GameObject item, int index);
}
