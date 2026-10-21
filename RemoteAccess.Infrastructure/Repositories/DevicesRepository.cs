using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Repositories;
using RemoteAccess.Infrastructure.Database;

namespace RemoteAccess.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для работы с девайсами
    /// </summary>
    public class DevicesRepository:IDevicesRepository
    {
        private readonly AppDbContext _context;
        public DevicesRepository(AppDbContext appDbContext){ _context = appDbContext; }

        public async Task CreateDevice(Device entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
        }
        public Task<List<Device>> GetDeviceByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<Device> GetDeviceById(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
