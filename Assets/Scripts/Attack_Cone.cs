using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Cone : MonoBehaviour {

    public Turret turretAI;

    public bool isLeft = false;

	void Awake () {

        turretAI = gameObject.GetComponentInParent<Turret>();
		
	}
	
	// Update is called once per frame
	void OnTriggerStay2D (Collider2D col) {

        if (col.CompareTag("Player"))
        {

            if (isLeft)
            {
                turretAI.Attack(false);
            }
            else
            {
                turretAI.Attack(true);

            }

        }
		
	}
}
