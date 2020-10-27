-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 27-Out-2020 às 01:36
-- Versão do servidor: 10.4.13-MariaDB
-- versão do PHP: 7.4.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `bankapp`
--

DELIMITER $$
--
-- Procedimentos
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `transfer` (IN `guid` BINARY(16), IN `amount` DECIMAL(25,2), IN `type` SET('Withdrawal','Deposit'))  NO SQL
insert into moneytransfers values((select uuid()), guid, amount, type)$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Estrutura da tabela `accounts`
--

CREATE TABLE `accounts` (
  `Guid` binary(16) NOT NULL,
  `Name` tinytext NOT NULL,
  `OwnerGuid` binary(16) NOT NULL,
  `Amount` decimal(25,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estrutura da tabela `customers`
--

CREATE TABLE `customers` (
  `Guid` binary(16) NOT NULL,
  `Name` text NOT NULL,
  `Email` text NOT NULL,
  `PhoneNumber` tinytext NOT NULL,
  `Address` text NOT NULL,
  `ZipCode` tinytext NOT NULL,
  `District` tinytext NOT NULL,
  `Country` tinytext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estrutura da tabela `moneytransfers`
--

CREATE TABLE `moneytransfers` (
  `Guid` binary(16) NOT NULL,
  `AccountGuid` binary(16) NOT NULL,
  `Amount` decimal(25,2) NOT NULL,
  `Type` set('Withdrawal','Deposit') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Acionadores `moneytransfers`
--
DELIMITER $$
CREATE TRIGGER `transfer` AFTER INSERT ON `moneytransfers` FOR EACH ROW if new.Type = "Withdrawal" THEN
	update accounts set Amount = accounts.Amount - new.Amount;
else if new.Type = "Deposit" THEN
	update accounts set Amount = accounts.Amount - new.Amount; 
    end if;
end if
$$
DELIMITER ;

--
-- Índices para tabelas despejadas
--

--
-- Índices para tabela `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`Guid`),
  ADD KEY `OwnerForeignKey` (`OwnerGuid`);

--
-- Índices para tabela `customers`
--
ALTER TABLE `customers`
  ADD PRIMARY KEY (`Guid`);

--
-- Índices para tabela `moneytransfers`
--
ALTER TABLE `moneytransfers`
  ADD PRIMARY KEY (`Guid`);

--
-- Restrições para despejos de tabelas
--

--
-- Limitadores para a tabela `accounts`
--
ALTER TABLE `accounts`
  ADD CONSTRAINT `OwnerForeignKey` FOREIGN KEY (`OwnerGuid`) REFERENCES `customers` (`Guid`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
