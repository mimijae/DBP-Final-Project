# 실감응용 데이터베이스 프로그래밍 기말 프로젝트
c# winform과 Oracle을 이용한 실감응용 데이터베이스 프로그래밍 기말 프로젝트입니다.

## 📌주제
오프라인 회원제 공산품 마트 데이터베이스 및 응용 프로그램 구현하기

## 🛠 기술스택
![C#](https://img.shields.io/badge/-CSharp-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Oracle](https://img.shields.io/badge/-Oracle-F80000?style=for-the-badge&logo=oracle&logoColor=white)

## ⚙️ 주요 기능

<관리자>
1. 새로운 상품 입고
2. 매장의 기존 상품 가격 변경 기능
3. 상품의 속성 현황 통계 (환불이 많은 것, 재고가 많은 것, 잘 팔리는 것, ..) 
4. 상품 판매액 현황 특정 기간을 지정하여 통계 확인

<판매사원>
1. 어떤 고객의 장바구니의 상품 판매 처리(영수증 생성, 판매액, 재고변화) 
2. 어떤 고객의 환불 요청 처리 (영수증을 기준으로 처리, 판매액, 반품된 상품수 변화)  
3. 우수고객, 불량고객의 선정 및 처리 기능

<고객>
1. 고객 메뉴에서 장바구니 담기 화면  (구매 가능한 상품, 가격 등 확인)
2. 상품 구매 후기 작성 및 다른 사람의 후기 확인 기능  

## 📋 ERD
![Alt text](static/ERD1.png)

## ✅ 회원가입 및 로그인 시나리오
![Alt text](static/login.gif)

## ✅ 상품 및 재고관리 시나리오
![Alt text](static/page1.gif)

## ✅ 상품 장바구니 담기 및 구매, 환불 시나리오
![Alt text](static/page2.gif)

## ✅ 구매, 환불 내역 확인 및 매출 통계 확인 시나리오
![Alt text](static/page3.gif)