using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rb;
    public float jump = 5;
    private bool isgrounded = false;

    private Animator anim;
    private Vector3 rotation;

    private CoinManager m;

    public GameObject panel;

    public GameObject kamera;

    public FixedJoystick joystick;

    public FloatingJoystick floatingJoystick;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rotation = transform.eulerAngles;
        m = GameObject.FindGameObjectWithTag("Text").GetComponent<CoinManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float richtung = Input.GetAxis("Horizontal");
        float mobile_richtung = joystick.Horizontal;
        float mobile_jump = floatingJoystick.Vertical;

        if (richtung != 0 || mobile_richtung !=0)
        {
            anim.SetBool("isRunning", true);
        }

        else
        {
            anim.SetBool("isRunning", false);
        }

        if (richtung < 0)
        {
            transform.eulerAngles = rotation - new Vector3(0, 180, 0);
            transform.Translate(Vector2.left * speed * richtung * Time.deltaTime);
        }

        if (richtung > 0)
        {
            transform.eulerAngles = rotation;
            transform.Translate(Vector2.right * speed * richtung * Time.deltaTime);
        }
        if (mobile_richtung < 0)
        {
            transform.eulerAngles = rotation - new Vector3(0, 180, 0);
            transform.Translate(Vector2.left * speed * mobile_richtung * Time.deltaTime);
        }

        if (mobile_richtung > 0)
        {
            transform.eulerAngles = rotation;
            transform.Translate(Vector2.right * speed * mobile_richtung * Time.deltaTime);
        }
        if (isgrounded == false)
        {
            anim.SetBool("isJumping", true);
        }

        else
        {
            anim.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) || mobile_jump > 0.5 && isgrounded)
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            FindAnyObjectByType<AudioManager>().Play("Jump");
            //FindAnyObjectByType<AudioManager>().Play("Run");
            isgrounded = false;
        }

        kamera.transform.position = new Vector3(transform.position.x, 0, -10);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isgrounded = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            panel.SetActive(true);
            FindAnyObjectByType<AudioManager>().Play("Die");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            m.Addmoney();
            Destroy(other.gameObject);
            FindAnyObjectByType<AudioManager>().Play("Coin");
        }
        if (other.gameObject.tag == "Spike")
        {
            panel.SetActive(true);
            Destroy(gameObject);
            FindAnyObjectByType<AudioManager>().Play("Die");
        }
        if (other.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
