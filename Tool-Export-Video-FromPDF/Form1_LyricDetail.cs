using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tool_Export_Video_FromPDF.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tool_Export_Video_FromPDF
{
    public partial class Form1_LyricDetail : Form
    {
        private HttpClient _client;

        public string MyProperty { get; set; }
        public int Index { get; set; }  

        public Form1_LyricDetail()
        {

            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            _client = new HttpClient();

        }

        private void Form1_LyricDetail_Load(object sender, EventArgs e)
        {
            // Use the MyProperty somewhere in your form
            // For example, you can display it in a TextBox
            textBox1.Text = MyProperty;


        }

        private void Form1_LyricDetail_Load_1(object sender, EventArgs e)
        {
            textBox1.Text = MyProperty;

        }

        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {
                // Assuming the REST API endpoint is "http://localhost:8080/data"
                string url = "http://127.0.0.1:5000/api/translate";
                // Create an object with the data you want to send
                // string[] array = StaticData.arrayString.ToArray();
                var itemClass = new TranslateText();
                itemClass.item = textBox1.Text.ToString();
                string json = JsonConvert.SerializeObject(itemClass);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                // Send a POST request
                HttpResponseMessage response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                string textPlan = JsonConvert.DeserializeObject<string>(responseContent);
                // List<String> translateList = JsonConvert.DeserializeObject<List<String>>(responseContent);
                // Update(translateList);
                textBox2.Text = textPlan;
                StaticData.arrayStringTranslate.Add(textPlan);
                // TODO: Process the response as needed
                // For example, you might want to display a success message in your form
            }
            catch (Exception ex)
            {
                // Handle or display the error as needed
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            // Download first language
            string  sentence = textBox1.Text;


            try
            {
                string url = "http://127.0.0.1:5000/api/download";
                var itemClass = new DownloadAudio();
                itemClass.item = textBox1.Text.ToString();
                itemClass.language = "en";
                itemClass.type = 0;
                itemClass.index = Index;

                string json = JsonConvert.SerializeObject(itemClass);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                // Send a POST request
                HttpResponseMessage response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                string textPlan = JsonConvert.DeserializeObject<string>(responseContent);
                textBox2.Text = textPlan;
            }
            catch (Exception ex)
            {
                // Handle or display the error as needed
                MessageBox.Show($"An error occurred: {ex.Message}");
            }


        }

        private async void button3_Click(object sender, EventArgs e)
        {
            // Download second language 
            string sentence = textBox2.Text;

            try
            {
                string url = "http://127.0.0.1:5000/api/download";
                var itemClass = new DownloadAudio();
                itemClass.item = textBox2.Text.ToString();
                itemClass.language = "vi";
                itemClass.type = 1;
                itemClass.index = Index;

                string json = JsonConvert.SerializeObject(itemClass);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                // Send a POST request
                HttpResponseMessage response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                string textPlan = JsonConvert.DeserializeObject<string>(responseContent);
                // List<String> translateList = JsonConvert.DeserializeObject<List<String>>(responseContent);
                // Update(translateList);
                textBox2.Text = textPlan;
                // TODO: Process the response as needed
                // For example, you might want to display a success message in your form
            }
            catch (Exception ex)
            {
                // Handle or display the error as needed
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }
    }

    class TranslateText
{
    public string item;
}

    class DownloadAudio
    {
        public string item { get; set; }
        public string language { get; set; }
        public int type { get; set; }
        public int index { get; set; }

    }
}
