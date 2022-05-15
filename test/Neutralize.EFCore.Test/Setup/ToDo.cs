using Neutralize.Models;

namespace Neutralize.EFCore.Test.Setup
{
    public class ToDo : Entity<int>
    {
        public bool Done { get; set; }
        public string Desacription { get; set; }
    }
}