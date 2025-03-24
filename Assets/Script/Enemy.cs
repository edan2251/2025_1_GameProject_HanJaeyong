using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int Health = 100;
    public float Timer = 1.0f;
    public int AttackPoint = 50;

    // Start is called before the first frame update
    void Start()
    {

        Health = 100;


    }//Start Fin.

    // Update is called once per frame
    void Update()
    {

        CharacterHealthUP();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Health -= AttackPoint;
        }

        CheckDeath();

    }//Update Fin.

    public void CharacterHit(int Damage)
    {
        Health -= Damage;
    }//.

    void CheckDeath()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("ÆÄ±«µÊ");
        }
    }//.

    void CharacterHealthUP()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Timer = 1;
            Health += 10;
        }
    }//.


}//Enemy Fin.
