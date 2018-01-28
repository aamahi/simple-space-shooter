using UnityEngine;

public class ParticleSystemDestroyer : MonoBehaviour
{
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void LateUpdate()
    {
        // Make sure the particle system attached to the game object is not empty.
        if (gameObject.particleSystem != null)
        {
            // Check if the particle system is done emitting particles and all particles are dead.
            if (!gameObject.particleSystem.IsAlive())
            {
                // Destroy remaining particles that have completed.
                Destroy(gameObject);
            }
        }
    }
}
