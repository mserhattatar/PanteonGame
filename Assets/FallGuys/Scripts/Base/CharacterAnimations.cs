using UnityEngine;

public class CharacterAnimations
{
    private readonly Animator cAnimator;
    private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
    private static readonly int Falling = Animator.StringToHash("Falling");
    private static readonly int FallingDown = Animator.StringToHash("FallingDown");
    private static readonly int StandingUp = Animator.StringToHash("StandingUp");
    private float lastRunSpeed;

    protected internal CharacterAnimations(Animator pAnimator)
    {
        cAnimator = pAnimator;
    }

    protected internal void SetRun(float runSpeed)
    {
        //dont use system tolerance or int
        if (runSpeed == lastRunSpeed)
            return;
        lastRunSpeed = runSpeed;
        cAnimator.SetFloat(RunSpeed, runSpeed);
    }

    /// <summary>
    /// Fall in to the space from ground
    /// </summary>
    protected internal void SetFalling(bool falling)
    {
        cAnimator.SetBool(Falling, falling);
        SetStandingUp(!falling);
    }

    /// <summary>
    /// fall in to the ground after hitting obstacles
    /// </summary>
    protected internal void SetFallingDown(bool fallingDown)
    {
        cAnimator.SetBool(FallingDown, fallingDown);
    }

    /// <summary>
    /// stand up after falling to space or falling down
    /// </summary>
    protected internal void SetStandingUp(bool standing)
    {
        cAnimator.SetBool(StandingUp, standing);
    }
}