using System;
using UnityEngine;

public class CrossResult : MonoBehaviour
{
    [SerializeField] private Vector3 firstVector;
    [SerializeField] private Vector3 secondVector;
    [SerializeField] private Vector3 thirdVector;
    [SerializeField] private double pyramidArea;
    [SerializeField] private float gizmoLength = 4.5f;

    private Vector3 _firstNormalized;
    private Vector3 _secondNormalized;
    private Vector3 _thirdNormalized;
    
    void Update()
    {
        secondVector = Quaternion.Euler(0f, 90f, 0f) * firstVector;
        thirdVector = Vector3.Cross(firstVector, secondVector);

        CalculateArea(firstVector, secondVector, thirdVector);
        
        pyramidArea = PyramidSurface(_firstNormalized, _secondNormalized, _thirdNormalized);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, firstVector * gizmoLength);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, secondVector * gizmoLength);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, thirdVector * gizmoLength);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Vector3.zero, _firstNormalized);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(Vector3.zero, _secondNormalized);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(Vector3.zero, _thirdNormalized);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_firstNormalized, _secondNormalized);
        Gizmos.DrawLine(_thirdNormalized, _secondNormalized);
        Gizmos.DrawLine(_thirdNormalized, _firstNormalized);

    }

    private void CalculateArea(Vector3 a, Vector3 b, Vector3 c)
    {
        float distanceA = a.magnitude;
        float distanceB = b.magnitude;
        float distanceC = c.magnitude;

        float minDistance = Mathf.Min(distanceA, distanceB, distanceC);

        _secondNormalized = a.normalized * minDistance;
        _thirdNormalized = b.normalized * minDistance;
        _firstNormalized = c.normalized * minDistance;
    }
    
    double PyramidSurface(Vector3 vector1, Vector3 vector2, Vector3 vector3)
    {
        double totalSurface = TriangleArea(vector1, vector2) + TriangleArea(vector1, vector3) + TriangleArea(vector3, vector2);

        return totalSurface;
    }

    double TriangleArea(Vector3 vector1, Vector3 vector2)
    {
        //LARGO DE LA BASE DEL TRIANGULO
        double triangleBase = MathF.Sqrt(MathF.Pow(vector2.x - vector1.x, 2) + MathF.Pow(vector2.y - vector1.y, 2) + MathF.Pow(vector2.z - vector1.z, 2));
        
        //PUNTO MEDIO ENTRE LOS 2 VECTORES 
        Vector3 middlePoint;
        middlePoint.x = (vector1.x + vector2.x) / 2;
        middlePoint.y = (vector1.y + vector2.y) / 2;
        middlePoint.z = (vector1.z + vector2.z) / 2;
        
        //USANDO LA DISTANCIA DEL PUNTO MEDIO AL 0,0 PARA SABER LA ALTURA
        double triangleHeight = Pythagoras(middlePoint);
        
        //AREA DEL TRIANGULO
        return triangleBase * triangleHeight / 2;
    }

    double Pythagoras(Vector3 vector)
    {
        return MathF.Sqrt(MathF.Pow(vector.x, 2) + MathF.Pow(vector.y, 2) + MathF.Pow(vector.z, 2));
    }
}