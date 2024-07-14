DROP TABLE IF EXISTS {databaseName}.maps;

create table {databaseName}.maps (
  id int not null,
  name varchar(100) not null,
  visit_count int not null default 0,
  minimap varchar(200) not null default '',
  icon varchar(200) not null default '',
  bg varchar(200) not null default '',
  constraint maps_pk primary key (id)
) engine = InnoDB default charset = utf8mb4 collate = utf8mb4_0900_ai_ci;