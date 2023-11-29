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
    public class ProductCard
    {
        private DataSet1 dataSet11;
        private DataSet1TableAdapters.CARTTableAdapter cARTTableAdapter;
        private DataSet1TableAdapters.REVIEWTableAdapter rEVIEWTableAdapter;
        private DataTable CartTable;
        public Panel CardPanel { get; private set; }
        public PictureBox ProductImage { get; private set; }
        public Label ProductName { get; private set; }
        public Label ProductPrice { get; private set; }
        public Label ProductCount { get; private set; }
        public Label ProductCategory { get; private set; }
        public Label ProductDate { get; private set; }
        public Label ProductContent { get; private set; }
        public Button AddToCartButton { get; private set; }
        public NumericUpDown numProductCount { get; private set; }
        public CheckBox SelectCheckBox { get; private set; }
        public string UserId { get; private set; }
        public string ItemId { get; private set; }
        public string ImagePath { get; private set; }
        public string Name { get; private set; }
        public string Price { get; private set; }
        public int Count { get; private set; }
        public int LiveCount { get; private set; }
        public string Category { get; private set; }
        public string Date { get; private set; }
        public string Content { get; private set; }
        private bool isAddToCartClicked = false;
        private goodsShowForm parentForm;
        public ProductCard(goodsShowForm parent, FlowLayoutPanel fPanelShowDetailGoods, string formName, string userId, string itemId, string imagePath, string name, string price, int count, string category, string date, string content)
        {
            UserId = userId;
            ItemId = itemId;
            ImagePath = imagePath; // 이미지경로
            Name = name; // 물품 이름
            Price = price; // 물품 가격
            LiveCount = count;
            Count = 1;
            Category = category;
            Date = date;
            Content = content;
            this.parentForm = parent;

            InitializeCardPanel(formName);
            InitializeProductImage(formName);
            InitializeProductName(formName);
            InitializeProductPrice(formName);
            InitializeProductCount(formName);

            if (formName != "cartForm")
            {
                dataSet11 = new DataSet1();
                cARTTableAdapter = new DataSet1TableAdapters.CARTTableAdapter();
                rEVIEWTableAdapter = new DataSet1TableAdapters.REVIEWTableAdapter();
                InitializeAddToCartButton();

            }
            else
            {
                InitializeSelectCheckBox();
                InitializeProductCountControl();
            }

            AddControlsToPanel();

            if(formName != "cartForm")
            {
                SetClickEvents(fPanelShowDetailGoods);
            }
           
        }

        private void InitializeCardPanel(string formName)
        {
            CardPanel = new Panel
            {
                Size = formName == "cartForm" ? new Size(800, 120) : new Size(200, 330),
                BorderStyle = BorderStyle.None,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10),
                Margin = new Padding(10)
            };

            CardPanel.Region = CreateRoundedRectangleRegion(CardPanel.Size);
        }

        private void InitializeProductImage(string formName)
        {
            ProductImage = new PictureBox
            {
                Size = formName == "cartForm" ? new Size(120, 120) : new Size(180, 180),
                Image = Image.FromFile(ImagePath),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = formName == "cartForm" ? new Point(35, 5): new Point(10, 5)
            };
        }

        private void InitializeProductName(string formName)
        {
            ProductName = new Label
            {
                Text = Name,
                Size = formName == "cartForm" ? new Size(100, 50): new Size(180, 30),
                Font = new Font("배달의민족 도현", 14, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = formName == "cartForm" ? new Point(180, 5) : new Point(10, 190),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private void InitializeProductPrice(string formName)
        {
            ProductPrice = new Label
            {
                Text = Price,
                Size = formName == "cartForm" ? new Size(110, 50): new Size(180, 30),
                Font = new Font("배달의민족 도현", 12, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Location = formName == "cartForm" ? new Point(180, 60) : new Point(10, 220),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private void InitializeProductCount(string formName)
        {
            ProductCount = new Label
            {
                Text = "남은 수량 :" + LiveCount.ToString(),
                Size = formName == "cartForm" ? new Size(150, 50): new Size(180, 30),
                Font = new Font("배달의민족 도현", 12, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Location = formName == "cartForm" ? new Point(300, 60) : new Point(10, 250),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private void InitializeProductCountControl()
        {
            numProductCount = new NumericUpDown
            {
                Minimum = 1,  // 최소값 설정
                Maximum = 100,  // 최대값 설정, 필요에 따라 조정 가능
                Value = 1,  // 현재 수량은 1 설정
                Size = new Size(120, 70),  // 적절한 크기 설정
                Location = new Point(600, 60),  // 적절한 위치 설정
                BorderStyle = BorderStyle.FixedSingle  // 보더 스타일 변경            
            };

            numProductCount.ValueChanged += (sender, e) =>
            {
                Count = (int)numProductCount.Value;
                // 필요한 경우 추가 작업을 수행, 예: 장바구니 수량 업데이트
            };
        }
        private void InitializeAddToCartButton()
        {
            // 버튼 생성 및 설정
            AddToCartButton = new Button
            {
                Text = "장바구니",
                Size = new Size(180, 30),
                Font = new Font("Arial", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Location = new Point((CardPanel.Width - 180) / 2, 290)
            };

            // 버튼 클릭 이벤트 핸들러
            AddToCartButton.Click += (sender, e) =>
            {
                AddItemToCart(UserId, ItemId, Name);
            };
        }


        private void AddItemToCart(string userId, string itemId, string name)
        {
            cARTTableAdapter.Fill(dataSet11.CART);
            DataTable cartTable = dataSet11.Tables["CART"];

            // CART 테이블에서 현재 사용자 ID와 아이템 ID가 일치하는 로우를 찾습니다.
            DataRow[] existingRows = cartTable.Select($"CUSTOMER_ID = '{userId}' AND ITEM_ID = '{itemId}'");

            // 일치하는 로우가 있다면, 이미 장바구니에 아이템이 있다는 메시지를 표시합니다.
            if (existingRows.Length > 0)
            {
                MessageBox.Show($"{name}은(는) 이미 장바구니에 있습니다.");
            }
            else
            {
                // 장바구니에 아이템이 없으므로 새로운 로우를 추가합니다.
                DataRow newRow = cartTable.NewRow();
                newRow["CUSTOMER_ID"] = userId;
                newRow["ITEM_ID"] = itemId;
                cartTable.Rows.Add(newRow);

                // 데이터베이스에 변경 사항을 업데이트하고 결과에 따라 메시지를 표시합니다.
                int numOfRows = cARTTableAdapter.Update(dataSet11.CART);
                if (numOfRows < 1)
                    MessageBox.Show("장바구니에 담는 것을 실패하였습니다.");
                else
                    MessageBox.Show($"{name}이(가) 장바구니에 추가되었습니다.");
            }
        }


        private void InitializeSelectCheckBox()
        {
            SelectCheckBox = new CheckBox
            {
                Size = new Size(20, 20),
                Location = new Point(10, (CardPanel.Height - 20) / 2)
            };
        }

        private void AddControlsToPanel()
        {
            if (SelectCheckBox != null)
            {
                CardPanel.Controls.Add(SelectCheckBox);
                CardPanel.Controls.Add(numProductCount);
            }
            CardPanel.Controls.Add(ProductImage);
            CardPanel.Controls.Add(ProductName);
            CardPanel.Controls.Add(ProductPrice);
            CardPanel.Controls.Add(ProductCount);


            if (AddToCartButton != null)
            {
                CardPanel.Controls.Add(AddToCartButton);
            }


        }

        private void SetClickEvents(FlowLayoutPanel fPanelShowDetailGoods)
        {
            CardPanel.Click += (sender, e) => ShowDetailInfo(fPanelShowDetailGoods);
            foreach (Control control in CardPanel.Controls)
            {
                if (control != AddToCartButton && control != SelectCheckBox)
                {
                    control.Click += (sender, e) => ShowDetailInfo(fPanelShowDetailGoods);
                }
            }
        }

        public void ShowDetailInfo(FlowLayoutPanel fPanelShowDetailGoods)
        {
            this.parentForm.CurrentItemId = this.ItemId;

            if (!isAddToCartClicked)
            {
               // fPanelShowDetailGoods.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                //fPanelShowDetailGoods.FlowDirection = FlowDirection.TopDown;
                fPanelShowDetailGoods.Controls.Clear();
                fPanelShowDetailGoods.Controls.Add(new PictureBox
                {
                    Size = new Size(fPanelShowDetailGoods.Width - 10, 220),  // 너비를 fPanelShowDetailGoods의 너비에 맞추고, 여백을 고려하여 10을 뺌
                    Image = Image.FromFile(ImagePath),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Location = new Point(5, 5)
                });

                fPanelShowDetailGoods.Controls.Add(new Label
                {
                    Text = Category,
                    Size = new Size(fPanelShowDetailGoods.Width - 10, 30),
                    Font = new Font("배달의민족 도현", 14, FontStyle.Regular),
                    ForeColor = Color.DimGray,
                    Location = new Point(5, 280),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                fPanelShowDetailGoods.Controls.Add(new Label
                {
                    Text = Name,
                    Size = new Size(fPanelShowDetailGoods.Width - 10, 30),
                    Font = new Font("Arial", 15, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(5, 190),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                fPanelShowDetailGoods.Controls.Add(new Label
                {
                    Text = Content,
                    Size = new Size(fPanelShowDetailGoods.Width - 10, 30),
                    Font = new Font("배달의민족 도현", 13, FontStyle.Regular),
                    ForeColor = Color.Black,
                    Location = new Point(5, 320),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                fPanelShowDetailGoods.Controls.Add(new Label
                {
                    Text = Price,
                    Size =  new Size(fPanelShowDetailGoods.Width - 10, 30),
                    Font = new Font("배달의민족 도현", 13, FontStyle.Regular),
                    ForeColor = Color.DimGray,
                    Location = new Point(5, 220),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                fPanelShowDetailGoods.Controls.Add(new Label
                {

                        Text = "남은 수량 :" + LiveCount.ToString(),
                        Size = new Size(fPanelShowDetailGoods.Width - 10, 30),
                    Font = new Font("Arial", 12, FontStyle.Regular),
                        ForeColor = Color.DimGray,
                        Location = new Point(5, 250),
                        TextAlign = ContentAlignment.MiddleCenter
                });

                fPanelShowDetailGoods.Controls.Add(new Label
                {
                    Text = "물품 등록일 : \r\n" + Date,
                    Size = new Size(fPanelShowDetailGoods.Width - 10, 55),
                    Font = new Font("배달의민족 도현", 12, FontStyle.Regular),
                    ForeColor = Color.DimGray,
                    Location = new Point(5, 300),
                    TextAlign = ContentAlignment.MiddleCenter
                });

                // 데이터베이스에서 해당 ITEM_ID의 리뷰 데이터를 불러옵니다.

                var reviews = GetReviewsFromDatabase(int.Parse(ItemId), UserId);

                // 불러온 리뷰들을 리뷰 패널에 추가합니다.
                foreach (var review in reviews)
                {
                    FlowLayoutPanel reviewFlowPanel = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.TopDown,
                        WrapContents = false,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        BorderStyle = BorderStyle.None, // 테두리를 없애기 위함
                    };

                    // 사용자 ID를 표시할 Label을 생성하고 Panel에 추가합니다.
                    Label userIdLabel = new Label
                    {
                        Text = review.Author,
                        Font = new Font("Arial", 11, FontStyle.Bold),
                        BackColor = Color.FromArgb(144, 238, 144),
                        AutoSize = true,
                        Size = new Size(fPanelShowDetailGoods.Width - 10, 30),

                        // 여기에 추가 스타일을 설정할 수 있습니다.
                    };
                    reviewFlowPanel.Controls.Add(userIdLabel);

                    // 별점을 표시할 Label을 생성하고 Panel에 추가합니다.
                    Label starLabel = new Label
                    {
                        Text = $"{new string('★', review.StarRating)}",
                        ForeColor = Color.Gold,
                        AutoSize = true,
                        Size = new Size(fPanelShowDetailGoods.Width - 10, 30),
                        // 여기에 추가 스타일을 설정할 수 있습니다.
                    };
                    reviewFlowPanel.Controls.Add(starLabel);

                    // 리뷰 내용을 표시할 Label을 생성하고 Panel에 추가합니다.
                    Label reviewLabel = new Label
                    {
                        Text = review.Content,
                        Size = new Size(fPanelShowDetailGoods.Width - 10, 30),
                        AutoSize = true,
                        // 여기에 추가 스타일을 설정할 수 있습니다.
                    };
                    reviewFlowPanel.Controls.Add(reviewLabel);

                    // 생성한 리뷰 Panel을 fPanelShowDetailGoods에 추가합니다.
                    fPanelShowDetailGoods.Controls.Add(reviewFlowPanel);
                }
            }

            isAddToCartClicked = false;
        }
        private List<Review> GetReviewsFromDatabase(int itemId, string customerId)
        {
            var reviewsList = new List<Review>();

            // REVIEW 테이블에서 데이터를 가져옵니다.
            rEVIEWTableAdapter.Fill(dataSet11.REVIEW);
            DataTable reviewTable = dataSet11.Tables["REVIEW"];

            // ITEM_ID와 CUSTOMER_ID에 해당하는 리뷰를 찾습니다.
            foreach (DataRow row in reviewTable.Rows)
            {
                if (Convert.ToInt32(row["ITEM_ID"]) == itemId)
                {
                    reviewsList.Add(new Review
                    {
                        ItemId = itemId,
                        Author = row["CUSTOMER_ID"].ToString(),
                        Content = row["CONTENT"].ToString(),
                        StarRating = Convert.ToInt32(row["STAR_COUNT"]),
                        // Date = DateTime.Now // 예시로 현재 날짜를 사용합니다. 실제로는 리뷰 날짜 컬럼이 필요합니다.
                        // Date 컬럼이 있다면 row에서 가져와야 합니다. 예: Date = (DateTime)row["REVIEW_DATE"]
                    });
                }
            }

            return reviewsList;
        }

        private Region CreateRoundedRectangleRegion(Size size)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, 20, 20, 180, 90);
            path.AddArc(size.Width - 20, 0, 20, 20, 270, 90);
            path.AddArc(size.Width - 20, size.Height - 20, 20, 20, 0, 90);
            path.AddArc(0, size.Height - 20, 20, 20, 90, 90);
            path.CloseAllFigures();

            return new Region(path);
        }
    }
    public class Review
    {
        public int ItemId { get; set; } // 리뷰가 달린 아이템의 ID
        public string Author { get; set; } // 리뷰 작성자
        public string Content { get; set; } // 리뷰 내용
        public int StarRating { get; set; } // 별점 (예: 1~5)
        public DateTime Date { get; set; } // 리뷰 작성 날짜

        // 추가적으로 필요한 다른 속성들을 여기에 선언할 수 있습니다.
    }

}
