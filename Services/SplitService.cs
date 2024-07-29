using AutoMapper;
using ProductAPI.Commands;
using ProductAPI.Database.Entities;
using ProductAPI.Database.Repositories;

namespace ProductAPI.Services
{
    public class SplitService : ISplitService
    {
        private readonly ISplitRepository _repository;

        private readonly IMapper _mapper;

        private readonly ILogger<SplitService> _logger;
        public SplitService(ISplitRepository repository, IMapper mapper, ILogger<SplitService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> SplitTransaction(string id, List<SplitTransactionCommand> splits)
        {
            return await _repository.SplitTransaction(id, _mapper.Map<List<SplitEntity>>(splits));
        }
    }
}
