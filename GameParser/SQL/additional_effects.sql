DROP TABLE IF EXISTS maple2_codex.additional_effects;

create table maple2_codex.additional_effects (
    id int not null,
    icon_path varchar(100) not null,
    name varchar(400) not null,
    description text not null,
    levels JSON not null,
    constraint additional_effects_pk primary key (id)
) engine = InnoDB default charset = utf8mb4 collate = utf8mb4_0900_ai_ci;