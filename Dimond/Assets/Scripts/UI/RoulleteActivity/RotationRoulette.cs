using UnityEngine;
using System.Collections;

public class RotationRoulette : MonoBehaviour
{
    [SerializeField] private RouletteArrow rouletteArrow;
    [SerializeField] private float SpeedRotation;//200;
    [SerializeField] private float TimeRotation;//20;
    [SerializeField] private float minSpeed = -200;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    private IRouletteBonusListener bonusListener;
    private Rigidbody2D rb2D;
    private Transform position;
    private float Z_Rotation = 0.0f;
    private float speedMinusRotation;
    private float timeRotationReset;

    public void InitBonusListener(IRouletteBonusListener bonusListener)
    {
        this.bonusListener = bonusListener;
    }

    public void StartRotation()
    {
        rb2D = GetComponent<Rigidbody2D>();
        position = transform;
        StopAllCoroutines();
        StartCoroutine("IEStartRotation");
    }

    private IEnumerator IEStartRotation()
    {
        SpeedRotation = Random.Range(minSpeed, maxSpeed);
        TimeRotation = Random.Range(minTime, maxTime);
       
        speedMinusRotation = SpeedRotation / 10;
        while (TimeRotation > 0)
        {
            rb2D.MoveRotation(rb2D.rotation + SpeedRotation * Time.fixedDeltaTime);
            SpeedRotation -= Time.fixedDeltaTime * speedMinusRotation;
            TimeRotation -= Time.fixedDeltaTime;
            transform.position = position.position;
            yield return null;
        }
        bonusListener.Bonus(rouletteArrow.GetBonus());
    }

}
