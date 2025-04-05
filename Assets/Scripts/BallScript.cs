using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float startingVelocity = 10f;
    [SerializeField] private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = (Vector2.right + Vector2.up).normalized * startingVelocity;//new Vector2(Random.value, Random.value) * startingVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
