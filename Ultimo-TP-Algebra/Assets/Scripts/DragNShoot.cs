using UnityEngine;

public class DragNShoot : MonoBehaviour
{
   public float power = 10f;
   
   private Ball ball;
   public Vector2 minPower;
   public Vector2 maxPower;
   private LineTrajectory lt;

   private Camera cam;
   private Vector2 force;
   private Vector3 startPoint;
   private Vector3 endPoint;

   private void Start()
   {
       cam = Camera.main;
       lt = GetComponent<LineTrajectory>();
       ball = GetComponent<Ball>();
   }

   private void Update()
   {
       if (Input.GetMouseButtonDown(0))
       {
           startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
           startPoint.z = 15;
           Debug.Log(startPoint);
       }

       if (Input.GetMouseButton(0))
       {
           Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
           currentPoint.z = 15f;
           lt.RenderLine(startPoint,currentPoint);
       }
       if (Input.GetMouseButtonUp(0))
       {
           endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
           endPoint.z = 15;

           force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
               Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));

           ball.AddImpulse(force,power);

           lt.EndLine();
       }
   }
}
