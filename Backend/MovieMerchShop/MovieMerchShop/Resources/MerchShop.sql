-- PostgreSQL database dump

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET default_tablespace = '';
SET default_with_oids = false;

---
--- drop tables
---

DROP TABLE IF EXISTS movie;
DROP TABLE IF EXISTS mug;
DROP TABLE IF EXISTS merchorders; -- Changed from 'orders'
DROP TABLE IF EXISTS poster;
DROP TABLE IF EXISTS shirt;
DROP TABLE IF EXISTS users;

-- Create movie table
CREATE TABLE movie (
    movie_id serial PRIMARY KEY
    -- Add other columns as needed
);

-- Create mug table
CREATE TABLE mug (
                     mug_id serial PRIMARY KEY,
                     color character varying(20) NOT NULL
);

-- Insert data into mug table
INSERT INTO mug (color) VALUES
                            ('Red'),
                            ('Blue'),
                            ('Green'),
                            ('Yellow');

-- Create merchorders table (renamed from orders)
CREATE TABLE merchorders (
                             order_id serial PRIMARY KEY,
                             user_id integer NOT NULL,
                             order_date date,
                             item_ordered character varying(50) NOT NULL
);

-- Create poster table
CREATE TABLE poster (
                        poster_id serial PRIMARY KEY,
                        size character varying(20) NOT NULL,
                        material character varying(50) NOT NULL
);

-- Insert data into poster table
INSERT INTO poster (size, material) VALUES
                                        ('24x36', 'Paper'),
                                        ('18x24', 'Canvas'),
                                        ('36x48', 'Vinyl');

-- Create shirt table
CREATE TABLE shirt (
                       shirt_id serial PRIMARY KEY,
                       size character varying(10) NOT NULL,
                       color character varying(20) NOT NULL
);

-- Insert data into shirt table
INSERT INTO shirt (size, color) VALUES
                                    ('Small', 'White'),
                                    ('Medium', 'Black'),
                                    ('Large', 'Blue'),
                                    ('XL', 'Red'),
                                    ('XXL', 'Green');

-- Create user table
CREATE TABLE users (
                       user_id serial PRIMARY KEY,
                       username character varying(30) NOT NULL,
                       password character varying(30) NOT NULL
);

-- Insert data into user table
INSERT INTO users (username, password) VALUES
                                           ('user1', 'password1'),
                                           ('user2', 'password2'),
                                           ('user3', 'password3');
