using UnityEngine;

public class MainCamera : MonoBehaviour
{
    /// <summary>
    /// Public properties.
    /// </summary>

    /// <summary>
    /// Private properties.
    /// </summary>
    private float playerOffsetWidth  = 0;
    private float playerOffsetHeight = 0;

    /// <summary>
    /// Initialization.
    /// </summary>
    private void Start()
    {
        this.playerOffsetWidth  = 0.08f;
        this.playerOffsetHeight = 0.08f;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Keep the player within the visible screen when using the mouse as an input controller.
        this.RepositionPlayerAndMouseInsideViewPort();

        // Keep the player within the visible screen.
        this.RepositionPlayerInsideViewPort();
    }

    /// <summary>
    /// Keeps the player within the visible screen.
    /// </summary>
    private void RepositionPlayerInsideViewPort()
    {
        // Main variables.
        Vector3 worldSpace = new Vector3(0, 0, 0);

        // Access the Player object so we can access its public properties and methods.
        Player playerObject = GameObject.Find(@"Player").GetComponent<Player>();

        // Get the players world space position relative to the cameras viewport space.
        Vector3 playerViewPortPosition = camera.WorldToViewportPoint(playerObject.transform.position);

        // Keep the player within the screen when moving too far left.
        if (playerViewPortPosition.x < (0 + this.playerOffsetWidth))
        {
            // Tell the Player object that it is outside of viewable screen space.
            Player.OutsideViewPortLeft = true;

            // Convert the viewspace position back to a world space position.
            worldSpace = camera.ViewportToWorldPoint(new Vector3((float)(0 + this.playerOffsetWidth), playerViewPortPosition.y, playerViewPortPosition.z));

            // Reposition the player back to a visible position.
            playerObject.RepositionPlayer(worldSpace);
        }

        // Keep the player within the screen when moving too far right.
        if (playerViewPortPosition.x > (1.0f - this.playerOffsetWidth))
        {
            // Tell the Player object that it is outside of viewable screen space.
            Player.OutsideViewPortRight = true;

            // Convert the viewspace position back to a world space position.
            worldSpace = camera.ViewportToWorldPoint(new Vector3((float)(1.0f - this.playerOffsetWidth), playerViewPortPosition.y, playerViewPortPosition.z));

            // Reposition the player back to a visible position.
            playerObject.RepositionPlayer(worldSpace);
        }

        // Keep the player within the screen when moving too far up.
        if (playerViewPortPosition.y > (1.0f - this.playerOffsetHeight))
        {
            // Tell the Player object that it is outside of viewable screen space.
            Player.OutsideViewPortTop = true;

            // Convert the viewspace position back to a world space position.
            worldSpace = camera.ViewportToWorldPoint(new Vector3(playerViewPortPosition.x, (float)(1.0f - this.playerOffsetHeight), playerViewPortPosition.z));

            // Reposition the player back to a visible position.
            playerObject.RepositionPlayer(worldSpace);
        }

        // Keep the player within the screen when moving too far down.
        if (playerViewPortPosition.y < (0 + this.playerOffsetHeight))
        {
            // Tell the Player object that it is outside of viewable screen space.
            Player.OutsideViewPortBottom = true;

            // Convert the viewspace position back to a world space position.
            worldSpace = camera.ViewportToWorldPoint(new Vector3(playerViewPortPosition.x, (float)(0 + this.playerOffsetHeight), playerViewPortPosition.z));

            // Reposition the player back to a visible position.
            playerObject.RepositionPlayer(worldSpace);
        }
    }

    /// <summary>
    /// Keeps the player within the visible screen when using the mouse as an input controller.
    /// </summary>
    private void RepositionPlayerAndMouseInsideViewPort()
    {
        // Tell the Player object where the mouse cursor is currently positioned.
        Player.MouseWorldSpacePosition = camera.ScreenToWorldPoint(Input.mousePosition);

        // Get the mouse cursor world space position relative to the cameras viewport space.
        Vector3 mouseCursorViewPortPosition = camera.WorldToViewportPoint(Player.MouseWorldSpacePosition);

        // Check if the mouse cursor is within the viewable screen.
        if ((mouseCursorViewPortPosition.x < 0) || (mouseCursorViewPortPosition.x > 1.0f) || (mouseCursorViewPortPosition.y < 0) || (mouseCursorViewPortPosition.y > 1.0f))
        {
            // Tell the Player object that the mouse cursor is outside of viewable screen space.
            Player.MouseOutsideViewPort = true;
        }
        else
        {
            // Tell the Player object that the mouse cursor back inside of viewable screen space.
            Player.MouseOutsideViewPort = false;
        }

        // Adjust the player position based on the offset for the left side and bottom part.
        if ((Player.MouseWorldSpacePosition.x < 0) || (Player.MouseWorldSpacePosition.y < 0))
        {
            Player.MouseWorldSpacePosition.x += this.playerOffsetWidth;
            Player.MouseWorldSpacePosition.y += this.playerOffsetHeight;
        }

        // Adjust the player position based on the offset for the right side and top part.
        if ((Player.MouseWorldSpacePosition.x > 0) || (Player.MouseWorldSpacePosition.y > 0))
        {
            Player.MouseWorldSpacePosition.x -= this.playerOffsetWidth;
            Player.MouseWorldSpacePosition.y -= this.playerOffsetHeight;
        }
    }
}
