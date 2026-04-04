using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Main game controller: UI, scoring, lives, randomization, camera. </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip ballFinishSound, buttonSound, newHighScoreSound;
    [SerializeField] Image[] buttonSeleced;
    [SerializeField] GameObject main, gameOver, isMusicOnIcon, isMusicOffIcon;
    [SerializeField] TextMeshProUGUI timerText, backgroundChosenText, livesText, placesText, scoreText, eventText, newHighScoreText;
    [SerializeField] int lives, finishersCount, score, highScore, backgroundChosen;
    public int FinishersCount => finishersCount;
    bool highestScoreReached;
    bool isMusicOn;
    float timer;

    [SerializeField] Obstacle[] obstacles;
    [SerializeField] Ball[] balls;
    [SerializeField] GameObject currentWinningBall;
    Ball chosenBall;

    readonly Color[] backgroundColors = new Color[]
    {
        new Color(0f, 0f, 0.8f),      // Blue
        new Color(0f, 0.5f, 1f),      // Light Blue
        new Color(0f, 0.5f, 0.6f),    // Teal
        new Color(0f, 0.7f, 0f),      // Green
        new Color(0.5f, 0.9f, 0f),    // Lime Green
        new Color(0.8f, 0.8f, 0f),    // Yellow
        new Color(0.9f, 0.5f, 0f),    // Orange
        new Color(0.7f, 0f, 0f),      // Red
        new Color(0.8f, 0f, 0.8f),    // Magenta
        new Color(0.5f, 0f, 1f),      // Purple
        new Color(0.5f, 0.5f, 0.5f),  // Gray
        new Color(0.1f, 0.1f, 0.1f),  // Black
    };

    // Load progress and setup
    void Awake()
    {
        LoadProgress();

        AudioListener.volume = isMusicOn ? 1 : 0;
        isMusicOnIcon.SetActive(isMusicOn);
        isMusicOffIcon.SetActive(!isMusicOn);

        lives = GameConstants.StartingLives;
        livesText.text = lives.ToString();

        backgroundChosenText.text = backgroundChosen + $"/{backgroundColors.Length}";
        mainCamera.backgroundColor = backgroundColors[(backgroundChosen - 1) % backgroundColors.Length];

        // Ensures a ball is selected by default to avoid null reference errors before the player makes a choice
        currentWinningBall = balls[0].gameObject;
        BallChoosing(0);
    }

    // Updates timer and makes camera follow the current winning ball
    void Update()
    {
        timerText.text = (timer += Time.deltaTime).ToString("F1");
        foreach (Ball x in balls) if (x.transform.position.y < currentWinningBall.transform.position.y) currentWinningBall = x.gameObject;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, currentWinningBall.transform.position.y, mainCamera.transform.position.z);
    }

    // Resets round state, randomizes all obstacles, teleports balls, then hides main UI after 1 frame to avoid physics glitch
    public void StartRound()
    {
        main.SetActive(false);
        audioSource.PlayOneShot(buttonSound);
        placesText.text = "      Places:";
        eventText.gameObject.SetActive(false);
        newHighScoreText.gameObject.SetActive(false);
        finishersCount = 0;
        timer = 0;

        foreach (var ball in balls) ball.Teleport();
        foreach (var obstacle in obstacles) obstacle.Randomize();
    }

    // Full session reset: restores lives and score, then returns to the ball-selection screen
    public void NewGame()
    {
        gameOver.SetActive(false);
        lives = GameConstants.StartingLives;
        livesText.text = lives.ToString();
        score = 0;
        scoreText.text = $"Chosen: <color=#{ColorUtility.ToHtmlStringRGB(chosenBall.GetColor())}>{chosenBall.name}</color>\n    Score: {score}\nH Score: {highScore}";
        placesText.text = "      Places:";
        eventText.gameObject.SetActive(false);
        main.SetActive(true);
    }

    // Balls call for this method when they reach the bottom ground
    public void Finished(Ball ball)
    {
        audioSource.PlayOneShot(ballFinishSound);
        finishersCount += 1;
        string colorHex = ColorUtility.ToHtmlStringRGB(ball.GetColor());
        placesText.text += $"\n{finishersCount}. <color=#{colorHex}>{ball.name}</color> ({timer.ToString("F1")})";

        if (finishersCount == 5)
        {
            eventText.gameObject.SetActive(true);
            int place = chosenBall.place;
            int gainedScore = place == 1 ? 100 : place == 2 ? 60 : place == 3 ? 40 : place == 4 ? 20 : 10;
            score += gainedScore;
            lives += place == 1 ? 1 : place == 2 ? 0 : -1;
            eventText.text = lives <= 0 ? $"Game over! Your score is: {score}" : place == 1 ? $"1st Place! Life + 1" : place == 2 ? $"2nd Place!" : place == 3 ? $"3rd Place!\nLife - 1" : place == 4 ? $"4th Place!\nLife - 1" : $"5th Place!\nLife - 1";

            livesText.text = lives.ToString();
            scoreText.text = $"Chosen: <color=#{ColorUtility.ToHtmlStringRGB(chosenBall.GetColor())}>{chosenBall.name}</color>\n    Score: {score} (<color=green>+{gainedScore}</color>)\nH Score: {highScore}";

            if (!highestScoreReached && score > highScore)
            {
                audioSource.PlayOneShot(newHighScoreSound);
                newHighScoreText.gameObject.SetActive(true);
                highestScoreReached = true;
                highScore = score;
            }

            if (lives > 0) main.SetActive(true);
            else gameOver.SetActive(true);
        }
    }

    // Toggle music on/off by clicking on icon
    public void MusicOnOff()
    {
        isMusicOn = !isMusicOn;
        AudioListener.volume = isMusicOn ? 1 : 0;
        isMusicOnIcon.SetActive(isMusicOn);
        isMusicOffIcon.SetActive(!isMusicOn);
        audioSource.PlayOneShot(buttonSound);
    }

    // Change background color by clicking on icon
    public void BackgroundColorChanger()
    {
        backgroundChosen = backgroundChosen == backgroundColors.Length ? 1 : backgroundChosen + 1;
        backgroundChosenText.text = backgroundChosen + $"/{backgroundColors.Length}";
        mainCamera.backgroundColor = backgroundColors[(backgroundChosen - 1) % backgroundColors.Length];
        audioSource.PlayOneShot(buttonSound);
    }

    // Ball choosing by clicking on ball colors
    public void BallChoosing(int index)
    {
        chosenBall = balls[index];
        foreach (Image x in buttonSeleced)
            x.color = new Color(x.color.r, x.color.g, x.color.b, 128f / 255f);
        buttonSeleced[index].color = new Color(buttonSeleced[index].color.r, buttonSeleced[index].color.g, buttonSeleced[index].color.b, 255f / 255f);
        scoreText.text = $"Chosen: <color=#{ColorUtility.ToHtmlStringRGB(chosenBall.GetColor())}>{chosenBall.name}</color>\n    Score: {score}\nH Score: {highScore}";
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region Save/Load
    void OnApplicationQuit() => SaveProgress();

    void SaveProgress()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("BackgroundChosen", backgroundChosen);
        PlayerPrefs.SetInt("IsMusicOn", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadProgress()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 100);
        backgroundChosen = PlayerPrefs.GetInt("BackgroundChosen", 1);
        isMusicOn = PlayerPrefs.GetInt("IsMusicOn", 1) == 1;
    }
    #endregion
}