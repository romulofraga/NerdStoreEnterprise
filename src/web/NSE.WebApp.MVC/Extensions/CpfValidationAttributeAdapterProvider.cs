using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace NSE.WebApp.MVC.Extensions;

public class CpfValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

    public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
    {
        if (attribute is CpfAttribute CpfAttribute) return new CpfAttributeAdapter(CpfAttribute, stringLocalizer);

        return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }
}