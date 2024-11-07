using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [System.Serializable]
    public class RoomCamera
    {
        public int roomNumber;  // 改为int类型
        public Camera camera;
        public Vector2 roomBounds;
        public Transform cameraPosition;
    }

    public RoomCamera[] roomCameras;
    private Camera currentActiveCamera;
    
    [Header("Transition Settings")]
    public float transitionDuration = 0.5f;
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        foreach (var room in roomCameras)
        {
            room.camera.gameObject.SetActive(false);
        }
        if (roomCameras.Length > 0)
        {
            currentActiveCamera = roomCameras[0].camera;
            currentActiveCamera.gameObject.SetActive(true);
        }
    }

    public void SwitchToRoom(int roomNumber)  // 改为接收int类型
    {
        var targetRoom = System.Array.Find(roomCameras, room => room.roomNumber == roomNumber);
        if (targetRoom != null && targetRoom.camera != currentActiveCamera)
        {
            StartCoroutine(TransitionToRoom(targetRoom.camera));
        }
    }

    private IEnumerator TransitionToRoom(Camera targetCamera)
    {
        float elapsedTime = 0;
        Camera prevCamera = currentActiveCamera;
        
        targetCamera.gameObject.SetActive(true);
        
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = transitionCurve.Evaluate(elapsedTime / transitionDuration);
            yield return null;
        }
        
        if (prevCamera != null)
            prevCamera.gameObject.SetActive(false);
            
        currentActiveCamera = targetCamera;
    }
}