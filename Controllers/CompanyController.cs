using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq; 

namespace PracticeCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CompanyController : Controller
    {
        private readonly List<Companies> _companyRepository;
        public CompanyController()
        {
            _companyRepository = new List<Companies>{
           new Companies(){ Id = 1,Name = "Shah" },
           new Companies(){ Id = 2,Name = "Tajdar" },
           new Companies(){ Id = 3,Name = "Alamzeb" }};
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            return Ok(_companyRepository);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var getCompany = _companyRepository.Find(x => x.Id == id);
            return Ok(getCompany);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanies(Companies companies)
        {
            
            _companyRepository.Add(companies);
            return CreatedAtAction("GetCompanyById", new {id=companies.Id}, companies);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanies(int id, Companies companies)
        {
            var getCompany = _companyRepository.Where(x => x.Id == id).FirstOrDefault();
            getCompany.Name = companies.Name;
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanies(int id)
        {
            var getCompany = _companyRepository.Where(x => x.Id == id).FirstOrDefault();
            _companyRepository.Remove(getCompany);
            return Ok(_companyRepository);
        }

    }


}