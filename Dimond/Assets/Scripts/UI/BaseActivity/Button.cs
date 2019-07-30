using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    [SerializeField] private BaseActivityController activityController;
    [SerializeField] private AnimationCurve step_anim;

    private bool used;

    private void OnMouseDown()
    {
        used = true;
    }

    private void OnMouseExit()
    {
        if (used)
            activityController.DiactivateSwipeCamera();
        used = false;
    }

    private void OnMouseUp()
    {
        if (used)
        {
            StartCoroutine("ScaleButton");
            activityController.OnClick(gameObject);
        }
        used = false;
    }

    private IEnumerator ScaleButton()
    {
        float _step = 0;
        while (_step < 1)
        {
            _step += 2 * Time.deltaTime;
            transform.localScale = new Vector3(step_anim.Evaluate(_step), step_anim.Evaluate(_step), 1);
            yield return null;
        }
    }
}
