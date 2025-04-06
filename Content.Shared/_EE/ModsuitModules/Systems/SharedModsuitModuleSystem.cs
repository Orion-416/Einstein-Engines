using Content.Shared.Containers.ItemSlots;
using Content.Shared._EE.ModsuitModules.Components;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;

namespace Content.Shared._EE.ModsuitModules.Systems;

public abstract class SharedModsuitModuleSystem : EntitySystem
{
    [Dependency] protected readonly IGameTiming Timing = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly IComponentFactory _compFactory = default!;
    [Dependency] private readonly ISerializationManager _serManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ModsuitModulesSlotComponent, EntInsertedIntoContainerMessage>(OnModuleInserted);
        SubscribeLocalEvent<ModsuitModulesSlotComponent, EntRemovedFromContainerMessage>(OnModuleRemoved);
        SubscribeLocalEvent<ModsuitModulesSlotComponent, ContainerIsInsertingAttemptEvent>(OnModuleInsertAttempt);
    }
    private void AddComponents(EntityUid modsuit,
        ComponentRegistry reg)
    {
        foreach (var (key, comp) in reg)
        {
            var compType = comp.Component.GetType();
            if (HasComp(modsuit, compType))
                continue;

            var newComp = (Component) _serManager.CreateCopy(comp.Component, notNullableOverride: true);
            newComp.Owner = modsuit;
            EntityManager.AddComponent(modsuit, newComp, true);
            if (newComp.NetSyncEnabled)
            {
                Dirty(modsuit, newComp);
            }
        }
    }

    private void RemoveComponents(EntityUid modsuit,
        ComponentRegistry reg)
    {
        foreach (var (key, comp) in reg)
        {
            RemComp(modsuit, comp.Component.GetType());
        }
    }
    private void OnModuleInsertAttempt(EntityUid uid, ModsuitModulesSlotComponent component, ContainerIsInsertingAttemptEvent args)
    {
        if (!component.Initialized)
            return;

        if (args.Container.ID != component.ModuleSlot1Id) /// I don't really know what im doing so this is how it will be for now.
            return;
        if (args.Container.ID != component.ModuleSlot2Id)
            return;
        if (args.Container.ID != component.ModuleSlot3Id)
            return;
        if (args.Container.ID != component.ModuleSlot4Id)
            return;

        if (!HasComp<ModsuitModuleComponent>(args.EntityUid))
        {
            args.Cancel();
        }
    }

    private void OnModuleInserted(EntityUid uid, ModsuitModulesSlotComponent component, EntInsertedIntoContainerMessage args)
    {
        if (!component.Initialized)
            return;

        if (args.Container.ID != component.ModuleSlot1Id) /// I don't really know what im doing so this is how it will be for now.
            return;
        if (args.Container.ID != component.ModuleSlot2Id)
            return;
        if (args.Container.ID != component.ModuleSlot3Id)
            return;
        if (args.Container.ID != component.ModuleSlot4Id)
            return;

        RaiseLocalEvent(uid, new ModsuitModulesChangedEvent(false), false);
        AddComponents(uid, args.Entity.Components.ModsuitModuleComponent.OnAdd);
    }

    protected virtual void OnModuleRemoved(EntityUid uid, ModsuitModulesSlotComponent component, EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != component.ModuleSlot1Id) /// I don't really know what im doing so this is how it will be for now.
            return;
        if (args.Container.ID != component.ModuleSlot2Id)
            return;
        if (args.Container.ID != component.ModuleSlot3Id)
            return;
        if (args.Container.ID != component.ModuleSlot4Id)
            return;
        RaiseLocalEvent(uid, new ModsuitModulesChangedEvent(true), false);
        RemoveComponents(uid, args.Entity.Components.ModsuitModuleComponent.OnAdd);
    }
    //
    /// <summary>
    /// Returns whether the entity has a slotted battery and <see cref="PowerCellDrawComponent.UseRate"/> charge.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="battery"></param>
    /// <param name="cell"></param>
    /// <param name="user">Popup to this user with the relevant detail if specified.</param>
    ///public abstract bool HasActivatableCharge(
    ///    EntityUid uid,
    ///    PowerCellDrawComponent? battery = null,
    ///    PowerCellSlotComponent? cell = null,
    ///    EntityUid? user = null);
    /// 
    /// <summary>
    /// Whether the power cell has any power at all for the draw rate.
    /// </summary>
    ///public abstract bool HasDrawCharge(
    ///    EntityUid uid,
    ///    PowerCellDrawComponent? battery = null,
    ///    PowerCellSlotComponent? cell = null,
    ///    EntityUid? user = null);
    
}
