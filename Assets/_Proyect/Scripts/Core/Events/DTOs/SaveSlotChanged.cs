namespace Project.Core.Events.DTOs
{
    public struct SaveSlotChanged
    {
        public string slotId;
        public SaveSlotChanged(string slotId) { this.slotId = slotId; }
    }
}
