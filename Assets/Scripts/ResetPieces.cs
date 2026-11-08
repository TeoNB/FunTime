// ResetPieces.cs
// Devuelve las tres piezas a su sitio al pulsar el botón Reiniciar, estén
// en la mano o encajadas en un socket. Va en el objeto vacío Interaccion.
using UnityEngine; // MonoBehaviour, Pose...
using UnityEngine.XR.Interaction.Toolkit.Interactables; // XRGrabInteractable
using UnityEngine.XR.Interaction.Toolkit.Interactors; // XRSocketInteractor
public class ResetPieces : MonoBehaviour
{
	// En el Inspector: las tres piezas y los tres sockets de la mesa.
	[SerializeField] XRGrabInteractable[] pieces; // Pieza_1, 2, 3 y 4
	[SerializeField] XRSocketInteractor[] sockets; // Socket_1, 2, 3 y 4
	[SerializeField] XRGrabInteractable[] telones; // Telon_1, 2, 3 y 4
	[SerializeField] XRGrabInteractable cuchillo;

	Pose[] startPoses; // dónde empezó cada pieza: posición y rotación
					   // Awake se ejecuta al cargar la escena, antes de que nadie toque las
					   // piezas: es el momento de apuntar dónde está cada una.
	Pose[] beginPoses;
	Vector3 cuchilloStart;
	void Awake()
	{
		// Creamos el array con un hueco por pieza.
		startPoses = new Pose[pieces.Length];
		for (int i = 0; i < pieces.Length; i++)
		{
			// Pose guarda posición y rotación juntas en un solo valor.
			Transform t = pieces[i].transform;
			startPoses[i] = new Pose(t.position, t.rotation);
		}
		beginPoses = new Pose[telones.Length];
		for (int i = 0; i < telones.Length; i++)
		{
			// Pose guarda posición y rotación juntas en un solo valor.
			Transform t = telones[i].transform;
			beginPoses[i] = new Pose(t.position, t.rotation);
		}
		cuchilloStart = cuchillo.transform.position;
	}
	// Enciende (true) o apaga (false) los tres sockets a la vez. Un socket
	// apagado suelta su pieza y no atrapa ninguna.
	void SetSockets(bool active)
	{
		foreach (XRSocketInteractor socket in sockets)
		{
			socket.socketActive = active;
		}
	}
	// Es public para que el botón lo encuentre en su lista On Click ().
	public void ResetAll()
	{
		// 1. Apagamos los sockets: si no, volverían a atrapar las piezas en
		// el mismo frame y se quedarían encajadas.
		SetSockets(false);
		for (int i = 0; i < pieces.Length; i++)
		{
			XRGrabInteractable piece = pieces[i];
			// 2. Si una mano o un socket sujeta la pieza, la soltamos. El
			// manager la pide como IXRSelectInteractable («seleccionable»).
			IXRSelectInteractable selectable = piece;
			if (piece.isSelected)
			{
				piece.interactionManager.CancelInteractableSelection(selectable);
			}
			// 3. La frenamos: si iba volando, saldría disparada de la mesa.
			Rigidbody body = piece.GetComponent<Rigidbody>();
			body.linearVelocity = Vector3.zero; // velocidad al moverse
			body.angularVelocity = Vector3.zero; // velocidad al girar
												 // 4. La ponemos donde empezó: primero la posición y luego la
												 // rotación que guardamos en Awake.
			Pose start = startPoses[i];
			piece.transform.position = start.position;
			piece.transform.rotation = start.rotation;
		}

		// 5. Invoke llama a un método pasado un tiempo, en segundos. El
		// nombre va entre comillas y tiene que ser exactamente el suyo.
		Invoke("SocketsOn", 0.5f);
	}
	public void ResetLoss()
	{
		for (int i = 0; i < telones.Length; i++)
		{
			XRGrabInteractable telon = telones[i];
			// 2. Si una mano o un socket sujeta la pieza, la soltamos. El
			// manager la pide como IXRSelectInteractable («seleccionable»).
			IXRSelectInteractable selectable = telon;
			if (telon.isSelected)
			{
				telon.interactionManager.CancelInteractableSelection(selectable);
			}
			// 3. La frenamos: si iba volando, saldría disparada de la mesa.
			Rigidbody body = telon.GetComponent<Rigidbody>();
			body.linearVelocity = Vector3.zero; // velocidad al moverse
			body.angularVelocity = Vector3.zero; // velocidad al girar
												 // 4. La ponemos donde empezó: primero la posición y luego la
												 // rotación que guardamos en Awake.
			Pose begin = beginPoses[i];
			telon.transform.position = begin.position;
			telon.transform.rotation = begin.rotation;
		}
		IXRSelectInteractable selected = cuchillo;
		if (cuchillo.isSelected)
		{
			cuchillo.interactionManager.CancelInteractableSelection(selected);
		}
		cuchillo.transform.position = cuchilloStart;
	}
	// Para entonces las piezas ya están en la mesa, lejos de los sockets.
	void SocketsOn()
	{
		SetSockets(true);
	}
}
