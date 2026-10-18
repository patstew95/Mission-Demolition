using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;
    public float bandWidth = 0.15f;
    public Material bandMat;
    //public Color bandColor = new Color(0.35f, 0.2f, 0.1f);

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidBody;

    private Transform leftArm;
    private Transform rightArm;

private LineRenderer leftBand;
private LineRenderer rightBand;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        leftArm = transform.Find("LeftArm");
        rightArm = transform.Find("RightArm");


        leftBand = MakeBandSegment("LeftBand", bandMat);
        rightBand = MakeBandSegment("RightBand", bandMat);
        UpdateBand(launchPos);
    }

        LineRenderer MakeBandSegment(string name, Material mat)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(transform, false);

            LineRenderer lr = go.AddComponent<LineRenderer>();
            lr.material = mat;
            lr.startWidth = lr.endWidth = bandWidth;
            lr.numCapVertices = 4;
            lr.useWorldSpace = true;
            lr.positionCount = 2;
            return lr;
        }
        Vector3 ArmTip(Transform arm)
        {
            return arm.position + arm.up * arm.lossyScale.y;
        }
        //stretches band 
        void UpdateBand(Vector3 projPos)
        {
            leftBand.SetPosition(0, ArmTip(leftArm));
            leftBand.SetPosition(1, projPos);
            rightBand.SetPosition(0, ArmTip(rightArm));
            rightBand.SetPosition(1, projPos);
        }

    
    void Update()
    {
        if (!aimingMode)
        {
            return;
    }
    Vector3 mousePos2D = Input.mousePosition;
    mousePos2D.z = -Camera.main.transform.position.z;

    Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
    Vector3 mouseDelta = mousePos3D - launchPos;

    float maxMagnitude = this.GetComponent<SphereCollider>().radius;

    if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        UpdateBand(projPos);

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRigidBody.isKinematic = false;
            projectileRigidBody.velocity = -mouseDelta * velocityMult;
            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot); // switch to slingshotview before setting POI
            FollowCam.POI = projectile; //set main camera
            //add line to projectile
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            UpdateBand(launchPos);
            MissionDemolition.SHOT_FIRED();
        }
    }

    void OnMouseEnter()
    {
        //print("Slingshot: OnmouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
         //print("Slingshot: OnmouseExit()");
         launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        projectileRigidBody = projectile.GetComponent<Rigidbody>();
        projectileRigidBody.isKinematic = true;
    }
    

}
