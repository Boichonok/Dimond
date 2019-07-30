using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Play Objects /Picture Backgrounds", fileName = "New Picture Background")]
public class BackgroundData : ScriptableObject
{
    [SerializeField]
    private int id;
    public int Id { get { return id; } }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite{ get { return sprite; }}
}
