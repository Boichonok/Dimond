using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActivityController : MonoBehaviour
{

    [SerializeField]
    protected Camera playPictureCamera = null;
 
    public void DiactivateSwipeCamera()
    {
        playPictureCamera.GetComponent<PlayPictureZoomController>().CanSwipe = false;
    }

    public virtual void OnClick(GameObject selectedUI)
    {
        playPictureCamera.GetComponent<PlayPictureZoomController>().CanSwipe = true;
    }

}
