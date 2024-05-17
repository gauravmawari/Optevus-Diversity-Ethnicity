using MediatR;
using Optevus.Ethnicity.Model.Response;

namespace Optevus.Ethnicity.Business.Commands
{
    public class SaveAssociationCommand : IRequest<bool>
    {
        public List<ApplicantAssociation> Associations { get; }
        public SaveAssociationCommand(List<ApplicantAssociation> associations)
        {
            Associations = associations;
        }
    }

}
