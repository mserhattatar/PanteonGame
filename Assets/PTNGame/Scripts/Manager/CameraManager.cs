using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject thirdPersonCamera, paintedWallCamera;

    private void OnEnable() => GameManager.FinisLineAction += ChangeCamera;

    private void OnDisable() => GameManager.FinisLineAction -= ChangeCamera;

    /// <summary>
    /// Change Camera if player come to finish line
    /// </summary>
    private void ChangeCamera()
    {
        thirdPersonCamera.SetActive(!thirdPersonCamera.activeInHierarchy);
        paintedWallCamera.SetActive(!paintedWallCamera.activeInHierarchy);
    }
}