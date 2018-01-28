using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Public properties.
    /// </summary>
    public float MinSpeed = 0;
    public float MaxSpeed = 0;

    /// <summary>
    /// Private properties.
    /// </summary>
    private float currentSpeed         = 0;
    private float currentRotationSpeed = 0;
    private float currentScaleX        = 0;
    private float currentScaleY        = 0;
    private float currentScaleZ        = 0;
    private float minRotationSpeed     = 0;
    private float maxRotationSpeed     = 0;
    private float minScale             = 0;
    private float maxScale             = 0;
    private float screenLeft           = 0;
    private float screenRight          = 0;
    private float screenTop            = 0;
    private float screenBottom         = 0;
    
    /// <summary>
    /// Initialization.
    /// </summary>
    private void Start()
    {
        // Initialize public properties.
        this.MinSpeed = 4.0f;
        this.MaxSpeed = 6.0f;

        // Initialize private properties.
        this.minRotationSpeed = 60.0f;
        this.maxRotationSpeed = 120.0f;
        this.minScale         = 1.0f;
        this.maxScale         = 5.0f;
        this.screenLeft       = -8.0f;
        this.screenRight      = 8.0f;
        this.screenTop        = 7.0f;
        this.screenBottom     = -5.0f;

        //Set the initial position of the enemy.
        this.InitializeEnemy();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Set the rotation speed the enemy should rotate at each frame.
        float rotationSpeed = this.currentRotationSpeed * Time.deltaTime;

        // Rotate the enemy downwards along the X-axis.
        gameObject.transform.Rotate(new Vector3(-1.0f, 0, 0) * rotationSpeed);

        // Set the distance the enemy should move at each frame.
        float moveDistance = this.currentSpeed * Time.deltaTime;

        // Animate the enemy.
        gameObject.transform.Translate(Vector3.down * moveDistance, Space.World);

        // Reposition the enemy back to the top after it has exited the screen at the bottom.
        if (gameObject.transform.position.y < this.screenBottom)
        {
            this.InitializeEnemy();

            // Increase the counter of missed enemies.
            Player.Missed++;
        }
    }

    /// <summary>
    // Repositions the enemy back to the top after it has exited the screen at the bottom.
    /// </summary>
    /// <param name="position"></param>
    public void InitializeEnemy()
    {
        // Randomly generate a rotation speed for the enemy.
        this.currentRotationSpeed = Random.Range(this.minRotationSpeed, this.maxRotationSpeed);

        // Randomly generate an X-scale for the enemy.
        this.currentScaleX = Random.Range(this.minScale, this.maxScale);

        // Randomly generate a Y-scale for the enemy.
        this.currentScaleY = Random.Range(this.minScale, this.maxScale);

        // Randomly generate a Z-scale for the enemy.
        this.currentScaleZ = Random.Range(this.minScale, this.maxScale);

        // Randomly generate a speed for the enemy.
        this.currentSpeed = Random.Range(this.MinSpeed, this.MaxSpeed);

        // Randomly generate an initial position of the enemy.
        gameObject.transform.position = new Vector3(Random.Range(this.screenLeft, this.screenRight), this.screenTop, 0);

        // Randomly generate an initial scale of the enemy.
        gameObject.transform.localScale = new Vector3(this.currentScaleX, this.currentScaleY, this.currentScaleZ);

    }
}
