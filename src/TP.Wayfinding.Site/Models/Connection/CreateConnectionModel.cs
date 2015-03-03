using FluentValidation.Attributes;
using TP.Wayfinding.Site.Components.Validators.Connection;

namespace TP.Wayfinding.Site.Models.Connection
{
    [Validator(typeof(CreateConnectionValidator))]
    public class CreateConnectionModel : ConnectionModel
    {
    }
}