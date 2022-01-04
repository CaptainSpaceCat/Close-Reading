using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //[SerializeField]
    //private List<string> wordList;

    [Tooltip("The main Player object")]
    public PlayerController player;

    private int level = 0;

    public InputField userGuess;
    //public Button submit;
    public TextMeshPro displayWordText;
    public Slider healthBar;
    public Image healthBarCover;

    public Canvas inputGuessUI;

    public float time = 30f;
    public float maxTime = 30f;
    public float deltaTime = 5f;

    public int randomSeed = 10;

    public TextAsset corpus;

    private TextInputHandler textSource;
    private DisplayWordHandler wordDisplay;

    public int walkSteps = 15;
    public float walkDelta = .1f;

    private bool gameOver = false;

    private CameraController cam;

    public AnimationCurve letterLengthCurve;
    public AnimationCurve zoomCurve;

    public AudioSource successClip;
    public AudioSource failClip;

    public Text scoreText;


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        wordDisplay = GetComponent<DisplayWordHandler>();
        inputGuessUI.gameObject.SetActive(false);
        textSource = new TextInputHandler(corpus);//, randomSeed);
        Cursor.lockState = CursorLockMode.Locked;
        userGuess.DeactivateInputField();

        //initialize first level
        wordDisplay.SetText(GenerateNextWord(level));
        Refresh();
        healthBar.maxValue = maxTime;
    }

    public Canvas pauseMenu;

    // Update is called once per frame
    void Update()
    {

        if (Cursor.lockState == CursorLockMode.Locked) {

            time -= Time.deltaTime;
            UpdateHealthBar();

            if (time <= 0f && !gameOver) {
                if (CheckSolution(userGuess.text)) {
                    inputGuessUI.gameObject.SetActive(false);
                    userGuess.DeactivateInputField();
                    userGuess.text = "";
                    player.controlState = PlayerController.ControlState.Move;
                    return;
                }
                inputGuessUI.gameObject.SetActive(false);
                userGuess.DeactivateInputField();
                userGuess.text = "";
                

                level = 0;
                gameOver = true;
                player.controlState = PlayerController.ControlState.Move;
                wordDisplay.SetText("Game Over");
                wordDisplay.Flush();
                displayWordText.color = Color.red;
                Refresh();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.None)
        {
            Resume();
        }

        if (gameOver)
        {
            healthBar.gameObject.SetActive(false);
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (player.controlState == PlayerController.ControlState.Move)
                {
                    player.controlState = PlayerController.ControlState.Type;
                    inputGuessUI.gameObject.SetActive(true);
                    //inputGuessUI.gameObject.GetComponent<Image>().color = Color.red;
                    userGuess.ActivateInputField();
                }
                else if (player.controlState == PlayerController.ControlState.Type)
                {
                    player.controlState = PlayerController.ControlState.Move;
                    CheckSolution(userGuess.text);
                    inputGuessUI.gameObject.SetActive(false);
                    //inputGuessUI.gameObject.GetComponent<Image>().color = new Color(0,0,0,255);
                    userGuess.DeactivateInputField();
                    userGuess.text = "";
                }
            }
        }
    }

    public void Resume() {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.gameObject.SetActive(false);
    }

    private void UpdateHealthBar() {
        healthBar.value = (int)time;
        if (healthBar.value >= maxTime/2) {
            healthBarCover.color = new Color(36/255f, 255/255f, 255/255f, 255/255f);
        } else if (healthBar.value > 10) {
            healthBarCover.color = new Color(230/255f, 180/255f, 0/255f, 255/255f);
        } else {
            healthBarCover.color = new Color(255/255f, 50/255f, 255/255f, 255/255f);
        }
    }

    private bool CheckSolution(string guess) {
        if (guess == wordDisplay.GetText()) {
            successClip.Play();
            level++;
            time += deltaTime;
            if (time > maxTime)
            {
                time = maxTime;
            }
            wordDisplay.SetText(GenerateNextWord(level));
            Refresh();
            return true;
        } else {
            failClip.Play();
            return false;
        }
    }

    private string GenerateNextWord(int difficultyClass) {
        float walkStart = letterLengthCurve.Evaluate(difficultyClass);
        for (int i = 0; i < walkSteps; i++) {
            if (Random.value <= .5f) {
                walkStart += walkDelta;
            } else {
                walkStart -= walkDelta;
            }
        }
        int wordLength = (int)Mathf.Round(walkStart);
        return textSource.getWordOfLength(wordLength);
    }

    private void Refresh() {
        scoreText.text = "Score: " + level;
        wordDisplay.Flush();
        cam.UpdateZoom(zoomCurve.Evaluate(level));
        player.ResetPosition(wordDisplay.GetBounds());
    }
}
