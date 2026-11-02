using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Reticle : MonoBehaviour
{
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] GameObject visual;

    static readonly List<ARRaycastHit> hits = new();

    public bool HasPose { get; private set; }
    public Pose Pose { get; private set; }

    // Raycast desde el centro de la pantalla en cada frame
    void Update()
    {
        Vector2 center = new(Screen.width * 0.5f, Screen.height * 0.5f);
        HasPose = raycastManager.Raycast(center, hits, TrackableType.PlaneWithinPolygon);
        visual.SetActive(HasPose);
        if (!HasPose) return;

        Pose = hits[0].pose;
        transform.SetPositionAndRotation(Pose.position, Pose.rotation);
    }
}
