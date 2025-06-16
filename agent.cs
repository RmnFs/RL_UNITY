using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Integrations.Match3;


public class agent : Agent
{
    [SerializeField] private Transform target;
    public int pelletCount = 6;
    public GameObject food;
    [SerializeField] private List<GameObject> spawnedPelletList = new List<GameObject>();
    //Environment 
    [SerializeField] private Transform environmentLocation;
    Material envMaterial;
    public GameObject env;

    public float moveSpeed = 4f;

    private Rigidbody rb;



    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        envMaterial = env.GetComponent<Renderer>().material;
    }




    public override void OnEpisodeBegin()
    {
        //agent
        transform.localPosition = new Vector3(Random.Range(-4f,4f),0.3f, Random.Range(-4f, 4f));

        //target.localPosition = new Vector3(Random.Range(-4f, 4f), 0.3f, Random.Range(-4f, 4f));

        CreatePellet();


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
            Vector3 pelletLocation = new Vector3(Random.Range(-4f, 4f), 0.3f, Random.Range(-4f, 4f));
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
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        //Vector3 velocity = new Vector3(moveX, 0f, moveZ) * Time.deltaTime * moveSpeed;
        //velocity = velocity.normalized *Time.deltaTime *moveSpeed;


        //transform.localPosition += velocity;
        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);



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
