// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ChestManager : MonoBehaviour
// {
//     [Tooltip("All possible crates where the chest can be hidden")]
//     public List<GameObject> crates = new List<GameObject>();
    
//     [Tooltip("Quidditch chest prefab")]
//     public GameObject quidditch_chest_prefab;
    
//     [Tooltip("Reference to the enemy spawner")]
//     public GameObject enemy_spawner;
    
//     private GameObject active_chest;
//     private int selected_crate_index;
    
//     // Do crate and chest check then Hide the chest
//     void Start()
//     {
//         // Return if crates less than 6
//         if (crates.Count < 6)
//         {
//             Debug.LogError("At least 6 crates are required! Please add enough crates in the Inspector.");
//             return;
//         }
        
//         // Return if chest does not exist
//         if (quidditch_chest_prefab == null)
//         {
//             Debug.LogError("Quidditch chest prefab not set!");
//             return;
//         }
        
//         // Hide the chest in a random crate
//         hide_chest_in_random_crate();
//     }
    
//     void hide_chest_in_random_crate()
//     {
//         // Select a random crate index
//         selected_crate_index = Random.Range(0, crates.Count);
//         GameObject selected_crate = crates[selected_crate_index];
        
//         // Log the selected crate for debugging
//         Debug.Log("Chest hidden in: " + selected_crate.name);
        
//         // Add a special component to the selected crate to respond to the Reducto spell
//         CrateController crate_controller = selected_crate.AddComponent<CrateController>();
//         crate_controller.contains_chest = true;
//         crate_controller.chest_prefab = quidditch_chest_prefab;
//         crate_controller.chest_manager = this;
        
//         // Add components to other crates as well, but without a chest
//         for (int i = 0; i < crates.Count; i++)
//         {
//             if (i != selected_crate_index)
//             {
//                 CrateController controller = crates[i].AddComponent<CrateController>();
//                 controller.contains_chest = false;
//             }
//         }
//     }
    
//     // Stop enemy spawning when the chest is opened
//     public void stop_enemy_spawning()
//     {
//         if (enemy_spawner != null)
//         {
//             EnemySpawner spawner = enemy_spawner.GetComponent<EnemySpawner>();
//             if (spawner != null)
//             {
//                 spawner.stop_spawning();
//             }
//         }
//     }
// }

// // Controls crate response to Reducto spell
// public class CrateController : MonoBehaviour
// {
//     public bool contains_chest = false;
//     public GameObject chest_prefab;
//     public ChestManager chest_manager;
    
//     // Crate pieces prefab for destruction effect
//     public GameObject crate_pieces_prefab;
    
//     // When the crate is hit by Reducto spell
//     public void on_reducto_hit()
//     {
//         // Spawn crate pieces for destruction effect
//         if (crate_pieces_prefab != null)
//         {
//             Instantiate(crate_pieces_prefab, transform.position, transform.rotation);
//         }
        
//         // If this crate contains the chest, reveal it
//         if (contains_chest && chest_prefab != null)
//         {
//             GameObject chest = Instantiate(chest_prefab, transform.position, Quaternion.identity);
//             QuidditchChest chest_controller = chest.AddComponent<QuidditchChest>();
//             chest_controller.chest_manager = chest_manager;
//         }
        
//         // Destroy the original crate
//         Destroy(gameObject);
//     }
    
//     // Detect spell collision
//     void OnTriggerEnter(Collider other)
//     {
//         // Check if it's a Reducto spell
//         if (other.CompareTag("Reducto"))
//         {
//             on_reducto_hit();
//             Destroy(other.gameObject); // Destroy the spell object
//         }
//     }
// }

// // Quidditch chest controller
// public class QuidditchChest : MonoBehaviour
// {
//     // Chest lid animation component
//     private Animator chest_animator;
    
//     // Chest opening sound
//     public AudioClip chest_open_sound;
//     private AudioSource audio_source;
    
//     // Ball prefabs
//     public GameObject quaffle_prefab;
//     public GameObject bludger_prefab;
//     public GameObject snitch_prefab;
    
