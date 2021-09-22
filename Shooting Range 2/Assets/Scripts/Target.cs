using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public GameObject destroyedVersion;
    public bool isBomb;

    ScoreTracker script;

    public bool destructible;
    public int points;

    private void Start()
    {

        script = FindObjectOfType<ScoreTracker>();

    }



    public void TakeDamage (float amount)
    {
        if (destructible)
        {
            health -= amount;
            if (health <= 0f)
            {
                Die();

            }
        }

        if (isBomb)
        {
            health -= amount;
            if (health <= 0f)
            gameObject.GetComponent<CustomBullet>().Explode();
        }
        script.ChangeScore(points);

    }

    void Die()
    {

        if (destroyedVersion != null)
        {

            GameObject newObject = Instantiate(destroyedVersion, transform.position, transform.rotation, destroyedVersion.transform.parent);
            Vector3 objectScale = gameObject.transform.localScale * 100;
            newObject.transform.localScale = new Vector3 (objectScale.x, objectScale.y, objectScale.z);

        }
        Destroy(gameObject);
    }

}
