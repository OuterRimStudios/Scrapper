public class SustainedAbility : Ability {

    public Sustained beamPrefab;
    public int initialDamage;

    Sustained beam;

    protected override void Start()
    {
        base.Start();
        beam = Instantiate(beamPrefab);
        UpdateTransform();
        beam.Initialize(initialDamage, afterEffects, playerManager.transform);
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        UpdateTransform();        
        beam.gameObject.SetActive(true);

        for (int i = 0; i < GetActiveModules().Count; i++)
            beam.SetModule(GetActiveModules()[i]);
        RemoveModules();
    }

    public override void DeactivateAbility()
    {
        beam.gameObject.SetActive(false);
        UpdateTransform();
        TriggerCooldown();
    }

    void UpdateTransform()
    {
        beam.transform.SetParent(playerManager.SpawnPosition());
        beam.transform.position = playerManager.SpawnPosition().position;
        beam.transform.rotation = playerManager.SpawnPosition().rotation;
    }
}