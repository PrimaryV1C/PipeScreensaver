using UnityEngine;
using System.Collections;

public class PipeSetter : MonoBehaviour
{
    public GameObject pipePrefab;
    public int totalPipeSegments;
    public Color[] pipeColors;
    private Color currentColor;
    Vector3 direction = Vector3.up;
    Quaternion rotation = Quaternion.identity;


    private void Start()
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
