using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Shoot : MonoBehaviour
{

    public float power = 2f;

    public GameObject ballPrefab;
    public GameObject ballsContainer;
    

    private int dots = 15;
    private Vector2 startPosition;
    private bool shoot, aiming;

    private GameObject Dots;
    private List<GameObject> projectilesPath;

    private Rigidbody2D ballRB;
    private GameController gc;
    

    private void Awake()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        Dots = GameObject.Find("Dots");
    }
    void Start()
    {
        
        projectilesPath = Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = false;
        }
        
    }

    void Update()
    {
        ballRB = ballPrefab.GetComponent<Rigidbody2D>();
        if (gc.shootCount <= 3 && !IsMouseOverUI())
        {
            Aim();
            Rotate();

        }
      
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void Aim()
    {
        if (shoot)
            return;

        if (Input.GetMouseButton(0))
        {
            if (!aiming)
            {
                aiming = true;
                startPosition = Input.mousePosition;
                gc.ChechShotCount();
            }
            else
            {
                PathCalculation();
            }


        }
        else if(aiming && !shoot)
        {
            aiming = false;
            HideDots();
            StartCoroutine(Shooting());
            if (gc.shootCount == 1)
                Camera.main.GetComponent<CameraShake>().RotateCameraToSide();

        }
    }

     
    Vector2 ShootFore(Vector3 force)
    {
        return (new Vector2(startPosition.x, startPosition.y) - new Vector2(force.x, force.y)) * power;
    }

    Vector2 DotPath(Vector2 startP,Vector2 startVel, float t)
    {
        return startP + startVel * t + 0.5f * Physics2D.gravity * t * t;
    }

    void PathCalculation()
    {
        Vector2 vel = ShootFore(Input.mousePosition) * Time.fixedDeltaTime / ballRB.mass;

        for(int i =0; i<projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = true;

            float t = i / 15f;
            Vector3 point = DotPath(transform.position, vel, t);
            point.z = 1;
            projectilesPath[i].transform.position = point;
        }
    }

    void ShowDots()
    {
        for (int i = 0; i <projectilesPath.Count ; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = true;
        }
    }

    void HideDots()
    {
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = false;
        }
    }

    void Rotate()
    {
        var dir = GameObject.Find("dot (1)").transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    IEnumerator Shooting()
    {
        for (int i = 0; i < gc.ballsCount ; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            ball.name = "Ball";
            ball.transform.SetParent(ballsContainer.transform);
            ballRB = ball.GetComponent<Rigidbody2D>();
            ballRB.AddForce(ShootFore(Input.mousePosition));

            int balls = gc.ballsCount - i;
            gc.ballsCountText.text = (gc.ballsCount-i-1).ToString();
        }

        yield return new WaitForSeconds(0.5f);
        gc.shootCount++;
        gc.ballsCountText.text = gc.ballsCount.ToString();
    }
}
