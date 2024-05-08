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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    private void GeneratePipe()
    {
        // Randomly select color for the pipe segment
        currentColor = pipeColors[Random.Range(0, pipeColors.Length)];
        startPosition = startPosition + direction;

        GameObject newPipe = Instantiate(pipePrefab, startPosition, rotation);
        newPipe.GetComponent<Renderer>().material.color = currentColor;
    }
}
