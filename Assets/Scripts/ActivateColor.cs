using UnityEngine; // MonoBehaviour, Color...
using UnityEngine.XR.Interaction.Toolkit; // ActivateEventArgs
using UnityEngine.XR.Interaction.Toolkit.Interactables; // XRGrabInteractable
														// RequireComponent: si al objeto le falta XR Grab Interactable, lo añade.
[RequireComponent(typeof(XRGrabInteractable))]
public class ActivateColor : MonoBehaviour
{
	// Colores por los que va pasando la pieza. Se cambian en el Inspector;
	// el primero debe ser el color con el que empieza el material.
	[SerializeField] Color[] colors = { Color.white, Color.red, Color.cyan };
	XRGrabInteractable grab; // el componente que deja agarrar la pieza
	Renderer rend; // el que dibuja la malla con su material
	int index; // posición del color actual dentro del array
			   // Awake se ejecuta al cargar el objeto, antes que OnEnable y Start: es
			   // el sitio para guardar referencias a otros componentes del objeto.
	void Awake()
	{
		grab = GetComponent<XRGrabInteractable>();
		rend = GetComponent<Renderer>();
	}
	// Al encender el script apuntamos OnActivated al evento activated.
	void OnEnable()
	{
		grab.activated.AddListener(OnActivated);
	}
	// Al apagarlo nos borramos, para no dejar la escucha colgada.
	void OnDisable()
	{
		grab.activated.RemoveListener(OnActivated);
	}
	// XRI lo llama al apretar el gatillo con la pieza agarrada; args no se usa.
	void OnActivated(ActivateEventArgs args)
	{
		// Pasamos al siguiente color. Si nos salimos del array (después
		// del último), volvemos al primero, que es la posición 0.
		index = index + 1;
		if (index >= colors.Length)
		{
			index = 0;
		}
		// rend.material crea una copia del material solo para esta pieza:
		// al cambiarle el color, las otras piezas no se enteran.
		rend.material.color = colors[index];
	}
}
 