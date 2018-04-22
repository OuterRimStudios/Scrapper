using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class AbilityCard : Selectable
{
	public Ability cardAbility;
	public Button cardButton;
	public Image abilityIcon;
	public TextMeshProUGUI abilityDescription;
    public Animator anim;

    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    
    bool flipping;

	public void InitializeCard(Ability _cardAbility)
	{
		cardAbility = _cardAbility;
		abilityIcon.sprite = cardAbility.abilityIcon;
		abilityDescription.text = cardAbility.abilityDescription;

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
	}

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {

            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);

            foreach(RaycastResult result in results)
            {
                if(result.gameObject == gameObject)
                {
                    if (!flipping)
                    {
                        flipping = true;
                        anim.SetTrigger("Flip");
                        StartCoroutine(WaitForFlip());
                    }
                }
            }
        }
    }

    IEnumerator WaitForFlip()
    {
        yield return new WaitForSeconds(.5f);
        flipping = false;
    }
}