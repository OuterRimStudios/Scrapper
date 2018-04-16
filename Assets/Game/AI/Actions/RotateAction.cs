using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Rotate")]
public class RotateAction : Action {

	public Vector3 rotation;

	public override void Act(StateController controller)
    {
        Rotate(controller);
    }
	
	void Rotate (StateController controller) {
		Quaternion desiredRotation = Quaternion.Euler(controller.transform.eulerAngles + rotation);
		controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, desiredRotation, Time.deltaTime * controller.enemyRefManager.currentChallengeTier.rotateSpeed);
	}
}
