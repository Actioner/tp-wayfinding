using FluentValidation.Attributes;
using IDB.Navigator.Site.Components.Validators.Connection;

namespace IDB.Navigator.Site.Areas.Admin.Models.Connection
{
    [Validator(typeof(CreateConnectionValidator))]
    public class CreateConnectionModel : ConnectionModel
    {
    }
}