-- Вставка данных в таблицу Категории
insert into categorie (category_name) values
('Сладкие'),
('Солёные'),
('Диетические');

-- Вставка данных в таблицу Булочек
insert into bun (name, price, category_id) values
('Круассан', 2.50, 1),
('Багет', 1.80, 2),
('Макарон', 3.00, 1),
('Сэндвич', 4.50, 2),
('Диетическая булочка', 2.00, 3);

-- Вставка данных в таблицу Ингредиентов
insert into ingredient (ingredient_name) values
('Мука'),
('Сахар'),
('Соль'),
('Дрожжи'),
('Киевское молоко'),
('Яйцо'),
('Шоколад'),
('Фрукты');

-- Вставка данных в таблицу Рецептов
insert into recipe (bun_id, ingredient_id, quantity) values
(1, 1, 200.00),  -- Круассан - Мука 200 г
(1, 2, 30.00),   -- Круассан - Сахар 30 г
(1, 3, 5.00),    -- Круассан - Соль 5 г
(2, 1, 300.00),  -- Багет - Мука 300 г
(2, 3, 7.00),    -- Багет - Соль 7 г
(2, 4, 10.00),   -- Багет - Дрожжи 10 г
(3, 1, 150.00),  -- Макарон - Мука 150 г
(3, 2, 100.00),  -- Макарон - Сахар 100 г
(4, 1, 250.00),  -- Сэндвич - Мука 250 г
(4, 5, 100.00),  -- Сэндвич - Киевское молоко 100 г
(5, 1, 200.00),  -- Диетическая булочка - Мука 200 г
(5, 6, 1.00);    -- Диетическая булочка - Яйцо 1 шт.

-- Вставка данных в таблицу Заказов
insert into "order" (bun_id, order_date, customer_name, quantity) values
(1, current_timestamp, 'Анна Смирнова', 3),  -- Заказ на 3 круассана
(2, current_timestamp, 'Иван Петров', 1),     -- Заказ на 1 багет
(3, current_timestamp, 'Ольга Сидорова', 5),   -- Заказ на 5 макаронов
(4, current_timestamp, 'Дмитрий Кузнецов', 2), -- Заказ на 2 сэндвича
(5, current_timestamp, 'Екатерина Волкова', 4); -- Заказ на 4 диетические булочки

insert into bun_table ("table", "column", description, data_type) values
('order', 'order_date', 'Дата и время заказа', 'DATETIME'),
('order', 'customer_name', 'Имя клиента, который сделал заказ', 'TEXT'),
('order', 'quantity', 'Количество заказанных булочек', 'INTEGER'),

('bun', 'name', 'Название булочки', 'TEXT'),
('bun', 'price', 'Цена булочки', 'REAL'),

('category', 'category_name', 'Название категории булочек', 'TEXT'),

('ingredient', 'ingredient_name', 'Название ингредиента', 'TEXT'),

('recipe', 'quantity', 'Количество данного ингредиента в рецепте', 'REAL');