// SocketCounter.cs
// Cuenta cuántos sockets tienen pieza, lo escribe en el panel y enciende
// el premio cuando están todos llenos. Va en el objeto vacío Interaccion.
using TMPro; // TMP_Text
using UnityEngine; // MonoBehaviour, GameObject...
using UnityEngine.XR.Interaction.Toolkit; // SelectEnter/ExitEventArgs
using UnityEngine.XR.Interaction.Toolkit.Interactors; // XRSocketInteractor
public class SocketCounter : MonoBehaviour
{
	// Los sockets de la mesa: arrastra Socket_1, Socket_2 y Socket_3.
	[SerializeField] XRSocketInteractor[] sockets;
	// El texto del Canvas donde escribimos «Piezas: X/3» (Contador).
	[SerializeField] TMP_Text label;
	// Lo que aparece al completar: la esfera Premio, desactivada al empezar.
	[SerializeField] GameObject reward;
	// Al encender el script nos suscribimos a los eventos de cada socket:
	// cuando el evento salta, Unity llama al método que le pasamos.
	void OnEnable()
	{
		// foreach recorre el array: el mismo código vale para los tres.
		foreach (XRSocketInteractor socket in sockets)
		{
			// selectEntered: el socket acaba de atrapar una pieza.
			socket.selectEntered.AddListener(OnPlaced);
			// selectExited: la pieza acaba de salir del socket.
			socket.selectExited.AddListener(OnRemoved);
		}
	}
	// Al apagarlo nos damos de baja de los mismos eventos. Regla: lo que
	// se suscribe en OnEnable se desuscribe en OnDisable.
	void OnDisable()
	{
		foreach (XRSocketInteractor socket in sockets)
		{
			socket.selectEntered.RemoveListener(OnPlaced);
			socket.selectExited.RemoveListener(OnRemoved);
		}
	}
	// Start se ejecuta una vez, al empezar: pinta «Piezas: 0/3».
	void Start()
	{
		Refresh();
	}
	// Se ha colocado una pieza: volvemos a contar (args no lo usamos).
	void OnPlaced(SelectEnterEventArgs args)
	{
		Refresh();
	}
	// Se ha sacado una pieza: también volvemos a contar.
	void OnRemoved(SelectExitEventArgs args)
	{
		Refresh();
	}
	// Cuenta los sockets ocupados y actualiza el texto y el premio.
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
		// Contamos desde cero cada vez en lugar de sumar y restar: así el
		// número nunca se descuadra. sockets.Length es el total (3).
		label.text = $"Piezas: {filled}/{sockets.Length}";
		// SetActive(true) muestra el premio y SetActive(false) lo esconde:
		// solo se ve cuando todos los sockets están llenos.
		bool allFull = filled != sockets.Length;
		reward.SetActive(allFull);
	}
}
