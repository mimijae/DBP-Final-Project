using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBP_Final_project
{
    public partial class PurchaseDetailsForm : Form
    {
        string currentUserId = UserSession.UserId;
        string currentUserType = UserSession.UserType;
        public PurchaseDetailsForm()
        {
            InitializeComponent();
            this.refundTableAdapter1.Fill(this.dataSet1.REFUND);
            this.purchaseditemTableAdapter1.Fill(this.dataSet1.PURCHASEDITEM);
            this.itemTableAdapter1.Fill(this.dataSet1.ITEM);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.YellowGreen;
            dataGridView1.EnableHeadersVisualStyles = false; // 이 설정이 없으면 색상이 적용되지 않을 수 있습니다.


            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.YellowGreen;
            dataGridView2.EnableHeadersVisualStyles = false; // 이 설정이 없으면 색상이 적용되지 않을 수 있습니다.

            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.YellowGreen;
            dataGridView3.EnableHeadersVisualStyles = false; // 이 설정이 없으면 색상이 적용되지 않을 수 있습니다.
            //dataGridView2.DataSource = dataSet1.Tables["REFUND"];
        }

        private void PurchaseDetailsForm_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'dataSet1.CUSTOMER_PURCHASE_DETAIL_SUMMAR' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.cUSTOMER_PURCHASE_DETAIL_SUMMARTableAdapter.Fill(this.dataSet1.CUSTOMER_PURCHASE_DETAIL_SUMMAR);
            // TODO: 이 코드는 데이터를 'dataSet11.CUSTOMER_PURCHASE_DETAILS' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.

            // TODO: 이 코드는 데이터를 'dataSet1.REFUNDREQUESTSVIEW' 테이블에 로드합니다. 필요 시 이 코드를 이동하나 제거할 수 있습니다.
            this.rEFUNDREQUESTSVIEWTableAdapter.Fill(this.dataSet1.REFUNDREQUESTSVIEW);
            // TODO: 이 코드는 데이터를 'dataSet1.CUSTOMERPURCHASESUMMARY' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.cUSTOMERPURCHASESUMMARYTableAdapter.Fill(this.dataSet1.CUSTOMERPURCHASESUMMARY);


        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 환불 버튼이 클릭되었는지 확인
            if (dataGridView2.Columns["RefundButton"] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                // 해당 행의 'RefundProcessing' 값을 가져옵니다.
                string refundProcessingValue = dataGridView2.Rows[e.RowIndex].Cells["rEFUNDPROCESSINGDataGridViewTextBoxColumn"].Value?.ToString();
                if (refundProcessingValue == "false")
                {
  
                    // PURCHASE_ID와 ITEM_ID를 안전하게 가져옵니다.
                    if (int.TryParse(dataGridView2.Rows[e.RowIndex].Cells["pURCHASEIDDataGridViewTextBoxColumn"].Value?.ToString(), out int purchaseId) &&
                        int.TryParse(dataGridView2.Rows[e.RowIndex].Cells["iTEMIDDataGridViewTextBoxColumn"].Value?.ToString(), out int itemId))
                    {
                        // REFUND 테이블의 해당 행을 찾아 업데이트합니다.
                        string customerEmail = dataGridView2.Rows[e.RowIndex].Cells["cUSTOMEREMAILDataGridViewTextBoxColumn"].Value?.ToString();
                        var refundRow = dataSet1.REFUND.FindByCUSTOMER_IDITEM_IDPURCHASE_ID(customerEmail, itemId, purchaseId);

                        if (refundRow != null)
                        {

                            // PURCHASE_ID와 ITEM_ID 조건에 맞는 행들을 필터링합니다.
                            string filterExpression = $"PURCHASE_ID = {purchaseId} AND ITEM_ID = {itemId}";

                            DataRow[] filteredRows = dataSet1.PURCHASEDITEM.Select(filterExpression);

                            // 필터링된 행들을 처리합니다.
                            foreach (DataRow row in filteredRows)
                            {

                                // ITEM 테이블에서 해당 ITEM_ID의 행을 찾습니다.
                                DataRow itemRow = dataSet1.ITEM.FindByITEM_ID(itemId);
                                if (itemRow != null)
                                {
                                    // 환불된 아이템 수만큼 ITEM_COUNT를 증가시킵니다.
                                    itemRow["ITEM_COUNT"] = Convert.ToInt32(itemRow["ITEM_COUNT"]) + Convert.ToInt32(row["ITEM_COUNT"]);
                                    itemTableAdapter1.Update(dataSet1.ITEM); // 변경된 재고를 데이터베이스에 반영
                                }
                            }


                            refundRow.REFUND_PROCESSING = "true"; // 값을 'true'로 업데이트
                            refundTableAdapter1.Update(dataSet1.REFUND); // 데이터베이스에 변경 사항을 반영
                            MessageBox.Show("환불이 수락되었습니다.");



                            // 버튼의 텍스트를 '환불완료'로 변경하고 비활성화합니다.
                            DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)dataGridView2.Rows[e.RowIndex].Cells["RefundButton"];
                            buttonCell.Value = "환불완료";
                            buttonCell.ReadOnly = true;
                            dataGridView2.InvalidateCell(buttonCell); // UI를 새로고침
                            this.rEFUNDREQUESTSVIEWTableAdapter.Fill(this.dataSet1.REFUNDREQUESTSVIEW);
                        }
                    }
                    else
                    {
                        MessageBox.Show("유효하지 않은 PURCHASE_ID 또는 ITEM_ID 값입니다.");
                    }
                }
            }
        }

    }
}
