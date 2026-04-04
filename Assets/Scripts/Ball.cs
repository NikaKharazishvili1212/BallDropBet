using UnityEngine;

/// <summary> Ball with physics, collision sounds and finish detection. </summary>
public class Ball : MonoBehaviour
{
    public enum BallColorType { Red, Cyan, Green, Purple, Yellow }
    [field: SerializeField] public BallColorType BallColor { get; private set; }
    [field: SerializeField] public int place { get; private set; }
    [SerializeField] GameManager gameManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitSound; // Ball hitting sound
    [SerializeField] Rigidbody rb;

    float lastSoundTime; // Timestamp of last played collision sound (used for cooldown)

    Vector3 startingPosition;

    void Awake() => startingPosition = transform.position;

    void OnCollisionEnter(Collision x)
    {
        // Ball hitting something - hitSound
        float relativeVelocity = x.relativeVelocity.magnitude;
        if (relativeVelocity >= GameConstants.MinVelocityForSound && Time.time - lastSoundTime > GameConstants.BallHitSoundCooldown)
        {
            audioSource.PlayOneShot(hitSound);
            lastSoundTime = Time.time;
        }

        // Ball hitting the bottom ground - finish
        if (x.gameObject.CompareTag(GameConstants.GroundTag))
        {
            transform.position = startingPosition;
            gameManager.Finished(this);
            place = gameManager.FinishersCount;
        }
    }

    // Called by GameManager on each round start. Moves the ball to the base spawn point with a random offset
    public void Teleport()
    {
        place = 0;
        transform.position = GameConstants.TeleportPosition;
    }

    // Returns Color for this ball's enum. Used for colored name in UI score text
    public Color GetColor()
    {
        return BallColor switch
        {
            BallColorType.Red => Color.red,
            BallColorType.Cyan => Color.cyan,
            BallColorType.Green => Color.green,
            BallColorType.Purple => Color.purple,
            BallColorType.Yellow => Color.yellow,
            _ => Color.white
        };
    }
}