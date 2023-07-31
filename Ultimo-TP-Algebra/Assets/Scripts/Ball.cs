using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball")]
    [SerializeField] private float radius;
    [SerializeField] private float drag;
    [SerializeField] private Vector2 initVel;
    [SerializeField] private float mass;
    [SerializeField] private readonly float tableFriction = 0.2f;

    [Header("Map")]
    [SerializeField] private Transform topLeft;
    [SerializeField] private Transform lowRight;
    
    private Vector3 vel;

    void Start()
    {
        vel = new Vector3(initVel.x, initVel.y, 0);
        drag = tableFriction * (mass * 9.8f); ;
    }

    void Update()
    {
        MapCollide();
        Move();
        Drag();
    }

    public void AddImpulse(Vector2 dir, float force)
    {
        vel += new Vector3(dir.x, dir.y, 0) * force;
    }

    void Drag()
    {
        vel.x -= vel.x * Time.deltaTime* drag;
        vel.y -= vel.y * Time.deltaTime* drag;
     
    }

    void UpdateSize()
    {
        Vector2 newScale = new Vector2(radius * 2, radius * 2);
        transform.localScale = newScale;
    }

    void Move()
    {
        transform.position += vel * Time.deltaTime;
    }

    void MapCollide()
    {
        Vector3 pos = transform.position;
        if (pos.x - radius <= topLeft.position.x)
        {
            vel.x *= -1;
            pos = new Vector3(topLeft.position.x + radius, pos.y, 0);
        }

        if (pos.x + radius >= lowRight.position.x)
        {
            vel.x *= -1;
            pos = new Vector3(lowRight.position.x - radius, pos.y, 0);
        }

        if (pos.y + radius >= topLeft.position.y)
        {
            vel.y *= -1;
            pos = new Vector3(pos.x, topLeft.position.y - radius, 0);
        }

        if (pos.y - radius <= lowRight.position.y)
        {
            vel.y *= -1;
            pos = new Vector3(pos.x, lowRight.position.y + radius, 0);
        }
    }

    public Vector2 GetCenter()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public float GetRadius()
    {
        return radius;
    }

    public Vector2 GetVelocity()
    {
        return new Vector2(vel.x, vel.y);
    }

    public void SetVelocity(Vector2 newVel)
    {
        vel = new Vector3(newVel.x, newVel.y, 0);
    }

    public void SetPosition(Vector2 newPos)
    {
        transform.position = newPos;
    }

    public float GetMass()
    {
        return mass;
    }

    public void SetDrag(float drag)
    {
        this.drag = drag;
    }

    public void SetRadius(float newRadius)
    {
        radius = newRadius;
    }

    void OnDrawGizmos()
    {
        UpdateSize();
    }
}