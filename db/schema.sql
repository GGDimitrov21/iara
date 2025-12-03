-- IARA PostgreSQL schema
-- Safe to run multiple times (IF NOT EXISTS used when possible)

BEGIN;

CREATE TABLE IF NOT EXISTS public.personnel
(
    person_id serial PRIMARY KEY,
    name varchar(100) NOT NULL,
    role varchar(50) NOT NULL,
    contact_email varchar(100) UNIQUE
);

CREATE TABLE IF NOT EXISTS public.vessels
(
    vessel_id serial PRIMARY KEY,
    reg_number varchar(50) UNIQUE NOT NULL,
    vessel_name varchar(100) NOT NULL,
    owner_details text,
    captain_id integer REFERENCES public.personnel(person_id),
    length_m numeric(6,2),
    width_m numeric(6,2),
    tonnage numeric(10,2),
    fuel_type varchar(50),
    engine_power_kw integer,
    displacement_tons numeric(8,2)
);
CREATE INDEX IF NOT EXISTS ix_vessels_captain ON public.vessels(captain_id);

CREATE TABLE IF NOT EXISTS public.species
(
    species_id serial PRIMARY KEY,
    species_name varchar(100) UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS public.permits
(
    permit_id serial PRIMARY KEY,
    vessel_id integer NOT NULL REFERENCES public.vessels(vessel_id),
    issue_date date NOT NULL,
    expiry_date date NOT NULL,
    is_active boolean DEFAULT true
);
CREATE INDEX IF NOT EXISTS ix_permits_vessel ON public.permits(vessel_id);

CREATE TABLE IF NOT EXISTS public.catch_quotas
(
    quota_id serial PRIMARY KEY,
    permit_id integer NOT NULL REFERENCES public.permits(permit_id),
    species_id integer NOT NULL REFERENCES public.species(species_id),
    year smallint NOT NULL,
    min_catch_kg numeric(10,2),
    avg_catch_kg numeric(10,2),
    max_catch_kg numeric(10,2) NOT NULL,
    fuel_hours_limit numeric(8,2),
    UNIQUE(permit_id, species_id, year)
);
CREATE INDEX IF NOT EXISTS ix_quotas_species ON public.catch_quotas(species_id);

CREATE TABLE IF NOT EXISTS public.logbook
(
    log_entry_id serial PRIMARY KEY,
    vessel_id integer NOT NULL REFERENCES public.vessels(vessel_id),
    captain_id integer NOT NULL REFERENCES public.personnel(person_id),
    start_time timestamp NOT NULL,
    duration_hours numeric(4,2),
    latitude numeric(9,6),
    longitude numeric(9,6),
    species_id integer NOT NULL REFERENCES public.species(species_id),
    catch_kg numeric(10,2) NOT NULL
);
CREATE INDEX IF NOT EXISTS ix_logbook_captain ON public.logbook(captain_id);

CREATE TABLE IF NOT EXISTS public.inspections
(
    inspection_id serial PRIMARY KEY,
    vessel_id integer NOT NULL REFERENCES public.vessels(vessel_id),
    inspector_id integer NOT NULL REFERENCES public.personnel(person_id),
    inspection_date timestamp NOT NULL,
    is_legal boolean NOT NULL,
    notes text
);
CREATE INDEX IF NOT EXISTS ix_inspections_inspector ON public.inspections(inspector_id);

CREATE TABLE IF NOT EXISTS public.tickets
(
    ticket_id serial PRIMARY KEY,
    ticket_number varchar(50) UNIQUE NOT NULL,
    expiry_date date,
    person_status varchar(50) NOT NULL,
    is_validated boolean DEFAULT false,
    validation_date timestamp,
    inspection_id integer REFERENCES public.inspections(inspection_id)
);
CREATE INDEX IF NOT EXISTS ix_tickets_inspection ON public.tickets(inspection_id);

COMMIT;
