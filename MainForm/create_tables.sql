create table if not exists category (
    category_id serial primary key,
    category_name varchar(255) not null,
    constraint uk_category unique (category_name)
);
comment on table category is 'Таблица Категории булочек';
comment on column category.category_id is 'Уникальный идентификатор категории';
comment on column category.category_name is 'Название категории булочек';

create table if not exists bun (
    bun_id serial primary key,
    name varchar(255) not null,
    price numeric(10, 2) not null,
    category_id int,
    constraint fk_category
        foreign key (category_id) 
        references category(category_id) 
        on delete set null,
    constraint uk_bun unique (name, price)
);
comment on table bun is 'Таблица Булочек';
comment on column bun.bun_id is 'Уникальный идентификатор булочки';
comment on column bun.name is 'Название булочки';
comment on column bun.price is 'Цена булочки';
comment on column bun.category_id is 'Идентификатор категории булочки';

create table if not exists ingredient (
    ingredient_id serial primary key,
    ingredient_name varchar(255) not null,
    constraint uk_ingredient unique (ingredient_name)
);
comment on table ingredient is 'Таблица Ингредиентов';
comment on column ingredient.ingredient_id is 'Уникальный идентификатор ингредиента';
comment on column ingredient.ingredient_name is 'Название ингредиента';

create table if not exists recipe (
    recipe_id serial primary key,
    bun_id int,
    ingredient_id int,
    quantity numeric(10, 2) not null,
    constraint fk_bun
        foreign key (bun_id) 
        references bun(bun_id) 
        on delete cascade,
    constraint fk_ingredient
        foreign key (ingredient_id) 
        references ingredient(ingredient_id) 
        on delete cascade,
    constraint unique_bun_ingredient
        unique (bun_id, ingredient_id)  -- уникальность комбинации булочки и ингредиента
);
comment on table recipe is 'Таблица Рецептов';
comment on column recipe.recipe_id is 'Уникальный идентификатор рецепта';
comment on column recipe.bun_id is 'Идентификатор булочки, к которой относится рецепт';
comment on column recipe.ingredient_id is 'Идентификатор ингредиента в рецепте';
comment on column recipe.quantity is 'Количество данного ингредиента в рецепте';

create table if not exists "order" (
    order_id serial primary key,
    bun_id int,
    order_date timestamp not null default current_timestamp,
    customer_name varchar(255) not null,
    quantity int not null,
    constraint fk_order_bun
        foreign key (bun_id) 
        references bun(bun_id) 
        on delete cascade,
    constraint uk_order unique (bun_id, order_date, customer_name, quantity)
);
comment on table "order" is 'Таблица Заказов';
comment on column "order".order_id is 'Уникальный идентификатор заказа';
comment on column "order".bun_id is 'Идентификатор булочки, которая была заказана';
comment on column "order".order_date is 'Дата и время заказа';
comment on column "order".customer_name is 'Имя клиента, который сделал заказ';
comment on column "order".quantity is 'Количество заказанных булочек';