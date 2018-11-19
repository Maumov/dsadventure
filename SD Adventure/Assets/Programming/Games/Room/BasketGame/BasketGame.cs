using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketGame : BaseGame
{
    [Header("Basket Game")]
    public Camera GameCam;
    public GameObject UIContent;
    public Text Question;
    public Text[] Answers;
    public GameObject[] Balls;
    GameObject currentBall;
    Vector3[] ballPositions;

    int max = 10;
    int total, a, b;
    int[] options = new int[3];
    bool repeated;

    int tries;
    int hits;

    Ray ray;
    RaycastHit hit;

    public Transform FinalPos;

    Vector3 initialScale = new Vector3(35, 35, 35);
    Vector3 finalScale = new Vector3(22, 22, 22);

    public override void StartGame()
    {
        base.StartGame();
        SetQuestion();
    }

    protected override void Initialize()
    {
        UIContent.SetActive(false);
        ballPositions = new Vector3[Balls.Length];
        for(int i = 0; i < ballPositions.Length; i++)
            ballPositions[i] = Balls[i].transform.position;

        Question.text = string.Empty;
        for(int i = 0; i < Answers.Length; i++)
            Answers[i].text = string.Empty;
    }

    void SetQuestion()
    {
        if(tries > 4)
        {
            Complete();
            enableControls = false;
            return;
        }

        Randomizer.Randomize(ballPositions);
        for(int i = 0; i < ballPositions.Length; i++)
        {
            Balls[i].transform.position = ballPositions[i];
            Balls[i].transform.localScale = initialScale;
        }

        total = Random.Range((int)(max * 0.4f), max + 1);
        a = Random.Range(1, total);
        b = total - a;

        do
        {
            repeated = false;
            options[0] = Random.Range(1, max + 1);
            options[1] = Random.Range(1, max + 1);

            if(options[0] == total)
                repeated = true;

            if(options[1] == total)
                repeated = true;

            if(options[0] == options[1])
                repeated = true;
        } while(repeated);

        options[2] = total;

        Randomizer.Randomize(options);

        Question.text = a + "+" + b;
        for(int i = 0; i < Balls.Length; i++)
        {
            Balls[i].name = options[i].ToString();
            Answers[i].text = options[i].ToString();
        }
        UIContent.SetActive(true);
    }

    protected override void CompleteValidations()
    {
        if(hits > 2)
        {
            acomplishmentLevel = 2;
            DataManager.AddProgressKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 2);
        }
        else if(hits > 0)
        {
            acomplishmentLevel = 1;
            DataManager.AddProgressKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 1);
        }
        else
        {
            acomplishmentLevel = 0;
            DataManager.AddProgressKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 0);
        }
    }

    private void Update()
    {
        if(!enableControls)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            ray = GameCam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 50))
            {
                if(hit.transform.CompareTag("Drag"))
                {
                    currentBall = hit.transform.gameObject;
                    CheckAnswer();
                }
            }
        }
    }

    [ContextMenu("Check")]
    void CheckAnswer()
    {
        UIContent.SetActive(false);
        ImportantAction();
        CompleteButton.SetActive(false);

        tries++;
        if(currentBall.name.Equals(total.ToString()))
            hits++;

        StartCoroutine(MoveBall());
    }

    IEnumerator MoveBall()
    {
        enableControls = false;
        float t = 0;
        Vector3 iniPos = currentBall.transform.position;
        LeanTween.scale(currentBall.gameObject, finalScale, 1);
        while(t < 1)
        {
            currentBall.transform.position = Vector3.Lerp(iniPos, FinalPos.position, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * 20;
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;
        SfxManager.Play(SFXType.Basket);
        while(t < 0.5f)
        {
            currentBall.transform.Translate(Vector3.down * 75 * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        enableControls = true;
        SetQuestion();
    }
}
