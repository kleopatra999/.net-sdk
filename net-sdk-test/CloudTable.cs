using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
namespace CB.Test
{
    [TestFixture]
    public class CloudTable
    {
        string tableName = "";

        [Test]
        public void x001_InitAppWithMasterKey()
        {
            tableName = CB.Test.Util.Methods._makeString();

            CB.Test.Util.Keys.InitWithMasterKey();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task x0_GetAllTables()
        {
            Util.Keys.InitWithMasterKey();

            List<CB.CloudTable> tables = await CB.CloudTable.GetAllAsync();

            if (tables.Count > 1)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        public async Task x002_DeleteTables()
        {
            var obj = new CB.CloudTable("Address");
            CB.CloudTable table = await obj.DeleteAsync();

            obj = new CB.CloudTable("Company");
            table = await obj.DeleteAsync();

            obj = new CB.CloudTable("Employee");
            table = await obj.DeleteAsync();

            Assert.IsTrue(true);
        }

        [Test]
        public async Task x003_CreateEmployeeTable()
        {
            var age = new CB.Column("Age");
            age.dataType = CB.DataType.Number.ToString();

            var name = new CB.Column("Name");
            name.dataType = CB.DataType.Text.ToString();

            CB.CloudTable obj = new CB.CloudTable("Employee");

            obj.AddColumn(age);
            obj.AddColumn(name);

            obj = await obj.SaveAsync();

            Assert.IsTrue(true);
        }

        [Test]
        public async Task x004_CreateCompanyTable()
        {
            var obj = new CB.CloudTable("Company");
            var Revenue = new CB.Column("Revenue");
            Revenue.dataType = CB.DataType.Number.ToString();
            var Name = new CB.Column("Name");
            Name.dataType =CB.DataType.Text.ToString();
            obj.AddColumn(Revenue);
            obj.AddColumn(Name);
           
            obj = await obj.SaveAsync();

            Assert.IsTrue(true);
        }

        [Test]
        public async Task x005_CreateAddressTable()
        {
            Util.Keys.InitWithMasterKey();
            var obj = new CB.CloudTable("Address");
            var City = new CB.Column("City");
            City.dataType = CB.DataType.Text.ToString();
            var PinCode = new CB.Column("PinCode");
            PinCode.dataType = CB.DataType.Number.ToString();
            obj.AddColumn(City);
            obj.AddColumn(PinCode);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task x005_ShouldUpdateANewColumnInATable()
        {

            var tableName1 = CB.Test.Util.Methods._makeString();
            var tableName2 = CB.Test.Util.Methods._makeString();

            var obj = new CB.CloudTable(tableName1);
            var obj1 = new CB.CloudTable(tableName2);

            obj = await obj.SaveAsync();
            obj1 = await obj1.SaveAsync();
            obj = await CB.CloudTable.GetAsync(obj);

            var column1 = new CB.Column("Name11", CB.DataType.Relation.ToString(), true, false);
            column1.relatedTo = tableName2;

            obj.AddColumn(column1);

            obj = await obj.SaveAsync();

            var column2 = new CB.Column("Name11");
            obj.DeleteColumn(column2);
            obj = await obj.SaveAsync();

            Assert.IsTrue(true);
        }

        [Test]
        public async Task x006_CreateDeleteTable()
        {
            Util.Keys.InitWithMasterKey();
            var tableName = CB.Test.Util.Methods._makeString();
            var obj = new CB.CloudTable(tableName);
            obj = await obj.SaveAsync();
            obj = await obj.DeleteAsync();

            Assert.IsTrue(true);
        }

        [Test]
        public async Task x007_CreateDeleteTable()
        {

            var tableName = CB.Test.Util.Methods._makeString();

            var obj = new CB.CloudTable(tableName);
            var table = await CB.CloudTable.GetAsync(obj);
            var column1 = new CB.Column("city", CB.DataType.Text.ToString(), true, false);
            table.AddColumn(column1);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task x008_AddColumnToTableAfterSave()
        {

            var tableName = CB.Test.Util.Methods._makeString();

            var obj = new CB.CloudTable(tableName);
            var table = await CB.CloudTable.GetAsync(obj);
            var column1 = new CB.Column("Name1", CB.DataType.Text.ToString(), true, false);
            table.AddColumn(column1);
            obj = await obj.SaveAsync();
            obj = await obj.DeleteAsync();
            Assert.IsTrue(true);
        }

        public async Task x008_GetAll()
        {
            List<CB.CloudTable> list = await CB.CloudTable.GetAllAsync();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(true);
            }
        }

        public async Task x008_ShouldNotRenameATable()
        {
            var obj = new CB.CloudTable(tableName);
            obj = await CB.CloudTable.GetAsync(obj);


            obj.Name = "Sample";

            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }

        public async Task x009_ShouldNotRenameAColumn()
        {
            var tableName = CB.Test.Util.Methods._makeString();

            var obj = new CB.CloudTable(tableName);
            var table = await CB.CloudTable.GetAsync(obj);
            var column1 = new CB.Column("Name1", CB.DataType.Text.ToString(), true, false);
            table.AddColumn(column1);
            obj = await obj.SaveAsync();

            obj.Columns.FirstOrDefault().name = "Sample";

            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }

        }

        public async Task x009_ShouldNotChangeTheDataTypeOfAColumn()
        {
            var tableName = CB.Test.Util.Methods._makeString();

            var obj = new CB.CloudTable(tableName);
            var table = await CB.CloudTable.GetAsync(obj);
            var column1 = new CB.Column("Name1", CB.DataType.Text.ToString(), true, false);
            table.AddColumn(column1);
            obj = await obj.SaveAsync();

            obj.Columns.FirstOrDefault().dataType = CB.DataType.Number.ToString();

            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }

        public async Task x010_ShouldNotChangeTheUniquePropertyOfDefaultColumn()
        {
            var tableName = CB.Test.Util.Methods._makeString();

            var obj = new CB.CloudTable(tableName);

            obj.Columns.First(o => o.name == "id").unique = false;

            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }

        }

        public async Task x010_ShouldNotChangeTheRequiredPropertyOfDefaultColumn()
        {
            var tableName = CB.Test.Util.Methods._makeString();

            var obj = new CB.CloudTable(tableName);
            var column1 = new CB.Column("Name1", CB.DataType.Text.ToString(), true, false);
            obj.AddColumn(column1);

            obj = await obj.SaveAsync();

            obj.Columns.First(o => o.name == "Name1").required = false;

            try
            {
                obj = await obj.SaveAsync();
                Assert.IsTrue(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.Fail("Cannot save a table when required of a column is changed.");
            }

        }

        public async Task x011_ShouldChangeTheUniquePropertyOfUserDefinedColumn()
        {
            var tableName = CB.Test.Util.Methods._makeString();

            var obj = new CB.CloudTable(tableName);
            var column1 = new CB.Column("Name1", CB.DataType.Text.ToString(), true, false);
            obj.AddColumn(column1);

            obj = await obj.SaveAsync();

            obj.Columns.First(o => o.name == "Name1").unique = true;

            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }

        }

        
        public async Task x012_ShouldNotDeleteTheDefaultColumnOfTheTable()
        {
            var tableName = CB.Test.Util.Methods._makeString(); 

            var obj = new CB.CloudTable(tableName);
            obj = await obj.SaveAsync();

            obj.DeleteColumn(obj.Columns.Single(o => obj.Name == "Id"));

            try
            {
                obj = await obj.SaveAsync();
                Assert.Fail("Cannot delete the default column");
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }

        }


    }
}

   