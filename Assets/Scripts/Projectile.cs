using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Rigidbody) )]
public class Projectile : MonoBehaviour
{
   const int LOOKBACK_COUNT = 10;
   private bool hitSomething = false;
   static List<Projectile> PROJECTILES = new List<Projectile>();

    [SerializeField]
    private bool _awake = true;
    public bool awake {
        get { return _awake; }
        private set { _awake = value; }
    }

    private Vector3     prevPos;
    // This private List stores the history of Projectile's move distance
    private List<float> deltas = new List<float>();
    private Rigidbody   rigid;

    void Start() {
        rigid = GetComponent<Rigidbody>();
        awake = true;
        prevPos = new Vector3(1000,1000,0);
        deltas.Add( 1000 );
        PROJECTILES.Add(this);
    }

        void FixedUpdate() {

        if ( rigid.isKinematic || !awake ) return;                     

        // Apply wind while the projectile is in flight
        if ( !hitSomething ) {                                         
            rigid.AddForce( Vector3.right * MissionDemolition.WIND,
                            ForceMode.Acceleration );
        }

        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add( deltaV3.magnitude );
        prevPos = transform.position;

        // Limit lookback; one of very few times that I'll use while!
        while ( deltas.Count > LOOKBACK_COUNT ) {
            deltas.RemoveAt( 0 );
        }

        // Iterate over deltas and find the greatest one
        float maxDelta = 0;

        foreach( float f in deltas ) {
            if ( f > maxDelta ) maxDelta = f;
        }

        // If projectile hasn't moved through sleep threshold
        if ( maxDelta <= Physics.sleepThreshold ) {
            // Set awake to false and put the rigidbody to sleep
            awake = false;
            rigid.Sleep();
        }
    }

    void OnCollisionEnter( Collision coll ) {                          // c
        hitSomething = true;
    }
    private void OnDestroy()
    {
        PROJECTILES.Remove(this);
    }
    
    static public void DESTROY_PROJECTILES()
    {
        foreach (Projectile p in PROJECTILES)
        {
            Destroy(p.gameObject);
        }
    }
}
