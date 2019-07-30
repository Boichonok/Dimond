using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Start : C_Level {

    public static C_Start mc_this;

    public override void f_Init()
    {
        if (mc_this == null)
        {
            mc_this = (C_Start)this;
        }
        base.f_Init();
    }

    public override C_Level f_Activ()
    {
        StartCoroutine("IEActiv");
        return this;
    }

    public override IEnumerator IEActiv()
    {
        mg_bg.SetActive(true);
        print("Work");
        C_UI.mc_this.f_ActivButton(0);
        yield return new WaitForSeconds(0.5f);
        C_UI.mc_this.f_ActivButton(3);
        yield return new WaitForSeconds(0.5f);
        C_UI.mc_this.m_clik = true;

    }

    public override void f_Diactiv()
    {
        base.f_Diactiv();
    }

    public override IEnumerator IEDiactiv()
    {
        C_UI.mc_this.f_DiactivButton(0);
        C_UI.mc_this.f_DiactivButton(3);
        yield return new WaitForSeconds(1);
    }
}
