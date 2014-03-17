using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using CustomerDataLayer;
using EDXInterface;
using System.Data.SqlClient;
using System.Data.EntityClient;

namespace CustomerObject
{
    public class CustomerObj : IEdx
    {
        public void ProcessPacket(string path)
        {
            DTDCUSTOMER b;
            XmlSerializer serializer = new XmlSerializer(typeof(DTDCUSTOMER));
            using (XmlReader reader = XmlReader.Create(path))
            {
                b = (DTDCUSTOMER)serializer.Deserialize(reader);
            }

            var context = new RUDUSEntities();
            string tempCustCode = context.cnfas.Select(a => a.template_cust_code).First();
            cust TempCust = context.custs.Where(a => a.cust_code == tempCustCode).First();
            //int i = 8;
            //foreach (CUSTCMDSERIES x in b.CUSTCMDSERIES)
            for (int i = 100; i < 208; i++)
            {

                //int Count = context.custs.Where(a => a.cust_code == x.CUST_CODECUSTCMDSERIES).Count();
                int Count = context.custs.Where(a => a.cust_code == "XXXXXX").Count();
                if (Count == 0)
                {

                    cust NewCust = TempCust.Clone();
                    NewCust.EntityKey = null;
                    NewCust.cust_code = "NCust" + i.ToString();
                    foreach (cuh a in NewCust.cuhs)
                    {

                        a.EntityKey = NewCust.EntityKey;
                        a.cust_code = NewCust.cust_code;
                    }
                    foreach (cusc a in NewCust.cuscs)
                    {

                        a.EntityKey = NewCust.EntityKey;
                        a.cust_code = NewCust.cust_code;
                    }

                    NewCust.cuso.EntityKey = null;
                    NewCust.cusoReference.EntityKey = null;
                    NewCust.cuso.custReference.EntityKey = null;
                    NewCust.cuso.cust_code = NewCust.cust_code;

                    foreach (cuig a in NewCust.cuigs)
                    {
                        a.EntityKey = null;
                        a.custReference.EntityKey = null;
                        a.cust_code = NewCust.cust_code;
                        a.hler_code = "11111";

                        foreach (cuiu c in a.cuius)
                        {
                            c.EntityKey = null;
                            c.cuigReference.EntityKey = null;
                            c.cust_code = NewCust.cust_code;

                        }
                    }
                    foreach (csdt a in NewCust.csdts)
                    {

                        a.EntityKey = NewCust.EntityKey;
                        a.cust_code = NewCust.cust_code;

                    }
                    foreach (ccon a in NewCust.ccons)
                    {

                        a.EntityKey = NewCust.EntityKey;
                        a.cust_code = NewCust.cust_code;

                    }
                    foreach (cuco a in NewCust.cucoes)
                    {

                        a.EntityKey = null;
                        a.custReference.EntityKey = null;                        
                        a.cust_code = NewCust.cust_code;

                    }
                    context.custs.AddObject(NewCust);
                    context.SaveChanges();
                }
                i++;
            }
        }

        private string BuildConnectionString()
        {
            // Specify the provider name, server and database.
            string providerName = "System.Data.SqlClient";
            string serverName = "WORMBOY";
            string databaseName = "RUDUS";

            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = true;
            //sqlBuilder.UserID = "Joe";
            //sqlBuilder.Password = "John";

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;            

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/CustomerModel.csdl|res://*/CustomerModel.ssdl|res://*/CustomerModel.msl";

            return entityBuilder.ToString();
        }
    }
}
