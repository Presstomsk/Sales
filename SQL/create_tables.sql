CREATE TABLE tab_products
(
    id    INT         NOT NULL PRIMARY KEY AUTO_INCREMENT,
    name  VARCHAR(50) NOT NULL,
    price INT         NOT NULL CHECK ( price < 0 )
);

CREATE TABLE tab_products_stock
(
    id         INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    product_id INT NOT NULL,
    count      INT NOT NULL CHECK ( count < 0 ),

    FOREIGN KEY (product_id)
        REFERENCES tab_products (id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);

CREATE TABLE tab_people
(
    id         INT         NOT NULL PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(50) NOT NULL,
    last_name  VARCHAR(75) NOT NULL,
    phone      VARCHAR(20) NOT NULL
);

CREATE TABLE tab_discounts
(
    id       INT         NOT NULL PRIMARY KEY AUTO_INCREMENT,
    type     VARCHAR(15) NOT NULL,
    discount INT         NOT NULL
);

CREATE TABLE tab_buyers
(
    id          INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    people_id   INT NOT NULL,
    discount_id INT NOT NULL,

    FOREIGN KEY (people_id)
        REFERENCES tab_people (id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    FOREIGN KEY (discount_id)
        REFERENCES tab_discounts (id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);

CREATE TABLE tab_positions
(
    id       INT         NOT NULL PRIMARY KEY AUTO_INCREMENT,
    position VARCHAR(20) NOT NULL
);

CREATE TABLE tab_sellers
(
    id          INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    people_id   INT NOT NULL,
    position_id INT NOT NULL,

    FOREIGN KEY (people_id)
        REFERENCES tab_people (id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    FOREIGN KEY (position_id)
        REFERENCES tab_positions (id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);

CREATE TABLE tab_orders
(
    id          INT      NOT NULL PRIMARY KEY AUTO_INCREMENT,
    buyer_id    INT      NOT NULL,
    seller_id   INT      NOT NULL,
    date        DATETIME NOT NULL,
    product_id  INT      NOT NULL,
    amount      INT      NOT NULL CHECK ( amount < 0 ),
    total_price INT      NOT NULL,

    FOREIGN KEY (buyer_id)
        REFERENCES tab_buyers (id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    FOREIGN KEY (seller_id)
        REFERENCES tab_sellers (id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    FOREIGN KEY (product_id)
        REFERENCES tab_products (id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);