using BusinessLogic.Interface;
using DataAccess.CustomModels;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.Json;




namespace BusinessLogic.Service
{
    public class EMSService : IEMService
    {
        private readonly ApplicationDbContext _db;


        public EMSService(ApplicationDbContext db)
        {
            _db = db;

        }


        public List<Records> EmsRecords()
        {

                var emprecords = from t1
                                 in _db.Employees
                                 join t2 in _db.Departments on t1.Departmentid equals t2.Departmentid

                                 select new Records
                                 {
                                     firstname = t1.Firstname,
                                     lastname = t1.Lastname,
                                     email = t1.Email,
                                     employeeid=t1.Employeeid,
                                     age = t1.Age,
                                     education = t1.Education,
                                     departmentname = t2.Name,
                                     company = t1.Company,
                                     experience = t1.Experience,
                                     package =t1.Package,

                              

                            };
                var result1 = emprecords.ToList();
                return result1;
            }


       

        public bool DeleteRecord(int employeeid)
        {
            try
            {
                var emprecord = _db.Employees.FirstOrDefault(x => x.Employeeid == employeeid);
                _db.Employees.Remove(emprecord);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public Records AddEmployees(Records model)
        {
              Employee emp = new Employee();


                
                emp.Firstname = model.firstname;
                emp.Lastname = model.lastname;
                emp.Email = model.email;
                emp.Age = model.age;
                emp.Departmentid = 4;
                emp.Package = model.package;
                emp.Company = model.company;
                emp.Education = model.education;
                emp.Experience = model.experience;

                _db.Employees.Add(emp);
                _db.SaveChanges();

            return model;
            


        }


        public Records EditEmp(int employeeid)
        {
            var emprecord = _db.Employees.FirstOrDefault(x => x.Employeeid == employeeid);
            if (emprecord != null)
            {
                var rc = new Records()
                {
                    firstname = emprecord.Firstname,
                   lastname=emprecord.Lastname,
                   email=emprecord.Email,
                   age=emprecord.Age,
                   education=emprecord.Education,
                   company=emprecord.Company,
                   experience=emprecord.Experience,
                   package=emprecord.Package,

                };
                return rc;
            }
            return null;
        }









    }
}