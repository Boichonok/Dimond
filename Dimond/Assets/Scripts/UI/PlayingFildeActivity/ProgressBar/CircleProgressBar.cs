using UnityEngine;
using System.Collections;

public class CircleProgressBar : MonoBehaviour
{
    public interface ICircleUpdateProgress
    {
        void UpdateProgress(int progressValue);
    }
    private float startPos = -0.14f;
    private float endPos = 0.0f;
    [SerializeField]
    private int progressValue = 0;
    private int maxProgress = 100;
    private TextMesh textProgressValue = null;
    private SpriteRenderer viewProgressValue = null;
    private ICircleUpdateProgress onCircleUpdateProgressListener;

    public void SetOnCircleUpddateProgressListener(ICircleUpdateProgress onCircleUpdateProgress)
    {
        this.onCircleUpdateProgressListener = onCircleUpdateProgress;
    }

    // Use this for initialization
    void Start()
    {
        textProgressValue = gameObject.GetComponentInChildren<TextMesh>();
        viewProgressValue = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateValue();
        onCircleUpdateProgressListener.UpdateProgress(progressValue);
    }

    private void UpdateValue()
    {
        if (progressValue <= maxProgress)
        {
            var value = ((progressValue * (-startPos * maxProgress))/maxProgress)/ maxProgress + startPos;
            viewProgressValue.transform.localPosition = new Vector3(- 0.002f, value, 0.4f);

        }
        else
        {
            progressValue = maxProgress;
        }
    }

    public void SetProgressValue(int progress)
    {
        if (this.textProgressValue != null)
        {
            this.progressValue = progress;
            this.textProgressValue.text = progress.ToString() + " %";
        }
    }
}
