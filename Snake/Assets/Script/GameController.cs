using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum Direction
    {
        LEFT, RIGHT, UP, DOWN
    }

    public Direction moveDirection;

    public float delayStep; // tempo entre um passo e outro
    public float step; // quantida de movimento a cada passo

    public Transform head;

    public List<Transform> tail;

    private Vector3 lastPos;

    public Transform food;
    public GameObject tailPrefab;

    public int col = 44;
    public int row =19;

    public Text txtScore;
    public Text txtHiScore;
    private int score;
    private int hiScore;

    public GameObject paineGameOver;
    public GameObject painelTitle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MoveSnake");
        SetFood();
        hiScore = PlayerPrefs.GetInt("HiScore");
        txtHiScore.text = "Hi-Score: " + hiScore.ToString();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            moveDirection = Direction.UP;
            head.rotation = Quaternion.Euler(0, 0, -90);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection = Direction.LEFT;
            head.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection = Direction.RIGHT;
            head.rotation = Quaternion.Euler(0, 0, 180);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection = Direction.DOWN;
            head.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    IEnumerator MoveSnake()
    {
        yield return new WaitForSeconds(delayStep);
        Vector3 nextPos = Vector3.zero;

        switch(moveDirection)
        {
            case Direction.DOWN:
                nextPos = Vector3.down;
                break;

            case Direction.LEFT:
                nextPos = Vector3.left;
                break;

            case Direction.RIGHT:
                nextPos = Vector3.right;
                break;

            case Direction.UP:
                nextPos = Vector3.up;
                break;
        }

        nextPos *= step;
        lastPos = head.position;
        head.position += nextPos;

        foreach(Transform t in tail)
        {
            Vector3 temp = t.position;
            t.position = lastPos;
            lastPos = temp;
            t.gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine("MoveSnake");
    }

    public void Eat()
    {
        Vector3 tailPosition = head.position;
        if(tail.Count > 0)
        {
            tailPosition = tail[tail.Count - 1].position;
        }

        GameObject temp = Instantiate(tailPrefab, tailPosition, transform.localRotation);
        tail.Add(temp.transform);
        score += 1;
        txtScore.text = "Score: " + score.ToString();
        SetFood();
    }

    void SetFood()
    {
        int x = Random.Range((col - 1) / 2 * -1, (col - 1) / 2);
        int y = Random.Range((row - 1) / 2 * -1, (row - 1) / 2);

        food.position = new Vector2(x * step, y * step); 
    }

    public void GameOver()
    {
        paineGameOver.SetActive(true);
        Time.timeScale = 0;
        if(score > hiScore)
        {
            PlayerPrefs.SetInt("HiScore", score);
            txtHiScore.text = "New Hi-Score: " + score.ToString();
        }
    }

    public void Jogar()
    {
        head.position = Vector3.zero;
        moveDirection = Direction.LEFT;
        foreach(Transform t in tail)
        {
            Destroy(t.gameObject);
        }
        tail.Clear();
        head.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        SetFood();
        score = 0;
        txtScore.text = "Score: 0";
        hiScore = PlayerPrefs.GetInt("HiScore");
        txtHiScore.text = "Hi-Score: " + hiScore.ToString();
        paineGameOver.SetActive(false);
        painelTitle.SetActive(false);
        Time.timeScale = 1;
    }
}
