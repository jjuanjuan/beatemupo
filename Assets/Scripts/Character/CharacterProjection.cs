using UnityEngine;
using UnityEngine.UI;

public class CharacterProjection : MonoBehaviour
{
    [Header("Projection")]
    [SerializeField] private float projectionDistance = 4f;
    [SerializeField] GameObject projectionObject;
    [SerializeField] Animator projectionAnimator;
    [SerializeField] float projectionMaxDuration = 10f;

    private Character character;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    private Vector3 worldDirection;

    private bool active;

    public bool Active => active;

    private Animator sourceAnimator;
    private CharacterAnimator sourceCharacterAnimator;
    private int lastState = -1;

    float timer;

    public void Initialize(Character character)
    {
        this.character = character;

        sourceAnimator =
            character.GetComponentInChildren<Animator>();
        sourceCharacterAnimator =
            character.GetComponent<CharacterAnimator>();

        projectionAnimator =
            projectionObject.GetComponentInChildren<Animator>();

        originalLocalPosition =
            transform.localPosition;

        originalLocalRotation =
            transform.localRotation;

        projectionObject.SetActive(false);

        timer = projectionMaxDuration;
    }

    public void Toggle()
    {
        if (active)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void Activate()
    {
        if (active)
            return;

        active = true;
        timer = 0f;

        // Guardamos la dirección en WORLD SPACE.
        worldDirection =
            character.transform.forward;

        worldDirection.y = 0f;

        if (worldDirection.sqrMagnitude < 0.001f)
            worldDirection = Vector3.forward;

        worldDirection.Normalize();

        // Desparentamos.
        projectionObject.transform.SetParent(null);

        UpdatePosition();

        // La copia puede tener la misma orientación
        // inicial del personaje.
        projectionObject.transform.rotation =
            character.transform.rotation;

        projectionObject.SetActive(true);
        SyncAnimatorState();
    }

    private void Update()
    {
        if (!active)
            return;

        timer += Time.deltaTime;
        if (timer >= projectionMaxDuration)
        {
            Deactivate();
            return;
        }

        UpdatePosition();
        CopyAnimatorParameters();
        SyncAnimatorState();
    }

    private void UpdatePosition()
    {
        Vector3 position =
            character.transform.position +
            worldDirection * projectionDistance;

        projectionObject.transform.position = position;
    }

    public void Deactivate()
    {
        if (!active)
            return;

        active = false;

        projectionObject.transform.SetParent(
            character.transform,
            false);

        projectionObject.transform.localPosition =
            originalLocalPosition;

        projectionObject.transform.localRotation =
            originalLocalRotation;

        projectionObject.SetActive(false);
    }

    private void CopyAnimatorParameters()
    {
        if (sourceAnimator == null ||
            projectionAnimator == null)
            return;

        foreach (AnimatorControllerParameter parameter
            in sourceAnimator.parameters)
        {
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Float:

                    projectionAnimator.SetFloat(
                        parameter.nameHash,
                        sourceAnimator.GetFloat(
                            parameter.nameHash));

                    break;

                case AnimatorControllerParameterType.Int:

                    projectionAnimator.SetInteger(
                        parameter.nameHash,
                        sourceAnimator.GetInteger(
                            parameter.nameHash));

                    break;

                case AnimatorControllerParameterType.Bool:

                    projectionAnimator.SetBool(
                        parameter.nameHash,
                        sourceAnimator.GetBool(
                            parameter.nameHash));

                    break;

                case AnimatorControllerParameterType.Trigger:
                    break;
            }
        }
    }

    private void SyncAnimatorState()
    {
        if (sourceCharacterAnimator == null ||
            projectionAnimator == null)
            return;

        int state =
            sourceCharacterAnimator.CurrentState;

        if (state == lastState)
            return;

        lastState = state;

        projectionAnimator.CrossFade(
            state,
            0.05f,
            0);
    }
}