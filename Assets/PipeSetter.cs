using UnityEngine;
using System.Collections;

public class PipeSetter : MonoBehaviour
{
    public GameObject pipePrefab;
    public int totalPipeSegments;
    public Color[] pipeColors;
    private Color currentColor;
    Vector3 startPosition = Vector3.zero;
    Vector3 direction = Vector3.up;
    Quaternion rotation = Quaternion.identity;


    private void Start()
    {
        StartCoroutine(GeneratePipesWithDelay(0.5f)); // Start generating pipes with a delay of 0.5 seconds
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
        // Randomly select color for the pipe segment
        currentColor = pipeColors[Random.Range(0, pipeColors.Length)];
        startPosition = startPosition + direction;

        GameObject newPipe = Instantiate(pipePrefab, startPosition, rotation);
        newPipe.GetComponent<Renderer>().material.color = currentColor;
    }
}
