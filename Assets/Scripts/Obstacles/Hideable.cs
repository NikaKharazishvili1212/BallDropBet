/// <summary> Obstacle. Toggles visibility at random intervals. </summary>
public class Hideable : Obstacle
{
    void ToggleVisibility() => gameObject.SetActive(!gameObject.activeSelf);

    // Called by GameManager on every game start to randomize delayTimer
    public override void Randomize()
    {
        CancelInvoke(nameof(ToggleVisibility));
        float delayTimer = GameConstants.RandomHideableDelayTime;
        InvokeRepeating(nameof(ToggleVisibility), delayTimer, delayTimer);
    }
}