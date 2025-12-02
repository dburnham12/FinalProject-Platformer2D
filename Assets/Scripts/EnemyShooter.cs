using UnityEngine;

public class EnemyShooter : MonoBehaviour
{

    [SerializeField] private GameObject shootPos;
    [SerializeField] private GameObject damagePellet;
    private float shootTimer = 0f;
    private float shootTime = 2f;
    private float playerRange = 10f;
    private GameObject player;

    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        shootTimer = shootTime;
    }

    // Update is called once per frame
    void Update()
    {
        bool distanceCheck = Vector2.Distance(player.transform.position, transform.position) < playerRange;
        bool positionCheck = (shootPos.transform.position.x < transform.position.x && player.transform.position.x < transform.position.x) ||
            (shootPos.transform.position.x > transform.position.x && player.transform.position.x > transform.position.x);

        if(distanceCheck && positionCheck)
        {
            if(shootTimer < shootTime)
            {
                shootTimer += Time.deltaTime;
            }
            else
            {
                animator.Play("Enemy_Shoot", -1, 0f);
                SoundManager.Instance.PlaySFX("SHOOT", .2f);
                Instantiate(damagePellet, shootPos.transform.position, Quaternion.identity);
                shootTimer = 0f;
            }
        }
        else
        {
            shootTimer = shootTime;
        }
    }
}
