using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorAppExcel.Components.atomic
{
    public class DialogMessageSimple :IDialogContentComponent
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
