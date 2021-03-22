using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public Text countText;
    private int count;
    private AudioSource boundSound;
    private void Awake()
    {
        boundSound = GameObject.Find("SoundManager").GetComponent<AudioSource>();
    }


    void Update()
    {
        if (transform.position.y <= -7)
            Destroy(gameObject);
        
    }


    public void SetStartingCount(int count)
    {
        this.count = count;
        countText.text = count.ToString();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name.Equals("Ball") && count > 0)
        {
            count--;
            Camera.main.GetComponent<CameraShake>().Shake();
            countText.text = count.ToString();
            boundSound.Play();
            if (count == 0)
            {
                Destroy(gameObject);
                Camera.main.GetComponent<CameraShake>().MediumShake();
                GameObject.Find("ExtraBall").GetComponent<Progres>().IncreaseCurrentWidth();
            }
        }
    }
}
