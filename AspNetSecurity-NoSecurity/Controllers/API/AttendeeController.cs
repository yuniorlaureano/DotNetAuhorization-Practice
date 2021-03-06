﻿using AspNetSecurity_NoSecurity.Models;
using AspNetSecurity_NoSecurity.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSecurity_NoSecurity.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowBankCom")]
    public class AttendeeController: Controller
    {
        private readonly AttendeeRepo repo;

        public AttendeeController(AttendeeRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public AttendeeModel Get(int id)
        {
            return repo.GetById(id);
        }

        [HttpPost]
        public IActionResult Post(int conferenceId, string name)
        {
            var attendee = repo.Add(new AttendeeModel {ConferenceId = conferenceId, Name = name});
            return new CreatedAtActionResult("Get", "Attendee", new {id = attendee.Id}, attendee);
        }
    }
}
