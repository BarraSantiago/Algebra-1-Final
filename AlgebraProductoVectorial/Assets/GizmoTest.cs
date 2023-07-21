using UnityEngine;

public class GizmoTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    struct Vector
    {
        Vector3 from;
        Vector3 to;
    }
    private void OnDrawGizmos()
    {
        Vector3 to = new Vector3(3, 3, 3);
        Gizmos.DrawLine(Vector3.zero, to);
    }
}
