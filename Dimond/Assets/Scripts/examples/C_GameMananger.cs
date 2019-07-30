using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_GameMananger : MonoBehaviour {

    [SerializeField] private C_Level[] c_level;

    private C_Level c_usedLevel;

    public static C_GameMananger mc_this;

	void Start () {

        if (mc_this == null)
        {
            mc_this = (C_GameMananger)this;
        }

        FindObjectOfType<C_UI>().f_Init();

        for (int i = 0; i < c_level.Length; i++)
        {
            c_level[i].f_Init();
        }

        c_usedLevel = c_level[0].f_Activ();
	}

    public void f_ChangeLevel(int _idLevel)
    {
        StartCoroutine("IEChangeLevel", _idLevel);
    }

    private IEnumerator IEChangeLevel(int _idLevel)
    {
        yield return c_usedLevel.StartCoroutine("IEDiactiv");

        c_usedLevel = c_level[_idLevel].f_Activ();
    }
	
}
