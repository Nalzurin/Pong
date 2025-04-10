using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float startingVelocity = 10f;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public BoxCollider2D boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector2 Direction = MatchManager.Instance.ScoreLeft >= MatchManager.Instance.ScoreRight ? Vector2.left : Vector2.right;
        rb.linearVelocity = (Direction + Vector2.down).normalized * startingVelocity;//new Vector2(Random.value, Random.value) * startingVelocity;
    }



}
