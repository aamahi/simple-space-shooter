using UnityEngine;

public class Win : MonoBehaviour
{
    /// <summary>
    /// Public properties.
    /// </summary>
    public Texture backgroundTexture;

    /// <summary>
    /// Private properties.
    /// </summary>
    private float buttonsOffsetTop    = 0;
    private float buttonsOffsetBottom = 0;
    private int   buttonsWidth        = 0;
    private int   buttonsHeight       = 0;
    private float textAreaWidth       = 0;
    private float textAreaHeight      = 0;

    /// <summary>
    /// Renders to the GUI at every frame update.
    /// </summary>
    private void OnGUI()
    {
        // Render the background texture to the screen.
        this.RenderBackground();

        // Render the border area.
        this.RenderBorder();

        // Render the game status.
        this.RenderGameStatus();

        // Render the game title header.
        this.RenderGameHeader();

        // Render the game scores.
        this.RenderGameScores();
        
        // Render the main buttons.
        this.RenderMainButtons();
    }

    /// <summary>
    // Renders the background texture to the screen.
    /// </summary>
    private void RenderBackground()
    {
        // Set the background area as a rectangle.
        Rect background = new Rect(0, 0, Screen.width, Screen.height);

        // Render the background texture to the screen.
        GUI.DrawTexture(background, backgroundTexture);
    }

    /// <summary>
    // Renders the border area.
    /// </summary>
    private void RenderBorder()
    {
        // Create a new GUI Style object based on the TextArea component.
        GUIStyle labelStyle = new GUIStyle(GUI.skin.textArea);

        // Render the border area around the text and button.
        GUI.Label(
            new Rect(
                (float)((Screen.width / 2) - (this.textAreaWidth / 2)),
                (float)((Screen.height / 2) - (this.textAreaHeight / 2)),
                this.textAreaWidth,
                this.textAreaHeight
            ),
            @"",
            labelStyle
        );
    }

    /// <summary>
    /// Renders the game header title.
    /// </summary>
    private void RenderGameHeader()
    {
        // Create a new GUI Style object based on the Label component.
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);

        // Set the GUI Style properties for the game title header.
        labelStyle.fontSize  = 40;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.padding   = new RectOffset(20, 20, 20, 20);
        labelStyle.alignment = TextAnchor.UpperCenter;

        // Render the game title header.
        GUI.Label(
            new Rect(
                (float)((Screen.width / 2) - (this.textAreaWidth / 2)),
                (float)((Screen.height / 2) + this.buttonsOffsetBottom), 
                this.textAreaWidth, 
                this.textAreaHeight
            ),
            @"Simple Space Shooter Game",
            labelStyle
        );
    }
    
    /// <summary>
    /// Renders the game scores.
    /// </summary>
    private void RenderGameScores()
    {
        // Set the player scores area properties.
        float playerScoresWidth    = 160.0f;
        float playerScoresHeight   = 160.0f;
        int   playerScoresFontSize = 12;

        // Renders the player scores.
        Player.RenderStatusLabel(
            (float)((Screen.width / 2) + 20),
            (float)((Screen.height / 2) - (this.buttonsHeight + 20)),
            playerScoresWidth,
            playerScoresHeight,
            playerScoresFontSize
        );
    }

    /// <summary>
    /// Renders the game status.
    /// </summary>
    private void RenderGameStatus()
    {
        // Create a new GUI Style object based on the Label component.
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);

        // Set the GUI Style properties for the game title header.
        labelStyle.fontSize  = 40;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.padding   = new RectOffset(20, 20, 20, 20);
        labelStyle.alignment = TextAnchor.UpperCenter;

        // Render the game status.
        GUI.Label(
            new Rect(
                (float)((Screen.width / 2) - (this.textAreaWidth / 2)),
                (float)((Screen.height / 2) - this.buttonsOffsetTop), 
                this.textAreaWidth, 
                this.textAreaHeight
            ),
            @"CONGRATULATIONS!",
            labelStyle
        );
    }

    /// <summary>
    /// Renders the main buttons.
    /// </summary>
    private void RenderMainButtons()
    {
        // Create a new GUI Style object based on the Button component.
        GUIStyle labelStyle = new GUIStyle(GUI.skin.button);

        // Set the GUI Style properties for the main buttons.
        labelStyle.fontSize  = 18;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.padding   = new RectOffset(20, 20, 20, 20);
        labelStyle.alignment = TextAnchor.UpperCenter;

        // Render the start button.
        bool playAgainButtonPressed = GUI.Button(
            new Rect(
                (float)((Screen.width / 2) - 160), 
                (float)((Screen.height / 2) - this.buttonsHeight), 
                this.buttonsWidth, 
                this.buttonsHeight
            ),
            @"PLAY AGAIN",
            labelStyle
        );

        // Render the main menu button.
        bool mainMenuButtonPressed = GUI.Button(
            new Rect(
                (float)((Screen.width / 2) - 160),
                (float)((Screen.height / 2) + 5),
                this.buttonsWidth,
                this.buttonsHeight
            ),
            @"MAIN MENU",
            labelStyle
        );

        // Check if the play again button has been pressed.
        if (playAgainButtonPressed || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Fire1"))
        {
            // Load the first game level.
            Application.LoadLevel(@"Level_001");
        }

        // Check if the main menu button has been pressed.
        if (mainMenuButtonPressed || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire2"))
        {
            // Load the main menu.
            Application.LoadLevel(@"MainMenu_001");
        }
    }

    /// <summary>
    /// Initialization.
    /// </summary>
    private void Start()
    {
        // Initialize private properties.
        this.buttonsOffsetTop    = 160;
        this.buttonsOffsetBottom = 70;
        this.buttonsWidth        = 160;
        this.buttonsHeight       = 60;
        this.textAreaWidth       = 640;
        this.textAreaHeight      = 420;
    }
}
