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
    public partial class goodsPaidForm : Form
    {
        DataSet1.PURCHASEDataTable purchasedataTable; // PURCHASEDataTable로 타입 변경
        DataSet1.PURCHASEDITEMDataTable purchasedItemTable;
        DataSet1.ITEMDataTable itemTable;
        string currentUserId = UserSession.UserId;
        string currentUserType = UserSession.UserType;
        string itemId = string.Empty;
        private List<PaidProductCard> productCards = new List<PaidProductCard>();  // ProductCard 인스턴스 목록
        public goodsPaidForm()
        {
            InitializeComponent();
            purchaseTableAdapter1.Fill(dataSet11.PURCHASE);
            purchaseditemTableAdapter1.Fill(dataSet11.PURCHASEDITEM);
            itemTableAdapter1.Fill(dataSet11.ITEM);

            purchasedataTable = dataSet11.PURCHASE;
            purchasedItemTable = dataSet11.PURCHASEDITEM;
            itemTable = dataSet11.ITEM;

            // 사용자 ID로 PURCHASE 테이블 검색
            string filterExpression = $"CUSTOMER_ID = '{currentUserId}'";
            DataRow[] purchaseRows = purchasedataTable.Select(filterExpression);


            var purchasedItemInfos = new List<(string ItemId, int Count, decimal TotalPrice, DateTime PurchaseDate)>();

            // 각 구매 건에 대해 PURCHASEDITEM 테이블 검색
            // 각 구매 건에 대해 PURCHASEDITEM 테이블 검색
            foreach (DataRow purchaseRow in purchaseRows)
            {
                int purchaseId = Convert.ToInt32(purchaseRow["PURCHASE_ID"]);

                DataRow[] purchasedItemRows = purchasedItemTable.Select($"PURCHASE_ID = {purchaseId}");


                foreach (DataRow purchasedItemRow in purchasedItemRows)
                {
                    try
                    {
                        // PURCHASEDITEM 테이블에서 아이템 정보 수집
                        var itemInfo = (
                            ItemId: purchasedItemRow["ITEM_ID"].ToString(),
                            Count: Convert.ToInt32(purchasedItemRow["ITEM_COUNT"]),
                            TotalPrice: Convert.ToDecimal(purchasedItemRow["TOTAL_PRICE"]),
                            PurchaseDate: Convert.ToDateTime(purchaseRow["PURCHASE_DATE"])
                        );
                        purchasedItemInfos.Add(itemInfo);

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error adding item info: {ex.Message}");
                    }
                }
            }

            var productInfos = new List<(string UserId, string ItemId, string ImagePath, string Name, string Price, string Count, string Date)>();
            foreach (var info in purchasedItemInfos)
            {
                DataRow[] itemRows = itemTable.Select($"ITEM_ID = {int.Parse(info.ItemId)}");


                foreach (DataRow itemRow in itemRows)
                {
                    var productInfo = (
                        UserId: currentUserId,
                        ItemId: info.ItemId,
                        ImagePath: itemRow["ITEM_IMAGE"].ToString(),
                        Name: itemRow["ITEM_NAME"].ToString(),
                        Price: info.TotalPrice.ToString(),
                        Count: info.Count.ToString(),
                        Date: info.PurchaseDate.ToString("yyyy-MM-dd")
                    );

                    productInfos.Add(productInfo);

                }


            }


            int currentTop = 0; // 현재 그룹 박스의 Top 위치를 저장하는 변수
            foreach (DataRow purchaseRow in purchaseRows.OrderByDescending(r => r["PURCHASE_DATE"])) // 최신 날짜의 구매가 먼저 오도록 정렬
            {
                int purchaseId = Convert.ToInt32(purchaseRow["PURCHASE_ID"]);
                var purchaseGroup = new PurchaseGroup();
                DataRow[] purchasedItemRows = purchasedItemTable.Select($"PURCHASE_ID = {purchaseId}");

                foreach (DataRow purchasedItemRow in purchasedItemRows)
                {
                    // PURCHASEDITEM 테이블에서 아이템 정보 수집
                    string itemId = purchasedItemRow["ITEM_ID"].ToString();
                    DataRow[] itemRows = itemTable.Select($"ITEM_ID = '{itemId}'");

                    foreach (DataRow itemRow in itemRows)
                    {
                        var productCard = new PaidProductCard(
                            currentUserId,
                            purchasedItemRow["PURCHASE_ID"].ToString(),
                            itemId,
                            itemRow["ITEM_IMAGE"].ToString(),
                           itemRow["ITEM_NAME"].ToString(),
                            purchasedItemRow["TOTAL_PRICE"].ToString(),
                           Convert.ToInt32(purchasedItemRow["ITEM_COUNT"]),
                            purchaseRow["PURCHASE_DATE"].ToString()
                        );
                        purchaseGroup.AddProductCard(productCard);
                    }
                }
                purchaseGroup.ArrangeItems();
                purchaseGroup.GroupBox.Top = currentTop + 20; // 그룹 박스의 Top 위치를 지정 (레이블 높이만큼 더해줌)

                // 레이블 생성 및 설정
                Label label = new Label();
                label.Text = $"[구매한 날짜 : {purchaseRow["PURCHASE_DATE"].ToString()}]";
                label.AutoSize = true;
                label.Location = new Point(10, currentTop); // 레이블의 위치를 그룹 박스 바로 위로 설정

                panel.Controls.Add(label); // 레이블을 Panel에 추가
                panel.Controls.Add(purchaseGroup.GroupBox); // 그룹 박스를 Panel에 추가

                currentTop += purchaseGroup.GroupBox.Height + 30;

            }

        }

        public class PurchaseGroup
        {
            public GroupBox GroupBox { get; set; }
            public List<PaidProductCard> ProductCards { get; set; }

            public PurchaseGroup()
            {
                GroupBox = new GroupBox();
                ProductCards = new List<PaidProductCard>();
                GroupBox.Padding = new Padding(25); // 그룹 박스에 25의 패딩을 추가
            }

            public void AddProductCard(PaidProductCard card)
            {
                ProductCards.Add(card);
                // 가정: PaidProductCard에 CardPanel이라는 Control을 반환하는 프로퍼티가 있음
                GroupBox.Controls.Add(card.CardPanel); // UI 컨트롤을 나타내는 card의 프로퍼티를 사용
            }

            // 구매 그룹 박스의 크기, 스타일 등을 설정하는 메서드
            public void groupboxText(string title)
            {
                GroupBox.Text = title;
            }
            public void SetProperties(string title, int width, int height)
            {

                GroupBox.Width = width;
                GroupBox.Height = height;
                // 추가적인 스타일 설정이 필요하다면 여기에 구현합니다.
            }

            // 그룹박스에 아이템을 적절히 배치하는 메서드
            public void ArrangeItems()
            {
                const int padding = 15; // 좌우 패딩
                int x = padding; // 시작 위치는 좌측 패딩부터
                int cardWidth = 150; // 카드의 너비
                int spaceBetweenCards = 10; // 카드 사이의 간격

                foreach (var card in ProductCards)
                {
                    card.CardPanel.Location = new Point(x, padding); // 위쪽 패딩도 동일하게 적용
                    x += cardWidth + spaceBetweenCards; // 다음 카드의 X 위치 계산
                }

                // 그룹박스의 너비를 카드 수와 패딩에 맞춰 동적으로 설정
                int groupBoxWidth = ProductCards.Count * (cardWidth + spaceBetweenCards) + padding;
                SetProperties(GroupBox.Text, groupBoxWidth, 275 + (2 * padding)); // 카드의 높이 + 위아래 패딩
            }

        }

    }

}
