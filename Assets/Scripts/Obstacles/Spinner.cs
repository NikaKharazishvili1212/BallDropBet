using UnityEngine;

/// <summary> Obstacle. Spins around Z-axis constantly. </summary>
public class Spinner : Obstacle
{
    [SerializeField] float spinSpeed;

    void Update() => transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

    // Called by GameManager on every game start to randomize spinSpeed
    public override void Randomize()
    {
        spinSpeed = GameConstants.RandomSpinnerSpeed;
        spinSpeed = Random.Range(1, 3) == 1 ? spinSpeed : -spinSpeed; // 50-50% chance to spin either right or left
    }
}