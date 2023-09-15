using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Speech.Synthesis;
using Tool_Export_Video_FromPDF.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace Tool_Export_Video_FromPDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            _client = new HttpClient();


            string imagePath = System.IO.Path.Combine(Application.StartupPath, "..\\..\\imgs\\tiktok-tools.png");
            this.BackgroundImage = Image.FromFile(imagePath);

        }

        private HttpClient _client;


        private void Form1_Load(object sender, EventArgs e)
        {


            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            listView1.View = View.Details;
            // Allow the user to edit item text.
            listView1.LabelEdit = true;
            // Allow the user to rearrange columns.
            //  listViewStudent.AllowColumnReorder = true;
            // Display check boxes.
            listView1.CheckBoxes = false;
            // Select the item and subitems when selection is made.
            listView1.FullRowSelect = true;
            // Display grid lines.
            listView1.GridLines = true;
            // Sort the items in the list in ascending order.
            // listViewStudent.Sorting = SortOrder.Ascending;
            listView1.Columns.Add("STT",100, HorizontalAlignment.Left);
            listView1.Columns.Add("Sentence",1000, HorizontalAlignment.Left);

            listView1.LabelEdit = true;
            listView2.LabelEdit = true;


            listView2.View = View.Details;
            // Allow the user to edit item text.
            listView2.LabelEdit = true;
            // Allow the user to rearrange columns.
            //  listViewStudent.AllowColumnReorder = true;
            // Display check boxes.
            listView2.CheckBoxes = false;
            // Select the item and subitems when selection is made.
            listView2.FullRowSelect = true;
            // Display grid lines.
            listView2.GridLines = true;
            // Sort the items in the list in ascending order.
            // listViewStudent.Sorting = SortOrder.Ascending;
            listView2.Columns.Add("STT", 100, HorizontalAlignment.Left);
            listView2.Columns.Add("Sentence", 1000, HorizontalAlignment.Left);

            listView1.MouseClick += ListView1_MouseClick;



        }

        static List<String> arrayAudioFile = new List<string>();


        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var clickedItem = listView1.SelectedItems[0].Text;
                // MessageBox.Show(clickedItem);

                int index = listView1.SelectedItems[0].Index;
               // MessageBox.Show("Index of clicked item: " + index);


                Form1_LyricDetail form = new Form1_LyricDetail();
                form.MyProperty = StaticData.arrayString[index];
                form.Index = index;
                form.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the initial directory and filter for the file dialog
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "All Files (*.*)|*.*";

            // Show the file dialog and check if the user clicked the OK button
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StaticData.arrayString.Clear();
                // Get the selected file name
                string selectedFileName = openFileDialog.FileName;

                // Process the selected file as needed
                // For example, you can display the file path in a label
                textBox1.Text = selectedFileName;


                string pdfPath = textBox1.Text.ToString();
                string text = ExtractTextFromPdf(pdfPath);

                // Split the text into sentences
                string[] sentences = Regex.Split(text, @"(?<=['""A-Za-z0-9][\.\!\?])\s+");

                // Print the sentences
                int index = 1;
                foreach (string sentence in sentences)
                {
                    if (!string.IsNullOrEmpty(sentence))
                    {
                        // string convertString = sentence.Replace(@"\n", "").Trim().Replace(" ","");


                        string pattern = @"\s+";
                        string replacement = " ";

                        string output = Regex.Replace(sentence, pattern, replacement).Replace("/", "");
                        // Console.WriteLine(sentence);
                        StaticData.arrayString.Add(output);

                        listView1.Items.Add(
                      new ListViewItem(new[] {
                   (index)+"",
                   sentence+"",
                           }));
                        index++;

                    }

                }
            }

        }

        private void Refresh()
        {

            listView2.Items.Clear();
            int index = 1;
            foreach (string sentence in StaticData.arrayStringTranslate)
            {
                // Console.WriteLine(sentence);
              //  StaticData.arrayString.Add(sentence);

                listView2.Items.Add(
              new ListViewItem(new[] {
                   (index)+"",
                   sentence+"",
                   }));
                index++;
            }
        }


        

        private  void Update(List<string> arrayAudioFile)
        {
            int index = 1;
            foreach (string sentence in arrayAudioFile)
            {
                // Console.WriteLine(sentence);
                StaticData.arrayString.Add(sentence);

                listView2.Items.Add(
              new ListViewItem(new[] {
                   (index)+"",
                   sentence+"",
                   }));
                index++;
            }
        }

        private static string ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                return text.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = 1;
            foreach(string sentence in StaticData.arrayString)
            {

                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                {

                    // Configure the audio output. 
                    synth.SetOutputToWaveFile(@"C:\DUAN\Export Youtube\"+ index+  ".wav");

                    // Create a SoundPlayer instance to play the output audio file.
                     System.Media.SoundPlayer m_SoundPlayer = new System.Media.SoundPlayer(@"C:\DUAN\Export Youtube\" + index + ".wav");

                    // Build a prompt.
                     PromptBuilder builder = new PromptBuilder();
                     builder.AppendText(sentence);

                    // Speak the prompt and play back the output file.
                     synth.Speak(builder);
                     m_SoundPlayer.Play();
                    arrayAudioFile.Add(@"C:\DUAN\Export Youtube\"+ index+  ".wav");

                }

                index ++;

            }

            Update(arrayAudioFile);



           



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void button7_Click(object sender, EventArgs e)
        {

            TranslateLang translate = new TranslateLang();
            foreach(string item in StaticData.arrayString)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string ouputItem = item.ToString();
                    translate.items.Add(ouputItem);

                }
                //= StaticData.arrayString;


            }

            // string api = "http://127.0.0.1:5000/api/translate";

            // Call api post method 

            foreach(string item in StaticData.arrayString)
            {


                try
                {
                    // Assuming the REST API endpoint is "http://localhost:8080/data"
                    string url = "http://127.0.0.1:5000/api/translate";

                    // Create an object with the data you want to send


                   // string[] array = StaticData.arrayString.ToArray();

                    string json = JsonConvert.SerializeObject(item);

                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    // Send a POST request
                    HttpResponseMessage response = await _client.PostAsync(url, content);

                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();

                    List<String> translateList = JsonConvert.DeserializeObject<List<String>>(responseContent);

                    Update(translateList);


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

        private void button5_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        static string bgFile = null;

        private void button3_Click(object sender, EventArgs e)
        {
            // path background file 
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set the initial directory and filter for the file dialog
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "All Files (*.*)|*.*";
            // Show the file dialog and check if the user clicked the OK button
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog.FileName;

                // Process the selected file as needed
                // For example, you can display the file path in a label
              //  textBox2.Text = selectedFileName;

                bgFile = openFileDialog.FileName;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // export video file MP4

          //  string[] audioFiles = { "path/to/audio1.mp3", "path/to/audio2.mp3" };
            string backgroundImage = bgFile;
            string outputFile = @"C:\DUAN\Export Youtube\abc_1.mp4";

            string folderPath = @"C:\DUAN\Export Youtube\output";
            string[] mp3Files = Directory.GetFiles(folderPath, "*.mp3");



            List<string> filesSource = new List<string>();
            List<string> filesDes = new List<string>();

            foreach (string file in mp3Files)
            {
                if (file.Contains("des"))
                {
                    filesDes.Add(file);

                }else
                {
                    filesSource.Add(file);
                }
            }
            filesDes.Sort();
            filesSource.Sort();

            List<string> filesMerge = new List<string>();
            for(int i =0;i< filesDes.Count; i++)
            {
                filesMerge.Add(filesSource[i]);
                filesMerge.Add(filesDes[i]);
            }

             string[] audioFiles = filesMerge.ToArray();



          
           /* string ffmpegPath = @"C:\DUAN\Export Youtube\ffmpeg-master-latest-win64-gpl\bin\ffmpeg.exe";
            Process process = new Process();

              string audioArgs = "";
              for (int i = 0; i < audioFiles.Length; i++)
              {
                  audioArgs += $"-i \"{audioFiles[i]}\" ";
              }
              string filterArgs = "";
              for (int i = 0; i < audioFiles.Length; i++)
              {
                  filterArgs += $"[{i}:0]";
              }
              filterArgs += $"concat=n={audioFiles.Length}:v=0:a=1[out]";
              process.StartInfo.FileName = ffmpegPath;
              process.StartInfo.Arguments = $"-loop 1 -i \"{backgroundImage}\" {audioArgs} -filter_complex \"{filterArgs}\" -map \"[out]\" -map 0:v:0 -shortest -c:v libx264 -preset ultrafast -crf 18 -c:a aac -b:a 192k -y \"{outputFile}\"";
              process.StartInfo.UseShellExecute = false;
              process.StartInfo.RedirectStandardOutput = true;
              process.StartInfo.UseShellExecute = false;
              process.StartInfo.RedirectStandardError = true;
              process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();*/

           // Run();

        }

        private void Run()
        {

            string outputFile = @"C:\DUAN\Export Youtube\outputdes1.mp3";
            string ffmpegPath = @"C:\DUAN\Export Youtube\ffmpeg-master-latest-win64-gpl\bin\ffmpeg.exe";
            string inputFilePath = @"C:\DUAN\Export Youtube\outputdes1.mp3";
            string outputFilePath = @"C:\DUAN\Export Youtube\output.mp3";

            Process process = new Process();
            process.StartInfo.FileName = ffmpegPath;
            process.StartInfo.Arguments = $"-i \"{inputFilePath}\" -c copy \"{outputFilePath}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

  
}
