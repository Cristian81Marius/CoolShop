INSERT INTO Suppliers (Id,Name, Description) Values ( 1,' Carturesti ','Book Shop company');
INSERT INTO Suppliers (Id,Name, Description) Values (2,'Kosmos','Game productor from Romania');

INSERT INTO ProductsCategory (Id,Name,Department, Description) Values ( 1,'Board Game','Battle','Players vs Players');
INSERT INTO ProductsCategory (Id,Name,Department, Description) Values ( 2,'Logic Puzzles','Puzzle','Logic puzzles come in all shapes and sizes, but the kind of puzzles we offer here are most commonly referred to as "logic grid"');

INSERT INTO Products (Id,Name, DefaultPrice, Currency, Description, SupplierId,CategoryId,ImageUrl,Amount) VALUES (1, 'Catan', 120, 'RON', 'Fantastic price. Large content ecosystem. Good parental controls. Helpful technical support.',1,1,'https://s13emagst.akamaized.net/products/407/406494/images/res_4deebbea0faa5c557897d344ec94f811.jpg?width=450&height=450&hash=DACD8EA18D92EE373FD445FC79E96C32',2);
INSERT INTO Products (Id,Name, DefaultPrice, Currency, Description, SupplierId,CategoryId,ImageUrl,Amount) VALUES (2, 'Monopoly', 150, 'RON', 'Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.',2,1,'https://s13emagst.akamaized.net/products/8418/8417398/images/res_3a2f4df22870b1e705e3c338f701257d.jpg?width=450&height=450&hash=6B38FF01B072EBDBECE5DE6427F09422',7);
INSERT INTO Products (Id,Name, DefaultPrice, Currency, Description, SupplierId,CategoryId,ImageUrl,Amount) VALUES (3, 'Obscurities Matchbox', 134, 'RON', 'Rezolvarea problemelor, oricare ar fi natura lor, presupune logica',1,2,'https://s13emagst.akamaized.net/products/29732/29731039/images/res_6a7f6fe59d025c7b84ae00697985bcdc.jpg?width=450&height=450&hash=72F4EABDD40EF84568DDB24F03D1C474',3);
INSERT INTO Products (Id,Name, DefaultPrice, Currency, Description, SupplierId,CategoryId,ImageUrl,Amount) VALUES (4, 'Rubick Cube', 40, 'RON', 'MoYu MofangJiao are acum o noua serie magnetica, numita linia MeiLong M.',2,2,'https://s13emagst.akamaized.net/products/37671/37670446/images/res_3f15fec7c61ce776377bf46256c21d9b.jpg?width=450&height=450&hash=D16172272973E90341B0FFFD2B6CE1E4',30);
INSERT INTO Products (Id,Name, DefaultPrice, Currency, Description, SupplierId,CategoryId,ImageUrl,Amount) VALUES (5, 'Chass', 101.0, 'RON', 'Joaca ca un profesionist - sau petrece timp cu prietenii si familia ',1,1,'https://handmadein.ro/wp-content/uploads/2021/12/44fbanp8-768x477.jpg',10);





