using UnityEngine;
using System.Collections;

public class AdvicesController : BaseActivityController
{
    public enum ADVICES
    {
        NONE = -1,
        SWIPE_LIST_CHOOSE_SECTION = 0,
        CHOOSE_LIST_ITEM_CHOOSE_SECTION = 1,
        OPEN_BRUSHS_LIST = 2,
        SWIPE_LIST_CHOOSE_BRUSHS = 3,
        CHOOSE_LIST_ITEM_BRUSHS = 4,
        COLORING_ONE_POINT = 5,
        COLORIMG_MORE_POINT = 6,
        ZOOMING_PICTURE_PLUS = 7,
        ZOOMING_PICTURE_MINUS = 8,
        SWIPE_PICTURE_LEFT = 9,
        SWIPE_PICTURE_RIGHT = 10,
        SWIPE_PICTURE_UP = 11,
        SWIPE_PICTURE_DOWN = 12,
        FINISHED = 13
    }
    [SerializeField] private GameObject first_advice_arm = null;
    [SerializeField] private GameObject second_advice_arm = null;

    [SerializeField] private DataBase dataBase;

    [SerializeField] private AnimationCurve SWIPE_LIST_CHOOSE_SECTION_anim_x;
    [SerializeField] private AnimationCurve SWIPE_LIST_CHOOSE_SECTION_anim_y;

    [SerializeField] private AnimationCurve CHOOSE_LIST_ITEM_CHOOSE_SECTION_anim_x;
    [SerializeField] private AnimationCurve CHOOSE_LIST_ITEM_CHOOSE_SECTION_anim_y;
    [SerializeField] private AnimationCurve CHOOSE_LIST_ITEM_anim_size;

    [SerializeField] private AnimationCurve OPEN_BRUSHS_LIST_anim_x;
    [SerializeField] private AnimationCurve OPEN_BRUSHS_LIST_anim_y;
    [SerializeField] private AnimationCurve SWIPE_LIST_CHOOSE_BRUSHS_anim_x;
    [SerializeField] private AnimationCurve SWIPE_LIST_CHOOSE_BRUSHS_anim_y;
    [SerializeField] private AnimationCurve CHOOSE_LIST_ITEM_BRUSHS_anim_x;
    [SerializeField] private AnimationCurve CHOOSE_LIST_ITEM_BRUSHS_anim_y;

    [SerializeField] private AnimationCurve COLORING_ONE_POINT_anim_x;
    [SerializeField] private AnimationCurve COLORING_ONE_POINT_anim_y;

    [SerializeField] private AnimationCurve COLORING_MORE_POINT_anim_x;
    [SerializeField] private AnimationCurve COLORING_MORE_POINT_anim_y;

    [SerializeField] private AnimationCurve ZOOMING_PICTURE_PLUS_finger1_anim_x;
    [SerializeField] private AnimationCurve ZOOMING_PICTURE_PLUS_finger1_anim_y;
    [SerializeField] private AnimationCurve ZOOMING_PICTURE_PLUS_finger2_anim_x;
    [SerializeField] private AnimationCurve ZOOMING_PICTURE_PLUS_finger2_anim_y;

    [SerializeField] private AnimationCurve ZOOMING_PICTURe_MINUS_finger1_anim_x;
    [SerializeField] private AnimationCurve ZOOMING_PICTURe_MINUS_finger1_anim_y;
    [SerializeField] private AnimationCurve ZOOMING_PICTURE_MINUS_finger2_anim_x;
    [SerializeField] private AnimationCurve ZOOMING_PICTURE_MINUS_finger2_anim_y;

    [SerializeField] private AnimationCurve SWIPE_PICTURE_LEFT_finger1_anim_x;
    [SerializeField] private AnimationCurve SWIPE_PICTURE_LEFT_finger1_anim_y;

    [SerializeField] private AnimationCurve SWIPE_PICTURE_RIGHT_finger1_anim_x;
    [SerializeField] private AnimationCurve SWIPE_PICTURE_RIGHT_finger1_anim_y;

    [SerializeField] private AnimationCurve SWIPE_PICTURE_UP_finger1_anim_x;
    [SerializeField] private AnimationCurve SWIPE_PICTURE_UP_finger1_anim_y;

