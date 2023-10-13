using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_databaskonstruktion.Utils
{
    public class Modal
    {
        public string Title { get; set; }
        public List<ModalInput> Inputs { get; set; }
    }

    public class ModalInput
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public IEnumerable<SelectListItem> DropdownItems { get; set; } // For dropdown type
        public string Placeholder { get; set; }
    }
}
