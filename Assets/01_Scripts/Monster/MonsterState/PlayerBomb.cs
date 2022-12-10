using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    float bombPower = 20.0f;
    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Bomb()
    {
        StartCoroutine(BombUP());
    }

    IEnumerator BombUP()
    {
        while(true)
        {
        yield return new WaitForSeconds(0.01f);
        characterController.Move(Vector3.up * bombPower * Time.deltaTime);
            if(transform.position.y>7f)
            {
                break;
            }
        }
    }

}
