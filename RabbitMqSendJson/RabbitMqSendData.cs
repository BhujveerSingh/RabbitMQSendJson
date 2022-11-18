using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Apigen;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using RabbitMqSendJson;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;


namespace RabbitMqSendJson
{
    public partial class RabbitMqSendData : Form
    {
        public RabbitMqSendData()
        {
            InitializeComponent();
        }
        //LocalHost Url:-http://localhost:15672/#/     ,   user_id = guest  , password=guest

        private void button1_Click(object sender, EventArgs e)
        {

            string jsontext = string.Empty;
            string filepath = @"D:\newUpdate.txt";
            using (StreamReader r = new StreamReader(filepath))
            {
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);
                jsontext = jobj.ToString();
            }

            string output = jsontext;
            dynamic data = "";
            data = JObject.Parse(output);
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.HostName = "localhost";
            factory.Port = 5672;
            IConnection conn = factory.CreateConnection();
            using (var channel = conn.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = output;
                var body = Encoding.UTF8.GetBytes(message);
                Thread.Sleep(2000);
                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                conn.Close();
                Thread.Sleep(1000);
                channel.Close();

            }
            Thread.Sleep(1000);


        }
    }
}
