using Swashbuckle.AspNetCore.Annotations;
using VebTech.Domain.Models.EnumModels;

namespace VebTech.Domain.Models.Parameters;

public class SortParameters
{
    public string OrderBy { get; set; } = string.Empty;

    [SwaggerParameter("0 - ascending, 1 - descending", Required = false)]
    public SortDirection SortOrder { get; set; }
}