using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuButtonListController : BaseActivityController
{
    private bool isListOpen = false;
    [SerializeField]
    private ScreenUIController screenUIController;
    [Space]
    [SerializeField] private AnimationCurve slide;

    [SerializeField] private Transform galleryButton;
    [SerializeField] private Transform yovoButton;
    [SerializeField] private Transform ratingButton;
    [SerializeField] private Transform noadsButton;

    [SerializeField] private float stepButtons;

    public void OnClickMenuButton()
    {
        if (isListOpen)
        {
            StartCoroutine("DissmiseList"); ;
        }
        else
        {
            StartCoroutine("ShowList");
        }
    }

    private IEnumerator ShowList()
    {

        float step_anim = 0;
        while(step_anim < 1)
        {
            step_anim += 2.5f * Time.deltaTime;
            yovoButton.gameObject.SetActive(true);
            yovoButton.localPosition = new Vector3(yovoButton.localPosition.x, slide.Evaluate(step_anim) + transform.localPosition.y, yovoButton.localPosition.z);
            ratingButton.gameObject.SetActive(true);
            ratingButton.localPosition = new Vector3(ratingButton.localPosition.x, slide.Evaluate(step_anim) + yovoButton.localPosition.y, ratingButton.localPosition.z);
            galleryButton.gameObject.SetActive(true);
            galleryButton.localPosition = new Vector3(galleryButton.localPosition.x, slide.Evaluate(step_anim) + ratingButton.localPosition.y, galleryButton.localPosition.z);
            noadsButton.gameObject.SetActive(true);
            noadsButton.localPosition = new Vector3(noadsButton.localPosition.x, slide.Evaluate(step_anim) + galleryButton.localPosition.y, noadsButton.localPosition.z);
            yield return null;
        }
        isListOpen = true;
    }

    private IEnumerator DissmiseList()
    {
        float step_anim = 1;
        while (step_anim > 0)
        {
            step_anim -= 2.5f * Time.deltaTime;
            yovoButton.localPosition = new Vector3(yovoButton.localPosition.x, slide.Evaluate(step_anim) + transform.localPosition.y, yovoButton.localPosition.z);
            ratingButton.localPosition = new Vector3(ratingButton.localPosition.x, slide.Evaluate(step_anim) + yovoButton.localPosition.y, ratingButton.localPosition.z);
            galleryButton.localPosition = new Vector3(galleryButton.localPosition.x, slide.Evaluate(step_anim) + ratingButton.localPosition.y, galleryButton.localPosition.z);
            noadsButton.localPosition = new Vector3(noadsButton.localPosition.x, slide.Evaluate(step_anim) + galleryButton.localPosition.y, noadsButton.localPosition.z);
            yield return null;
        }
        yovoButton.gameObject.SetActive(false);
        ratingButton.gameObject.SetActive(false);
        galleryButton.gameObject.SetActive(false);
        noadsButton.gameObject.SetActive(false);

        isListOpen = false;
    }

    public override void OnClick(GameObject selectedUI)
    {
        base.OnClick(selectedUI);
        if (selectedUI != null)
        {
            switch (selectedUI.tag)
            {
                case "gallery_button":
                    {
                        screenUIController.ShowGalleryActivity().OnStart();
                        screenUIController.ResetLastActivity();
                    }
                    break;
                case "yovo_button":
                    {

                    }
                    break;
                case "rating_button": 
                    {

                    }
                    break;
                case "noads_button":
                    {

                    }
                    break;                
            }

        }

    }

}
