using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Button : MonoBehaviour
{

    public float m_minPos;
    public float m_maxPos;
    private float _z = -1.1f;
    private float _x = 0;
    private float _y = 0;

    public float speed;
    private void OnMouseDrag()
    {

      
      //  _y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        _y += Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * 5f;
       // _x = Mathf.Clamp(_x, m_minPos, m_maxPos);
        transform.position = new Vector3(_x , _y, _z);

    }

    private void OnMouseDown()
    {
        _y = transform.position.y;
    }
    private void OnMouseExit()
    {

    }
}
