using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorkInterfaces;
using AuthServer.Service.MapProfile;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericService<T, Tdto> : IGenericService<T, Tdto> where T : class where Tdto : class
    {
        private readonly IGenericRepository<T> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GenericService(IGenericRepository<T> genericRepository, IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<Tdto>> TCreateAsync(Tdto tdto)
        {
            var newEntity = ObjectMapper.Mapper.Map<T>(tdto);
            await _genericRepository.CreateAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = ObjectMapper.Mapper.Map<Tdto>(newEntity);
            return ResponseDto<Tdto>.Success(newDto, 200);
        }

        public async Task<ResponseDto<NoContentDto>> TDeleteAsync(int id)
        {
            var findValue = await _genericRepository.GetByIdAsync(id);
            if(findValue is null)
            {
              return ResponseDto<NoContentDto>.Fail($" {id}'ye Ait Data Bulunamadı", 404, true);
            }
            _genericRepository.Delete(findValue);
            await _unitOfWork.CommitAsync();
            return ResponseDto<NoContentDto>.Success(204);
        }

        public async Task<ResponseDto<IEnumerable<Tdto>>> TGetByFilter(Expression<Func<T, bool>> filter)
        {
            var values =await _genericRepository.GetByFilter(filter).ToListAsync();
            var newDtos = ObjectMapper.Mapper.Map<List<Tdto>>(values);
            return ResponseDto<IEnumerable<Tdto>>.Success(newDtos, 200);
        }

        public async Task<ResponseDto<Tdto>> TGetByIdAsync(int id)
        {
            var findValue=await _genericRepository.GetByIdAsync(id);
            if (findValue is  null)
            {
                return ResponseDto<Tdto>.Fail($" {id}'ye Ait Data Bulunamadı", 404, true);
            }
            var newDto = ObjectMapper.Mapper.Map<Tdto>(findValue);
            return ResponseDto<Tdto>.Success(newDto, 200);
        }

        public async Task<ResponseDto<IEnumerable<Tdto>>> TGetListAllAsync()
        {
            var values = await _genericRepository.GetListAll().ToListAsync();
            var newDto = ObjectMapper.Mapper.Map<List<Tdto>>(values);
            return ResponseDto<IEnumerable<Tdto>>.Success(newDto, 200);
        }

        public async Task<ResponseDto<NoContentDto>> TUpdateAsync(Tdto tdto,int id)
        {
            var findValue = await _genericRepository.GetByIdAsync(id);
            if(findValue is null)
            {
                return ResponseDto<NoContentDto>.Fail($" {id}'ye Ait Data Bulunamadı", 404, true);
            }
            //benim göndereceğim parametrelerimi Entitye Çevir
            var newDto = ObjectMapper.Mapper.Map<T>(tdto);
            _genericRepository.Update(newDto);
            await _unitOfWork.CommitAsync();
            return ResponseDto<NoContentDto>.Success(204);

        }
    }
}
