using System.Text.Json;

namespace BoutiqueEnLigne.Models
{
    public class FavorisViewModel
    {
        public List<Dictionary<string, JsonElement>> Items { get; set; } = new List<Dictionary<string, JsonElement>>();
    }
} 