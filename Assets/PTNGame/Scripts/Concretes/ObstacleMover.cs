using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [Header("Move")] [SerializeField] private MovePosEnum movePosE;
    [SerializeField] private float localMinPos, localMaxPos;
    [SerializeField] [Range(0.01f, 20f)] private float moveSpeed = 1f;
    private float goPosValue;

    [Header("Rotate")] [SerializeField] private RotatePosEnum rotatePosE;
    [SerializeField] [Range(20f, 200f)] private float rotationSpeed = 20f;
    private Vector3 rotationPos;

    public Vector3 PlayerRotationPos { get; private set; }

    private void Start()
    {
        goPosValue = localMinPos;
        SetRotatorPos();
    }

    private void LateUpdate()
    {
        Rotator();
        Mover();
    }

    #region Rotator

    private void Rotator()
    {
        if (rotatePosE == RotatePosEnum.DoNotRotate) return;

        transform.Rotate(rotationPos, Time.deltaTime * rotationSpeed);
    }

    private void SetRotatorPos()
    {
        switch (rotatePosE)
        {
            case RotatePosEnum.X:
                rotationPos = Vector3.right;
                break;
            case RotatePosEnum.NegativeX:
                rotationPos = Vector3.left;
                break;
            case RotatePosEnum.Y:
                rotationPos = Vector3.up;
                break;
            case RotatePosEnum.NegativeY:
                rotationPos = Vector3.down;
                break;
            case RotatePosEnum.Z:
                rotationPos = Vector3.forward;
                PlayerRotationPos = Vector3.left * rotationSpeed / 20;
                break;
            case RotatePosEnum.NegativeZ:
                rotationPos = Vector3.back;
                PlayerRotationPos = Vector3.right * rotationSpeed / 20;
                break;
            case RotatePosEnum.DoNotRotate:
                rotationPos = Vector3.right;
                break;
            default:
                rotationPos = Vector3.right;
                break;
        }
    }

    #endregion

    #region Mover

    private void Mover()
    {
        if (movePosE == MovePosEnum.DoNotMove) return;

        var position = transform.localPosition;

        Vector3 goPos;
        switch (movePosE)
        {
            case MovePosEnum.X:
                goPos = new Vector3(goPosValue, position.y, position.z);
                CheckObstaclePos(position.x);
                break;
            case MovePosEnum.Y:
                goPos = new Vector3(position.x, goPosValue, position.z);
                CheckObstaclePos(position.y);

                break;
            case MovePosEnum.Z:
                goPos = new Vector3(position.x, position.y, goPosValue);
                CheckObstaclePos(position.z);
                break;
            default:
                goPos = position;
                break;
        }

        transform.localPosition = Vector3.MoveTowards(position, goPos, Time.deltaTime * moveSpeed);
    }

    private void CheckObstaclePos(float pos)
    {
        if (pos < localMinPos + 0.01f)
            goPosValue = localMaxPos;
        else if (pos > localMaxPos - 0.01f)
            goPosValue = localMinPos;
    }

    #endregion
}

internal enum RotatePosEnum
{
    DoNotRotate,
    X,
    Y,
    Z,
    NegativeX,
    NegativeY,
    NegativeZ
}

internal enum MovePosEnum
{
    DoNotMove,
    X,
    Y,
    Z
}