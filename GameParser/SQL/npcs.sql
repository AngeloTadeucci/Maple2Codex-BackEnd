DROP TABLE IF EXISTS maple2_codex.npcs;

create table maple2_codex.npcs
(
    id             int          not null,
    name           varchar(100) not null,
    kfm            text         not null,
    is_boss        tinyint      not null default 0,
    npc_type       int          not null default 0,
    gender         tinyint      not null default 0,
    level          int          not null default 0,
    portrait       varchar(200) not null default '',
    stats          text         not null,
    visit_count    int          not null default 0,
    animations     text         not null,
    race           varchar(100) not null default '',
    class_name     varchar(100) not null default '',
    field_metadata text         not null,
    title          varchar(100) not null default '',
    shop_id        int          not null default 0,
    skills         text         not null,
    constraint npcs_pk primary key (id)
) engine = InnoDB
  default charset = utf8mb4
  collate = utf8mb4_0900_ai_ci;