using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
        
        LinkedList<Vector3> options = new LinkedList<Vector3>(directions); //Create a new linked list from all direction options

        Vector3 direction = GetValidDirection(options);

        if(direction ==  Vector3.zero){ //When no direction is returned a new pipe should be initialized
            newPipe = true;
            return;
            }

        if(direction == previousDirection){ //Checks whether there is a curve or the pipe goes straight

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction); //Rotates the pipe towards the direction
            
            startPosition += direction; //Updates position
            
            GameObject newPipe = Instantiate(pipePrefab, startPosition, rotation); //Renders the pipe
            newPipe.GetComponent<Renderer>().material.color = currentColor;
        }
        else{

            startPosition += previousDirection; //When the pipe curves a curve sphere is created first
            
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction); //Rotates the pipe towards the direction
            
            GameObject newCurve = Instantiate(curvePrefab, startPosition, rotation); //Renders a new curve
            newCurve.GetComponent<Renderer>().material.color = currentColor;

            startPosition += direction; //Updates position

            GameObject newPipe = Instantiate(pipePrefab, startPosition, rotation); //Renders the pipe
            newPipe.GetComponent<Renderer>().material.color = currentColor;
        }

        previousDirection = direction;
    }
private Vector3 GetValidDirection(LinkedList<Vector3> options)
{
    // Checks if the pipe is in a dead end
    if (options.Count == 0)
    {
        Debug.Log("No valid options");
        return Vector3.zero;
    }

    int directionIndex = Random.Range(0, options.Count); // Randomly select a direction index from the predetermined list

    // Node gets the direction at the random index
    var node = options.First;
    for (int i = 0; i < directionIndex; i++)
    {
        node = node.Next;
    }

    Vector3 direction = node.Value;
    Vector3 nextPosition = startPosition + direction;

    Vector3 adjustedNextPosition = nextPosition + (direction * 0.75f); //Endpoint of the pipe
    Vector3 curvedPosition = adjustedNextPosition + previousDirection; //Position taking curve into account

    // Check if the entire bounds of the pipe prefab fit within the collider bounds
    if (!spaceBounds.Contains(adjustedNextPosition))
    {
        // If the next position is outside the collider bounds, remove the direction from options
        options.Remove(node);

        return GetValidDirection(options); // Choose a different direction from the updated options
    }
    if(direction != previousDirection){

        // If the pipe curves, it checks whether the curve will also fit the bounds
        if(!spaceBounds.Contains(curvedPosition)){

            // If the next position is outside the collider bounds, remove the direction from options
            options.Remove(node);

            return GetValidDirection(options); // Choose a different direction from the updated options
        }
    } else curvedPosition = Vector3.zero;

    // Check if the next position would intersect with any existing pipe
    Collider[] colliders = Physics.OverlapSphere(adjustedNextPosition, 1);

    foreach (Collider col in colliders)
    {
        if (col.gameObject.CompareTag("Pipe"))
        {
            //Debug.Log("Pipe Collision");
            // If collision detected with another pipe, choose a different direction
            options.Remove(node);
            return GetValidDirection(options);
        }
    }

    if(curvedPosition != Vector3.zero){

        colliders = Physics.OverlapSphere(curvedPosition, 0.5f);

        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("Pipe"))
            {
                //Debug.Log("Pipe Collision");
                // If collision detected with another pipe, choose a different direction
                options.Remove(node);
                return GetValidDirection(options);
            }
        } 
    }
    return direction; // Return the chosen direction if it's valid
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
