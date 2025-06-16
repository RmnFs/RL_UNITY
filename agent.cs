using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Integrations.Match3;
using TMPro;


public class agent : Agent
{
    [SerializeField] private Transform target;
    public int pelletCount = 6;
    public GameObject food;
    [SerializeField] private List<GameObject> spawnedPelletList = new List<GameObject>();
    //Environment cariavke
    [SerializeField] private Transform environmentLocation;
    Material envMaterial;
    public GameObject env;

    public float moveSpeed = 12f;
    public float rotationSpeed = 100f;

    private Rigidbody rb;

    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float energyPerPellet = 20f;
    public float energyDepletionRate = 0.2f; // Energy lost per step
    private float currentEnergy;

    [Header("UI Settings")]
    public TextMeshProUGUI energyText;



    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        envMaterial = env.GetComponent<Renderer>().material;
    }




    public override void OnEpisodeBegin()
    {

        
        //agent
        //transform.localPosition = new Vector3(Random.Range(-4f,4f),0.3f, Random.Range(-4f, 4f));
        Vector3 pelletLocation = new Vector3(Random.Range(-16f, 16f), 0.45f, Random.Range(-16f, 16f)); // For 4x scale


        //target.localPosition = new Vector3(Random.Range(-4f, 4f), 0.3f, Random.Range(-4f, 4f));

        CreatePellet();

        currentEnergy = maxEnergy; // Reset energy
        //envMaterial.color = Color.white;



    }

    private void CreatePellet()
    {

        if (spawnedPelletList.Count !=0)
        {
            RemovePellet(spawnedPelletList);
        }

        for (int i = 0; i < pelletCount; i++)
        {
            GameObject newPellet = Instantiate(food);
            //MAke pellet child of env
            newPellet.transform.parent = environmentLocation;
            //Vector3 pelletLocation = new Vector3(Random.Range(-4f, 4f), 0.3f, Random.Range(-4f, 4f)); BEFORE SCALE
            Vector3 pelletLocation = new Vector3(Random.Range(-16f, 16f), 0.45f, Random.Range(-16f, 16f));

            //spwn in 
            newPellet.transform.localPosition=pelletLocation;

            spawnedPelletList.Add(newPellet);
        }
    }

    private void RemovePellet(List<GameObject> toBeDeleted)
    {

        foreach (GameObject i in toBeDeleted)
        {
            Destroy(i.gameObject);
        }
        toBeDeleted.Clear();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(target.localRotation);
        sensor.AddObservation(currentEnergy / maxEnergy);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {

        //Energy Management

        currentEnergy -= energyDepletionRate * Time.fixedDeltaTime;


        if (currentEnergy / maxEnergy < 0.3f)
        {
            AddReward(-0.001f); // Make this small so it doesn't discourage survival
        }
        if (currentEnergy <= 0f)
        {
            AddReward(-10f); // Same penalty as hitting a wall
            RemovePellet(spawnedPelletList);
            EndEpisode();
        }

        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        //Vector3 velocity = new Vector3(moveX, 0f, moveZ) * Time.deltaTime * moveSpeed;
        //velocity = velocity.normalized *Time.deltaTime *moveSpeed;


        //transform.localPosition += velocity;
        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        //transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);  THIS WAS BEFORE THE SCALE CHANGE
        transform.Rotate(0f, moveRotate * rotationSpeed * Time.deltaTime, 0f, Space.Self);

       



    }

    void Update()
    {
        // Make sure the reference is not empty to avoid errors
        if (energyText != null)
        {
            // Update the text with the current energy value
            // "F1" formats the number to show only one decimal place
            energyText.text = "Energy: " + currentEnergy.ToString("F1");
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions =actionsOut.ContinuousActions;
        continuousActions[0]= Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Pellet") 
        {

            // Replenish energy
            currentEnergy += energyPerPellet;
            currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy); // Don't let energy go over max

            spawnedPelletList.Remove(other.gameObject);
            Destroy(other.gameObject);
            AddReward(10f);
            //envMaterial.color = Color.blue;
            //envMaterial.color = Color.green;
            if (spawnedPelletList.Count ==0)
            {
                
                RemovePellet(spawnedPelletList);
                AddReward(5f);
                EndEpisode();
            }
        }
        if (other.gameObject.tag == "Wall")
        {
            envMaterial.color =Color.red;
            RemovePellet(spawnedPelletList);
            AddReward(-10f);
            EndEpisode();
        }
    }



}
