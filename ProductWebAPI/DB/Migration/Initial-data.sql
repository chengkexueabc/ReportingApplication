CREATE DATABASE `product` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;


CREATE TABLE `transactions` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProductCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Price` double NOT NULL,
  `Quantity` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE `shippings` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProductCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Destination` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ShippingTime` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


/*
-- Query: SELECT * FROM product.transactions
LIMIT 0, 1000

-- Date: 2023-02-05 11:11
*/
INSERT INTO `` (`Id`,`ProductCode`,`ProductName`,`Price`,`Quantity`) VALUES (1,'001','T-Shirt',105,500);
INSERT INTO `` (`Id`,`ProductCode`,`ProductName`,`Price`,`Quantity`) VALUES (2,'002','Jacket',80,1000);
INSERT INTO `` (`Id`,`ProductCode`,`ProductName`,`Price`,`Quantity`) VALUES (3,'002','Jacket',120,1500);


/*
-- Query: SELECT * FROM product.shippings
LIMIT 0, 1000

-- Date: 2023-02-05 11:09
*/
INSERT INTO `` (`Id`,`ProductCode`,`ProductName`,`Destination`,`ShippingTime`) VALUES (1,'001','T-Shirt','GuangDong',10);
INSERT INTO `` (`Id`,`ProductCode`,`ProductName`,`Destination`,`ShippingTime`) VALUES (2,'002','Jacket','HuBei',20);
