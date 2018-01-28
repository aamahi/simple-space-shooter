using UnityEngine;

public class Stars : MonoBehaviour
{
    /// <summary>
    /// Public properties.
    /// </summary>
    public static float Speed = 0;

    /// <summary>
    /// Private properties.
    /// </summary>
    private float screenTop    = 0;
    private float screenBottom = 0;

    /// <summary>
    /// Initialization.
    /// </summary>
    private void Start()
    {
        // Initialize public properties.
        Stars.Speed = 1;

        // Initialize private properties.
        this.screenTop    = 20.0f;
        this.screenBottom = -14.0f;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Set the distance the stars should move at each frame.
        float moveDistance = Stars.Speed * Time.deltaTime;

        // Animate the stars.
        gameObject.transform.Translate(Vector3.down * moveDistance, Space.World);

        // Reposition the stars back to the top after it has exited the screen at the bottom.
        if (gameObject.transform.position.y < this.screenBottom)
        {
            // Reposition the stars.
            this.RepositionStars();
        }

    }

    /// <summary>
    // Repositions the stars back to the top.
    /// </summary>
    /// <param name="position"></param>
    public void RepositionStars()
    {
        // Set the initial starting position of the stars.
        gameObject.transform.position = new Vector3(0, this.screenTop, 3.0f);
    }
}
