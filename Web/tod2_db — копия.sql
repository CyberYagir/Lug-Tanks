-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Дек 07 2021 г., 06:11
-- Версия сервера: 5.6.41
-- Версия PHP: 7.2.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `tod2_db`
--
CREATE DATABASE IF NOT EXISTS `tod2_db` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `tod2_db`;

-- --------------------------------------------------------

--
-- Структура таблицы `users`
--

CREATE TABLE `users` (
  `id` int(1) NOT NULL,
  `name` varchar(8) NOT NULL,
  `weapon` tinyint(2) DEFAULT '0',
  `corpus` tinyint(2) DEFAULT '0',
  `exp` int(11) DEFAULT '0',
  `level` int(11) DEFAULT '1',
  `password` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `users`
--

INSERT INTO `users` (`id`, `name`, `weapon`, `corpus`, `exp`, `level`, `password`) VALUES
(25, '1234', 0, 0, 0, 1, '$2y$10$gz7EyHICvpuaFtpv65Kx7ux4Dm.hf0isAUq7uapj67ZqK4ve2HSYy'),
(26, 'yagir', 1, 0, 360, 1, '$2y$10$tDoCs39J.1df102zyA92eOMI0vSCOW3MU5hNWwm/xAtu8xfvrlkiK'),
(27, 'yagir1', 1, 0, 210, 1, '$2y$10$Suv3kmgmrYLFXkvLamyvnOcpcUZ3UTXwAzyK2dz/Hc3knLRQjOkN6'),
(28, 'test', 0, 0, 45, 1, '$2y$10$g0EwWKGcdZb8byLXXGoCieP/4fgpO7z7H.1Dyu8C8Bp8FiP5ps/NG'),
(29, 'petux', 0, 0, 0, 1, '$2y$10$f4iwOl5TPTHKhMsSfIlE/eixCnwk.0QccSWZEv51uLzSP.NCJeMr6');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `users`
--
ALTER TABLE `users`
  MODIFY `id` int(1) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
