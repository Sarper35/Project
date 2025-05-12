using System;
using K_PathFinder.Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace K_PathFinder.Samples {
    [RequireComponent(typeof(PathFinderAgent), typeof(CharacterController))]
    public class FollowTarget : MonoBehaviour {
        public LineRenderer line;
        
        // public SimplePatrolPath patrol;
        [Range(0f, 5f)] public float speed = 3;

        public GameObject follow;
        
        private CharacterController controler;
        private PathFinderAgent agent;  
        private int currentPoint; //current patrol point 
          
        void Start() {

            controler = GetComponent<CharacterController>();
            agent = GetComponent<PathFinderAgent>();     
            agent.SetRecievePathDelegate(RecivePathDelegate, AgentDelegateMode.ThreadSafe); //setting here delegate to update line renderrer

            //queue navmesh
            PathFinder.QueueGraph(new Bounds(transform.position, Vector3.one * 20), agent.properties);
        }
        
        void Update() {
        if (Input.GetMouseButtonDown(0))
            agent.SetGoalMoveHere(follow.transform.position, applyRaycast: true);
 
            if (agent.haveNextNode) {
                Vector2 moveDirection = agent.nextNodeDirectionVector2.normalized;                  
                controler.SimpleMove(new Vector3(moveDirection.x, 0, moveDirection.y) * speed);
                float rotation_angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                Quaternion target = Quaternion.Euler(0f, rotation_angle, 0f);
                // Quaternion target = Quaternion.Euler(0f, moveDirection.x/moveDirection.y * 90f, .0f); Math.Atanh()
                transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * 10f);
            }
            // }
            // else
            //     RecalculatePath(); //get path to current point            
        }



        //Debug and checks handling
        private void RecivePathDelegate(Path path) {
            // if (path.pathType != PathResultType.Valid)
            //     Debug.LogWarningFormat("path is not valid. reason: {0}", path.pathType);
            ExampleThings.PathToLineRenderer(agent.positionVector3, line, path, 0.2f);
        }
    }
}