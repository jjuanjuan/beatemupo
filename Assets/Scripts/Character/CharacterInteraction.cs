using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Faction faction;

    [SerializeField] private float talkDistance = 2f;
    [SerializeField] private float targetRefreshTime = 0.1f;

    private float targetRefreshTimer = 1f;
    private CharacterContext context;
    private Character target;
    public Character Target => target;
    public Faction Faction => faction;

    public void Initialize(
        CharacterContext context)
    {
        this.context = context;
    }

    public void Tick()
    {
        targetRefreshTimer -= Time.deltaTime;

        if (targetRefreshTimer <= 0f)
        {
            targetRefreshTimer =
                targetRefreshTime;

            FindTarget();
        }

        if (target == null) return;

        float distance =
            Vector3.Distance(
                transform.position,
                target.transform.position);

        if (distance <= talkDistance)
        {
            HandleTalkDistance();
        }

    }

    private void FindTarget()
    {
        target =
            context.Targeting.FindClosestCharacter(
                talkDistance);
    }

    private void HandleTalkDistance()
    {
        if (!CanTalkTo(target))
            return;

        if (context.Brain.InteractPressed)
        {
            context.Motor.FaceTarget(
                target, false);
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        Debug.Log(
            $"{name} starts dialogue with {target.name}");
    }

    private bool CanTalkTo(Character target)
    {
        if (target == null)
            return false;

        return !AreEnemies(target);
    }

    private bool AreEnemies(Character target)
    {
        CharacterInteraction targetInteraction =
            target.GetComponent<CharacterInteraction>();

        if (targetInteraction == null)
            return false;

        return faction == Faction.Enemy &&
               targetInteraction.Faction == Faction.Player
            ||
               faction == Faction.Player &&
               targetInteraction.Faction == Faction.Enemy;
    }
}