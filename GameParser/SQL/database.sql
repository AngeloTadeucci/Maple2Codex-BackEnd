DROP
    DATABASE IF EXISTS maple2_codex;
CREATE
    DATABASE maple2_codex;

CREATE TABLE maple2_codex.items
(
    id                  INT              NOT NULL,
    name                varchar(100)     not NULL,
    tooltip_description text             not NULL,
    guide_description   text             not NULL,
    main_description    text             not NULL,
    rarity              tinyint unsigned not NULL default 1,
    is_outfit           tinyint          not NULL,
    job_limit           JSON             not NULL,
    job_recommend       JSON             not NULL,
    level_min           INT              not NULL default 0,
    level_max           INT              not NULL default 0,
    gender              tinyint unsigned not NULL default 0,
    icon_path           varchar(100)     not null default '',
    visit_count         int              not null default 0,
    pet_id              int              not null default 0,
    is_ugc              tinyint          not null default 0,
    transfer_type       int              not null default 0,
    sellable            tinyint          not null default 0,
    breakable           tinyint          not null default 0,
    fusionable          tinyint          not null default 0,
    skill_id            int              not null default 0,
    skill_level         int              not null default 0,
    stack_limit         int              not null default 0,
    tradeable_count     int              not null default 0,
    repackage_count     int              not null default 0,
    sell_price          JSON             not null,
    kfms                JSON             not null,
    icon_code           tinyint          not null,
    move_disable        tinyint          not null,
    remake_disable      tinyint          not null,
    gear_score          int              not null,
    enchantable         tinyint          not null,
    dyeable             tinyint          not null,
    constants_stats     JSON             NOT NULL,
    static_stats        JSON             NOT NULL,
    random_stats        JSON             NOT NULL,
    random_stat_count   tinyint          not null,
    slot                tinyint unsigned NOT NULL,
    set_info            JSON             NOT NULL,
    set_name            varchar(100)     not null default '',
    item_preset         varchar(4)       not null default '',
    glamour_count       int              not null default 0,
    repackage_scrolls   text             not null,
    repackage_limit     int              not null default 0,
    box_id              int              not null default 0,
    item_type           int              not null default 0,
    represent_option    int              not null default 0,
    CONSTRAINT items_pk PRIMARY KEY (id)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

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

create table maple2_codex.item_boxes
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

create table maple2_codex.maps
(
    id             int          not null,
    name           varchar(100) not null,
    visit_count    int          not null default 0,
    portrait       varchar(200) not null default '',
    constraint maps_pk primary key (id)
) engine = InnoDB
  default charset = utf8mb4
  collate = utf8mb4_0900_ai_ci;

  create table maple2_codex.achieves
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