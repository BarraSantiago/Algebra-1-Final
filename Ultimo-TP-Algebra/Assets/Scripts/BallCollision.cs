using UnityEngine;

public class BallCollision : MonoBehaviour
{
    [SerializeField] private Ball[] balls;
    [SerializeField] private Ball[] holes;

    [SerializeField] private float drag;
    [SerializeField] private float radius;
    [SerializeField] private float whiteRadius;

    void Start()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].GetComponent<Ball>().SetDrag(drag);
            if (i == 0)
            {
                balls[i].GetComponent<Ball>().SetRadius(whiteRadius);
            }

            balls[i].GetComponent<Ball>().SetRadius(radius);
        }
    }

    void Update()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            if (!balls[i].gameObject.activeInHierarchy)
                continue;

            for (int j = i; j < balls.Length; j++)
            {
                if (!balls[j].gameObject.activeInHierarchy)
                    continue;

                if (j == i)
                    continue;

                if (BallsColliding(balls[i], balls[j]))
                    CollideBalls(balls[i], balls[j]);
            }

            for (int j = 0; j < holes.Length; j++)
            {
                if (PointBallCollision(holes[j], balls[i].GetCenter()))
                {
                    balls[i].gameObject.SetActive(false);

                    if (i == 0)
                    {
                        balls[i].SetPosition(new Vector2(-6, 0.03f));
                        balls[i].gameObject.SetActive(true);
                        balls[i].SetVelocity(Vector2.zero);
                    }
                }
            }
        }
    }

    // Verifica si dos bolas están colisionando
    // Compara la distancia entre los centros de las bolas con la suma de sus radios
    // Si la distancia es menor o igual a la suma de los radios, las bolas están colisionando
    private bool BallsColliding(Ball ball1, Ball ball2)
    {
        // Calcula la distancia entre los centros de las bolas usando.
        float distance =
            Mathf.Sqrt(
                Mathf.Pow(ball2.GetCenter().x - ball1.GetCenter().x, 2) +
                Mathf.Pow(ball2.GetCenter().y - ball1.GetCenter().y, 2));

        // Obtiene los radios de ambas bolas
        float ball1Radius = ball1.GetRadius();
        float ball2Radius = ball2.GetRadius();

        // Si la distancia entre las bolas es menor o igual a la suma de sus radios, ajusta la posición de ball2 y devuelve true
        if (distance <= ball1Radius + ball2Radius)
        {
            ball2.SetPosition(ball1.GetCenter() + (ball1Radius + ball2Radius) *
                ((ball2.GetCenter() - ball1.GetCenter()).normalized));

            return true;
        }

        return false;
    }

    // Ajusta las velocidades de dos bolas tras una colision
    // Utiliza la física de colisiones para simular cómo rebotan las bolas una contra otra
    private void CollideBalls(Ball ball1, Ball ball2)
    {
        // Calcula el vector entre los centros de las bolas
        Vector2 res = ball2.GetCenter() - ball1.GetCenter();

        // Calcula el vector tangente a la colisión
        Vector2 tan = new Vector2(-res.y, res.x);

        // Calcula los productos punto tangenciales y normales
        float tanDotProductBall1 = ball1.GetVelocity().x * tan.x + ball1.GetVelocity().y * tan.y;
        float tanDotProductBall2 = ball2.GetVelocity().x * tan.x + ball2.GetVelocity().y * tan.y;
        float normalDotProductBall1 = Vector2.Dot(ball1.GetVelocity(), res.normalized);
        float normalDotProductBall2 = Vector2.Dot(ball2.GetVelocity(), res.normalized);

        // Calcula el momento resultante para cada bola
        float momentum1 = (normalDotProductBall1 / (ball1.GetMass() * 2)) + normalDotProductBall2;
        float momentum2 = (normalDotProductBall2 / (ball2.GetMass() * 2)) + normalDotProductBall1;

        // Ajusta las velocidades de las bolas según los cálculos anteriores
        ball1.SetVelocity(tan * tanDotProductBall1 + res.normalized * momentum1);
        ball2.SetVelocity(tan * tanDotProductBall2 + res.normalized * momentum2);
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].GetComponent<Ball>().SetDrag(drag);
            balls[i].GetComponent<Ball>().SetRadius(radius);

            if (i == 0)
            {
                balls[i].GetComponent<Ball>().SetRadius(whiteRadius);
            }
        }
    }

    // Verifica si una bola está colisionando con un punto en el espacio
    // Compara la distancia entre el centro de la bola y el punto con el radio de la bola
    private bool PointBallCollision(Ball ball, Vector3 point)
    {
        // Calcula la distancia entre la posición de la bola y el punto
        float dis = Mathf.Abs((ball.transform.position - point).magnitude);

        // Si la distancia es menor o igual al radio de la bola, devuelve true, indicando una colisión
        return (dis <= ball.GetRadius());
    }
}