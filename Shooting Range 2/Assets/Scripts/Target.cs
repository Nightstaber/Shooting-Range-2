using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    // Health and destroyed prefab
    [SerializeField]
    float health = 50f;
    [SerializeField]
    GameObject destroyedVersion;
    [SerializeField]
    bool isBomb;

    ScoreTracker script;
    PopupTarget popTarget;

    // Destructible and score
    [SerializeField]
    bool destructible;
    [SerializeField]
    int points;

    private void Start()
    {
        // Set a reference to ScoreTracker
        script = FindObjectOfType<ScoreTracker>();
        if (gameObject.GetComponentInParent<PopupTarget>())
            popTarget = gameObject.GetComponentInParent<PopupTarget>();
        }


    // Take Damage script. 
    public void TakeDamage (float amount)
    {
        // If destructible, then let it take damage, and if it goes <= 0, it will die.
        if (destructible)
        {
            health -= amount;
            if (health <= 0f)
            {
                Die();

            }
        }

        // If the script is attached to a bomb, it will explode rather than Die()
        if (isBomb)
        {
            health -= amount;
            if (health <= 0f)
            gameObject.GetComponent<CustomBullet>().Explode();
        }

        // Set the scoretext
        if (popTarget && popTarget.GetRaised())
            script.ChangeScore(points);
        else
        if (!popTarget)
            script.ChangeScore(points);


    }
    // If there is a destroyed version of the prefab, it will instantiate and set that.
    void Die()
    {

        if (destroyedVersion != null)
        {

            GameObject newObject = Instantiate(destroyedVersion, transform.position, transform.rotation, destroyedVersion.transform.parent);
            Vector3 objectScale = gameObject.transform.localScale * 100;
            newObject.transform.localScale = new Vector3 (objectScale.x, objectScale.y, objectScale.z);

        }
        
        // Destroys the original gameObject
        Destroy(gameObject);
    }

}
