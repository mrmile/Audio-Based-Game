using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] bool invincible;

    public int maxHits;
    public int currentHits;
    [SerializeField] float invulnerableTime;
    float currentInvTime;
    [SerializeField] float knockBackTime;
    float currentKnockBackTime;

    [SerializeField] bool canTakeHits;
    
    Vector2 knockBackDirection;
    PlayerMovement movement;
    [SerializeField] float knockBackMinSpeed;
    [SerializeField] float knockBackMaxSpeed;
    float knockBackSpeed;

    [SerializeField] int flashTimesPerSecond;
    float flashingTime;
    [SerializeField] float currentFlashingTime;
    SpriteRenderer sp;

    AudioSource hitSfx;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        sp = GetComponentInChildren<SpriteRenderer>();
        hitSfx = GetComponent<AudioSource>();

        canTakeHits = true;
        flashingTime = 1.0f / flashTimesPerSecond * invulnerableTime;
    }

    // Update is called once per frame
    void Update()
    {



        if (currentInvTime > 0)
        {
            currentInvTime -= Time.deltaTime;
        }
        else
        {
            canTakeHits = true;
        }

        if (currentKnockBackTime > 0)
        {
            movement.movement = knockBackDirection;
            movement.MoveCharacter(movement.movement, knockBackSpeed);

            currentKnockBackTime -= Time.deltaTime;
        }
        else
        {
            movement.canMove = true;
        }


        if (currentInvTime > 0)
        {
            //print(currentFlashingTime);
            if (currentFlashingTime > 0) currentFlashingTime -= Time.deltaTime;
            else
            {
                sp.enabled = !sp.enabled;
                currentFlashingTime = flashingTime;
            }
            canTakeHits = false;
        }
        else 
        {
            if (currentFlashingTime != flashingTime || !sp.enabled)
            {
                sp.enabled = true;
                currentFlashingTime = flashingTime;
            }
                
            if (movement.dashing) 
                canTakeHits = false;
            else
            canTakeHits = true;
        }

        if (invincible) canTakeHits = false;


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //print("test");
        if (collision.gameObject.CompareTag("Obstacle") && canTakeHits)
        {
            //print("hit");
            hitSfx.Play();
            currentHits++;
            canTakeHits = false;

            currentInvTime = invulnerableTime;

            currentKnockBackTime = knockBackTime;
            movement.canMove = false;

            knockBackDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            knockBackSpeed = Random.Range(knockBackMinSpeed, knockBackMaxSpeed);
        }
    }


}
