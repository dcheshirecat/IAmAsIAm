using System.Linq;
using System.Text;
using IAmAsIAm.Core.Safety;
using TMPro;
using UnityEngine;

namespace IAmAsIAm.Runtime.Safety
{
    /// <summary>
    /// Lists crisis resources, with the player's region first and the international directory
    /// always included. Links open in the system browser. On consoles, where a browser may not
    /// be available, URLs are shown as text.
    /// </summary>
    public sealed class CrisisResourcesView : MonoBehaviour
    {
        [SerializeField] TMP_Text body;
        [Tooltip("ISO region code. If empty, it's guessed from the system language and can be changed in settings.")]
        [SerializeField] string regionOverride;

        void OnEnable() => Render();

        public void Render()
        {
            var data = GameServices.Crisis;
            if (body == null || data == null) return;

            string region = string.IsNullOrEmpty(regionOverride) ? GuessRegion() : regionOverride;
            var ordered = data.resources
                .OrderByDescending(r => r.region == region)
                .ThenBy(r => r.region == "INTL" ? 0 : 1);

            var sb = new StringBuilder();
            sb.AppendLine(data.disclaimer).AppendLine();
            foreach (var r in ordered)
            {
                if (r.region != region && r.region != "INTL") continue;
                sb.Append("<b>").Append(r.name).AppendLine("</b>");
                if (!string.IsNullOrEmpty(r.contact)) sb.AppendLine(r.contact);
                if (!string.IsNullOrEmpty(r.url)) sb.Append("<link=\"").Append(r.url).Append("\"><u>").Append(r.url).AppendLine("</u></link>");
                if (!string.IsNullOrEmpty(r.notes)) sb.AppendLine(r.notes);
                sb.AppendLine();
            }
            body.text = sb.ToString();
        }

        /// <summary>Hook this to a TMP link click handler.</summary>
        public void OpenLink(string url)
        {
            if (GameServices.Crisis.resources.Any(r => r.url == url)) Application.OpenURL(url);
        }

        /// <summary>
        /// Uses the OS region where the platform reports one. Otherwise only the international
        /// directory is shown. Players can set their region in settings (PreferencesData.crisisRegion).
        /// Never guess from language: an English speaker could be anywhere.
        /// </summary>
        static string GuessRegion()
        {
            try
            {
                string code = System.Globalization.RegionInfo.CurrentRegion.TwoLetterISORegionName;
                return string.IsNullOrEmpty(code) || code == "IV" ? "INTL" : code.ToUpperInvariant();
            }
            catch (System.Exception)
            {
                return "INTL";
            }
        }
    }
}