//     // Ball spawn points
//     public Transform quaffle_spawn_point;
//     public Transform bludger_spawn_point_1;
//     public Transform bludger_spawn_point_2;
//     public Transform snitch_spawn_point;
    
//     // Reference to the chest manager
//     public ChestManager chest_manager;
    
//     // Whether the chest is already open
//     private bool is_open = false;
    
//     void Awake()
//     {
//         chest_animator = GetComponent<Animator>();
//         audio_source = gameObject.AddComponent<AudioSource>();
        
//         // If spawn points aren't found, use chest position
//         if (quaffle_spawn_point == null) quaffle_spawn_point = transform;
//         if (bludger_spawn_point_1 == null) bludger_spawn_point_1 = transform;
//         if (bludger_spawn_point_2 == null) bludger_spawn_point_2 = transform;
//         if (snitch_spawn_point == null) snitch_spawn_point = transform;
//     }
    
//     // Detect spell collision
//     void OnTriggerEnter(Collider other)
//     {
//         // Check if it's an Alohomora spell
//         if (!is_open && other.CompareTag("Alohomora"))
//         {
//             open_chest();
//             Destroy(other.gameObject); // Destroy the spell object
//         }
//     }
    
//     // Open the chest and release the balls
//     void open_chest()
//     {
//         is_open = true;
        
//         // Play animation
//         if (chest_animator != null)
//         {
//             chest_animator.SetTrigger("Open");
//         }
        
//         // Play sound
//         if (audio_source != null && chest_open_sound != null)
//         {
//             audio_source.clip = chest_open_sound;
//             audio_source.Play();
//         }
        
//         // Stop enemy spawning
//         if (chest_manager != null)
//         {
//             chest_manager.stop_enemy_spawning();
//         }
        
//         // Delay ball release to sync with animation
//         StartCoroutine(release_balls_after_delay(1.0f));
//     }
    
//     // Delay ball release
//     IEnumerator release_balls_after_delay(float delay)
//     {
//         yield return new WaitForSeconds(delay);
        
//         // Release Quaffle
//         if (quaffle_prefab != null)
//         {
//             GameObject quaffle = Instantiate(quaffle_prefab, quaffle_spawn_point.position, Quaternion.identity);
//             quaffle.AddComponent<QuaffleController>();
//         }
        
//         // Release two Bludgers
//         if (bludger_prefab != null)
//         {
//             GameObject bludger1 = Instantiate(bludger_prefab, bludger_spawn_point_1.position, Quaternion.identity);
//             GameObject bludger2 = Instantiate(bludger_prefab, bludger_spawn_point_2.position, Quaternion.identity);
//             bludger1.AddComponent<BludgerController>();
//             bludger2.AddComponent<BludgerController>();
//         }
        
//         // Release the Golden Snitch
//         if (snitch_prefab != null)
//         {
//             GameObject snitch = Instantiate(snitch_prefab, snitch_spawn_point.position, Quaternion.identity);
//             snitch.AddComponent<SnitchController>();
//         }
//     }
// }

// // Quaffle controller
// public class QuaffleController : MonoBehaviour
// {
//     public float speed = 10f;
//     public float max_height = 20f;
//     private Rigidbody rb;
    
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         if (rb == null)
//         {
//             rb = gameObject.AddComponent<Rigidbody>();
//         }
        
//         // Launch Quaffle upward
//         Vector3 launch_direction = Vector3.up + Random.insideUnitSphere * 0.3f;
//         rb.AddForce(launch_direction * speed, ForceMode.Impulse);
//     }
    
//     void Update()
//     {
//         // If flying too high, start falling
//         if (transform.position.y > max_height)
//         {
//             rb.linearVelocity = new Vector3(rb.linearVelocity.x, -rb.linearVelocity.y * 0.5f, rb.linearVelocity.z);
//         }
//     }
// }

// // Bludger controller
// public class BludgerController : MonoBehaviour
// {
//     public float speed = 15f;
//     public float detection_radius = 20f;
//     private Rigidbody rb;
//     private Transform target;
    
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         if (rb == null)
//         {
//             rb = gameObject.AddComponent<Rigidbody>();
//         }
        
