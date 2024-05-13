using UnityEngine;
using System.Collections;

public class PipeSetter : MonoBehaviour
{
    public GameObject pipePrefab;
    public GameObject curvePrefab;
    public int totalPipeSegments;
    public Color[] pipeColors;
    private Color currentColor;
    Vector3 startPosition = new Vector3(0, 3, 0);
    Vector3 direction = Vector3.up;
    private Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward }; // Predetermined directions
    Quaternion rotation = Quaternion.identity;
    BoxCollider space;
    Bounds spaceBounds;
    Vector3 previousDirection;
    bool newPipe = true;
    int iterationCap = 0;


    private void Start()
    {   
        //Get the Bounds of the Area
        space = gameObject.GetComponent<BoxCollider>();
        spaceBounds = space.bounds;

        //Start the coroutine with interval time of 0.2 seconds
        StartCoroutine(GeneratePipesWithDelay(0.2f));
    }

    private IEnumerator GeneratePipesWithDelay(float delay)
    {
        for (int i = 0; i < totalPipeSegments; i++)
        {
            GeneratePipe();
            yield return new WaitForSeconds(delay); // Wait for the specified delay before generating the next pipe
        }
    }

    private void GeneratePipe()
    {

        if(newPipe){ //Checks whether to initialize a new pipe
            NewPipe();
            iterationCap = 0; //Variable to cap the maximum recursion calls is reset
        }
        if(direction ==  Vector3.zero){ //When no direction is returned a new pipe should be initialized
            newPipe = true;
            return;
            }
private void NewPipe(){

    iterationCap++;

    currentColor = pipeColors[Random.Range(0, pipeColors.Length)]; //Picks a new random color from the color list

    //Randomly selects a new point withinn the bounds
    int x = Random.Range((int)spaceBounds.min.x, (int)spaceBounds.max.x + 1);
    int y = Random.Range((int)spaceBounds.min.y, (int)spaceBounds.max.y + 1);        
    int z = Random.Range((int)spaceBounds.min.z, (int)spaceBounds.max.z + 1);
    Vector3 randomPosition = new Vector3(x, y, z);

    // Check if there is a pipe at the random position
    Collider[] colliders = Physics.OverlapSphere(randomPosition, 1f); // Adjust radius as needed

    foreach (Collider col in colliders)
    {
        if (col.gameObject.CompareTag("Pipe"))
        {
            // If there is a pipe at the position, recursively call this method to find a new position
            if(iterationCap < 100){ // Ends the search after 100 iterations
                NewPipe();
            }
         }
    }

    startPosition = randomPosition; //Updates the startPosition
    newPipe = false;

    }
}
