using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour
{
    [SerializeField] private GameObject ballContainer;
    [SerializeField] private Text ballCounterText;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private float ballSpeed = 5f;
    [SerializeField] private float ballLaunchDelay = 0.2f;

    public static Vector3 launchPosition { get; private set; }

    private List<Ball> balls = new List<Ball>();
    private int returnedBalls = 0;
    private int ballsToAdd = 0;

    // Start is called before the first frame update
    void Start()
    {
        CreateBall();
        UpdateBallCounterText();
        balls[0].transform.position = ballContainer.transform.position;
        UpdateBallPosition(balls[0].transform.position);
    }

    private void UpdateBallCounterText()
    {
        ballCounterText.text = returnedBalls + "x";
    }

    public void LaunchBalls()
    {
        StartCoroutine("LaunchBallsWithDelay");
    }

    private IEnumerator LaunchBallsWithDelay()
    {
        foreach (Ball ball in balls)
        {
            ball.GetComponent<Rigidbody2D>().velocity = 
                PlayerController.launchDirection * ballSpeed;

            returnedBalls--;
            UpdateBallCounterText();

            yield return new WaitForSeconds(ballLaunchDelay);
        }
    }

    public void UpdateBallPosition(Vector3 newPosition)
    {
        launchPosition = newPosition;
    }

    private void CreateBall()
    {
        Ball ball = Instantiate(
            ballPrefab,
            launchPosition,
            Quaternion.identity,
            ballContainer.transform);

        ball.SetManager(this);
        balls.Add(ball);
        returnedBalls++;
    }

    private void CreatePowerupBalls()
    {
        for (int i = 0; i < ballsToAdd; i++)
        {
            CreateBall();
        }

        ballsToAdd = 0;
    }

    public void ReturnBall(Ball ball)
    {
        returnedBalls++;

        if (returnedBalls == 1) UpdateBallPosition(ball.transform.position);
        else ball.MoveToLaunchPosition(launchPosition);

        if (returnedBalls == balls.Count)
        {
            CreatePowerupBalls();
            UpdateBallCounterText();
            FindObjectOfType<GameController>().DeactivateBalls();
        }
    }

    public void IncrementBallCount()
    {
        ballsToAdd++;
    }
}
