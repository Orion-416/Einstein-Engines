using Content.Shared.Containers.ItemSlots;

namespace Content.Shared._EE.ModsuitModules.Components;

[RegisterComponent]
public sealed partial class ModsuitModulesSlotComponent : Component
{
    /// <summary>
    /// The actual item-slot that contains the module. Allows all the interaction logic to be handled by <see cref="ItemSlotsSystem"/>.
    /// </summary>

    [DataField("ModuleSlot1Id", required: true)]
    public string ModuleSlot1Id = string.Empty;
    [DataField("ModuleSlot2Id", required: true)]
    public string ModuleSlot2Id = string.Empty;
    [DataField("ModuleSlot3Id", required: true)]
    public string ModuleSlot3Id = string.Empty;
    [DataField("ModuleSlot4Id", required: true)]
    public string ModuleSlot4Id = string.Empty;

}

/// <summary>
///     Raised directed at an entity with a power cell slot when the power cell inside has its charge updated or is ejected/inserted.
/// </summary>
public sealed class ModsuitModulesChangedEvent : EntityEventArgs
{
    public readonly bool Ejected;

    public ModsuitModulesChangedEvent(bool ejected)
    {
        Ejected = ejected;
    }
}