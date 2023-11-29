using Oracle.ManagedDataAccess.Client;
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
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            InitializeComponent();


            this.purchaseditemTableAdapter1.Fill(this.dataSet1.PURCHASEDITEM);
            this.purchaseTableAdapter1.Fill(this.dataSet1.PURCHASE);

        }
        private void ChartForm_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'dataSet1.SALESSUMMARYBYCATEGORY' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.sALESSUMMARYBYCATEGORYTableAdapter.Fill(this.dataSet1.SALESSUMMARYBYCATEGORY);
            // TODO: 이 코드는 데이터를 'dataSet1.SALESSUMMARYBYDAYOFWEEK' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.sALESSUMMARYBYDAYOFWEEKTableAdapter.Fill(this.dataSet1.SALESSUMMARYBYDAYOFWEEK);
            // TODO: 이 코드는 데이터를 'dataSet1.SALESSUMMARY' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.

            // TODO: 이 코드는 데이터를 'dataSet1.MOSTREFUNDEDITEMS' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.mOSTREFUNDEDITEMSTableAdapter.Fill(this.dataSet1.MOSTREFUNDEDITEMS);
            // TODO: 이 코드는 데이터를 'dataSet1.BESTSELLINGITEMS' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.bESTSELLINGITEMSTableAdapter.Fill(this.dataSet1.BESTSELLINGITEMS);
            // TODO: 이 코드는 데이터를 'dataSet1.SALESSUMMARY' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.

        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            // DateTimePicker에서 날짜를 얻는 방법
            var startDate = dateTimePickerStart.Value;
            var endDate = dateTimePickerEnd.Value;

            // PURCHASE와 PURCHASEITEM 데이터 가져오기
            purchaseTableAdapter1.Fill(dataSet1.PURCHASE);
            purchaseditemTableAdapter1.Fill(dataSet1.PURCHASEDITEM);

            // 데이터셋에서 조건에 맞는 데이터만 필터링
            string filterExpression = string.Format("PURCHASE_DATE >= #{0}# AND PURCHASE_DATE <= #{1}#", startDate.ToShortDateString(), endDate.ToShortDateString());

            // 월별로 그룹화된 데이터를 저장할 DataTable 생성
            DataTable monthlySalesData = new DataTable();
            monthlySalesData.Columns.Add("PurchaseMonth", typeof(string));
            monthlySalesData.Columns.Add("TotalSales", typeof(decimal));

            var groupedData = dataSet1.PURCHASE
                .AsEnumerable()
                .Where(row => row.Field<DateTime>("PURCHASE_DATE") >= startDate && row.Field<DateTime>("PURCHASE_DATE") <= endDate)
                .GroupBy(row => new
                {
                    Year = row.Field<DateTime>("PURCHASE_DATE").Year,
                    Month = row.Field<DateTime>("PURCHASE_DATE").Month
                })
                .Select(group => new
                {
                    PurchaseMonth = new DateTime(group.Key.Year, group.Key.Month, 1).ToString("yyyy-MM"),
                    TotalSales = group.Sum(g =>
                        dataSet1.PURCHASEDITEM
                        .AsEnumerable()
                        // PURCHASE_ID 필드 값을 int로 변환
                        .Where(item => Convert.ToInt32(item["PURCHASE_ID"]) == Convert.ToInt32(g["PURCHASE_ID"]))
                        // TOTAL_PRICE 필드 값을 decimal로 변환
                        .Sum(item => Convert.ToInt32(item["TOTAL_PRICE"])))
                });

            foreach (var entry in groupedData)
            {
                monthlySalesData.Rows.Add(entry.PurchaseMonth, entry.TotalSales);
            }

            // 차트 데이터 바인딩
            chartSales.DataSource = monthlySalesData;
            chartSales.Series["Series1"].XValueMember = "PurchaseMonth";
            chartSales.Series["Series1"].YValueMembers = "TotalSales";
            chartSales.DataBind();
        }
    }
}
