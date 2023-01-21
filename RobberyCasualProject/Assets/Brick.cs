using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField]
    internal Vector2Int[] _segmentsFromCenter;


    private GameObject _selectedObject;
    private readonly Vector3 _verticalOffset = Vector3.up * 0.3f;
    private readonly Plane _plane = new Plane(Vector3.up, Vector3.zero);

    [SerializeField, Header("Diagnostics")]
    internal bool _inGrid;

    private Ray getRay
    {
        get
        {
            Vector3 mousePosFar, mousePosNear;
            mousePosFar = mousePosNear = Input.mousePosition;
            mousePosFar.z = Camera.main.farClipPlane;
            mousePosNear.z = Camera.main.nearClipPlane;
            var worldFar = Camera.main.ScreenToWorldPoint(mousePosFar);
            var worldNear = Camera.main.ScreenToWorldPoint(mousePosNear);
            return new Ray(worldNear, worldFar - worldNear);
        }
    }

    private void OnMouseDrag()
    {
        if (_inGrid)
        {
            GridManager.Instance.ExtrudeFromGrid(this);
            _inGrid = false;
        }

       _plane.Raycast(getRay, out float enter);
        transform.position = getRay.GetPoint(enter) + _verticalOffset;

        Cursor.visible = false;
    }

    private void OnMouseUp()
    {
        _plane.Raycast(getRay, out float enter);
        int layer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        if(Physics.Raycast(getRay, out RaycastHit hit))
        {
            if (hit.collider.GetComponentInParent<GridManager>() != null)
                GridManager.Instance.TryPlaceBrick(hit.collider.gameObject, this);
        }
        else
        {
            transform.position = getRay.GetPoint(enter);
        }
        Cursor.visible = true;

        gameObject.layer = layer;
    }
}
