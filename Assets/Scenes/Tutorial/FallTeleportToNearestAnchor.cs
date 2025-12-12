using System.Collections.Generic;
using UnityEngine;


public class FallTeleportToNearestAnchor : MonoBehaviour
{
    [Header("Player / Rig")]
    [Tooltip("Root transform of the player or XR Origin. If empty, this GameObject is used.")]
    public Transform playerRoot;

    [Header("Fall Settings")]
    [Tooltip("If playerRoot.y goes below this value, teleport will trigger.")]
    public float fallYThreshold = -5f;

    [Header("Teleportation")]
    [Tooltip("Teleportation Provider used by the XR rig.")]
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportationProvider;

    [Tooltip("List of teleport anchors that can be used as respawn points.")]
    public List<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor> teleportAnchors = new List<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor>();

    private void Awake()
    {
        if (playerRoot == null)
            playerRoot = transform;

        if (teleportationProvider == null)
            teleportationProvider = FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();
    }

    private void Update()
    {
        if (playerRoot == null || teleportationProvider == null || teleportAnchors.Count == 0)
            return;

        if (playerRoot.position.y < fallYThreshold)
        {
            TeleportToNearestAnchor();
        }
    }

    private void TeleportToNearestAnchor()
    {
        // Find nearest anchor by distance to the player
        UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor nearest = null;
        float nearestSqrDist = float.MaxValue;
        Vector3 playerPos = playerRoot.position;

        foreach (var anchor in teleportAnchors)
        {
            if (anchor == null)
                continue;

            // Prefer the anchor's teleport point, fall back to its transform
            Transform target = anchor.teleportAnchorTransform != null
                ? anchor.teleportAnchorTransform
                : anchor.transform;

            float sqrDist = (target.position - playerPos).sqrMagnitude;
            if (sqrDist < nearestSqrDist)
            {
                nearestSqrDist = sqrDist;
                nearest = anchor;
            }
        }

        if (nearest == null)
            return;

        Transform dest = nearest.teleportAnchorTransform != null
            ? nearest.teleportAnchorTransform
            : nearest.transform;

        var request = new UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest
        {
            destinationPosition = dest.position,
            destinationRotation = dest.rotation,
            matchOrientation = UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.MatchOrientation.WorldSpaceUp
        };

        teleportationProvider.QueueTeleportRequest(request);
    }
}
