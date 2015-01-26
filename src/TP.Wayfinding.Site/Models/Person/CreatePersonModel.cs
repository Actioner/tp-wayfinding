using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using TP.Wayfinding.Site.Components.Validators.Person;

namespace TP.Wayfinding.Site.Models.Person
{
    [Validator(typeof(CreatePersonValidator))]
    public class CreatePersonModel : PersonModel
    {
    }
}