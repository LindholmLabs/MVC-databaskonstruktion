using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_databaskonstruktion.Utils
{
    public class ModalBuilder
    {
        private ModalContext _modal = new ModalContext { Inputs = new List<ModalInput>() };

        public ModalBuilder SetTitle(string title)
        {
            _modal.Title = title;
            return this;
        }

        public ModalBuilder SetIdentifier(string identifier)
        {
            _modal.Identifier = identifier;
            return this;
        }

        public ModalBuilder SetAction(string action, string controller)
        {
            _modal.Action = action;
            _modal.Controller = controller;
            return this;
        }

        public ModalBuilder AddInput(string id, string label, string type, string placeholder = "", IEnumerable<SelectListItem> dropdownItems = null)
        {
            _modal.Inputs.Add(new ModalInput { Id = id, Label = label, Type = type, Placeholder = placeholder, DropdownItems = dropdownItems });
            return this;
        }

        public ModalContext Build()
        {
            return _modal;
        }
    }
}
