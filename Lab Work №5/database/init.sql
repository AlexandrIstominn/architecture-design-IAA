-- Модель БД для приложения выявления ботов в социальных сетях
-- PostgreSQL

CREATE TABLE IF NOT EXISTS users (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS bot_ratings (
    rating_id SERIAL PRIMARY KEY,
    description VARCHAR(255) NOT NULL,
    rating_value INT NOT NULL
);

CREATE TABLE IF NOT EXISTS accounts (
    account_id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    social_network VARCHAR(50) NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'Active'
);

CREATE TABLE IF NOT EXISTS analysis_history (
    history_id SERIAL PRIMARY KEY,
    user_id INT NOT NULL REFERENCES users(user_id),
    analysis_date TIMESTAMP NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'Pending',
    social_network VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS analysis_results (
    result_id SERIAL PRIMARY KEY,
    history_id INT NOT NULL REFERENCES analysis_history(history_id),
    account_id INT NOT NULL REFERENCES accounts(account_id),
    rating_id INT NOT NULL REFERENCES bot_ratings(rating_id),
    status VARCHAR(50) NOT NULL,
    analysis_date TIMESTAMP NOT NULL
);

CREATE TABLE IF NOT EXISTS reports (
    report_id SERIAL PRIMARY KEY,
    user_id INT NOT NULL REFERENCES users(user_id),
    report_date TIMESTAMP NOT NULL,
    file_format VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS exported_results (
    export_id SERIAL PRIMARY KEY,
    report_id INT NOT NULL REFERENCES reports(report_id),
    file_location VARCHAR(500) NOT NULL,
    format VARCHAR(50) NOT NULL,
    export_date TIMESTAMP NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_analysis_history_user ON analysis_history(user_id);
CREATE INDEX IF NOT EXISTS idx_analysis_results_history ON analysis_results(history_id);
CREATE INDEX IF NOT EXISTS idx_analysis_results_account ON analysis_results(account_id);
CREATE INDEX IF NOT EXISTS idx_reports_user ON reports(user_id);

-- Данные по умолчанию: рейтинги ботов
INSERT INTO bot_ratings (rating_id, description, rating_value) VALUES
    (1, 'Неизвестно', 0),
    (2, 'Низкая вероятность бота', 1),
    (3, 'Средняя вероятность бота', 2),
    (4, 'Высокая вероятность бота', 3),
    (5, 'Подтверждённый бот', 4)
ON CONFLICT (rating_id) DO NOTHING;

-- Тестовый пользователь (пароль: test123)
INSERT INTO users (user_id, username, email, password) VALUES
    (1, 'testuser', 'test@example.com', 'test123')
ON CONFLICT (user_id) DO NOTHING;

-- Обновление последовательностей (для корректной автоинкрементации)
SELECT setval(pg_get_serial_sequence('bot_ratings', 'rating_id'), COALESCE((SELECT MAX(rating_id) FROM bot_ratings), 1));
SELECT setval(pg_get_serial_sequence('users', 'user_id'), COALESCE((SELECT MAX(user_id) FROM users), 1));
