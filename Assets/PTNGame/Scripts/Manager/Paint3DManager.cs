using System.Collections.Generic;
using UnityEngine;

public class Paint3DManager : MonoBehaviour
{
    private bool startPaining;
    private List<Transform> createdBrushesList = new List<Transform>();

    // The camera that looks at the model, and the camera that looks at the canvas.
    [SerializeField] private Camera sceneCamera, canvasCam;

    [SerializeField] private GameObject brushContainer, objectToPainted;


    private void OnEnable() => GameManager.FinisLineAction += StartFinishWallPaint;

    private void OnDisable() => GameManager.FinisLineAction -= StartFinishWallPaint;

    private void LateUpdate()
    {
        if (startPaining)
            PaintWall();
    }

    private void StartFinishWallPaint()
    {
        objectToPainted.SetActive(true);
        startPaining = true;
    }


    // The main action, instantiates a brush or decal entity at the clicked position on the UV map
    private void PaintWall()
    {
        var uvWorldPosition = Vector3.zero;
        if (GetWallPosFromWorldPos(ref uvWorldPosition))
        {
            //TODO:create object pool 
            var brushObj = (GameObject) Instantiate(
                Resources.Load("BrushEntity"), brushContainer.transform, true);
            // The position of the brush (in the UVMap)
            brushObj.transform.localPosition = uvWorldPosition;
            createdBrushesList.Add(brushObj.transform);
        }

        if (createdBrushesList.Count <= 500)
            return;

        foreach (var brush in brushContainer.GetComponentsInChildren<Transform>())
        {
            if (brush.gameObject.GetInstanceID() != brushContainer.GetInstanceID())
            {
                Destroy(brush.gameObject);
            }
        }

        GameManager.Instance.StartNextLevel();
    }


    // Returns the position on the texuremap according to a hit in the mesh collider

    private bool GetWallPosFromWorldPos(ref Vector3 uvWorldPosition)
    {
        //TODO: change input mouse pos to touch pos
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX || UNITY_WEBGL
        Ray cursorRay = sceneCamera.ScreenPointToRay(Input.mousePosition);
#else
    Ray cursorRay = sceneCamera.ScreenPointToRay(Input.mousePosition);
#endif


        if (!Physics.Raycast(cursorRay, out var hit, 200f))
            return false;

        if (hit.collider == null || !hit.collider.gameObject.CompareTag("FinishWall"))
            return false;

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return false;

        Vector2 pixelUV = hit.textureCoord;

        var orthographicSize = canvasCam.orthographicSize;
        uvWorldPosition.x = pixelUV.x - orthographicSize; //To center the UV on X
        uvWorldPosition.y = pixelUV.y - orthographicSize; //To center the UV on Y
        uvWorldPosition.z = 0.0f;

        return true;
    }
}