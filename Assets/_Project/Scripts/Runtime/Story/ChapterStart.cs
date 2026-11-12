using IAmAsIAm.Runtime.Interaction;
using UnityEngine;

namespace IAmAsIAm.Runtime.Story
{
    /// <summary>
    /// Something in the world that opens a chapter's start screen (content warnings, who to start as,
    /// or a summary instead). In the test room it's a door marked with the chapter.
    /// </summary>
    public sealed class ChapterStart : MonoBehaviour, IInteractable
    {
        [SerializeField] string chapterId = "prologue-quaker";
        [SerializeField] string prompt = "Begin the Prologue";

        public string Prompt => prompt;
        public bool CanInteract => !string.IsNullOrEmpty(chapterId);

        public void Interact()
        {
            var screen = FindAnyObjectByType<StoryScreen>();
            if (screen != null) screen.OpenChapter(chapterId);
        }
    }
}
