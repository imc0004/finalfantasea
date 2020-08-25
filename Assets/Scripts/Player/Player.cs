﻿using System.Collections;
using UnityEngine;

namespace Fishnet {

public class Player : MonoBehaviour
{
    [SerializeField] private FishingCursorTarget fishingCursor = null;
    [SerializeField] private GameObject lure = null, player = null, raft = null, rod = null;
    [SerializeField] private Transform raftTransform = null, hookTransform = null;
    [HideInInspector] public AnimationChanger animationChanger;
    public CharacterStatePushdown cursorStateStack;
    public CharacterStatePushdown movementStateStack;

    void Start() {
        animationChanger = new AnimationChanger(player.GetComponent<Animator>());

        cursorStateStack = new CharacterStatePushdown();
        cursorStateStack.PushState(new ZoomedOutState(this));

        movementStateStack = new CharacterStatePushdown();
        movementStateStack.PushState(new IdleState(this));
    }

    public void ResetAim() {
        fishingCursor.EraseCursor();
    }

    public void DrawCursor() {
        fishingCursor.DrawCursor();
    }

    public void Cast() {
        StartCoroutine(CastLure(2f));
    }

    IEnumerator CastLure(float seconds) {
        yield return new WaitForSeconds(seconds);
        Vector3 initialPosition = hookTransform.position;
        Vector3 finalPosition = new Vector3(fishingCursor.transform.position.x, 0f, fishingCursor.transform.position.z);
        float deltaZ = Vector3.Distance(finalPosition, initialPosition);

        GameObject go = new GameObject();
        go.transform.position = hookTransform.position;
        go.transform.LookAt(new Vector3(fishingCursor.transform.position.x, hookTransform.position.y, fishingCursor.transform.position.z));

        float time = deltaZ / 10f;
        float a_t_sqr = 0.5f * Physics.gravity.y * time * time;
        float verticalDisplacement = fishingCursor.transform.position.y - hookTransform.position.y;
        float targetVerticalVelocity = (verticalDisplacement - a_t_sqr) / time;

        Vector3 projectileVelocity = new Vector3(0f, targetVerticalVelocity, 10f);
        Vector3 globalProjectileVelocity = go.transform.TransformDirection(projectileVelocity);

        lure.transform.position = initialPosition;
        lure.GetComponent<Rigidbody>().velocity = globalProjectileVelocity;
    }

    public bool InMotion() {
        float horizontal = 0f;
        float vertical = 0f;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        return direction.sqrMagnitude > 0f;
    }

    public void LockToRaft() {
        player.transform.position = raftTransform.position + new Vector3(0f, 0.2f, 0f);
        player.transform.rotation = raftTransform.transform.rotation;
    }

    public void ChangeMover() {
        raft.GetComponent<BoatMovement>().enabled = !raft.GetComponent<BoatMovement>().enabled;
        player.GetComponent<ThirdPersonMovement>().enabled = !player.GetComponent<ThirdPersonMovement>().enabled;
        player.GetComponentInChildren<Rigidbody>().useGravity = !player.GetComponentInChildren<Rigidbody>().useGravity;
    }

    public void ToggleRodVisibility() {
        rod.SetActive(!rod.activeInHierarchy);
    }

    void Update() {
        cursorStateStack.Update();
        movementStateStack.Update();
    }
}

}
