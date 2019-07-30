using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PicturesVerticalListView : ListView
{
    [SerializeField] private GameObject RowObj = null;

    private List<UIPictureController> items = new List<UIPictureController>();

    [SerializeField] private GameObject upSidePref = null;
    [SerializeField] private GameObject bottonSidePref = null;

    private IClickVListChPictureActivityItem clickChPListener;
    private IClickVListView clickListItems;

    [SerializeField] private float startRowPosX = -0.29f;
    [SerializeField] private float startRowPosY = 0.37f;
    [SerializeField] private float stepRowPosY = 0.7f;
    [SerializeField] private float stepColPosX = 0.57f;

    [SerializeField] private float startRowPos1xN_Y = -0.5f;
    [SerializeField] private float stepRowPos1xN_Y = 0.3f;

    private int columsCount = 2;
    private Vector2 scrollPosition;
    private Vector2 firstScrollPosition;
    private Vector2 secondScrollPosition;
    private float container_posY;
    private GameObject containerList;

    private float scrollTime = 1.3f;
    private float scrollSpeed;
    private bool canTouchItem = false;

    public void InitIClickVerticalListViewItem(IClickVListView listener)
    {
        clickChPListener = listener as IClickVListChPictureActivityItem;
    }

    public void InitIClickGalleryListViewItem(IClickVListView listener)
    {
        clickListItems = listener;
    }

    public void AddContentToItemsByScrObj(UIPictureData[] contents)
    {
        for (int i = 0; i < contents.Length; i++)
        {
            items[i].InitData(contents[i]);
            items[i].GetComponent<ListItemButton>().InitList(this as PicturesVerticalListView);
            items[i].transform.GetChild(0).GetComponent<ListItemButton>().InitList(this as PicturesVerticalListView);
            items[i].transform.GetChild(1).GetComponent<ListItemButton>().InitList(this as PicturesVerticalListView);
        }
    }

    public void AddContentToItemsByPictProxy(PictureTable.PictureProxy[] picturesProxy)
    {
        for (int i = 0; i < picturesProxy.Length; i++)
        {
            items[i].InitData(picturesProxy[i].id);
            items[i].GetComponent<ListItemButton>().InitList(this as PicturesVerticalListView);
        }
    }

    public List<UIPictureController> GetAllContent()
    {
        return items;
    }

    public void ClearList()
    {
        for (int i = 0; i < items.Count; i++)
        {
            var parent = items[i].transform.parent.gameObject;
            Destroy(items[i].gameObject);
            if (parent != null)
                Destroy(parent);
        }
        items.Clear();
    }

    /*
    * OnClick item's UI elements machine
    */

    public override IEnumerator TouchFirst(GameObject containerList)
    {
        canTouchItem = false;
        this.containerList = containerList;
        firstScrollPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        container_posY = containerList.transform.localPosition.y;
        scrollPosition = Vector2.zero;
        scrollTime = 0.1f;
        scrollSpeed = 4.1f;
        yield return null;
    }

    public override IEnumerator TouchSecond()
    {
        secondScrollPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        scrollPosition = secondScrollPosition - firstScrollPosition;
        scrollPosition.x = 0.0f;
        scrollTime = 1.3f;
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


        if (selectedUI != null && selectedUI.tag == Utils.TAGS.PICTURE)
        {
            if (clickChPListener != null)
                clickChPListener.OnClickPicture(selectedUI.GetComponent<UIPictureController>());
            if (clickListItems != null)
                clickListItems.OnClickPicture(selectedUI.GetComponent<UIPictureController>());

        }
        else
            if (selectedUI != null && selectedUI.tag == Utils.TAGS.BUY_BUTTON)
        {
            if (clickChPListener != null)
                clickChPListener.OnClickBuyPicture(selectedUI.GetComponentInParent<UIPictureController>());

        }
        else
          if (selectedUI != null && selectedUI.tag == Utils.TAGS.RESET_BUTTON)
        {
            if (clickChPListener != null)
                clickChPListener.OnClickResetPicture(selectedUI.GetComponentInParent<UIPictureController>());

        }
        if (selectedUI != null)
        {
            selectedUI = null;
            //  containerList = null;
        }

    }

    public override IEnumerator TouchDragList(GameObject containerList)
    {

        if (containerList != null && containerList.tag == Utils.TAGS.SCROLL_LIST_CONTAINER)
        {
            canTouchItem = false;
           
            container_posY += Input.GetAxis("Mouse Y") * Time.fixedDeltaTime / drugSpeed;
            scroll = container_posY;
            containerList.transform.localPosition = new Vector3(0.0f, scroll, -0.1f);
            UpLimitSide();
            BottonLimitSide();

        }
        yield return null;
    }
    public override IEnumerator InercionList()
    {
        if (containerList != null && containerList.tag == Utils.TAGS.SCROLL_LIST_CONTAINER)
        {
            while (scrollTime > 0.0f && scrollSpeed > 0.0f)
            {
                scroll += scrollPosition.y * Time.fixedDeltaTime * scrollSpeed;
                containerList.transform.localPosition = new Vector3(0.0f, scroll, -0.1f);
                scrollTime -= Time.deltaTime * 2.1f;
                scrollSpeed -= Time.deltaTime * 4.1f;
                UpLimitSide();
                BottonLimitSide();
                yield return null;
            }
        }
    }

    private Vector3 ScreenToWorldPoint(Vector3 point)
    {
        return Camera.main.ScreenToWorldPoint(point);
    }

    /*
     * Creating list View matrix 2 x N
     */
    public void drawItems2xN(int N)
    {
        columsCount = 2;
        for (int i = 0, k = 0; i < N; i++)
        {
            if (k < N)
            {
                var row = Instantiate(RowObj);
                var rowCol = row.GetComponent<BoxCollider2D>();
                row.transform.SetParent(ItemsContainerPref.transform);
                row.transform.localPosition = new Vector3(startRowPosX,
                                                           startRowPosY + i * -1 * stepRowPosY,
                                                           -1.0f);

                for (int j = 0; j < columsCount; j++)
                {
                    if (k < N)
                    {
                        var item = Instantiate(itemsPref);
                        item.transform.SetParent(row.transform);
                        item.transform.localPosition = new Vector3(j * stepColPosX,
                                                                   0,
                                                                   -1.0f);

                        items.Add(item.GetComponent<UIPictureController>());
                        k++;
                    }
                }
            }
        }

    }

    public void drawItems1xN(int N)
    {
        columsCount = 1;
        startRowPosX = 0.04f;
        for (int i = 0, k = 0; i < N; i++)
        {
            if (k < N)
            {
                var row = Instantiate(RowObj);
                var rowCol = row.GetComponent<BoxCollider2D>();
                row.transform.SetParent(ItemsContainerPref.transform);
                row.transform.localPosition = new Vector3(startRowPosX, startRowPos1xN_Y + i * -1 * stepRowPos1xN_Y, -1.0f);
                for (int j = 0; j < columsCount; j++)
                {
                    if (k < N)
                    {
                        var item = Instantiate(itemsPref);
                        item.transform.SetParent(row.transform);
                        item.transform.localPosition = new Vector3(j * stepColPosX, 0, -1.0f);
                        items.Add(item.GetComponent<UIPictureController>());
                        k++;
                    }
                }
            }
        }

    }


    private void UpLimitSide()
    {
        if (containerList != null)
        {
            var distanceFirstItemToTopSide = items[0].transform.position.y - upSidePref.transform.position.y;
            if (distanceFirstItemToTopSide <= -0.7f && distanceFirstItemToTopSide > -0.8f)
            {
                lastScroll = scroll;
            }
            if (distanceFirstItemToTopSide < -0.8f)
            {
                scroll = Mathf.Clamp(scroll, lastScroll - 0.02f, lastScroll);
                containerList.transform.localPosition = new Vector3(0.0f, scroll, -0.1f);
            }
        }
    }

    private void BottonLimitSide()
    {
        if (containerList != null)
        {
            var distanceLastItemToBottomSide = items[items.Count - 1].transform.position.y - bottonSidePref.transform.position.y;
            if (distanceLastItemToBottomSide >= 0.7f && distanceLastItemToBottomSide < 0.8f)
            {
                lastScroll = scroll;
            }
            if (distanceLastItemToBottomSide > 0.8f)
            {
                scroll = Mathf.Clamp(scroll, lastScroll - 0.02f, lastScroll);
                containerList.transform.localPosition = new Vector3(0.0f, scroll, -0.1f);
            }
        }
    }

    public override IEnumerator UpdateColorNearSides()
    {
        while (scrollTime > 0.0f && scrollSpeed > 0.0f)
        {
            if (items.Count != 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var distanceUp = Vector2.Distance(items[i].transform.position, upSidePref.transform.position);
                    var distanceBotton = Vector2.Distance(items[i].transform.position, bottonSidePref.transform.position);
                    SpriteRenderer view = null;
                    if (items[i].GetComponent<SpriteRenderer>() != null)
                        view = items[i].GetComponent<SpriteRenderer>();
                    SpriteRenderer buyButtonView = null;
                    if (items[i].transform.childCount != 0 && items[i].transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
                        buyButtonView = items[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
                    SpriteRenderer coinOnBuyButton = null;
                    if (buyButtonView != null && buyButtonView.transform.childCount != 0)
                        if (buyButtonView.transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
                            coinOnBuyButton = buyButtonView.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    SpriteRenderer resetButtonView = null;
                    if (items[i].transform.childCount > 1)
                        if (items[i].transform.GetChild(1).GetComponent<SpriteRenderer>() != null)
                            resetButtonView = items[i].transform.GetChild(1).GetComponent<SpriteRenderer>();
                    var hideUp = distanceUp - 0.4f;
                    var hideBotton = distanceBotton - 0.4f;
                    if (distanceUp < 0.6f)
                    {
                        if (view != null)
                            view.color = new Color(view.color.r, view.color.g, view.color.b, hideUp);
                        if (buyButtonView != null && coinOnBuyButton != null)
                        {
                            buyButtonView.color = new Color(buyButtonView.color.r, buyButtonView.color.g, buyButtonView.color.b, hideUp);
                            coinOnBuyButton.color = new Color(coinOnBuyButton.color.r, coinOnBuyButton.color.g, coinOnBuyButton.color.b, hideUp);
                        }
                        if (resetButtonView != null)
                            resetButtonView.color = new Color(resetButtonView.color.r, resetButtonView.color.g, resetButtonView.color.b, hideUp);

                    }
                    else if (distanceBotton < 0.6f)
                    {
                        if (view != null)
                            view.color = new Color(view.color.r, view.color.g, view.color.b, hideBotton);
                        if (buyButtonView != null && coinOnBuyButton != null)
                        {
                            buyButtonView.color = new Color(buyButtonView.color.r, buyButtonView.color.g, buyButtonView.color.b, hideBotton);
                            coinOnBuyButton.color = new Color(coinOnBuyButton.color.r, coinOnBuyButton.color.g, coinOnBuyButton.color.b, hideBotton);
                        }
                        if (resetButtonView != null)
                            resetButtonView.color = new Color(resetButtonView.color.r, resetButtonView.color.g, resetButtonView.color.b, hideBotton);
                    }
                    else
                    {
                        if (view != null)
                            view.color = new Color(view.color.r, view.color.g, view.color.b, 1);
                        if (buyButtonView != null && coinOnBuyButton != null)
                        {
                            buyButtonView.color = new Color(buyButtonView.color.r, buyButtonView.color.g, buyButtonView.color.b, 1);
                            coinOnBuyButton.color = new Color(coinOnBuyButton.color.r, coinOnBuyButton.color.g, coinOnBuyButton.color.b, 1);
                        }
                        if (resetButtonView != null)
                            resetButtonView.color = new Color(resetButtonView.color.r, resetButtonView.color.g, resetButtonView.color.b, 1);
                    }

                }
            }
            yield return null;
        }

    }

}
