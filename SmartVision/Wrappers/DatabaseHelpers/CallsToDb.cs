﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Helpers;
using Objects.ContactInformation;
using Objects.Person;

namespace FaceAnalysis
{
    public class CallsToDb
    {
        private const string getMisingPersonsUrl = "http://viltomas.eu/api/MissingPersons";

        /// <summary>
        /// Grabs missing person data from DB.
        /// </summary>
        /// <param name="matchedFace">ID of face to grab</param>
        /// <returns>Contact information</returns>
        public ContactInformation GetMissingPersonData(string matchedFace)
        {
            HttpClientWrapper wrapper = new HttpClientWrapper();
            var missingPersonsJson = wrapper.Get(getMisingPersonsUrl).Result;
            var missingPersonsList = JsonConvert.DeserializeObject<List<MissingPerson>>(missingPersonsJson);
            MissingPerson foundMissingPerson;
            try
            {
               foundMissingPerson = missingPersonsList.First(x => x.faceToken == matchedFace);
            }
            catch (InvalidOperationException)
            {
                Debug.WriteLine("No matching records in DB found");
                return null;
            }

            string missingPersonFirstName = foundMissingPerson.firstName;
            string missingPersonLastName = foundMissingPerson.lastName;

            var contactPerson = foundMissingPerson.ContactPersons.FirstOrDefault();
            string contactPersonPhoneNumber = contactPerson.phoneNumber;
            string contactPersonEmailAddress = contactPerson.emailAddress;

            return new ContactInformation(missingPersonFirstName, missingPersonLastName,
                contactPersonPhoneNumber, contactPersonEmailAddress);
        }

        /// <summary>
        /// Grabs missing person data from DB.
        /// </summary>
        /// <returns>A list of missing people</returns>
        public List<MissingPerson> GetPeopleData()
        {
            HttpClientWrapper wrapper = new HttpClientWrapper();
            var missingPersonsJson = wrapper.Get(getMisingPersonsUrl).Result;
            var missingPersonsList = JsonConvert.DeserializeObject<List<MissingPerson>>(missingPersonsJson);

            return missingPersonsList;

        }
    }
}