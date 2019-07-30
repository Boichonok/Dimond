using UnityEngine;
using System.Collections;

public enum PictureType
{
    FreePicture,
    AdsPicture,
    BuyByCoinPicture,
    LockedPicture
}

public enum PictureMaterial
{
    Stounes,
    Strands,
    Colors,
    NONE
}

public class UIPictureController : MonoBehaviour
{
    [SerializeField]
    private UIPictureData data;
    public UIPictureData Data { get { return data; } }

    public int pictureID { get { return data.PitureId; } private set { if (data != null) pictureID = data.PitureId; else pictureID = value; } }

    private PictureType currentPictureType;

    public string PictureName { get { return data.name; } }

    public bool isBuying { get; set; }

    public bool isFirstOpen { get; set; }

    private int prise;

    private SpriteRenderer pictureView;

    [SerializeField]
    private GameObject buyButton = null;

    [SerializeField]
    private Sprite[] iconsBuyButton = null;//0 - icons for buy by coin, 1 - for ads picture


    private int maxWhatchAds = 2;

    private int whatchAdsCounter = 0;


    // Use this for initialization
    void Start()
    {
        isBuying = false;
        isFirstOpen = true;
        currentPictureType = data.PictureType;
        prise = data.Prise;
        SwitchPictureType();
    }

  


    public void UpdatePictureState()
    {

        if (isBuying && currentPictureType != PictureType.FreePicture)
        {
            currentPictureType = PictureType.FreePicture;
        }
        SwitchPictureType();
    }

    public void UpdatePictureView(byte[] pictureInBytes, TextureFormat format)
    {
        var sprite = Utils.MakeSpriteFromBytes(pictureInBytes, 329f, 400f, format, 640f);
        pictureView.sprite = sprite;
    }

    public void InitData(UIPictureData data_)
    {
        data = data_;
        gameObject.AddComponent<SpriteRenderer>().sprite = data.Image;
        pictureView = GetComponent<SpriteRenderer>();

    }

    public void InitData(int pictureId)
    {
        //var sprite = Utils.MakeSpriteFromBytes(pictureInBytes, 329f, 400f, format, 640f);
        data = new UIPictureData(pictureId, null, 0, PictureType.FreePicture);
        gameObject.AddComponent<SpriteRenderer>();
        pictureView = GetComponent<SpriteRenderer>();
    }

    public bool OnClickSelectPicture()
    {
        if (isBuying)
        {
            return true;
        }
        return false;
    }

    public void OnClickBuyPicture()
    {
        if (!isBuying)
        {
            switch (currentPictureType)
            {
                case PictureType.BuyByCoinPicture:
                    {
                        var coinsFilde = FindObjectOfType<Coins>();
                        coinsFilde.RemoveCoin(prise);
                        isBuying = true;
                    }
                    break;
                case PictureType.AdsPicture:
                    {
                        if (buyButton != null)
                        {
                            if (whatchAdsCounter < maxWhatchAds - 1)
                            {
                                whatchAdsCounter++;
                                var text = buyButton.GetComponentInChildren<TextMesh>();
                                text.text = whatchAdsCounter.ToString() + "/" + maxWhatchAds.ToString();
                            }
                            else
                            {
                                isBuying = true;
                            }
                        }
                    }
                    break;
                case PictureType.LockedPicture:
                    {
                    }
                    break;
            }
            UpdatePictureState();
        }
    }

    public void OnClickResetPicture(bool isReset,UIPictureData uIPictureData)
    {
        if(isReset)
        {
            prise = uIPictureData.Prise;
            isFirstOpen = true;
            isBuying = true;
            currentPictureType = PictureType.FreePicture;
            pictureView.sprite = uIPictureData.Image;
            UpdatePictureState();
        }
    }


    private void SwitchPictureType()
    {
        switch (currentPictureType)
        {
            case PictureType.FreePicture:
                {
                    if (buyButton != null)
                        buyButton.SetActive(false);
                    isBuying = true;
                }
                break;
            case PictureType.BuyByCoinPicture:
                {
                    if (buyButton != null)
                    {
                        buyButton.SetActive(true);
                        var text = buyButton.GetComponentInChildren<TextMesh>();
                        text.text = prise.ToString();
                        var icon = buyButton.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        icon.sprite = iconsBuyButton[0];
                    }
                }
                break;
            case PictureType.AdsPicture:
                {
                    if (buyButton != null)
                    {
                        buyButton.SetActive(true);
                        var text = buyButton.GetComponentInChildren<TextMesh>();
                        text.text = whatchAdsCounter.ToString() + "/" + maxWhatchAds.ToString();
                        var icon = buyButton.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        icon.sprite = iconsBuyButton[1];
                    }
                }
                break;
            case PictureType.LockedPicture:
                {
                    if (buyButton != null)
                    {
                        buyButton.SetActive(true);
                        var text = buyButton.GetComponentInChildren<TextMesh>();
                        text.text = "Locked";
                        text.transform.localPosition = new Vector2(-0.8f, 0.0f);
                        var icon = buyButton.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        icon.sprite = null;
                    }
                }
                break;
        }
    }

}
