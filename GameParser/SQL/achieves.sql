DROP TABLE IF EXISTS {databaseName}.achieves;

create table {databaseName}.achieves
(
    id             int          not null,
    name           varchar(100) not null,
    visit_count    int          not null default 0,
    portrait       varchar(200) not null default '',
    description             text         not NULL,
    complete_description    text         not NULL,
    icon           varchar(100) not null,
    constraint trophies_pk primary key (id)
) engine = InnoDB
  default charset = utf8mb4
  collate = utf8mb4_0900_ai_ci;