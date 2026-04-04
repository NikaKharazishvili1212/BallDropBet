using UnityEngine;

/// <summary> Game-wide constants and random values for obstacles. </summary>
public static class GameConstants
{
    public const int StartingLives = 3;

    public const string GroundTag = "Ground";
    public const float BallHitSoundCooldown = 0.1f; // Prevents multiple sounds from rapid micro-collisions
    public const float MinVelocityForSound = 4; // Minimum impact speed required to trigger collision sound
    public const float MoverTouchDistance = 1;

    // Base spawn point; a random offset is added per ball on each round start
    public static Vector3 TeleportPosition => new Vector3(0, 100, 0) + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
    public static float RandomHideableDelayTime => Random.Range(1f, 4f);
    public static float RandomMoveXSpeed => Random.Range(5f, 10f);
    public static float RandomRotatorSpeed => Random.Range(100f, 200f);
    public static float RandomSpinnerSpeed => Random.Range(100f, 500f);
}