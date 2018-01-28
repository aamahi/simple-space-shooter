using UnityEngine;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Public properties.
    /// </summary>
    public Texture backgroundTexture;

    /// <summary>
    /// Private properties.
    /// </summary>
    private float borderWidth             = 0;
    private float borderHeight            = 0;
    private float headerTextWidth         = 0;
    private float headerTextHeight        = 0;
    private float inputButtonsWidth       = 0;
    private float inputButtonsHeight      = 0;
    private float inputHeaderTextWidth    = 0;
    private float inputHeaderTextHeight   = 0;
    private float instructionsTextWidth   = 0;
    private float instructionsTextHeight  = 0;
    private float mainButtonsWidth        = 0;
    private float mainButtonsHeight       = 0;
    private int   selectedInputController = 0;
    
    /// <summary>
    /// Renders to the GUI at every frame update.
    /// </summary>
    private void OnGUI()
    {
        // Render the background texture to the screen.
        this.RenderBackground();

        // Render the border area.
        this.RenderBorder();

        // Render the game title header.
        this.RenderGameHeader();

        // Render the instructions text.
        this.RenderInstructions();

        // Render input selections.
        this.RenderInputSelection();

        // Render the main buttons.
        this.RenderMainButtons();
    }

    /// <summary>
    /// Initialization.
    /// </summary>
    private void Start()
    {
        // Initialize private properties.
        this.borderWidth             = 640.0f;
        this.borderHeight            = 420.0f;
        this.headerTextWidth         = 640.0f;
        this.headerTextHeight        = 420.0f;
        this.inputButtonsWidth       = 210.0f;
        this.inputButtonsHeight      = 80.0f;
        this.inputHeaderTextWidth    = 210.0f;
        this.inputHeaderTextHeight   = 25.0f;
        this.instructionsTextWidth   = 400.0f;
        this.instructionsTextHeight  = 360.0f;
        this.mainButtonsWidth        = 160.0f;
        this.mainButtonsHeight       = 60.0f;
        this.selectedInputController = 0;
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

        // Render the border area.
        GUI.Label(
            new Rect(
                (float)((Screen.width / 2) - (this.borderWidth / 2)),
                (float)((Screen.height / 2) - (this.borderHeight / 2)),
                this.borderWidth,
                this.borderHeight
            ),
            @"",
            labelStyle
        );
    }

    /// <summary>
    //// Renders the game title header.
    /// </summary>
    private void RenderGameHeader()
    {
        // Create a new GUI Style object based on the Label component.
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);

        // Set the GUI Style properties for the game title header.
        labelStyle.fontSize  = 32;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.padding   = new RectOffset(20, 20, 20, 20);
        labelStyle.alignment = TextAnchor.UpperCenter;

        // Render the game title header.
        GUI.Label(
            new Rect(
                (float)((Screen.width / 2) - (this.headerTextWidth / 2)),
                (float)((Screen.height / 2) - 210),
                this.headerTextWidth,
                this.headerTextHeight
            ),
            @"Simple Space Shooter Game",
            labelStyle
        );
    }

    /// <summary>
    /// Renders input selections.
    /// </summary>
    private void RenderInputSelection()
    {
        // Create a string array of available input controllers.
        string[] inputControllers = new string[] { @"Keyboard", @"Gamepad", @"Mouse" };

        // Create a new GUI Style object based on the Label component.
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);

        // Set the GUI Style properties for the input header.
        labelStyle.fontSize  = 18;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.alignment = TextAnchor.UpperCenter;

        // Render the input header label.
        GUI.Label(
            new Rect(
                (float)((Screen.width / 2) + 80),
                (float)((Screen.height / 2) - 125),
                this.inputHeaderTextWidth,
                this.inputHeaderTextHeight
            ),
            @"Select Input Controller",
            labelStyle
        );
        
        // Create a new GUI Style object based on the TextField component.
        labelStyle = new GUIStyle(GUI.skin.textField);

        // Set the GUI Style properties for the input selected label.
        labelStyle.fontSize = 16;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.alignment = TextAnchor.UpperCenter;

        // Render the selected input label.
        GUI.Label(
            new Rect(
                (float)((Screen.width / 2) + 80),
                (float)((Screen.height / 2) - (125 - (inputHeaderTextHeight + 10))),
                this.inputHeaderTextWidth,
                this.inputHeaderTextHeight
            ),
            inputControllers[this.selectedInputController],
            labelStyle
        );
        
        // Create a new GUI Style object based on the Button component.
        labelStyle = new GUIStyle(GUI.skin.button);

        // Set the GUI Style properties for the input selection buttons.
        labelStyle.fontSize  = 16;
        labelStyle.fontStyle = FontStyle.Bold;

        // Render the input selection buttons.
        this.selectedInputController = GUI.SelectionGrid(
            new Rect(
                (float)((Screen.width / 2) + 80),
                (float)((Screen.height / 2) - (125 - ((inputHeaderTextHeight * 2) + 20))),
                this.inputButtonsWidth,
                this.inputButtonsHeight
            ),
            this.selectedInputController,
            inputControllers,
            1,
            labelStyle
        );

        // Tell the Player object which input controller that has been selected.
        Player.SelectedInputController = this.selectedInputController;
    }

    /// <summary>
    /// Renders the instructions text.
    /// </summary>
    private void RenderInstructions()
    {
        // Create a new GUI Style object based on the Label component.
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);

        // Set the GUI Style properties for the instructions text.
        labelStyle.fontSize  = 18;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.padding   = new RectOffset(20, 20, 20, 20);
        labelStyle.alignment = TextAnchor.UpperLeft;

        // Render the instructions text.
        GUI.Label(
            new Rect(
                (float)((Screen.width / 2) - (this.borderWidth / 2)),
                (float)((Screen.height / 2) - 150),
                this.instructionsTextWidth,
                this.instructionsTextHeight
            ),
            "Keyboard Instructions\n\n" +
            "Move:\t\tWASD or Arrow keys\n" +
            "Fire projectile:\tSPACEBAR\n\n" +
            "XBOX360 Gamepad Instructions\n\n" +
            "Move:\t\tLeft Analog Stick\n" +
            "Fire projectile:\tGreen A Button\n\n" +
            "Mouse Instructions\n\n" +
            "Move:\t\tMouse\n" +
            "Fire projectile:\tLEFT-CLICK",
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

        // Set the GUI Style properties for the start button.
        labelStyle.fontSize  = 18;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.padding   = new RectOffset(20, 20, 20, 20);
        labelStyle.alignment = TextAnchor.UpperCenter;

        // Render the start button.
        bool startButtonPressed = GUI.Button(
            new Rect(
                (float)((Screen.width / 2) + (80 + 25)),
                (float)((Screen.height / 2) + ((115 - 5) - this.mainButtonsHeight)),
                this.mainButtonsWidth,
                this.mainButtonsHeight
            ),
            @"START GAME",
            labelStyle
        );

        // Render the quit button.
        bool quitButtonPressed = GUI.Button(
            new Rect(
                (float)((Screen.width / 2) + (80 + 25)),
                (float)((Screen.height / 2) + 115),
                this.mainButtonsWidth,
                this.mainButtonsHeight
            ),
            @"QUIT",
            labelStyle
        );

        // Check if the start button has been pressed.
        if (startButtonPressed || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Load the first game level.
            Application.LoadLevel(@"Level_001");
        }
        else if (Input.GetButtonDown("Fire1") && !Input.GetMouseButtonDown(0))
        {
            // Tell the Player object to use the gamepad as the input controller.
            Player.SelectedInputController = Player.InputController.Gamepad;

            // Load the first game level.
            Application.LoadLevel(@"Level_001");
        }

        // Check if the main menu button has been pressed.
        if (quitButtonPressed || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire2"))
        {
            // Quit the game.
            Application.Quit();
        }
    }

}
