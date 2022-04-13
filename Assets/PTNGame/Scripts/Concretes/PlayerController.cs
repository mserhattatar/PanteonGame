using System;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private Joystick joystick;

    private const float Speed = 5;
    private const float RotationSpeed = 120;

    private void Awake() => Init();

    private void FixedUpdate()
    {
        if (!CanMove)
            return;

        transform.Translate(Vector3.forward * Time.deltaTime * Speed * joystick.Vertical);
        transform.Rotate(Vector3.up, Time.deltaTime * RotationSpeed * joystick.Horizontal);
    }

    protected override float SetRunAniSpeed()
    {
        return joystick.Vertical != 0 ? Math.Abs(joystick.Vertical) : Math.Abs(joystick.Horizontal);
    }

    protected override void OnFinishLine()
    {
        GameManager.FinisLineAction?.Invoke();
    }
}