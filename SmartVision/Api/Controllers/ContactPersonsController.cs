﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Api.Models;

namespace Api.Controllers
{
    public class ContactPersonsController : ApiController
    {
        private pstop2018Entities1 db = new pstop2018Entities1();

        // GET: api/ContactPersons
        public IQueryable<ContactPerson> GetContactPersons()
        {
            return db.ContactPersons;
        }

        // GET: api/ContactPersons/5
        [ResponseType(typeof(ContactPerson))]
        public IHttpActionResult GetContactPerson(int id)
        {
            ContactPerson contactPerson = db.ContactPersons.Find(id);
            if (contactPerson == null)
            {
                return NotFound();
            }

            return Ok(contactPerson);
        }

        // PUT: api/ContactPersons/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContactPerson(int id, ContactPerson contactPerson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactPerson.Id)
            {
                return BadRequest();
            }

            db.Entry(contactPerson).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactPersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ContactPersons
        [ResponseType(typeof(ContactPerson))]
        public IHttpActionResult PostContactPerson(ContactPerson contactPerson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ContactPersons.Add(contactPerson);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = contactPerson.Id }, contactPerson);
        }

        // DELETE: api/ContactPersons/5
        [ResponseType(typeof(ContactPerson))]
        public IHttpActionResult DeleteContactPerson(int id)
        {
            ContactPerson contactPerson = db.ContactPersons.Find(id);
            if (contactPerson == null)
            {
                return NotFound();
            }

            db.ContactPersons.Remove(contactPerson);
            db.SaveChanges();

            return Ok(contactPerson);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactPersonExists(int id)
        {
            return db.ContactPersons.Count(e => e.Id == id) > 0;
        }
    }
}