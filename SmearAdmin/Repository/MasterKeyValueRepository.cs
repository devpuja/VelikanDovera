using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Repository
{
    public class MasterKeyValueRepository : Repositories<MasterKeyValue>, IMasterKeyValueRepository
    {
        public MasterKeyValueRepository(SmearAdminDbContext context) : base(context){ }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;

        public async Task<PagingResult<MasterKeyValue>> GetAllMastersAsync(string masterFor, int pageIndex, int pageSize)
        {
            //int pageNum = Convert.ToInt32(newPage);
            int totalCount = 0, totalPage = 0;
            pageIndex += 1;

            totalCount = _appDbContext.MasterKeyValue
                .Where(m => m.Type.Equals(masterFor))
                .ToList()
                .Count();

            var data= await _appDbContext.MasterKeyValue
                .Where(m => m.Type.Equals(masterFor))
                .OrderBy(o => o.Value)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync().ConfigureAwait(false);

            totalPage = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

            var pagingData = new PagingResult<MasterKeyValue>
            {
                Items = data.AsEnumerable(),
                TotalCount = totalCount,
                TotalPage = totalPage
            };
            
            return await Task.FromResult(pagingData);
        }

        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForEmployeeAsync()
        {
            var dataDep = await _appDbContext.MasterKeyValue
                .Where(dep => dep.Type.Equals(EmployeeConstant.Department)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataDes = await _appDbContext.MasterKeyValue
                .Where(des => des.Type.Equals(EmployeeConstant.Desigination)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataGrade = await _appDbContext.MasterKeyValue
                .Where(g => g.Type.Equals(EmployeeConstant.Grade)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataHQ = await _appDbContext.MasterKeyValue
                .Where(h => h.Type.Equals(EmployeeConstant.HQ)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataRegion = await _appDbContext.MasterKeyValue
                .Where(r => r.Type.Equals(EmployeeConstant.Region)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            return new MasterKeyValueResult<MasterKeyValue>
            {
                ItemsDep = dataDep,
                ItemsDes = dataDes,
                ItemsGrade = dataGrade,
                ItemsHQ = dataHQ,
                ItemsRegion = dataRegion
            };
        }

        //public async Task<(List<MasterKeyValue> dep, List<MasterKeyValue> des, List<MasterKeyValue> grade, List<MasterKeyValue> region)> GetMastersForEmployeeAsync()
        //{
        //    var dataDep = await _appDbContext.MasterKeyValue
        //        .Where(dep => dep.Type.Equals(Constants.EmployeeConstant.Department)).OrderBy(o => o.Value).ToListAsync();

        //    var dataDes = await _appDbContext.MasterKeyValue
        //        .Where(des => des.Type.Equals(Constants.EmployeeConstant.Desigination)).OrderBy(o => o.Value).ToListAsync();

        //    var dataGrade = await _appDbContext.MasterKeyValue
        //        .Where(g => g.Type.Equals(Constants.EmployeeConstant.Grade)).OrderBy(o => o.Value).ToListAsync();

        //    var dataRegion = await _appDbContext.MasterKeyValue
        //        .Where(r => r.Type.Equals(Constants.EmployeeConstant.Region)).OrderBy(o => o.Value).ToListAsync();

        //    return (dataDep, dataDes, dataGrade, dataRegion);
        //}

        //public void CreateMasterKeyValue(MasterKeyValue masterKeyValue)
        //{
        //    _appDbContext.MasterKeyValue.Add(masterKeyValue);
        //}

        //public Task<IEnumerable<MasterKeyValueViewModel>> GetMasterKeyValue(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<MasterKeyValueViewModel> GetMasterKeyValue(int count)
        //{
        //    return _appDbContext.MasterKeyValue.OrderByDescending(c => c.Id).Take(count).ToList();
        //}

        //public IEnumerable<MasterKeyValueViewModel> GetCoursesWithAuthors(int pageIndex, int pageSize = 10)
        //{
        //    return _appDbContext.MasterKeyValue
        //        .OrderBy(c => c.ValueName)
        //        .Skip((pageIndex - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();
        //}

        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForDoctorAsync()
        {
            var dataQualification = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.Qualification)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataSpeciality = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.Speciality)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataBrand = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.Brand)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataClass = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.Class)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataBestDayToMeet = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.BestDayToMeet)).ToListAsync().ConfigureAwait(false);

            var dataBestTimeToMeet = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.BestTimeToMeet)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataGender = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.Gender)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataVisitFrequency = await (from mst in _appDbContext.MasterKeyValue
                                  where mst.Type.Equals(EmployeeConstant.VisitFrequency)
                                  select mst.Value).FirstOrDefaultAsync();

            var dataCommunity = await _appDbContext.MasterKeyValue
                .Where(r => r.Type.Equals(DoctorConstant.Community)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            return new MasterKeyValueResult<MasterKeyValue>
            {
                ItemsQualification = dataQualification,
                ItemsSpeciality = dataSpeciality,
                ItemsBrand = dataBrand,
                ItemsClass = dataClass,
                ItemsBestDayToMeet = dataBestDayToMeet,
                ItemsBestTimeToMeet = dataBestTimeToMeet,
                ItemsGender = dataGender,
                ItemVisitFrequency = dataVisitFrequency,
                ItemsCommunity = dataCommunity
            };
        }

        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForChemistAsync()
        {
            var dataClass = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.Class)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataRegion = await _appDbContext.MasterKeyValue
                .Where(r => r.Type.Equals(ChemistConstant.Region)).ToListAsync().ConfigureAwait(false);

            var dataBestTimeToMeet = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.BestTimeToMeet)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);
                        
            var dataCommunity = await _appDbContext.MasterKeyValue
                .Where(r => r.Type.Equals(DoctorConstant.Community)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            return new MasterKeyValueResult<MasterKeyValue>
            {
                ItemsClass = dataClass,
                ItemsRegion = dataRegion,
                ItemsBestTimeToMeet = dataBestTimeToMeet,                
                ItemsCommunity = dataCommunity
            };
        }

        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForStockistAsync()
        {
            var dataRegion = await _appDbContext.MasterKeyValue
                .Where(r => r.Type.Equals(ChemistConstant.Region)).ToListAsync().ConfigureAwait(false);

            var dataBestTimeToMeet = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(DoctorConstant.BestTimeToMeet)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataCommunity = await _appDbContext.MasterKeyValue
                .Where(r => r.Type.Equals(DoctorConstant.Community)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            return new MasterKeyValueResult<MasterKeyValue>
            {
                ItemsRegion = dataRegion,
                ItemsBestTimeToMeet = dataBestTimeToMeet,
                ItemsCommunity = dataCommunity
            };
        }

        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForHolidayAsync()
        {
            var dataCommunity = await _appDbContext.MasterKeyValue
                .Where(r => r.Type.Equals(DoctorConstant.Community)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            return new MasterKeyValueResult<MasterKeyValue>
            {
                ItemsCommunity = dataCommunity
            };
        }
    }
}
