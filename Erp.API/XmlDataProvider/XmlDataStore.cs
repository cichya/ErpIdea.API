﻿using Erp.API.Models;
using Erp.API.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Erp.API.XmlDataProvider
{
	public class XmlDataStore : IDataStore<EmployeeModel>
	{
		private readonly IXmlService xmlService;
		private readonly string filePath;
		private readonly IList<EmployeeModel> employees;

		public XmlDataStore(IXmlService xmlService, string filePath)
		{
			this.xmlService = xmlService;
			this.filePath = filePath;
			this.employees = this.LoadXml();
		}

		public void Add(EmployeeModel entity)
		{
			lock (this)
			{
				this.employees.Add(entity);
			}
		}

		public void Update(EmployeeModel entity)
		{
			lock (this)
			{
				var elementToUpdate = this.employees.FirstOrDefault(x => x.Id == entity.Id);

				if (elementToUpdate != null)
				{
					elementToUpdate.FirstName = entity.FirstName;
					elementToUpdate.LastName = entity.LastName;
					elementToUpdate.Birth = entity.Birth;
					elementToUpdate.Salary = entity.Salary;
					elementToUpdate.WorkingPosition = entity.WorkingPosition;
					elementToUpdate.TaxNumber = entity.TaxNumber;
				}
			}
		}

		public void Remove(EmployeeModel employeeModel)
		{
			lock (this)
			{
				this.employees.Remove(this.employees.FirstOrDefault(x => x.Id == employeeModel.Id));
			}
		}

		public IQueryable<EmployeeModel> Get()
		{
			return this.employees.AsQueryable();
		}

		private IList<EmployeeModel> LoadXml()
		{
			lock (this)
			{
				return this.xmlService.LoadXml(this.filePath);
			}
		}

		public void Save()
		{
			lock (this)
			{
				this.xmlService.SaveXml(this.filePath, this.employees);
			}
		}
	}
}

