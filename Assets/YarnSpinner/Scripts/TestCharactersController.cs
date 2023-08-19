using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TestCharactersController : MonoBehaviour
{

    [SerializeField] private float speed;
    //[SerializeField] private GameObject dialogueSystem;

    private CharacterController CharacterController;

    private void Start()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        CharacterController.Move(moveDirection * Time.deltaTime * speed);
    }

    //Commands must be in script attached to game object its running on (e.g. the charachter)
    [YarnCommand("walk")] //exposing function to Yarn Spinner to automatically add it as a command (easy way)
    public void Walk(Transform destination)//string/int/bool/GameObject/Component/no parameters
    {
        transform.position = destination.transform.position;
    }

    //using tags
    [YarnCommand("walkToTag")] //exposing function to Yarn Spinner to automatically add it as a command (easy way)
    public void WalkToTag(string destinationTag)
    {
        GameObject destination = GameObject.FindGameObjectWithTag(destinationTag);

        if (destination != null)
        {
            transform.position = destination.transform.position; 
        }
    }
}
