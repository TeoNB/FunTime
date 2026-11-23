using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Operation : MonoBehaviour
{
	[SerializeField] XRSocketInteractor[] sockets;

	public List<GameObject> organs;

	[SerializeField] GameObject reward;
	[SerializeField] Material material;

	Color colorStart;

	private bool lose;
	void OnEnable()
	{
		foreach (XRSocketInteractor socket in sockets)
		{
			socket.selectEntered.AddListener(OnPlaced);

			socket.selectExited.AddListener(OnRemoved);
		}
	}
	void OnDisable()
	{
		foreach (XRSocketInteractor socket in sockets)
		{
			socket.selectEntered.RemoveListener(OnPlaced);
			socket.selectExited.RemoveListener(OnRemoved);
		}
	}
	void Start()
	{
		colorStart = material.color;
		Refresh();
	}
	
	void OnPlaced(SelectEnterEventArgs args)
	{
		Refresh();
	}
	
	void OnRemoved(SelectExitEventArgs args)
	{
		Refresh();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (organs.Contains(collision.gameObject))
		{
			material.color = Color.red;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (organs.Contains(collision.gameObject))
		{
			material.color = colorStart;
		}
	}

	void Refresh()
	{
		// hasSelection vale true si el socket tiene una pieza ahora mismo.
		int filled = 0;
		foreach (XRSocketInteractor socket in sockets)
		{
			if (socket.hasSelection)
			{
				filled = filled + 1;
			}
		}
		// SetActive(true) muestra el premio y SetActive(false) lo esconde:
		// solo se ve cuando todos los sockets están llenos.
		bool allFull = filled == sockets.Length;
		reward.SetActive(allFull);
	}
}
