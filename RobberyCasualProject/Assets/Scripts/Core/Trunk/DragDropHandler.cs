using UnityEngine;

public class DragDropHandler : MonoBehaviour
{
    protected Ray getRay
    {
        get
        {
            var camera = TrunkGrid.Instance._renderer;
            Vector3 mousePosFar, mousePosNear;
            mousePosFar = mousePosNear = Input.mousePosition;
            mousePosFar.z = camera.farClipPlane;
            mousePosNear.z = camera.nearClipPlane;
            var worldFar = camera.ScreenToWorldPoint(mousePosFar);
            var worldNear = camera.ScreenToWorldPoint(mousePosNear);
            return new Ray(worldNear, worldFar - worldNear);
        }
    }
}