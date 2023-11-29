using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBP_Final_project
{
    public partial class InventoryManagementForm : Form
    {
        private string savedImagePath;
        string currentUserId = UserSession.UserId;
        string currentUserType = UserSession.UserType;
        public InventoryManagementForm()
        {
            InitializeComponent();
            // 이미지 드롭을 위한 이벤트 핸들러 설정
            // 이 코드는 폼의 Load 이벤트 핸들러나 다른 초기화 코드 부분에 추가할 수 있습니다.
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.YellowGreen;
            dataGridView1.EnableHeadersVisualStyles = false; // 이 설정이 없으면 색상이 적용되지 않을 수 있습니다.


            imagePbox.AllowDrop = true;
            imagePbox.DragEnter += new DragEventHandler(imagePbox_DragEnter);
            imagePbox.DragDrop += new DragEventHandler(imagePbox_DragDrop);
        }

        private void imagePbox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string sourceFilePath = files[0];
                    imagePbox.Image = Image.FromFile(sourceFilePath);

                    // 지정된 경로를 여기에 설정합니다.
                    string targetFolderPath = @"C:\Users\USER\Desktop\3학년2학기\DBP\실습과제코드\DBP Final project\ImageStorage";

                    // 해당 경로가 존재하지 않는 경우 폴더를 생성합니다.
                    if (!Directory.Exists(targetFolderPath))
                    {
                        Directory.CreateDirectory(targetFolderPath);
                    }

                    // 파일을 새 위치로 복사합니다 (파일명 충돌을 방지하기 위해 고유한 이름을 생성합니다).
                    string uniqueFileName = Path.GetFileNameWithoutExtension(sourceFilePath) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(sourceFilePath);
                    savedImagePath = Path.Combine(targetFolderPath, uniqueFileName);

                    // 파일을 새 위치로 복사합니다.
                    File.Copy(sourceFilePath, savedImagePath, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("이미지를 드롭하는 동안 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void imagePbox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void InventoryManagementForm_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'dataSet1.ITEM' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.iTEMTableAdapter.Fill(this.dataSet1.ITEM);
            dataGridView1.DataSource = dataSet1.Tables["ITEM"];
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                // Assigning values to text boxes
                idTbox.Text = row.Cells["ITEM_ID"].Value.ToString();
                nameTbox.Text = row.Cells["ITEM_NAME"].Value.ToString();
                priceTbox.Text = row.Cells["ITEM_PRICE"].Value.ToString();
                categoryTbox.Text = row.Cells["ITEM_CATEGORY"].Value.ToString();
                countTbox.Text = row.Cells["ITEM_COUNT"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["ITEM_DATE"].Value);
                contentTbox.Text = row.Cells["ITEM_CONTENT"].Value.ToString();

                // Handling the image
                string imagePath = row.Cells["ITEM_IMAGE"].Value.ToString();
                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                {
                    imagePbox.Image = Image.FromFile(imagePath); // Loading image into PictureBox
                }
                else
                {
                    imagePbox.Image = null; // Clearing the PictureBox if no valid image path is found
                }
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // ITEMREFUNDVIEW DataTable에 새로운 행을 생성합니다.
                DataRow newRow = dataSet1.ITEM.NewRow();
                newRow["ITEM_ID"] = int.Parse(idTbox.Text.Trim());
                newRow["ADMIN_ID"] = currentUserId;
                newRow["ITEM_IMAGE"] = savedImagePath;
                newRow["ITEM_NAME"] = nameTbox.Text.Trim();
                newRow["ITEM_PRICE"] = decimal.Parse(priceTbox.Text.Trim());
                newRow["ITEM_CATEGORY"] = categoryTbox.Text.Trim();
                newRow["ITEM_COUNT"] = int.Parse(countTbox.Text.Trim());
                newRow["ITEM_DATE"] = dateTimePicker1.Value;
                newRow["ITEM_CONTENT"] = contentTbox.Text.Trim();

                // 새로운 행을 DataTable에 추가합니다.
                dataSet1.ITEM.Rows.Add(newRow);

                // 변경 사항을 데이터베이스에 업데이트합니다.
                int numOfRows = iTEMTableAdapter.Update(dataSet1.ITEM);
                if (numOfRows < 1)
                {
                    MessageBox.Show("물품 등록 실패");
                }
                else
                {
                    MessageBox.Show("물품 등록 성공");
                    // 여기서 변경 사항을 dataGridView1에 반영합니다.
                    iTEMTableAdapter.Fill(dataSet1.ITEM);
                    dataGridView1.Refresh(); // UI를 새로고칩니다.
                }
            }
            catch (FormatException fex)
            {
                MessageBox.Show("숫자 입력 형식이 잘못되었습니다: " + fex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터를 저장하는 동안 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void modifyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // 선택된 행의 데이터를 가져옵니다.
                DataRow modifyRow = dataSet1.ITEM.Rows.Find(dataGridView1.SelectedCells[0].OwningRow.Cells["ITEM_ID"].Value);
                if (modifyRow != null)
                {
                    // 가져온 데이터를 수정합니다.
                    modifyRow["ITEM_ID"] = int.Parse(idTbox.Text.Trim());
                    modifyRow["ADMIN_ID"] = currentUserId;

                    // savedImagePath가 null이면 원래의 이미지 경로를 사용합니다.
                    modifyRow["ITEM_IMAGE"] = string.IsNullOrEmpty(savedImagePath) ? dataGridView1.SelectedCells[0].OwningRow.Cells["ITEM_IMAGE"].Value.ToString() : savedImagePath;

                    modifyRow["ITEM_NAME"] = nameTbox.Text.Trim();
                    modifyRow["ITEM_PRICE"] = decimal.Parse(priceTbox.Text.Trim());
                    modifyRow["ITEM_CATEGORY"] = categoryTbox.Text.Trim();
                    modifyRow["ITEM_COUNT"] = int.Parse(countTbox.Text.Trim());
                    modifyRow["ITEM_DATE"] = dateTimePicker1.Value;
                    modifyRow["ITEM_CONTENT"] = contentTbox.Text.Trim();

                    // 변경 사항을 데이터베이스에 업데이트합니다.
                    int numOfRows = iTEMTableAdapter.Update(modifyRow);
                    if (numOfRows < 1)
                    {
                        MessageBox.Show("물품 수정 실패");
                    }
                    else
                    {
                        MessageBox.Show("물품 수정 성공");
                        // 여기서 변경 사항을 dataGridView1에 반영합니다.
                        iTEMTableAdapter.Fill(dataSet1.ITEM);
                        dataGridView1.Refresh(); // UI를 새로고칩니다.
                    }
                }
            }
            catch (FormatException fex)
            {
                MessageBox.Show("숫자 입력 형식이 잘못되었습니다: " + fex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터를 수정하는 동안 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // 선택된 행의 데이터를 가져옵니다.
                DataRow deleteRow = dataSet1.ITEM.Rows.Find(dataGridView1.SelectedCells[0].OwningRow.Cells["ITEM_ID"].Value);
                if (deleteRow != null)
                {
                    // 가져온 데이터를 삭제합니다.
                    deleteRow.Delete();

                    // 변경 사항을 데이터베이스에 업데이트합니다.
                    int numOfRows = iTEMTableAdapter.Update(dataSet1.ITEM);
                    if (numOfRows < 1)
                    {
                        MessageBox.Show("물품 삭제 실패");
                    }
                    else
                    {
                        MessageBox.Show("물품 삭제 성공");
                        // 여기서 변경 사항을 dataGridView1에 반영합니다.
                        iTEMTableAdapter.Fill(dataSet1.ITEM);
                        dataGridView1.Refresh(); // UI를 새로고칩니다.
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터를 삭제하는 동안 오류가 발생했습니다: " + ex.Message);
            }
        }
    }
}
