using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Lab1_CS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView(); // Ініціалізація DataGridVie
        }

        private void InitializeDataGridView()
        {
            // Додаємо стовпці у DataGridView
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Символ";
            dataGridView1.Columns[1].Name = "Частота";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

           openFileDialog1.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog1.FileName;
                    string text = System.IO.File.ReadAllText(filePath);

                    richTextBox1.Text = text; 

                    Dictionary<char, double> frequencies = CalculateRelativeFrequencies(text);
                    double entropy = calcAvgEntropy(frequencies);
                    double informationContent = entropy * text.Length;

                    long fileSizeInBytes = CalculateFileSizeInBytes(filePath);

                    dataGridView1.Rows.Clear(); 

                    foreach (var kvp in frequencies)
                    {
                        dataGridView1.Rows.Add(kvp.Key, kvp.Value); 
                    }

                    label5.Text = $"Ентропія: {entropy}";
                    label6.Text = $"Кількість інформації: {calcAvgEntropy(frequencies) * CalculateTotalCharactersCount(text) / 8.00} байт";
                    label8.Text = $"{CalculateTotalCharactersCount(text)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Сталася помилка: {ex.Message}");
                }
            }
        }

        private long CalculateFileSizeInBytes(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            long fileSizeInBytes = fileInfo.Length;

            return fileSizeInBytes;
        }

        private Dictionary<char, double> CalculateRelativeFrequencies(string text)
        {
            SortedDictionary<char, double> frequencies = new SortedDictionary<char, double>();

            int totalSymbols = 0;

            text = text.ToLower();

            foreach (char c in text)
            {
                if (!Char.IsWhiteSpace(c))
                {
                    if (frequencies.ContainsKey(c))
                    {
                        frequencies[c]++;
                    }
                    else
                    {
                        frequencies[c] = 1;
                    }

                    totalSymbols++;
                }
            }
            Dictionary<char, double> relativeFrequencies = new Dictionary<char, double>();

            foreach (var kvp in frequencies)
            {
                double relativeFrequency = (double)kvp.Value / totalSymbols;
                relativeFrequencies.Add(kvp.Key, relativeFrequency);
            }

            return relativeFrequencies;
        }

        double calcAvgEntropy(Dictionary<char, double> relativeFrequencies)
        {
            double result = 0;
            foreach (var elem in relativeFrequencies)
            {
                if (1 / elem.Value != 0)
                    result += elem.Value * Math.Log(1 / elem.Value, 2);
            }

            return result;
        }

        private int CalculateTotalCharactersCount(string text)
        {
            SortedDictionary<char, int> analText = new SortedDictionary<char, int>();
            int letterCount = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (!analText.ContainsKey(Char.ToLower(text[i])) && (Char.IsLetter(text[i]) || (Char.IsLetterOrDigit(text[i]))))
                {
                    analText.Add(Char.ToLower(text[i]), 1);
                    letterCount++;
                }
                else if (analText.ContainsKey(Char.ToLower(text[i])))
                {
                    analText[Char.ToLower(text[i])]++;
                    letterCount++;
                }
            }

            return letterCount;
        }

        private static readonly char[] Base64Letters = new[]
                                        {
                                              'A'
                                            , 'B'
                                            , 'C'
                                            , 'D'
                                            , 'E'
                                            , 'F'
                                            , 'G'
                                            , 'H'
                                            , 'I'
                                            , 'J'
                                            , 'K'
                                            , 'L'
                                            , 'M'
                                            , 'N'
                                            , 'O'
                                            , 'P'
                                            , 'Q'
                                            , 'R'
                                            , 'S'
                                            , 'T'
                                            , 'U'
                                            , 'V'
                                            , 'W'
                                            , 'X'
                                            , 'Y'
                                            , 'Z'
                                            , 'a'
                                            , 'b'
                                            , 'c'
                                            , 'd'
                                            , 'e'
                                            , 'f'
                                            , 'g'
                                            , 'h'
                                            , 'i'
                                            , 'j'
                                            , 'k'
                                            , 'l'
                                            , 'm'
                                            , 'n'
                                            , 'o'
                                            , 'p'
                                            , 'q'
                                            , 'r'
                                            , 's'
                                            , 't'
                                            , 'u'
                                            , 'v'
                                            , 'w'
                                            , 'x'
                                            , 'y'
                                            , 'z'
                                            , '0'
                                            , '1'
                                            , '2'
                                            , '3'
                                            , '4'
                                            , '5'
                                            , '6'
                                            , '7'
                                            , '8'
                                            , '9'
                                            , '+'
                                            , '/'
                                        };

        public static BitArray Append(BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }

        public void Reverse(ref BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }
        }

        string Encode3bytes(byte[] bytes)
        {
            string result = "";

            BitArray oneArr = new BitArray(new byte[] { bytes[0] });
            Reverse(ref oneArr);

            BitArray secondArr = new BitArray(new byte[] { bytes[1] });
            Reverse(ref secondArr);

            BitArray thirdArr = new BitArray(new byte[] { bytes[2] });

            Reverse(ref thirdArr);

            BitArray bitArray = new BitArray(0);
            bitArray = Append(oneArr, secondArr);
            bitArray = Append(bitArray, thirdArr);


            BitArray tmpBit = new BitArray(6);
            int j = 0;
            for (int i = 0; i < bitArray.Length; i++)
            {
                tmpBit.Set(i - 6 * j, bitArray[i]);
                if (((i + 1) % 6 == 0 || i == 23) && i != 0)
                {
                    byte[] tmpbyte = new byte[1];
                    tmpBit.CopyTo(tmpbyte, 0);
                    int value = 0;
                    Reverse(ref tmpBit);
                    for (int k = 0; k < tmpBit.Count; k++)
                    {
                        if (tmpBit[k])
                            value += Convert.ToInt16(Math.Pow(2, k));
                    }
                    result += Base64Letters[value];
                    j++;

                    if (i / 8 >= 1 && bytes[1] == 0 && bytes[2] == 0)
                    {
                        result += "==";
                        return result;
                    }
                    if (i / 8 >= 2 && bytes[2] == 0)
                    {
                        result += "=";
                        return result;
                    }

                }


            }

            return result;

        }

        private string EncodeBase64(byte[] inarr, out double informationContentBase64)
        {
            List<byte> arr = inarr.ToList();
            string result = "";

            while (arr.Count % 3 != 0)
            {
                arr.Add(0);
            }

            for (int i = 0; i < arr.Count; i += 3)
            {
                result += Encode3bytes(new byte[] { arr[i], arr[i + 1], arr[i + 2] });
            }

            // Обчислюємо кількість інформації в base64-закодованому варіанті
            informationContentBase64 = result.Length * 8;

            return result;
        }



        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void label6_Click_1(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] fileBytes = File.ReadAllBytes(openFileDialog1.FileName);

                    // Отримуємо base64-кодований рядок та кількість інформації в base64-закодованому варіанті
                    double informationContentBase64 = 0;
                    string base64EncodedString = EncodeBase64(fileBytes, out informationContentBase64);

                    // Кількість інформації в байтах (поділено на 8 бітів в байті)
                    double informationContentBytes = informationContentBase64 / 8;

                    // Відображаємо кількість інформації в байтах
                    label4.Text = $"Кількість інформації в base64: {informationContentBytes} байт";

                    // Відображаємо закодований рядок в richTextBox
                    richTextBox2.Text = base64EncodedString;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка: {ex.Message}");
                }
            }
        }





        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Base64 files (*.base64)|*.base64|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog1.FileName;

                    File.WriteAllText(filePath, richTextBox2.Text);

                    MessageBox.Show("Файл успішно збережено.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Сталася помилка при збереженні файлу: {ex.Message}");
                }
            }
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }
    }
}