    [SerializeField] private AnimationCurve SWIPE_PICTURE_DOWN_finger1_anim_x;
    [SerializeField] private AnimationCurve SWIPE_PICTURE_DOWN_finger1_anim_y;


    private bool isNextAdvice = false;

    private bool isAdviceBusy = false;

    private ADVICES lastAdvice = ADVICES.NONE;

    public void NextAdvice(ADVICES advice, bool isAdviceBusy_)
    {
        first_advice_arm.SetActive(false);
        second_advice_arm.SetActive(false);
        if (dataBase.GetSettings().isAdvicersActive)
        {
            isAdviceBusy = isAdviceBusy_;

            if (!isAdviceBusy && (int)advice - (int)lastAdvice == 1)
            {
                ChooseAdvice(advice);
                lastAdvice = advice;
            }
            else
            {
                if (lastAdvice == ADVICES.NONE)
                    lastAdvice = ADVICES.SWIPE_LIST_CHOOSE_SECTION;
                ChooseAdvice(lastAdvice);

            }
        }
    }
    private void ChooseAdvice(ADVICES advice)
    {
        StopAllCoroutines();
        switch (advice)
        {
            case ADVICES.SWIPE_LIST_CHOOSE_SECTION:
                {
                    isAdviceBusy = true;
                    StartCoroutine("SWIPE_LIST_CHOOSE_SECTION");
                }
                break;
            case ADVICES.CHOOSE_LIST_ITEM_CHOOSE_SECTION:
                {
                    isAdviceBusy = true;
                    StartCoroutine("CHOOSE_LIST_ITEM_CHOOSE_SECTION");
                }
                break;
            case ADVICES.OPEN_BRUSHS_LIST:
                {
                    isAdviceBusy = true;
                    StartCoroutine("OPEN_BRUSHS_LIST");
                }
                break;
            case ADVICES.SWIPE_LIST_CHOOSE_BRUSHS:
                {
                    isAdviceBusy = true;
                    StartCoroutine("SWIPE_LIST_CHOOSE_BRUSHS");
                }
                break;
            case ADVICES.CHOOSE_LIST_ITEM_BRUSHS:
                {
                    isAdviceBusy = true;
                    StartCoroutine("CHOOSE_LIST_ITEM_BRUSHS");
                }
                break;
            case ADVICES.COLORING_ONE_POINT:
                {
                    isAdviceBusy = true;
                    StartCoroutine("COLORING_ONE_POINT");
                }
                break;
            case ADVICES.COLORIMG_MORE_POINT:
                {
                    isAdviceBusy = true;
                    StartCoroutine("COLORIMG_MORE_POINT");
                }
                break;
            case ADVICES.ZOOMING_PICTURE_PLUS:
                {
                    isAdviceBusy = true;
                    StartCoroutine("ZOOMING_PICTURE_PLUS");
                }
                break;
            case ADVICES.ZOOMING_PICTURE_MINUS:
                {
                    isAdviceBusy = true;
                    StartCoroutine("ZOOMING_PICTURE_MINUS");
                }
                break;
            case ADVICES.SWIPE_PICTURE_LEFT:
                {
                    isAdviceBusy = true;
                    StartCoroutine("SWIPE_PICTURE_LEFT");
                }
                break;
            case ADVICES.SWIPE_PICTURE_RIGHT:
                {
                    isAdviceBusy = true;
                    StartCoroutine("SWIPE_PICTURE_RIGHT");
                }
                break;
            case ADVICES.SWIPE_PICTURE_UP:
                {
                    isAdviceBusy = true;
                    StartCoroutine("SWIPE_PICTURE_UP");
                }
                break;
            case ADVICES.SWIPE_PICTURE_DOWN:
                {
                    isAdviceBusy = true;
                    StartCoroutine("SWIPE_PICTURE_DOWN");
                }
                break;
            case ADVICES.FINISHED:
                {
                    isAdviceBusy = true;
                    StartCoroutine("IEFinishedAdvices", dataBase);
                }
                break;
        }
    }

    private IEnumerator SWIPE_LIST_CHOOSE_SECTION()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(SWIPE_LIST_CHOOSE_SECTION_anim_x.Evaluate(step),
                                                                       SWIPE_LIST_CHOOSE_SECTION_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }

