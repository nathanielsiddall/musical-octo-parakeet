CREATE DATABASE boilerplate;

CREATE USER boilerplate_system WITH PASSWORD 'password';

GRANT ALL PRIVILEGES ON DATABASE boilerplate TO boilerplate_system;

ALTER DATABASE boilerplate OWNER TO boilerplate_system;

ALTER SCHEMA public OWNER TO boilerplate_system;


