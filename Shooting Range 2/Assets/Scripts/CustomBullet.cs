using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    // Assignables
    Rigidbody rb;
    [SerializeField]
    GameObject explosion;
    [SerializeField]
    LayerMask whatIsEnemies;
    [SerializeField]
    AudioClip explosionClip;

    // Stats
    [Range(0f,1f)]
    float bounciness;
    [SerializeField]
    bool useGravity;

    // Damage   
    [SerializeField]
    int explosionDamage;
    [SerializeField]
    float explosionRange;
    [SerializeField]
    float explosionForce;

    // Lifetime
    [SerializeField]
    int maxCollisions;
    [SerializeField]
    float maxLifetime;
    bool explodeOnTouch = true;

    // Bools
    bool exploded = false;

    int collisions = 0;
    PhysicMaterial physics_mat;

    private void Start()
    {
        // Run a setup script on start
        Setup();
    }

    private void Update()
    {

        // Explosion by time
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();

        // If gameobject has exploded but is not destroyed
        if (exploded)
            Destroy(gameObject, 0.05f);
    }

    public void Explode()
    {
        // Check if there is an explosion to instantiate and it has not already exploded

        if (explosion != null && !exploded)
        {
            GameObject go = Instantiate(explosion, transform.position, Quaternion.identity);
            exploded = true;

            // If there is an audiosource, play clip at the explosion site
            if (gameObject.GetComponent<AudioSource>())
            {
                AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);

            }
            // Check for enemies to damage
            Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange);
            for (int i = 0; i < enemies.Length; i++)
            {
                // Get component of enemy and call TakeDamage
                if (enemies[i].GetComponent<Target>())
                    enemies[i].GetComponent<Target>().TakeDamage(explosionDamage);
            }
            // Check for enemies to move
            Collider[] enemiesToMove = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
            for (int i = 0; i < enemiesToMove.Length; i++)
            {

                // Add explosion force if enemy has a rigidbody
                if (enemies[i].GetComponent<Rigidbody>())
                    enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);

            }
            
            // BUG: If going VERY close, the bullet might ricochet, before exploding.
            Destroy(gameObject, 0.05f);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Count up collisions
        collisions++;

        // Explode if bullet hits an enemy directly and ExplodeOnTouch is enabled
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();

        // Explosion by amount of collisions
        if (collisions > maxCollisions && !exploded) Explode();
    }

    private void Setup()
    {
        // Set the Rigidbody component
        rb = gameObject.GetComponent<Rigidbody>();
        // Set number of collisions to 0
        collisions = 0;
        // Create a new Physic material with low friction and high bounce value
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        // Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        // Set gravity
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw explosion range in red color, when object is selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
