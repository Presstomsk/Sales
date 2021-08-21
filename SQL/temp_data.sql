#Заполенение таблицы продуктов
INSERT INTO host1323541_sbd10.tab_products (name, price)
VALUES ('Phone', 5000);

INSERT INTO host1323541_sbd10.tab_products (name, price)
VALUES ('Car', 50000);

#Заполенение таблицы остатков
INSERT INTO host1323541_sbd10.tab_products_stock (product_id, count)
VALUES (1, 10);

INSERT INTO host1323541_sbd10.tab_products_stock (product_id, count)
VALUES (2, 5);

INSERT INTO host1323541_sbd10.tab_people (first_name, last_name, phone)
VALUES ('Andrey', 'Starinin', '+79042144491');

INSERT INTO host1323541_sbd10.tab_people (first_name, last_name, phone)
VALUES ('Anonim', 'Anonimus', '+71231234567');

INSERT INTO host1323541_sbd10.tab_discounts (type, discount)
VALUES ('Gold', 50);

INSERT INTO host1323541_sbd10.tab_discounts (type, discount)
VALUES ('Silver', 25);

INSERT INTO host1323541_sbd10.tab_buyers (people_id, discount_id)
VALUES (2, 1);

INSERT INTO host1323541_sbd10.tab_positions (position)
VALUES ('chief manager');

INSERT INTO host1323541_sbd10.tab_positions (position)
VALUES ('manager');

INSERT INTO host1323541_sbd10.tab_sellers (people_id, position_id)
VALUES (1, 1);