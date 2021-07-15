using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text ballsCountText;
    public int shootCount;
    public int ballsCount;
    public int score;
    public GameObject[] block;
    public List<GameObject> levels;
    public GameObject GameOverScreen;

   
    private GameObject level1,level2;
    private GameObject ballContainer;
    private Vector2 level1pos, level2pos;
    private ShotCounter shotCounter;
    private bool firstShoot = true;
    
   
    private void Awake()
    {
        shotCounter = GameObject.Find("ShotCounter").GetComponent<ShotCounter>();
        ballsCountText = GameObject.Find("BallsCountText").GetComponent<Text>();
        ballContainer = GameObject.Find("BallContainer");
    }

    void Start()
    {
        PlayerPrefs.DeleteKey("Level");
        Physics2D.gravity = new Vector2(0,-17);
        ballsCount = PlayerPrefs.GetInt("BallsCount", 5);
        ballsCountText.text = ballsCount.ToString();
        
        SpawnLevel();
        GameObject.Find("Cannon").GetComponent<Animator>().SetBool("MoveIn", true);

    }

    
    void Update()
    {
        if(ballContainer.transform.childCount==0 && shootCount == 4)
        {
            GameOverScreen.SetActive(true);
            GameObject.Find("Cannon").GetComponent<Animator>().SetBool("MoveIn", false);
        }

        if (shootCount > 2)
            firstShoot = false;
        else
            firstShoot = true;
        CheckBlocks();
    }

    void SpawnNewLevel(int numberLevel1, int numberLevel2,int min, int max)
    {
        if(shootCount>1)
            Camera.main.GetComponent<CameraShake>().RotateCameraToFront();

        shootCount = 1; 


        level1pos = new Vector2(3.5f, 1);
        level2pos = new Vector2(3.5f, -3.4f);
        level1 = levels[numberLevel1];
        level2 = levels[numberLevel2];

        Instantiate(level1, level1pos, Quaternion.identity);
        Instantiate(level2, level2pos, Quaternion.identity);

        SetBlocksCount(min, max);

    }


    void SetBlocksCount(int min,int max)
    {
        block = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < block.Length; i++)
        {
            int count = Random.Range(min, max);
            block[i].GetComponent<Block>().SetStartingCount(count);
        }
    }


    void SpawnLevel()
    {
        if (PlayerPrefs.GetInt("Level",0) == 0)
            SpawnNewLevel(0, 17, 3, 5);

        if (PlayerPrefs.GetInt("Level") == 1)
            SpawnNewLevel(1, 18, 3, 5);

        if (PlayerPrefs.GetInt("Level") == 2)
            SpawnNewLevel(2, 19, 3, 6);

        if (PlayerPrefs.GetInt("Level") == 3)
            SpawnNewLevel(5, 20, 4, 7);

        if (PlayerPrefs.GetInt("Level") == 4)
            SpawnNewLevel(12, 28, 5, 8);

        if (PlayerPrefs.GetInt("Level") == 5)
            SpawnNewLevel(14, 29, 7, 10);

        if (PlayerPrefs.GetInt("Level") == 6)
            SpawnNewLevel(15, 30, 6, 12);

        if (PlayerPrefs.GetInt("Level") == 7)
            SpawnNewLevel(16, 31, 9, 15);

    }


    public void CheckBlocks()
    {
        block = GameObject.FindGameObjectsWithTag("Block");
        if (block.Length < 1)
        {
            RemoveBalls();
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            SpawnLevel();

            if (ballsCount >= PlayerPrefs.GetInt("BallsCount", 5))
                PlayerPrefs.SetInt("BallsCount", ballsCount);

            if (firstShoot)
                score += 5;
            else
                score += 3;
        }
    }

    void RemoveBalls()
    {
        GameObject[] ball = GameObject.FindGameObjectsWithTag("Ball");

        for (int i = 0; i < ball.Length; i++)
        {
            Destroy(ball[i]);
        }
    }


    public void ChechShotCount()
    {
        if(shootCount == 1)
        {
            shotCounter.SetTopText("SHOT");
            shotCounter.SetBottomText("1/3");
            shotCounter.Flash();

        }

        if (shootCount == 2)
        {
            shotCounter.SetTopText("SHOT");
            shotCounter.SetBottomText("2/3");
            shotCounter.Flash();

        }

        if (shootCount == 3)
        {
            shotCounter.SetTopText("FINAL");
            shotCounter.SetBottomText("SHOT");
            shotCounter.Flash();

        }
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
