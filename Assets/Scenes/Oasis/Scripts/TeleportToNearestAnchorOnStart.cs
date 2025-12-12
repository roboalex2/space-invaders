using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportToNearestAnchorOnStart : MonoBehaviour
{
    [Header("Player / Rig")]
    [Tooltip("Root transform of the player or XR Origin. If empty, this GameObject is used.")]
    public Transform playerRoot;

    [Header("Teleportation")]
    [Tooltip("Teleportation Provider used by the XR rig.")]
    public TeleportationProvider teleportationProvider;

    [Tooltip("List of teleport anchors that can be used as spawn points.")]
    public List<TeleportationAnchor> teleportAnchors = new List<TeleportationAnchor>();

    [Header("Timing")]
    [Tooltip("Optional delay (seconds) before teleport to allow XR to initialize.")]
    public float initialDelaySeconds = 0f;

    [Header("Debug")]
    public bool logTeleport = true;

    private bool _hasTeleported;

    private void Awake()
    {
        if (playerRoot == null)
            playerRoot = transform;

        if (teleportationProvider == null)
            teleportationProvider = FindObjectOfType<TeleportationProvider>();
    }

    private void Start()
    {
        if (_hasTeleported)
            return;

        if (initialDelaySeconds > 0f)
            StartCoroutine(TeleportAfterDelay(initialDelaySeconds));
        else
            TeleportToNearestAnchorOnce();
    }

    private IEnumerator TeleportAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        TeleportToNearestAnchorOnce();
    }

    private void TeleportToNearestAnchorOnce()
    {
        if (_hasTeleported)
            return;

        if (playerRoot == null || teleportationProvider == null || teleportAnchors == null || teleportAnchors.Count == 0)
        {
            if (logTeleport)
                Debug.LogWarning("[StartTeleportToNearestAnchor] Missing playerRoot, teleportationProvider, or teleportAnchors.", this);
            return;
        }

        TeleportationAnchor nearest = null;
        float nearestSqrDist = float.MaxValue;
        Vector3 playerPos = playerRoot.position;

        foreach (var anchor in teleportAnchors)
        {
            if (anchor == null)
                continue;

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

        var request = new TeleportRequest
        {
            destinationPosition = dest.position,
            destinationRotation = dest.rotation,
            matchOrientation = MatchOrientation.WorldSpaceUp
        };

        teleportationProvider.QueueTeleportRequest(request);
        _hasTeleported = true;

        if (logTeleport)
            Debug.Log($"[StartTeleportToNearestAnchor] Teleport queued to: {nearest.name}", this);
    }
}
