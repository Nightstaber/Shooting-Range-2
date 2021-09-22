using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    // Assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    //public GameObject bulletHolePrefab;
    public AudioClip explosionClip;

    // Stats
    [Range(0f,1f)]
    public float bounciness;
    public bool useGravity;

    // Damage   
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    // Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    // Bools
    bool exploded = false;

    int collisions = 0;
    PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {

        // Explosion by time
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();

        if (exploded)
            Destroy(gameObject, 0.05f);
    }

    public void Explode()
    {
        if (explosion == null)
        {
            //GameObject go = Instantiate(bulletHolePrefab, transform.position, Quaternion.identity);
        }

        if (explosion != null && !exploded)
        {
            GameObject go = Instantiate(explosion, transform.position, Quaternion.identity);
            exploded = true;


            if (gameObject.GetComponent<AudioSource>())
            {
                Debug.Log("Audiosource found");
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

            Collider[] enemiesToMove = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
            for (int i = 0; i < enemiesToMove.Length; i++)
            {

                // Add explosion force if enemy has a rigidbody
                if (enemies[i].GetComponent<Rigidbody>())
                    enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);

            }

            /*

            // Destroy gameobject after short delay
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
                mr.enabled = false;
            TrailRenderer tr = gameObject.GetComponent<TrailRenderer>();
                tr.enabled = false;
            */
            //exploded = true;
            
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
        if (collisions > maxCollisions) Explode();
    }

    private void Setup()
    {
        collisions = 0;
        // Create a new Physic material
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
