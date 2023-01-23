
using Autofac;
using AutoMapper;
using BeyondIT.MicroLoan.Api.Infrastructure.Extensions;
using BeyondIT.MicroLoan.Api.Models;
using BeyondIT.MicroLoan.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace BeyondIT.MicroLoan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    // [ServiceFilter(typeof(JwtAuthorizationAttribute))
    [ResponseCache(Duration = 5)]
    public abstract class BeyondITRestApiController<TEntity, TEntityDto> : MicroLoanController where TEntity: class where TEntityDto: class
    {
        [HttpGet(Order = 1)]
        public virtual async Task<List<TEntityDto>> Get()
        {
            return await Task.Run(() => new List<TEntityDto>());
        }
        [HttpGet("{id:int}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            return await Task.Run(() => NotFound());
        }
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TEntityDto dto)
        {
            return await Task.Run(() => Forbid());
        }
        [HttpPut("{id:int}")]
        public virtual async Task<IActionResult> Update([FromBody] TEntityDto dto, int id)
        {
            return await Task.Run(() => Forbid());
        }
        protected async Task<List<TEntityDto>> GetResourceList(Func<List<TEntity>> func)
        {
            return await GetResourceList<TEntity, TEntityDto>(func);
        }

        protected async Task<IActionResult> GetResource(Func<TEntity> func, Action<TEntity, TEntityDto> action = null)
        {
            return await GetResource<TEntity, TEntityDto>(func, action);
        }
        protected async Task<List<TDto>> GetResourceList<T, TDto>(Func<List<T>> func)
        {
            List<T> list = await Task.Run(() => func.Invoke());
            var mapper = Startup.Container.Resolve<IMapper>();
            return mapper.Map<List<TDto>>(list);
        }

        protected async Task<IActionResult> GetResource<T, TDto>(Func<T> func, Action<T, TDto> action = null)
        {
            T entity = await Task.Run(() => func.Invoke());
            if (entity == null)
            {
                return NotFound();
            }
            var mapper = Startup.Container.Resolve<IMapper>();
            var entityDto = mapper.Map<TDto>(entity);
            action?.Invoke(entity, entityDto);
            return Ok(entityDto);
        }


        protected async Task<IActionResult> CreateResource<TEntity1, TEntityDto1, TKey1>(Func<TEntity1> func,
          Expression<Func<TEntity1, TKey1>> keyExpression, string createdAtActionName = "GetById", bool includeCreatedObject = true) where TEntity1 : class
          where TEntityDto1 : class
          where TKey1 : struct
        {
            TEntity1 entity;
            if (ModelState.IsValid)
            {
                try
                {
                    entity = await Task.Run(() => func.Invoke());
                }
                catch (BeyondITLoanException exception)
                {
                    return BadRequest(new BadRequestError(exception.Message));
                }
            }
            else
            {
                return BadRequest(new BadRequestError(ModelState.GetErrors().FirstOrDefault()));
            }

            object propertyValue = null;
            var member = (MemberExpression)keyExpression.Body;
            var propInfo = (PropertyInfo)member.Member;
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propInfo.Name);
            if (propertyInfo != null)
            {
                propertyValue = propertyInfo.GetValue(entity, null);
            }

            if (includeCreatedObject)
            {
                var mapper = Startup.Container.Resolve<IMapper>();
                return CreatedAtAction(createdAtActionName, new { id = propertyValue }, mapper.Map<TEntityDto1>(entity));
            }
            return CreatedAtAction(createdAtActionName, new { id = propertyValue });
        }

        protected async Task<IActionResult> CreateResource<TKey>(Func<TEntity> func,
          Expression<Func<TEntity, TKey>> keyExpression, string createdAtActionName = "GetById",
          bool includeCreatedObject = true) where TKey : struct
        {
            return await CreateResource<TEntity, TEntityDto, TKey>(func, keyExpression, createdAtActionName,
                includeCreatedObject);
        }
        protected async Task<IActionResult> UpdateResource(Action action)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() => action.Invoke());

                }
                catch (Exception exception)
                {
                    return BadRequest(new BadRequestError(exception.Message));
                }
            }
            else
            {
                return BadRequest(new BadRequestError(ModelState.GetErrors().FirstOrDefault()));
            }

            return new NoContentResult();
        }
    }
}