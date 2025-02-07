using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;

namespace Reports.Service
{
    public class DescriptionService : IDescriptionService
    {
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DescriptionService(IDescriptionRepository descriptionRepository, IUnitOfWork unitOfWork) 
        {
            _descriptionRepository = descriptionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task AddDescription(string description, string collectionName)
        {
            var gender = collectionName.Contains("Female", StringComparison.OrdinalIgnoreCase) ? "Female" : "Male";

            var savedDescription = new Description
            {
                Gender = gender
                
            };

            switch (collectionName)
            {
                case "Satisfatório":
                    savedDescription.AtendedDescription = description;
                    break;

                case "Parcialmente satisfatório":
                    savedDescription.ParciallyAtendedDescription = description;
                    break;

                case "Insatisfatório":
                    savedDescription.UnatendedDescription = description;
                    break;

                default:
                    throw new ArgumentException($"Collection name '{collectionName}' is not valid.", nameof(collectionName));
            }
             _descriptionRepository.Add(savedDescription);

            await _unitOfWork.SaveChangesAsync();        
        }

    }
}