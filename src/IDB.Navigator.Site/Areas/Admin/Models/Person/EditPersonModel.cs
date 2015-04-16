using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using IDB.Navigator.Site.Components.Validators.Person;

namespace IDB.Navigator.Site.Areas.Admin.Models.Person
{
    [Validator(typeof(EditPersonValidator))]
    public class EditPersonModel : PersonModel
    {
    }
}