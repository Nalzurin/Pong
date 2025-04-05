using UnityEngine;
using static Data;

public class GoalScript : MonoBehaviour
{
    [SerializeField] PlayerSide side;

    public void SetPlayerSide(PlayerSide _side)
    {
        side = _side;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(BALLTAG))
        {
           Destroy(collision.gameObject);
           MatchManager.Instance.Score(side);
        }
    }
}
