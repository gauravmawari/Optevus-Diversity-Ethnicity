using MediatR;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business.Commands
{
    public class SaveAssociationCommandHandler : IRequestHandler<SaveAssociationCommand, bool>
    {
        private readonly IJobRepository _jobRepository;
        public SaveAssociationCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<bool> Handle(SaveAssociationCommand request, CancellationToken cancellationToken)
        {
            return await _jobRepository.SaveAssociationAsync(request.Associations);
        }
    }
}
