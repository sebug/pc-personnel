do $$
BEGIN
IF NOT EXISTS (
   SELECT *
   FROM INFORMATION_SCHEMA.TABLES
   WHERE TABLE_NAME = 'AspNetRoles') THEN

CREATE TABLE "AspNetRoles" (
"Id" VARCHAR(256) NOT NULL PRIMARY KEY,
"Name" VARCHAR(256) NULL,
"NormalizedName" VARCHAR(256) NULL,
"ConcurrencyStamp" VARCHAR(256) NULL
);
END IF;

IF NOT EXISTS (
   SELECT *
   FROM INFORMATION_SCHEMA.TABLES
   WHERE TABLE_NAME = 'AspNetUsers') THEN

CREATE TABLE "AspNetUsers" (
"Id" VARCHAR(256) NOT NULL PRIMARY KEY,
"UserName" VARCHAR(256) NULL,
"NormalizedUserName" VARCHAR(256) NULL,
"Email" VARCHAR(256) NULL,
"NormalizedEmail" VARCHAR(256) NULL,
"EmailConfirmed" BOOLEAN NOT NULL,
"PasswordHash" VARCHAR(256) NULL,
"SecurityStamp" VARCHAR(256) NULL,
"ConcurrencyStamp" VARCHAR(256) NULL,
"PhoneNumber" VARCHAR(256) NULL,
"PhoneNumberConfirmed" BOOLEAN NOT NULL,
"TwoFactorEnabled" BOOLEAN NOT NULL,
"LockoutEnd" TIMESTAMP WITH TIME ZONE NULL,
"LockoutEnabled" BOOLEAN NOT NULL,
"AccessFailedCount" INT NOT NULL
);
END IF;

IF NOT EXISTS (
   SELECT *
   FROM INFORMATION_SCHEMA.TABLES
   WHERE TABLE_NAME = 'AspNetRoleClaims') THEN

CREATE SEQUENCE aspnet_role_claims_sequence;
CREATE TABLE "AspNetRoleClaims" (
"Id" INT NOT NULL DEFAULT nextval('aspnet_role_claims_sequence') PRIMARY KEY,
"RoleId" VARCHAR(256) NOT NULL REFERENCES "AspNetRoles" ("Id"),
"ClaimType" VARCHAR(256) NULL,
"ClaimValue" VARCHAR(1024) NULL
);
END IF;

IF NOT EXISTS (
   SELECT *
   FROM INFORMATION_SCHEMA.TABLES
   WHERE TABLE_NAME = 'AspNetUserClaims') THEN

CREATE SEQUENCE aspnet_user_claims_sequence;
CREATE TABLE "AspNetUserClaims" (
"Id" INT NOT NULL DEFAULT nextval('aspnet_user_claims_sequence') PRIMARY KEY,
"UserId" VARCHAR(256) NOT NULL REFERENCES "AspNetUsers" ("Id"),
"ClaimType" VARCHAR(256) NULL,
"ClaimValue" VARCHAR(1024) NULL
);
END IF;

IF NOT EXISTS (
   SELECT *
   FROM INFORMATION_SCHEMA.TABLES
   WHERE TABLE_NAME = 'AspNetUserLogins') THEN

CREATE TABLE "AspNetUserLogins" (
"LoginProvider" VARCHAR(128) NOT NULL,
"ProviderKey" VARCHAR(128) NOT NULL,
"ProviderDisplayName" VARCHAR(512) NULL,
"UserId" VARCHAR(256) NOT NULL REFERENCES "AspNetUsers" ("Id"),
PRIMARY KEY ("LoginProvider", "ProviderKey")
);
END IF;


END;
$$;
