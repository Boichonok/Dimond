using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class C_Level : MonoBehaviour {

    public GameObject mg_bg;

	public virtual void f_Init()
    {
        mg_bg.SetActive(false);
    }

    public virtual C_Level f_Activ()
    {
        return (C_Level)this;
    }

    public virtual IEnumerator IEActiv()
    {
        yield return null;
    }
    

    public virtual void f_Diactiv()
    {
        StartCoroutine("IEDiactiv");
    }

    public virtual IEnumerator IEDiactiv()
    {
        yield return null;
    }
}
