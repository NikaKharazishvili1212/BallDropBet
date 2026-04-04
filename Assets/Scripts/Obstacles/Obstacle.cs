using UnityEngine;

/// <summary> Base class for all obstacles. Exposes Randomize() for GameManager to call on each round start. </summary>
public abstract class Obstacle : MonoBehaviour
{
    public abstract void Randomize();
}