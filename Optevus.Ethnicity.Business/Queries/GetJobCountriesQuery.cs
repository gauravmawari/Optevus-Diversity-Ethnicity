using MediatR;
using Optevus.Ethnicity.Model.Response;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetJobCountriesQuery : IRequest<List<Country>>
    {
    }

}
