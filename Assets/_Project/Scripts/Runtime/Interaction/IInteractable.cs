namespace IAmAsIAm.Runtime.Interaction
{
    public interface IInteractable
    {
        /// <summary>Short verb phrase for the prompt: "Hold the stone", "Smell the lavender".</summary>
        string Prompt { get; }
        bool CanInteract { get; }
        void Interact();
    }
}
