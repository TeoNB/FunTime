using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Rise : MonoBehaviour
{
	XRGrabInteractable telon;
	float riseSpeed = 0.15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		telon = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
		if (!telon.isSelected)
		{
			Vector3 height = transform.position;

			height.y = Mathf.MoveTowards(height.y, 2.25f, riseSpeed * Time.deltaTime);
			
			transform.position = height;
		}
    }
}
