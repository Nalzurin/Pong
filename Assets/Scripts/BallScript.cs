using UnityEngine;
using UnityEngine.Rendering;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float startingVelocity = 10f;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public BoxCollider2D boxCollider;
    [SerializeField] public AudioSource audioSource;
    public Vector2 pitchRange = new Vector2(0.95f, 1.05f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        Vector2 Direction = MatchManager.Instance.ScoreLeft >= MatchManager.Instance.ScoreRight ? Vector2.left : Vector2.right;
        rb.linearVelocity = (Direction + Vector2.down).normalized * startingVelocity;//new Vector2(Random.value, Random.value) * startingVelocity;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (audioSource != null)
        {
            audioSource.Play();
            audioSource.pitch += 0.025f;
        }
    }



}
