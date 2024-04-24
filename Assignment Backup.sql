toc.dat                                                                                             0000600 0004000 0002000 00000007212 14612203416 0014440 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        PGDMP       2                |            Employee_DataBase    16.1    16.1     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false         �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false         �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false         �           1262    65579    Employee_DataBase    DATABASE     �   CREATE DATABASE "Employee_DataBase" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_India.1252';
 #   DROP DATABASE "Employee_DataBase";
                postgres    false         �            1259    65593 
   department    TABLE     p   CREATE TABLE public.department (
    departmentid integer NOT NULL,
    name character varying(128) NOT NULL
);
    DROP TABLE public.department;
       public         heap    postgres    false         �            1259    65625    employee    TABLE     }  CREATE TABLE public.employee (
    employeeid integer NOT NULL,
    departmentid integer NOT NULL,
    firstname character varying(100) NOT NULL,
    lastname character varying(100),
    age character varying(10),
    email character varying(50),
    education character varying(100),
    company character varying(50),
    experience integer,
    package character varying(50)
);
    DROP TABLE public.employee;
       public         heap    postgres    false         �            1259    65624    employee_employeeid_seq    SEQUENCE     �   ALTER TABLE public.employee ALTER COLUMN employeeid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.employee_employeeid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    217         �          0    65593 
   department 
   TABLE DATA           8   COPY public.department (departmentid, name) FROM stdin;
    public          postgres    false    215       4841.dat �          0    65625    employee 
   TABLE DATA           �   COPY public.employee (employeeid, departmentid, firstname, lastname, age, email, education, company, experience, package) FROM stdin;
    public          postgres    false    217       4843.dat �           0    0    employee_employeeid_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public.employee_employeeid_seq', 27, true);
          public          postgres    false    216         V           2606    65597    department pk_department 
   CONSTRAINT     `   ALTER TABLE ONLY public.department
    ADD CONSTRAINT pk_department PRIMARY KEY (departmentid);
 B   ALTER TABLE ONLY public.department DROP CONSTRAINT pk_department;
       public            postgres    false    215         X           2606    65629    employee pk_employee 
   CONSTRAINT     Z   ALTER TABLE ONLY public.employee
    ADD CONSTRAINT pk_employee PRIMARY KEY (employeeid);
 >   ALTER TABLE ONLY public.employee DROP CONSTRAINT pk_employee;
       public            postgres    false    217         Y           2606    65630    employee fk_employee    FK CONSTRAINT     �   ALTER TABLE ONLY public.employee
    ADD CONSTRAINT fk_employee FOREIGN KEY (departmentid) REFERENCES public.department(departmentid);
 >   ALTER TABLE ONLY public.employee DROP CONSTRAINT fk_employee;
       public          postgres    false    4694    217    215                                                                                                                                                                                                                                                                                                                                                                                              4841.dat                                                                                            0000600 0004000 0002000 00000000050 14612203416 0014244 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        1	It
2	Sales
3	Marketing
4	QA
5	HR
\.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        4843.dat                                                                                            0000600 0004000 0002000 00000001201 14612203416 0014245 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        3	2	trg	trgtrtrg	55	aaaa@gmail.com	frreg	gtrgtgr	5	8
4	3	rrg	fd	33	ed@gmail.com	refrf	rerf	8	yy
13	4	gtrt	tyhy	yuh	yhy	uyu	uju	\N	uy
14	4	rt	v	89	rr@gmail.com	btb	tb	\N	tb
15	4	trtr	thth	yt	ythhy	ythy	yt	\N	yt
16	4	tyh	tt	y	user@gmail.com	yh	yt	\N	y
17	4	fref	rffr	ferf	rer	r	re	\N	r
18	4	fref	efr	reer	rr	rr	rer	\N	rfrf
19	4	trg	trgtrtrg	55	aaaa@gmail.com	frreg	gtrgtgr	5	8
20	4	r	rgtg	tgr	gtr	gtrgtr	rtr	\N	trt
21	4	ujuj	kikkoi	5	oko	hh	hghg	\N	jhjh
22	4	yhuyhu	u	yyh	yy	uu	juuju	\N	ju
23	4	gtrh	ujhuj	juju	juj	juj	uuj	\N	uj
24	4	gtrt	tyhy	yuh	yhy	uyu	uju	\N	uy
25	4	gtrt	tyhy	yuh	yhy	uyu	uju	\N	u
27	4	dsfd	ffd	fggf	dffd	gf	fgf	\N	f
\.


                                                                                                                                                                                                                                                                                                                                                                                               restore.sql                                                                                         0000600 0004000 0002000 00000007320 14612203416 0015365 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        --
-- NOTE:
--
-- File paths need to be edited. Search for $$PATH$$ and
-- replace it with the path to the directory containing
-- the extracted data files.
--
--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1
-- Dumped by pg_dump version 16.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE "Employee_DataBase";
--
-- Name: Employee_DataBase; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "Employee_DataBase" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_India.1252';


ALTER DATABASE "Employee_DataBase" OWNER TO postgres;

\connect "Employee_DataBase"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: department; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.department (
    departmentid integer NOT NULL,
    name character varying(128) NOT NULL
);


ALTER TABLE public.department OWNER TO postgres;

--
-- Name: employee; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.employee (
    employeeid integer NOT NULL,
    departmentid integer NOT NULL,
    firstname character varying(100) NOT NULL,
    lastname character varying(100),
    age character varying(10),
    email character varying(50),
    education character varying(100),
    company character varying(50),
    experience integer,
    package character varying(50)
);


ALTER TABLE public.employee OWNER TO postgres;

--
-- Name: employee_employeeid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.employee ALTER COLUMN employeeid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.employee_employeeid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Data for Name: department; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.department (departmentid, name) FROM stdin;
\.
COPY public.department (departmentid, name) FROM '$$PATH$$/4841.dat';

--
-- Data for Name: employee; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.employee (employeeid, departmentid, firstname, lastname, age, email, education, company, experience, package) FROM stdin;
\.
COPY public.employee (employeeid, departmentid, firstname, lastname, age, email, education, company, experience, package) FROM '$$PATH$$/4843.dat';

--
-- Name: employee_employeeid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.employee_employeeid_seq', 27, true);


--
-- Name: department pk_department; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.department
    ADD CONSTRAINT pk_department PRIMARY KEY (departmentid);


--
-- Name: employee pk_employee; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employee
    ADD CONSTRAINT pk_employee PRIMARY KEY (employeeid);


--
-- Name: employee fk_employee; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employee
    ADD CONSTRAINT fk_employee FOREIGN KEY (departmentid) REFERENCES public.department(departmentid);


--
-- PostgreSQL database dump complete
--

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                