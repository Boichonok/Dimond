using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_UI : MonoBehaviour {

    [SerializeField] private C_Button[] c_button;

    [Space]
    [SerializeField] private Transform mt_leftAnchor;
    [SerializeField] private Transform mt_RightAnchor;

    [HideInInspector] public bool m_clik;

    public AnimationCurve ac_scale;

    public static C_UI mc_this;

    public void f_Init()
    {

        if (mc_this == null)
        {
            mc_this = (C_UI)this;
        }

        mt_leftAnchor.localPosition = new Vector3(Camera.main.ScreenToWorldPoint(Vector2.zero).x, 0, 0);
        mt_RightAnchor.localPosition = new Vector3(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x, 0, 0);

        for (int i = 0; i < c_button.Length; i++)
        {
//            c_button[i].f_StartGame();
        }
        m_clik = false;
    }

    /// <summary>
    /// 0 - star, 2 - quit, 3 - next, 4 - rate
    /// </summary>
    /// <param name="_idButton"></param>
    public void f_ActivButton(int _idButton)
    {
       // c_button[_idButton].f_Activ();
    }

    /// <summary>
    /// 0 - star, 2 - quit, 3 - next, 4 - rate
    /// </summary>
    /// <param name="_idButton"></param>
    public void f_DiactivButton(int _idButton)
    {
    //   c_button[_idButton].f_Diactiv();
    }

    public void f_ClikButton(E_Enums _enum)
    {
        switch (_enum)
        {
            case E_Enums.star:
                {
                    C_GameMananger.mc_this.f_ChangeLevel(1);
                    break;
                }
            case E_Enums.quit:
                {
                    break;
                }
            case E_Enums.next:
                {
                    break;
                }
            case E_Enums.rate:
                {
                    break;
                }
            case E_Enums.color_0:
                {
                    break;
                }
            case E_Enums.color_1:
                {
                    break;
                }
            case E_Enums.color_2:
                {
                    break;
                }
        }
    }
    public void f_CLikItems(E_EnumsItems _enum)
    {
        switch (_enum)
        {
            case E_EnumsItems.star:
                break;
            case E_EnumsItems.quit:
                break;
            case E_EnumsItems.next:
                break;
            case E_EnumsItems.rate:
                break;
            case E_EnumsItems.color_0:
                break;
            case E_EnumsItems.color_1:
                break;
            case E_EnumsItems.color_2:
                break;
            default:
                break;
        }
    }
}
