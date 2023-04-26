using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContest.Dtos.Categories;
using PhotoContest.Exceptions;
using PhotoContest.Helpers;
using PhotoContest.Models.Mappers;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Services.CategoriesService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Controllers.Api
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {        
        private readonly ICategoriesService categoriesService;
        private readonly IAuthorizationHelper authorizationHelper;        
        private readonly ICategoryMapper categoryMapper;

        public CategoriesApiController(ICategoriesService categoriesService, ICategoryMapper categoryMapper, IAuthorizationHelper authorizationHelper)
        {          
            this.categoriesService = categoriesService;
            this.authorizationHelper = authorizationHelper;         
            this.categoryMapper = categoryMapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromHeader] string userLogName, [FromHeader] string password)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName,password);

                var categories = await this.categoriesService.GetAll(userLog);

                var categoriesToView = this.categoryMapper.ConvertCategoriesToCategoriesResponseDto(categories);

                return Ok(categoriesToView);
            }
            catch (AuthorizationException e) 
            {
                return NotFound(e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidInputException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromHeader] string userLogName, [FromHeader] string password, [FromBody] CategoryRequestDto dto)
        {
            try
            {
                var user = await this.authorizationHelper.TryGetUser(userLogName,password);
                if (user.Role.Name != "admin")  
                {
                    throw new UnauthorizedException("Only organazator can create the category");
                }

                var category = this.categoryMapper.ConvertCategoryRequestDtoToCategoryForCreate(dto);

                var createdCategory = await this.categoriesService.Create(category, user);
                var categoryToView = this.categoryMapper.ConvertCategoryToCategoryResponseDto(createdCategory);

                return this.StatusCode(StatusCodes.Status201Created, categoryToView);
            }
            catch (UnauthorizedException e)
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (AuthorizationException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromHeader] string userLogName, [FromHeader] string password, [FromBody] CategoryRequestDto dto)
        {
            try
            {
                var user = await this.authorizationHelper.TryGetUser(userLogName,password);
                if (user.Role.Name != "admin") 
                {
                    throw new UnauthorizedException("Only organazator can update the category");
                }

                var oldCategoryToUpdate = await this.categoriesService.GetById(id);               

                var categoryToUpdate = this.categoryMapper.ConvertCategoryRequestDtoToCategoryForUpdate(dto, oldCategoryToUpdate);

                var updatedCategory = await this.categoriesService.Update(id, categoryToUpdate, user);

                var categoryToView = this.categoryMapper.ConvertCategoryToCategoryResponseDto(updatedCategory);

                return this.StatusCode(StatusCodes.Status201Created, categoryToView);
            }
            catch (UnauthorizedException e)
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (AuthorizationException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}
