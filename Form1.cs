using iText.Kernel.Pdf;
using iText;
using iText.Layout;
using iText.Layout.Element;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_Notepad
{
    public partial class FormNotepad : Form
    {
        public FormNotepad()
        {
            InitializeComponent();
        }

        private float originalFontSize = 10.2f;
        bool m_bIsTextChanged = true;
        string m_strOpenFilePath = null;
        string m_strSaveFilePath = null;
        string m_strSaveasFilePath = null;
        string m_strPDFFilePath = null;
        private void FormNotepad_Load(object sender, EventArgs e)
        {
            try
            {
                originalFontSize = richtxtboxNotepad.Font.Size;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richtxtboxNotepad.Clear();
                this.Text = "Untitled1.txt";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Thread newWindow = new Thread(() => Application.Run(new FormNotepad()));
                newWindow.Start();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "txt files(*.txt)|*.txt|All Files (*.*)|*.*";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.Text = openFileDialog.SafeFileName;
                        m_strOpenFilePath = openFileDialog.FileName;
                        string[] content = File.ReadAllLines(openFileDialog.FileName);
                        foreach (string line in content)
                        {
                            richtxtboxNotepad.AppendText(line + "\r\n");
                            richtxtboxNotepad.ScrollToCaret();
                        }
                        #region <Commented Code>
                        /*cutToolStripMenuItem.Enabled = true;
                        cutToolStripMenuItem.ForeColor = Color.Black;
                        copyToolStripMenuItem.Enabled = true;
                        copyToolStripMenuItem.ForeColor = Color.Black;
                        deleteToolStripMenuItem.Enabled = true;
                        deleteToolStripMenuItem.ForeColor = Color.Black;
                        selectAllToolStripMenuItem.Enabled = true;
                        selectAllToolStripMenuItem.ForeColor = Color.Black;*/
                        #endregion
                    }

                });
                Task.Run(() => StatusBarProgressBar("Opening File"));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    
                    if ( this.Text == "Untitled1.txt")
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "txt files(*.txt)|*.txt|All Files(*.*)|*.*";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            this.Text = saveFileDialog.FileName;
                            m_strSaveFilePath = saveFileDialog.FileName;
                            File.AppendAllText(saveFileDialog.FileName, richtxtboxNotepad.Text);
                        }
                    }
                    else if (this.Text.StartsWith("*"))
                    {
                        this.Text = this.Text.Substring(1);
                        File.WriteAllText(m_strOpenFilePath, string.Empty);                       
                        File.AppendAllText(m_strOpenFilePath, richtxtboxNotepad.Text);
                    }
                });
                Task.Run(() => StatusBarProgressBar("Saving File"));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "txt files(*.txt)|*.txt|All Files(*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.Text = saveFileDialog.FileName;
                        m_strSaveasFilePath = saveFileDialog.FileName;
                        File.AppendAllText(saveFileDialog.FileName, richtxtboxNotepad.Text);
                    }
                });
                Task.Run(() => StatusBarProgressBar("Saving File"));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "pdf files(*.pdf)|*.pdf|All Files(*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        m_strPDFFilePath= saveFileDialog.FileName;
                        PdfWriter pdfWriter = new PdfWriter(saveFileDialog.FileName);
                        PdfDocument pdfDocument = new PdfDocument(pdfWriter);
                        Document document = new Document(pdfDocument);
                        try
                        {
                            foreach (string content in richtxtboxNotepad.Text.Split("\n"))
                            {
                                Paragraph paragraph = new Paragraph(content);
                                document.Add(paragraph);
                            }

                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        finally
                        {
                            document.Close();
                        }
                    }
                });
                Task.Run(() => StatusBarProgressBar("Saving to pdf"));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    string strSelectedItem = richtxtboxNotepad.SelectedText;
                    if (richtxtboxNotepad.SelectionLength > 0)
                    {
                        Clipboard.SetText(richtxtboxNotepad.SelectedText);
                        richtxtboxNotepad.Cut();
                        //.Text = strSelectedItem.Replace(strSelectedItem, string.Empty);
                    }
                });
                Task.Run(() => StatusBarProgressBar("updating..."));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    if (richtxtboxNotepad.SelectionLength > 0)
                    {
                        Clipboard.SetText(richtxtboxNotepad.SelectedText);
                        richtxtboxNotepad.Copy();
                    }
                });
                Task.Run(() => StatusBarProgressBar("updating..."));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void richtxtboxNotepad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                m_bIsTextChanged = true;
                #region <Commented Code>
                /*if(richtxtboxNotepad.Text == " " || richtxtboxNotepad.Text != null)
                {
                    cutToolStripMenuItem.Enabled = true;
                    cutToolStripMenuItem.ForeColor = Color.Black;
                    copyToolStripMenuItem.Enabled = true;
                    copyToolStripMenuItem.ForeColor = Color.Black;
                    deleteToolStripMenuItem.Enabled = true;
                    deleteToolStripMenuItem.ForeColor = Color.Black;
                    selectAllToolStripMenuItem.Enabled = true;
                    selectAllToolStripMenuItem.ForeColor = Color.Black;
                }*/
                #endregion
                if (richtxtboxNotepad.SelectedText == " " || richtxtboxNotepad.SelectedText != null)
                {
                    cutToolStripMenuItem.Enabled = true;
                    cutToolStripMenuItem.ForeColor = Color.Black;
                    copyToolStripMenuItem.Enabled = true;
                    copyToolStripMenuItem.ForeColor = Color.Black;
                    deleteToolStripMenuItem.Enabled = true;
                    deleteToolStripMenuItem.ForeColor = Color.Black;
                    selectAllToolStripMenuItem.Enabled = true;
                    selectAllToolStripMenuItem.ForeColor = Color.Black;                    
                }
                if (m_bIsTextChanged)
                {
                    if (!this.Text.StartsWith("*"))
                    {
                        this.Text = "*" + this.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    //richtxtboxNotepad.Text = Clipboard.GetText();
                    richtxtboxNotepad.Paste();
                });
                Task.Run(() => StatusBarProgressBar("updating..."));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    if (richtxtboxNotepad.SelectionLength > 0)
                    {
                        richtxtboxNotepad.SelectedText = " ";
                    }
                });
                Task.Run(() => StatusBarProgressBar("updating..."));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // To Do...
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richtxtboxNotepad.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string strCurrentTime = DateTime.Now.ToString("h:mm tt MM/dd/yyyy");
                int iInsertIndex = richtxtboxNotepad.SelectionStart;
                string strContent = richtxtboxNotepad.Text;
                string strNewText = strContent.Insert(iInsertIndex, strCurrentTime);
                richtxtboxNotepad.Text = strNewText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //wordWrapToolStripMenuItem.Checked = true;
                if (wordWrapToolStripMenuItem.Checked)
                {
                    wordWrapToolStripMenuItem.Checked = false;
                    richtxtboxNotepad.WordWrap = false;
                }
                else if (wordWrapToolStripMenuItem.Checked == false)
                {
                    wordWrapToolStripMenuItem.Checked = true;
                    richtxtboxNotepad.WordWrap = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    FontDialog fontDialog = new FontDialog();
                    if (fontDialog.ShowDialog() == DialogResult.OK)
                    {
                        Font selectedFont = fontDialog.Font;
                        if (richtxtboxNotepad.SelectionLength > 0)
                        {
                            richtxtboxNotepad.SelectionFont = selectedFont;
                        }
                        else
                        {
                            richtxtboxNotepad.Font = selectedFont;
                        }
                    }
                });
                Task.Run(() => StatusBarProgressBar("updating..."));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                /*int i = 1;
                richtxtboxNotepad.ZoomFactor = i + 1;*/
                ChangeFontSize(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                /*richtxtboxNotepad.ZoomFactor = richtxtboxNotepad.ZoomFactor - 1;*/
                ChangeFontSize(-1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void restoreZoomDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richtxtboxNotepad.Font = new Font(richtxtboxNotepad.Font.FontFamily, originalFontSize, richtxtboxNotepad.Font.Style);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ChangeFontSize(float deltaSize)
        {
            try
            {
                // Decrease the font size of the selected text or all text in the RichTextBox
                if (richtxtboxNotepad.SelectionLength > 0)
                {
                    // Change font size of selected text
                    richtxtboxNotepad.SelectionFont = new Font(richtxtboxNotepad.SelectionFont.FontFamily,
                                                          richtxtboxNotepad.SelectionFont.Size + deltaSize,
                                                          richtxtboxNotepad.SelectionFont.Style);
                }
                else
                {
                    // Change font size of all text
                    richtxtboxNotepad.Font = new Font(richtxtboxNotepad.Font.FontFamily, richtxtboxNotepad.Font.Size + deltaSize,
                                                richtxtboxNotepad.Font.Style);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusBarToolStripMenuItem.Checked)
                {
                    statusBarToolStripMenuItem.Checked = false;
                    statusBarNotepad.Visible = false;
                }
                else if (statusBarToolStripMenuItem.Checked == false)
                {
                    statusBarToolStripMenuItem.Checked = true;
                    statusBarNotepad.Visible = true;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void StatusBarProgressBar(string strLabelMessage)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    statusBarProgressBar.Visible = true;
                    statusBarLabel.Text = strLabelMessage;
                    statusBarProgressBar.Maximum = 50;
                    statusBarProgressBar.Step = 5;
                });
                // Reset progress bar after completion
                for (int j = 0; j < statusBarProgressBar.Maximum; j++)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        statusBarProgressBar.PerformStep();
                        Thread.Sleep(10);
                        //Application.DoEvents();
                    });
                }
                //statusBarLabel.Text = " ";
                Invoke((MethodInvoker)delegate
                {
                    statusBarProgressBar.Value = 0;
                    statusBarLabel.Visible= false;
                    statusBarProgressBar.Visible = false;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #region<Commented Code>
        /*private void UpdateFormTitle()
        {
            try
            {
                if (this.Text.Length > 0 && !this.Text.EndsWith("*"))
                {
                    this.Text = "*" + this.Text;// Add asterisk (*) to the end of the title
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
        #endregion
    }
}