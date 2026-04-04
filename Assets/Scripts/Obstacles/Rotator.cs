using UnityEngine;

/// <summary> Obstacle. Rotates around Z-axis constantly. </summary>
public class Rotator : Obstacle
{
    [SerializeField] float rotationSpeed;

    void Update() => transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

    // Called by GameManager on every game start to randomize rotationSpeed
    public override void Randomize()
    {
        rotationSpeed = GameConstants.RandomRotatorSpeed;
        rotationSpeed = Random.Range(1, 3) == 1 ? rotationSpeed : -rotationSpeed; // 50-50% chance to rotate either right or left
    }
}