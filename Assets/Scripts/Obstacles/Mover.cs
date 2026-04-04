using UnityEngine;

/// <summary> Obstacle. Moves left/right; reverses on non-Ball obstacle. </summary>
public class Mover : Obstacle
{
    [SerializeField] float moveXSpeed;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(moveXSpeed, 0, 0), out hit, GameConstants.MoverTouchDistance))
            if (!hit.collider.CompareTag("Ball"))
                moveXSpeed = -moveXSpeed;
                
        transform.position += new Vector3(moveXSpeed, 0, 0) * Time.deltaTime;
    }

    // Called by GameManager on every game start to randomize moveXSpeed
    public override void Randomize()
    {
        moveXSpeed = GameConstants.RandomMoveXSpeed;
        moveXSpeed = Random.Range(1, 3) == 1 ? moveXSpeed : -moveXSpeed; // 50-50% chance to move either right or left
    }
}