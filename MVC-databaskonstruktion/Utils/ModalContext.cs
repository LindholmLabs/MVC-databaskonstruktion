﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_databaskonstruktion.Utils
{
    public class ModalContext
    {
        public string Title { get; set; }
        public string Identifier { get; set; }
        public string Action { get; set; }
        public string? Controller { get; set; }
        public List<ModalInput> Inputs { get; set; }
    }

    public class ModalInput
    {
        public string Id { get; set; }
        public string? Label { get; set; }
        public string Type { get; set; }
        public IEnumerable<SelectListItem>? DropdownItems { get; set; } // For dropdown type
        public string ?Placeholder { get; set; }
    }
}
