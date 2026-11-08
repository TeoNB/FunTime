using TMPro; // TMP_Text: los textos de TextMeshPro
using Unity.XR.CoreUtils; // XROrigin: el jugador (rig) de la escena
using UnityEngine; // MonoBehaviour, SerializeField...
using UnityEngine.XR.Management; // XRGeneralSettings y XRLoader
public class HeadsetInfo : MonoBehaviour
{
	// [SerializeField] deja el campo privado pero visible en el Inspector:
	// ahí arrastras el XR Origin de la escena y el texto del Canvas.
	[SerializeField] XROrigin origin;
	[SerializeField] TMP_Text label;
	// Update se ejecuta una vez por frame (de 72 a 120 veces por segundo
	// en el Quest 3): el texto siempre enseña los datos del momento.
	void Update()
	{
		// El loader es lo que arrancó el runtime XR: OpenXR en el visor.
		// Si no arrancó ninguno (simulador en el editor), vale null.
		XRLoader loader = XRGeneralSettings.Instance.Manager.activeLoader;
		// ... sigue en la diapositiva siguiente
		// ... sigue dentro de Update()
		// Empezamos suponiendo que estamos en el simulador. Si hay un
		// loader, cambiamos el texto por su nombre.
		string runtime = "Simulador";
		if (loader != null)
		{
			runtime = loader.name;
		}
		// Altura de la cámara (tus ojos) sobre el suelo del XR Origin, en
		// metros. La pone el tracking del visor, no la escena.
		float height = origin.CameraInOriginSpaceHeight;
		// Giro de la cabeza alrededor del eje vertical (Y), de 0 a 360
		// grados. Con 0 miras hacia +Z, donde está el cubo.
		float yaw = origin.Camera.transform.eulerAngles.y;
		// Cadena con $: lo que va entre llaves se sustituye por su valor.
		// {height:0.00} escribe dos decimales, {yaw:0} ninguno y \n salta
		// a la línea siguiente. Las tres líneas se unen con +.
		label.text = $"Runtime: {runtime}\n" +
		$"Altura de los ojos: {height:0.00} m\n" +
	   $"Giro: {yaw:0}°";
	}
}
