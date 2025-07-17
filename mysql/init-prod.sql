-- Cria o banco de dados se ainda não existir
CREATE DATABASE IF NOT EXISTS reportsdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Cria o usuário com senha (se ainda não existir)
CREATE USER IF NOT EXISTS 'admin_user_api'@'%' IDENTIFIED BY 'EvloSEd3d5&*';
ALTER USER 'admin_user_api'@'%' IDENTIFIED BY 'EvloSEd3d5&*';

-- Garante permissões completas SOMENTE no banco reportsdb
GRANT ALL PRIVILEGES ON reportsdb.* TO 'admin_user_api'@'%';

-- Aplica as alterações de permissões
FLUSH PRIVILEGES;
