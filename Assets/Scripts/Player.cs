using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    /// <summary>
    /// Game/Player States.
    /// </summary>
    private enum State
    {
        Dead,
        Invincible,
        Playing
    }

    /// <summary>
    /// Input Controllers.
    /// </summary>
    public struct InputController
    {
        public const int Keyboard = 0;
        public const int Gamepad  = 1;
        public const int Mouse    = 2;
    }

    /// <summary>
    /// Public properties.
    /// </summary>
    public GameObject     ExplosionPrefab;
    public GameObject     ProjectilePrefab;
    public static int     Lives                   = 0;
    public static int     Missed                  = 0;
    public static int     Score                   = 0;
    public static bool    OutsideViewPortLeft     = false;
    public static bool    OutsideViewPortRight    = false;
    public static bool    OutsideViewPortTop      = false;
    public static bool    OutsideViewPortBottom   = false;
    public static Vector3 MouseWorldSpacePosition = new Vector3(0, 0, 0);
    public static bool    MouseOutsideViewPort    = false;
    public static int     SelectedInputController = InputController.Keyboard;

    /// <summary>
    /// Private properties.
    /// </summary>
    private float playerRecoverBlinkCount  = 0;
    private float playerRecoverBlinkRate   = 0;
    private float playerRecoverBlinkTotal  = 0;
    private float playerRecoverSlideSpeed  = 0;
    private float playerRecoverTime        = 0;
    private float projectileOffset         = 0;
    private float speed                    = 0;
    private static float statusLabelLeft   = 0;
    private static float statusLabelTop    = 0;
    private static float statusLabelWidth  = 0;
    private static float statusLabelHeight = 0;
    private State state;

    /// <summary>
    /// Renders to the GUI at every frame update.
    /// </summary>
    private void OnGUI()
    {
        // Render the status label to the screen.
        Player.RenderStatusLabel();
    }

    /// <summary>
    /// Triggers when another game object collides with the player.
    /// </summary>
    /// <param name="otherObject"></param>
    private void OnTriggerEnter(Collider otherObject)
    {
        // We cannot be killed if we are invincible or already killed.
        if (this.state == State.Playing)
        {
            // Check if the enemy hit the player.
            if (otherObject.gameObject.tag == @"enemy")
            {
                // Decrease the player lives.
                Player.Lives--;

                // Access the Enemy object so we can access its public properties and methods.
                Enemy enemyObject = otherObject.gameObject.GetComponent<Enemy>();

                // Reposition the enemy.
                enemyObject.InitializeEnemy();

                // Destroy the player.
                StartCoroutine(this.DestroyPlayer());
            }
        }
    }

    /// <summary>
    /// Initialization.
    /// </summary>
    private void Start() 
    {
        // Initialize public properties.
        Player.Missed = 0;
        Player.Lives  = 10;
        Player.Score  = 0;

        // Initialize private properties.
        this.playerRecoverBlinkRate  = 0.1f;
        this.playerRecoverBlinkTotal = 10.0f;
        this.playerRecoverSlideSpeed = 5.0f;
        this.playerRecoverTime       = 1.5f;
        this.projectileOffset        = 1.2f;
        Player.statusLabelLeft       = 10.0f;
        Player.statusLabelTop        = 10.0f;
        Player.statusLabelWidth      = 140.0f;
        Player.statusLabelHeight     = 85.0f;
        this.state                   = State.Playing;

        // Initialize the player.
        this.InitializePlayer();
	}

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update() 
    {
        // Only respond to user input while the player is alive.
        if ((this.state == State.Playing) || (this.state == State.Invincible))
        {
            // Move the player based on user input.
            this.MovePlayerByUserInput();

            // Fire the projectile.
            this.FireProjectile();

            // Check if the user pressed the ESCAPE key.
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire2"))
            {
                // Load the main menu.
                Application.LoadLevel(@"MainMenu_001");
            }
        }
	}

    /// <summary>
    /// Keeps blinking the player object while invincible.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlinkPlayer()
    {
        // Keep blinking the player object while invincible.
        while (this.playerRecoverBlinkCount < this.playerRecoverBlinkTotal)
        {
            // Toggle between hiding and rendering the player object.
            if (gameObject.renderer.enabled)
            {
                gameObject.renderer.enabled = false;

                // Increase the blink counter.
                this.playerRecoverBlinkCount++;
            }
            else
            {
                gameObject.renderer.enabled = true;
            }

            // Wait before continuing.
            yield return new WaitForSeconds(this.playerRecoverBlinkRate);
        }

        // Reset the blink counter.
        this.playerRecoverBlinkCount = 0;

        // Render the player back to the screen.
        gameObject.renderer.enabled = true;

        // Set the player state to playing.
        this.state = State.Playing;
    }

    /// <summary>
    /// Kills and explodes the player.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyPlayer()
    {
        // Set the player state to dead.
        this.state = State.Dead;

        // Make the player explode.
        Instantiate(this.ExplosionPrefab, gameObject.transform.position, Quaternion.identity);

        // Hide the player from the screen.
        gameObject.renderer.enabled = false;

        // Hide the player off-screen.
        this.HidePlayer();

        // Wait before continuing.
        yield return new WaitForSeconds(this.playerRecoverTime);

        // Check if the user still has some lives available.
        if (Player.Lives > 0)
        {
            // Render the player back to the screen.
            gameObject.renderer.enabled = true;

            // Set the player state to invincible.
            this.state = State.Invincible;

            // Slide the player upwards into the visible screen and make it invincible.
            StartCoroutine(this.SlidePlayerBackAfterDeath());

            // Keep blinking the player object while invincible.
            StartCoroutine(this.BlinkPlayer());
        }
        else
        {
            // Load the game over level.
            Application.LoadLevel(@"Lose_001");
        }
    }

    /// <summary>
    /// Fires the projectile.
    /// </summary>
    private void FireProjectile()
    {
        // Fire the projectile.
        switch (Player.SelectedInputController)
        {
            // Handle user input from the keyboard.
            case Player.InputController.Keyboard:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    this.FireProjectileExecute();
                }

                break;
            // Handle user input from the gamepad controller.
            case Player.InputController.Gamepad:
                if (Input.GetButtonDown(@"Fire1"))
                {
                    this.FireProjectileExecute();
                }

                break;
            // Handle user input from the mouse.
            case Player.InputController.Mouse:
                if (Input.GetMouseButtonDown(0))
                {
                    this.FireProjectileExecute();
                }

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Fires the projectile.
    /// </summary>
    private void FireProjectileExecute()
    {
        // Calculate the starting position of the projectile based on the player position.
        Vector3 playerPosition = new Vector3(
            gameObject.transform.position.x,
            (float)(gameObject.transform.position.y + this.projectileOffset),
            gameObject.transform.position.z
        );

        // Fire the projectile from the position of the player.
        Instantiate(this.ProjectilePrefab, playerPosition, Quaternion.identity);
    }

    /// <summary>
    // Hides the player outside the visible screen.
    /// </summary>
    /// <param name="position"></param>
    private void HidePlayer()
    {
        // Set the hiding position of the player.
        gameObject.transform.position = new Vector3(0, -5.1f, 0);
    }

    /// <summary>
    // Repositions the player back to the initial starting position.
    /// </summary>
    /// <param name="position"></param>
    private void InitializePlayer()
    {
        // Set the initial player speed.
        this.speed = 10.0f;
        
        // Set the initial position of the player.
        gameObject.transform.position = new Vector3(0, -3.3f, 0);
    }

    /// <summary>
    /// Handles user input from a gamepad controller.
    /// </summary>
    private void ManageGamepadInputController(float moveDistance)
    {
        // Move the player further left only as long as the player has not reached the outer left edge of the screen.
        if (!Player.OutsideViewPortLeft && (Input.GetAxis(@"Horizontal") < 0))
        {
            gameObject.transform.Translate(Input.GetAxis(@"Horizontal") * Vector3.right * moveDistance, Space.World);
        }

        // Move the player further right only as long as the player has not reached the outer right edge of the screen.
        if (!Player.OutsideViewPortRight && (Input.GetAxis(@"Horizontal") > 0))
        {
            gameObject.transform.Translate(Input.GetAxis(@"Horizontal") * Vector3.right * moveDistance, Space.World);
        }

        // Move the player further up only as long as the player has not reached the top edge of the screen.
        if (!Player.OutsideViewPortTop && (Input.GetAxis(@"Vertical") > 0))
        {
            gameObject.transform.Translate(Input.GetAxis(@"Vertical") * Vector3.up * moveDistance, Space.World);
        }

        // Move the player further down only as long as the player has not reached the bottom edge of the screen.
        if (!Player.OutsideViewPortBottom && (Input.GetAxis(@"Vertical") < 0))
        {
            gameObject.transform.Translate(Input.GetAxis(@"Vertical") * Vector3.up * moveDistance, Space.World);
        }

        // Allow the player to move if the player moves in any other direction than left.
        if (Player.OutsideViewPortLeft && (Input.GetAxis(@"Horizontal") > 0))
        {
            Player.OutsideViewPortLeft = false;
        }

        // Allow the player to move if the player moves in any other direction than right.
        if (Player.OutsideViewPortRight && (Input.GetAxis(@"Horizontal") < 0))
        {
            Player.OutsideViewPortRight = false;
        }

        // Allow the player to move if the player moves in any other direction than up.
        if (Player.OutsideViewPortTop && (Input.GetAxis(@"Vertical") < 0))
        {
            Player.OutsideViewPortTop = false;
        }

        // Allow the player to move if the player moves in any other direction than down.
        if (Player.OutsideViewPortBottom && (Input.GetAxis(@"Vertical") > 0))
        {
            Player.OutsideViewPortBottom = false;
        }
    }

    /// <summary>
    /// Handles user input from the keyboard.
    /// </summary>
    private void ManageKeyboardInputController(float moveDistance)
    {
        // Move the player further left only as long as the player has not reached the outer left edge of the screen.
        if (!Player.OutsideViewPortLeft && 
            (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        )
        {
            // Move the player.
            gameObject.transform.Translate(Vector3.left * moveDistance, Space.World);
        }
        
        // Move the player further right only as long as the player has not reached the outer right edge of the screen.
        if (!Player.OutsideViewPortRight && 
            (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        )
        {
            // Move the player.
            gameObject.transform.Translate(Vector3.right * moveDistance, Space.World);
        }

        // Move the player further up only as long as the player has not reached the top edge of the screen.
        if (!Player.OutsideViewPortTop && 
            (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        )
        {
            // Move the player.
            gameObject.transform.Translate(Vector3.up * moveDistance, Space.World);
        }

        // Move the player further down only as long as the player has not reached the bottom edge of the screen.
        if (!Player.OutsideViewPortBottom && 
            (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        )
        {
            // Move the player.
            gameObject.transform.Translate(Vector3.down * moveDistance, Space.World);
        }

        // Allow the player to move if the player moves in any other direction than left.
        if (Player.OutsideViewPortLeft && 
            (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A))
        )
        {
            Player.OutsideViewPortLeft = false;
        }

        // Allow the player to move if the player moves in any other direction than right.
        if (Player.OutsideViewPortRight && 
            (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D))
        )
        {
            Player.OutsideViewPortRight = false;
        }

        // Allow the player to move if the player moves in any other direction than up.
        if (Player.OutsideViewPortTop && 
            (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W))
        )
        {
            Player.OutsideViewPortTop = false;
        }

        // Allow the player to move if the player moves in any other direction than down.
        if (Player.OutsideViewPortBottom && 
            (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S))
        )
        {
            Player.OutsideViewPortBottom = false;
        }
    }

    /// <summary>
    /// Handles user input from the mouse.
    /// </summary>
    private void ManageMouseInputController()
    {
        // Only move the player as long as the mouse cursor is within the visible screen space.
        if (!Player.MouseOutsideViewPort)
        {
            // Move the player.
            gameObject.transform.position = new Vector3(Player.MouseWorldSpacePosition.x, Player.MouseWorldSpacePosition.y, gameObject.transform.position.z);
        }
    }

    /// <summary>
    /// Moves the player based on user input.
    /// </summary>
    private void MovePlayerByUserInput()
    {
        // Main variables.
        float moveDistance = 0;

        // Set the distance the player should move.
        moveDistance = this.speed * Time.deltaTime;

        // Handle user input based on the selected input controller.
        switch (Player.SelectedInputController)
        {
            // Handle user input from the keyboard.
            case Player.InputController.Keyboard:
                this.ManageKeyboardInputController(moveDistance);
                break;
            // Handle user input from the gamepad controller.
            case Player.InputController.Gamepad:
                this.ManageGamepadInputController(moveDistance);
                break;
            // Handle user input from the mouse.
            case Player.InputController.Mouse:
                this.ManageMouseInputController();
                break;
            default:
                break;
        }
    }

    /// <summary>
    // Renders the status label to the screen.
    /// </summary>
    public static void RenderStatusLabel(float left = 0, float top = 0, float width = 0, float height = 0, int fontSize = 0)
    {
        // Set the status label area as a rectangle.
        Rect statusLabel;

        // Use default position and size values if none are given.
        if ((left == 0) && (top == 0) && (width == 0) && (height == 0))
        {
            statusLabel = new Rect(Player.statusLabelLeft, Player.statusLabelTop, Player.statusLabelWidth, Player.statusLabelHeight);
        }
        // Otherwise use the provided values.
        else
        {
            statusLabel = new Rect(left, top, width, height);
        }

        // Set the status label content.
        string statusContent =
            "Score:\t" + Player.Score.ToString() + "/1000\n" +
            "Lives:\t" + Player.Lives.ToString() + "\n" + 
            "Missed:\t" + Player.Missed.ToString();

        // Create a new GUI Style object based on the Label component.
        GUIStyle labelStyle = new GUIStyle(GUI.skin.textArea);

        // Set the GUI Style properties for the status label.
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.padding   = new RectOffset(20, 20, 20, 20);
        labelStyle.alignment = TextAnchor.MiddleLeft;

        // Set the Font size.
        if (fontSize == 0)
        {
            labelStyle.fontSize = 11;
        }
        else
        {
            labelStyle.fontSize = fontSize;
        }
        
        // Render the status label to the GUI.
        GUI.Label(statusLabel, statusContent, labelStyle);
    }

    /// <summary>
    /// Repositions the player to the provided position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void RepositionPlayer(Vector3 position)
    {
        // Repositions the player to the provided position.
        gameObject.transform.position = position;
    }

    /// <summary>
    /// Slides the player upwards into the visible screen and make it invincible.
    /// </summary>
    private IEnumerator SlidePlayerBackAfterDeath()
    {
        // Slide the player upwards into the visible screen.
        while (gameObject.transform.position.y < -3.3f)
        {
            // Set the distance the player should move at each frame.
            float moveDistance = this.playerRecoverSlideSpeed * Time.deltaTime;

            // Slide the player upwards into the visible screen.
            gameObject.transform.position = new Vector3(0, (float)(gameObject.transform.position.y + moveDistance), 0);

            // Wait until the next fixed frame rate update.
            yield return new WaitForFixedUpdate();
        }
    }

}
