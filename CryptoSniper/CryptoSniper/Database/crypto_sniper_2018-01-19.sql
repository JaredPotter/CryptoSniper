# ************************************************************
# Sequel Pro SQL dump
# Version 4541
#
# http://www.sequelpro.com/
# https://github.com/sequelpro/sequelpro
# ************************************************************


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


# Dump of table BuyOrder
# ------------------------------------------------------------

DROP TABLE IF EXISTS `BuyOrder`;

CREATE TABLE `BuyOrder` (
  `buy_order_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `buy_price` decimal(11,2) NOT NULL DEFAULT '0.00',
  `order_date` datetime NOT NULL,
  `timeout` datetime NOT NULL,
  `user_id` int(11) NOT NULL,
  `amount` decimal(11,0) DEFAULT NULL,
  `buy_count` int(11) DEFAULT NULL COMMENT 'Number of times to place this order',
  `completed` int(11) DEFAULT NULL,
  `vendor_order_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`buy_order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table HistoricalPriceBchUsd
# ------------------------------------------------------------

DROP TABLE IF EXISTS `HistoricalPriceBchUsd`;

CREATE TABLE `HistoricalPriceBchUsd` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `price` decimal(11,4) NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=924 DEFAULT CHARSET=latin1;



# Dump of table HistoricalPriceBtcUsd
# ------------------------------------------------------------

DROP TABLE IF EXISTS `HistoricalPriceBtcUsd`;

CREATE TABLE `HistoricalPriceBtcUsd` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `price` decimal(11,4) NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=934 DEFAULT CHARSET=latin1;



# Dump of table HistoricalPriceBtgUsd
# ------------------------------------------------------------

DROP TABLE IF EXISTS `HistoricalPriceBtgUsd`;

CREATE TABLE `HistoricalPriceBtgUsd` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `price` decimal(11,4) NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=927 DEFAULT CHARSET=latin1;



# Dump of table HistoricalPriceDashUsd
# ------------------------------------------------------------

DROP TABLE IF EXISTS `HistoricalPriceDashUsd`;

CREATE TABLE `HistoricalPriceDashUsd` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `price` decimal(11,4) NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=139 DEFAULT CHARSET=latin1;



# Dump of table HistoricalPriceEthUsd
# ------------------------------------------------------------

DROP TABLE IF EXISTS `HistoricalPriceEthUsd`;

CREATE TABLE `HistoricalPriceEthUsd` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `price` decimal(11,4) NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=929 DEFAULT CHARSET=latin1;



# Dump of table HistoricalPriceXrpUsd
# ------------------------------------------------------------

DROP TABLE IF EXISTS `HistoricalPriceXrpUsd`;

CREATE TABLE `HistoricalPriceXrpUsd` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `price` decimal(11,4) NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=927 DEFAULT CHARSET=latin1;



# Dump of table HistoricalPriceZecUsd
# ------------------------------------------------------------

DROP TABLE IF EXISTS `HistoricalPriceZecUsd`;

CREATE TABLE `HistoricalPriceZecUsd` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `price` decimal(11,4) NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=139 DEFAULT CHARSET=latin1;



# Dump of table InstantOrder
# ------------------------------------------------------------

DROP TABLE IF EXISTS `InstantOrder`;

CREATE TABLE `InstantOrder` (
  `order_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `completed` tinyint(1) NOT NULL DEFAULT '0',
  `buy_date` datetime NOT NULL,
  `sell_date` datetime DEFAULT NULL,
  `user_id` int(11) NOT NULL,
  `buy_price` decimal(11,2) NOT NULL,
  `sell_price` decimal(11,2) NOT NULL DEFAULT '0.00',
  `profit_percentage` decimal(11,2) NOT NULL DEFAULT '0.00',
  `amount` decimal(11,2) NOT NULL,
  `crypto_currency` varchar(11) NOT NULL DEFAULT '',
  `vendor_buy_id` int(11) DEFAULT NULL,
  `vendor_sell_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;



# Dump of table SellOrder
# ------------------------------------------------------------

DROP TABLE IF EXISTS `SellOrder`;

CREATE TABLE `SellOrder` (
  `sell_order_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `sell_price` decimal(11,2) DEFAULT NULL,
  PRIMARY KEY (`sell_order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table User
# ------------------------------------------------------------

DROP TABLE IF EXISTS `User`;

CREATE TABLE `User` (
  `user_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(32) NOT NULL DEFAULT '',
  `cexio_user_id` varchar(15) DEFAULT '',
  `cexio_key` varchar(32) DEFAULT '',
  `cexio_secret` varchar(32) DEFAULT '',
  `price_derivative_time` int(11) NOT NULL,
  `investment_percentage` decimal(11,2) NOT NULL,
  `next_investment_check` datetime NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;



# Dump of table UserInvestmentPlan
# ------------------------------------------------------------

DROP TABLE IF EXISTS `UserInvestmentPlan`;

CREATE TABLE `UserInvestmentPlan` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `currency` varchar(11) NOT NULL DEFAULT '',
  `percent` decimal(2,2) NOT NULL,
  `user_id` int(11) NOT NULL,
  `fall_percent` int(11) DEFAULT NULL,
  `stabalize_percent` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;




/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