            yield return null;
        }
    }

    private IEnumerator CHOOSE_LIST_ITEM_CHOOSE_SECTION()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(CHOOSE_LIST_ITEM_CHOOSE_SECTION_anim_x.Evaluate(step),
                                                                       CHOOSE_LIST_ITEM_CHOOSE_SECTION_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);

            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator OPEN_BRUSHS_LIST()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(OPEN_BRUSHS_LIST_anim_x.Evaluate(step),
                                                                       OPEN_BRUSHS_LIST_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator SWIPE_LIST_CHOOSE_BRUSHS()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(SWIPE_LIST_CHOOSE_BRUSHS_anim_x.Evaluate(step),
                                                                       SWIPE_LIST_CHOOSE_BRUSHS_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator CHOOSE_LIST_ITEM_BRUSHS()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(CHOOSE_LIST_ITEM_BRUSHS_anim_x.Evaluate(step),
                                                                       CHOOSE_LIST_ITEM_BRUSHS_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator COLORING_ONE_POINT()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(COLORING_ONE_POINT_anim_x.Evaluate(step),
                                                                       COLORING_ONE_POINT_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator COLORIMG_MORE_POINT()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(COLORING_MORE_POINT_anim_x.Evaluate(step),
                                                                       COLORING_MORE_POINT_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator ZOOMING_PICTURE_PLUS()
    {
        first_advice_arm.SetActive(true);
        second_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(ZOOMING_PICTURE_PLUS_finger1_anim_x.Evaluate(step),
                                                                       ZOOMING_PICTURE_PLUS_finger1_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
                second_advice_arm.transform.localPosition = new Vector3(ZOOMING_PICTURE_PLUS_finger2_anim_x.Evaluate(step),
                                                                     ZOOMING_PICTURE_PLUS_finger2_anim_y.Evaluate(step),
                                                                     first_advice_arm.transform.localPosition.z);
                second_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator ZOOMING_PICTURE_MINUS()
    {
        first_advice_arm.SetActive(true);
        second_advice_arm.SetActive(true);
        float step = 1;
        while (isAdviceBusy)
        {
            if (step > 0)
            {
                step -= 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(ZOOMING_PICTURe_MINUS_finger1_anim_x.Evaluate(step),
                                                                       ZOOMING_PICTURe_MINUS_finger1_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
                second_advice_arm.transform.localPosition = new Vector3(ZOOMING_PICTURE_MINUS_finger2_anim_x.Evaluate(step),
                                                                        ZOOMING_PICTURE_MINUS_finger2_anim_y.Evaluate(step),
                                                                     first_advice_arm.transform.localPosition.z);
                second_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 1;
            }
            yield return null;
        }
    }

    private IEnumerator SWIPE_PICTURE_LEFT()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(SWIPE_PICTURE_LEFT_finger1_anim_x.Evaluate(step),
                                                                       SWIPE_PICTURE_LEFT_finger1_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator SWIPE_PICTURE_RIGHT()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(SWIPE_PICTURE_RIGHT_finger1_anim_x.Evaluate(step),
                                                                       SWIPE_PICTURE_RIGHT_finger1_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator SWIPE_PICTURE_UP()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(SWIPE_PICTURE_UP_finger1_anim_x.Evaluate(step),
                                                                       SWIPE_PICTURE_UP_finger1_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator SWIPE_PICTURE_DOWN()
    {
        first_advice_arm.SetActive(true);
        float step = 0;
        while (isAdviceBusy)
        {
            if (step < 1)
            {
                step += 0.7f * Time.deltaTime;
                first_advice_arm.transform.localPosition = new Vector3(SWIPE_PICTURE_DOWN_finger1_anim_x.Evaluate(step),
                                                                       SWIPE_PICTURE_DOWN_finger1_anim_y.Evaluate(step),
                                                                       first_advice_arm.transform.localPosition.z);
                first_advice_arm.transform.localScale = new Vector3(CHOOSE_LIST_ITEM_anim_size.Evaluate(step),
                                                                    CHOOSE_LIST_ITEM_anim_size.Evaluate(step), 1);
            }
            else
            {
                step = 0;
            }
            yield return null;
        }
    }

    private IEnumerator IEFinishedAdvices(DataBase dataBase)
    {
        dataBase.GetSettings().isAdvicersActive = false;
        dataBase.SaveSettings();
        yield return null;
    }
}
