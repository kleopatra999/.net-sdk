using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CB.Test
{
    [TestFixture]
    public class CloudFile
    {
        [Test]
        public void saveFileDataAndName()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            var rsponse = file.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task returnFileWithCloudObject()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            await file.SaveAsync();
            if (file.Url != null)
            {
                var obj = new CB.CloudObject("Company");
                obj.Set("File", file);
                await obj.SaveAsync();
                var fileObj = (CB.CloudFile)obj.Get("File");
                if (fileObj.Url != null)
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    throw new CB.Exception.CloudBoostException("Did not get the file object back.");
                }
            }
        }
        
        [Test]
        public async Task shouldSaveFileAndGiveURL()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            await file.SaveAsync();
            if (file.Url != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Unable to get the url");
            }
        }

        [Test]
        public async Task deleteFileWithDataAndName()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            await file.SaveAsync();
            if (file.Url != null)
            {
                await file.DeleteAsync();
                if (file.Url == null)
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    throw new CB.Exception.CloudBoostException("File delete error");
                }
                
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Unable to get the url");
            }
        }

        [Test] 
        public async Task saveAndFetchFile()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            await file.SaveAsync();
            if (file.Url != null)
            {
                await file.GetFileContentAsync();
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Unable to get the url");
            }
        }

        [Test]
        public async Task includeOverFile()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            await file.SaveAsync();
            var obj = new CB.CloudObject("Sample");
            obj.Set("file", file);
            obj.Set("name", "abcd");
            await obj.SaveAsync();
            var id = obj.ID;
            var query = new CB.CloudQuery("Sample");
            query.EqualTo("id", id);
            query.Include("file");
            var response = (List<CB.CloudObject>)await query.FindAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveFileDataAndNameThenFetch()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            await file.SaveAsync();
            var obj = new CB.CloudObject("Sample");
            obj.Set("file", file);
            obj.Set("name", "abcd");
            await obj.SaveAsync();
            var fileObj = (CB.CloudFile)obj.Get("file");
            if (fileObj.Url != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Unable to fetch the file");
            }
        }

        [Test]
        public async Task saveFileAndGetRelation()
        {
            var obj1 = new CB.CloudObject("Employee");
            var obj2 = new CB.CloudObject("Company");
            obj1.Set("Name", "abcd");
            obj1.Set("Name", "pqrs");
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            await file.SaveAsync();
            var obj = new CB.CloudObject("Sample");
            obj2.Set("File", file);
            obj1.Set("Company", obj2);
            await obj1.SaveAsync();
            var query = new CB.CloudQuery("Employee");
            query.Include("Company.File");
            query.EqualTo("id", obj1.ID);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task getFileObjectWithNotReadAccess()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            file.ACL.SetPublicReadAccess(false);
            await file.SaveAsync();
            if (file == null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Unable to get ACL working");
            }
        }

        [Test]
        public async Task shoudNotGetFileWithNoReadAccess()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            file.ACL.SetPublicReadAccess(false);
            var response = await file.SaveAsync();
            var result = await response.GetFileContentAsync();
            if (result != null)
            {
                throw new CB.Exception.CloudBoostException("Should not retrieve file");
            }
            else
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task shouldNotDeleteFileNoWriteAccess()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(".net cloudfile testing!");
            string name = "sample.txt";
            string type = "txt";
            var file = new CB.CloudFile(data, name, type);
            file.ACL.SetPublicReadAccess(false);
            var response = await file.SaveAsync();
            response = await response.DeleteAsync();
            if (response != null)
            {
                throw new CB.Exception.CloudBoostException("Should not retrieve file");
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
    }
}