//         // Initial random movement
//         Vector3 random_direction = Random.onUnitSphere;
//         rb.AddForce(random_direction * speed, ForceMode.Impulse);
        
//         // Start looking for targets
//         StartCoroutine(find_target_routine());
//     }
    
//     IEnumerator find_target_routine()
//     {
//         while (true)
//         {
//             find_closest_dementor();
//             yield return new WaitForSeconds(1.0f);
//         }
//     }
    
//     void find_closest_dementor()
//     {
//         GameObject[] dementors = GameObject.FindGameObjectsWithTag("Dementor");
//         float closest_distance = float.MaxValue;
        
//         foreach (GameObject dementor in dementors)
//         {
//             float distance = Vector3.Distance(transform.position, dementor.transform.position);
//             if (distance < closest_distance && distance < detection_radius)
//             {
//                 closest_distance = distance;
//                 target = dementor.transform;
//             }
//         }
//     }
    
//     void FixedUpdate()
//     {
//         if (target != null)
//         {
//             // Move toward target
//             Vector3 direction = (target.position - transform.position).normalized;
//             rb.linearVelocity = direction * speed;
//         }
//     }
    
//     void OnCollisionEnter(Collision collision)
//     {
//         // If colliding with a dementor
//         if (collision.gameObject.CompareTag("Dementor"))
//         {
//             // Destroy the dementor
//             Destroy(collision.gameObject);
            
//             // Bounce off
//             Vector3 reflection = Vector3.Reflect(rb.linearVelocity, collision.contacts[0].normal);
//             rb.linearVelocity = reflection;
//         }
//     }
// }

// // Golden Snitch controller
// public class SnitchController : MonoBehaviour
// {
//     public float speed = 20f;
//     public float direction_change_interval = 1.5f;
//     private Rigidbody rb;
    
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         if (rb == null)
//         {
//             rb = gameObject.AddComponent<Rigidbody>();
//         }
        
//         // Start random flight pattern
//         StartCoroutine(change_direction_routine());
//     }
    
//     IEnumerator change_direction_routine()
//     {
//         while (true)
//         {
//             // Random new direction
//             Vector3 new_direction = Random.onUnitSphere;
//             rb.linearVelocity = new_direction * speed;
            
//             yield return new WaitForSeconds(direction_change_interval);
//         }
//     }
    
//     void OnTriggerEnter(Collider other)
//     {
//         // If caught by player (assuming player has specific tag or component)
//         if (other.CompareTag("Player"))
//         {
//             // Game end logic
//             Debug.Log("Golden Snitch caught, game over!");
//             // Add game victory logic here
//         }
//     }
// }

// // Enemy spawner controller extension
// public class EnemySpawner : MonoBehaviour
// {
//     public GameObject dementor_prefab;
//     public float spawn_interval = 3f;
//     public int max_dementors = 20;
    
//     private bool should_spawn = true;
//     private Coroutine spawn_routine;
    
//     void Start()
//     {
//         spawn_routine = StartCoroutine(spawn_dementors_routine());
//     }
    
//     // Stop spawning enemies
//     public void stop_spawning()
//     {
//         should_spawn = false;
//         if (spawn_routine != null)
//         {
//             StopCoroutine(spawn_routine);
//         }
//     }
    
//     IEnumerator spawn_dementors_routine()
//     {
//         while (should_spawn)
//         {
//             // Check current number of dementors in the scene
//             GameObject[] dementors = GameObject.FindGameObjectsWithTag("Dementor");
            
//             if (dementors.Length < max_dementors)
//             {
//                 // Spawn position logic
//                 Vector3 spawn_position = transform.position + Random.insideUnitSphere * 10f;
//                 spawn_position.y = 5f; // Ensure reasonable height
                
//                 Instantiate(dementor_prefab, spawn_position, Quaternion.identity);
//             }
            
//             yield return new WaitForSeconds(spawn_interval);
//         }
//     }
// }