using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Telon : MonoBehaviour
{
	[SerializeField] XRGrabInteractable[] telones;

	[SerializeField] GameObject reward;

	[SerializeField] GameObject cuchillo;

	bool[] below;

	bool win = false;

	ResetPieces reset;

	void OnEnable()
	{
		// foreach recorre el array: el mismo código vale para los tres.
		foreach (XRGrabInteractable telon in telones)
		{
			// selectEntered: el telon acaba de bajar de la linea.
			telon.selectEntered.AddListener(OnPlaced);
			// selectExited: la pieza acaba de sobrepasar la linea por arriba.
			telon.selectExited.AddListener(OnRemoved);
		}
	}

	void OnDisable()
	{
		foreach (XRGrabInteractable telon in telones)
		{
			telon.selectEntered.RemoveListener(OnPlaced);
			telon.selectExited.RemoveListener(OnRemoved);
		}
	}

	void Start()
	{
		reset = GetComponent<ResetPieces>();
		below = new bool[telones.Length];
		Refresh();
	}

	void Update()
	{
		for (int i = 0; i < telones.Length; i++)
		{
			if (telones[i].transform.position.y <= 1.78f)
			{
				below[i] = true;
			}
			else
			{
				below[i] = false;
			}
		}

		Refresh();
	}
	// Se ha bajado un telon: volvemos a contar (args no lo usamos).
	void OnPlaced(SelectEnterEventArgs args)
	{
		Refresh();
	}
	// Se ha elevado demasiado un telon: también volvemos a contar.
	void OnRemoved(SelectExitEventArgs args)
	{
		Refresh();
	}

	void Refresh()
	{
		// down vale true si el telon está bajo la linea.
		int down = 0;
		for (int i = 0; i < telones.Length; i++)
		{
			if (below[i])
			{
				down++;
			}
		}
		// Contamos desde cero cada vez en lugar de sumar y restar: así el
		// número nunca se descuadra. telones.Length es el total (4).
		Debug.Log($"Piezas: {down}/{telones.Length}");
		// SetActive(true) muestra el premio y SetActive(false) lo esconde:
		// solo se ve cuando todos los sockets están llenos.
		bool allDown = down == telones.Length;
		if(allDown && cuchillo.transform.position.x < -3.7f)
		{
			win = true;
		}
		reward.SetActive(win);
		if(!allDown && cuchillo.transform.position.x < -3.7f && !win)
		{
			reset.ResetLoss();
		}
	}
}
