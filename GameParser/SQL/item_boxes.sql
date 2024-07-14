DROP TABLE IF EXISTS {databaseName}.item_boxes;

create table {databaseName}.item_boxes
(
    uid             int auto_increment,
    box_id          int     not null,
    item_id         int     not null,
    item_id2        int     not null,
    min_count       int     not null,
    max_count       int     not null,
    rarity          tinyint not null,
    smart_drop_rate int     not null,
    group_drop_id   int     not null,
    constraint item_boxes_pk primary key (uid)
) engine = InnoDB
  default charset = utf8mb4
  collate = utf8mb4_0900_ai_ci;