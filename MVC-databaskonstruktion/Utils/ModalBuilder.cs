using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_databaskonstruktion.Utils
{
    public class ModalBuilder
    {
        private Modal _modal = new Modal { Inputs = new List<ModalInput>() };

        public ModalBuilder WithTitle(string title)
        {
            _modal.Title = title;
            return this;
        }

        public ModalBuilder AddInput(string id, string label, string type, string placeholder = "", IEnumerable<SelectListItem> dropdownItems = null)
        {
            _modal.Inputs.Add(new ModalInput { Id = id, Label = label, Type = type, Placeholder = placeholder, DropdownItems = dropdownItems });
            return this;
        }

        public Modal Build()
        {
            return _modal;
        }
    }
}
