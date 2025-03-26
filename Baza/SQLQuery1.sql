CREATE DATABASE HMS;
GO
USE HMS;
GO

CREATE TABLE gost ( 
    id INT IDENTITY(1,1) PRIMARY KEY,
    ime VARCHAR(50) NOT NULL,
    prezime VARCHAR(50) NOT NULL,
    telefon VARCHAR(20) NOT NULL,
    drzavljanstvo VARCHAR(50) NOT NULL,
    pol VARCHAR(1) CHECK (pol IN ('M', 'Z')),
    pasos VARCHAR(20) UNIQUE,
    licna_karta VARCHAR(20) UNIQUE
);
GO

CREATE TABLE soba (
    broj_sobe INT PRIMARY KEY,
    sprat INT NOT NULL,
    tip_sobe VARCHAR(20) CHECK (tip_sobe IN ('Jednokrevetna', 'Dvokrevetna', 'Trokrevetna')) NOT NULL,
    status_rada VARCHAR(20) CHECK (status_rada IN ('Slobodna', 'Zauzeta', 'U odrzavanju')) NOT NULL DEFAULT 'Slobodna',
    napomena TEXT NULL,
    cena_po_noci DECIMAL(10,2) NOT NULL,
    poslednji_datum_odrzavanja DATE NULL
);
GO

CREATE TABLE rezervacija (
    id INT IDENTITY(1,1) PRIMARY KEY,
    gost_id INT NOT NULL,
    broj_sobe INT NOT NULL,
    datum_pocetka_rez DATE NOT NULL,
    datum_kraja_rez DATE NOT NULL,
    ukupna_cena DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    FOREIGN KEY (gost_id) REFERENCES gost(id) ON DELETE CASCADE,
    FOREIGN KEY (broj_sobe) REFERENCES soba(broj_sobe) ON DELETE CASCADE
);
GO

CREATE TABLE osoblje (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    sifra VARCHAR(255) NOT NULL,
    uloga VARCHAR(10) CHECK (uloga IN ('admin', 'radnik')) NOT NULL
);
GO

INSERT INTO osoblje (username, sifra, uloga) VALUES 
('admin', 'admin123', 'admin'),
('radnik', 'radnik123', 'radnik');
GO

INSERT INTO soba (broj_sobe, sprat, tip_sobe, status_rada, napomena, cena_po_noci, poslednji_datum_odrzavanja) VALUES 
(101, 1, 'Jednokrevetna', 'Slobodna', NULL, 50.00, '2024-03-01'),
(202, 2, 'Dvokrevetna', 'Slobodna', NULL, 80.00, '2024-03-02'),
(303, 3, 'Trokrevetna', 'Slobodna', NULL, 120.00, '2024-03-03');
GO

INSERT INTO gost (ime, prezime, telefon, drzavljanstvo, pol, pasos, licna_karta) VALUES 
('Filip', 'Isakovic', '0655290704', 'Srbija', 'M', 'AA123456', '123456789'),
('Katarina', 'Jovanović', '0637022503', 'Srbija', 'Z', 'BB654321', '987654321');
GO