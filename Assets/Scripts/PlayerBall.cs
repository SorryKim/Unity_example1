using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    Rigidbody rigid;
    public float jumpPower;
    bool isJump;
    public int itemCount;
    public GameManagerLogic manager;
    AudioSource audio;

    void Awake()
    {
        itemCount = 0;
        rigid = GetComponent<Rigidbody>();
        isJump = false;
        audio = GetComponent<AudioSource>();
        
    }

    private void Update()
    {
        // 공 점프
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {

        // 공의 상하좌우 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            isJump = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            itemCount++;
            audio.Play();
            manager.GetItem(itemCount);
            other.gameObject.SetActive(false);
        }

        else if(other.tag == "Finish")
        {
            // Game Clear!
            if(manager.totalItemCount == itemCount)
            {
                if (manager.stage == 1)
                    SceneManager.LoadScene(0);
                else
                    SceneManager.LoadScene("Example1_" + (manager.stage+1).ToString());
            }
            // Restart
            else
            {
                SceneManager.LoadScene("Example1_" + manager.stage.ToString());
            }
        }
    }
}
