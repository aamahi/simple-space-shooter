using UnityEngine;

public class Projectile : MonoBehaviour 
{
    /// <summary>
    /// Public properties.
    /// </summary>
    public GameObject ExplosionPrefab;

    /// <summary>
    /// Private properties.
    /// </summary>
    private Enemy enemyObject;
    private float screenTop = 0;
    private float speed     = 0;

    /// <summary>
    /// Triggers when another game object collides with the projectile.
    /// </summary>
    /// <param name="otherObject"></param>
    private void OnTriggerEnter(Collider otherObject)
    {
        // Check if the projectile hit the enemy.
        if (otherObject.gameObject.tag == @"enemy")
        {
            // Make the enemy explode.
            Instantiate(this.ExplosionPrefab, this.enemyObject.gameObject.transform.position, this.enemyObject.gameObject.transform.rotation);

            // Increase the enemy speed.
            this.enemyObject.MinSpeed += 0.1f;
            this.enemyObject.MaxSpeed += 0.2f;

            // Reposition the enemy.
            this.enemyObject.InitializeEnemy();

            // Destroy the projectile after it has hit the enemy.
            Destroy(gameObject);

            // Increase the player score.
            Player.Score += 10;

            // Check if the player has completed the level.
            if (Player.Score >= 1000)
            {
                Application.LoadLevel(@"Win_001");
            }
        }
    }

    /// <summary>
    /// Initialization.
    /// </summary>
    private void Start() 
    {
        // Initialize private properties.
        this.screenTop = 6.3f;

        // Set the initial projectile speed.
        this.speed = 10.0f;

        // Access the Enemy object so we can access its public properties and methods.
        this.enemyObject = GameObject.Find(@"Enemy").GetComponent<Enemy>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Set the distance the projectile should move at each frame.
        float moveDistance = this.speed * Time.deltaTime;

        // Animate the projectile.
        gameObject.transform.Translate(Vector3.up * moveDistance);

        // Destroy the projectile after it exits the screen at the top.
        if (gameObject.transform.position.y > this.screenTop)
        {
            Destroy(gameObject);
        }
    }
}
