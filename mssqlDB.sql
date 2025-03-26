GO
CREATE DATABASE reader_trpo

GO
USE reader_trpo

CREATE TABLE user_roles (
	id BIGINT PRIMARY KEY IDENTITY,
	[name] NVARCHAR(255) NOT NULL,

	CONSTRAINT UQ_user_role_name UNIQUE ([name])
);

INSERT INTO user_roles VALUES ('user'), ('admin');

CREATE TABLE users (
	id BIGINT PRIMARY KEY IDENTITY,
	role_id BIGINT NOT NULL DEFAULT(1),
	email NVARCHAR(255) NOT NULL,
	[password] VARBINARY(64) NOT NULL,

	CONSTRAINT UQ_user_email UNIQUE (email),
	FOREIGN KEY (role_id) REFERENCES user_roles(id) ON DELETE CASCADE
);

INSERT INTO users VALUES (	-- user with admin rights
	2, -- admin user role
	'admin@ya.ru', -- admin email
	0x2432612431302443726439566547504B4C6A54725A46566F766575524F544734494A796D32646D79576639735446366662784D434379593970456A75 -- password '12345678' in hash
)

CREATE TABLE genres (
	id BIGINT PRIMARY KEY IDENTITY,
	[name] NVARCHAR(255) NOT NULL,
	[description] NVARCHAR(MAX),

	CONSTRAINT UQ_genre_name UNIQUE ([name])
);

CREATE TABLE authors (
	id BIGINT PRIMARY KEY IDENTITY,
	surname NVARCHAR(255) NOT NULL,
	[name] NVARCHAR(255) NOT NULL,
	patronymic NVARCHAR(255) NOT NULL,
	[description] NVARCHAR(255),

	CONSTRAINT UQ_author_full_name UNIQUE (surname, [name], patronymic)
);

CREATE TABLE book_statuses (
	id BIGINT PRIMARY KEY IDENTITY,
	[name] NVARCHAR(255) NOT NULL,

	CONSTRAINT UQ_book_status_name UNIQUE ([name])
);

INSERT INTO book_statuses VALUES ('available')

CREATE TABLE books (
	id BIGINT PRIMARY KEY IDENTITY,
	genre_id BIGINT NOT NULL,
	author_id BIGINT NOT NULL,
	status_id BIGINT NOT NULL DEFAULT(1),
	[year] SMALLINT NOT NULL,
	[name] NVARCHAR(255) NOT NULL,
	[count] INT NOT NULL DEFAULT(0),
	[image] NVARCHAR(255) NOT NULL,
	[description] NVARCHAR(MAX),
	
	CONSTRAINT UQ_book_image UNIQUE ([image]),
	FOREIGN KEY (genre_id) REFERENCES genres(id) ON DELETE CASCADE,
	FOREIGN KEY (author_id) REFERENCES authors(id) ON DELETE CASCADE,
	FOREIGN KEY (status_id) REFERENCES book_statuses(id) ON DELETE CASCADE
);

CREATE TABLE order_statuses (
	id BIGINT PRIMARY KEY IDENTITY,
	[name] NVARCHAR(255) NOT NULL,

	CONSTRAINT UQ_order_status_name UNIQUE ([name])
);

INSERT INTO order_statuses VALUES ('in_progress'), ('completed'), ('basket');

CREATE TABLE orders (
	id BIGINT PRIMARY KEY IDENTITY,
	[user_id] BIGINT NOT NULL,
	status_id BIGINT NOT NULL DEFAULT(1),
	[date] DATETIME NOT NULL,
	
	FOREIGN KEY ([user_id]) REFERENCES users(id) ON DELETE CASCADE,
	FOREIGN KEY (status_id) REFERENCES order_statuses(id) ON DELETE CASCADE
)

CREATE TABLE order_books (
	id BIGINT PRIMARY KEY IDENTITY,
	book_id BIGINT NOT NULL,
	order_id BIGINT NOT NULL,
	[count] INT NOT NULL DEFAULT(1),

	CONSTRAINT CK_order_book_count_over_or_equal_one CHECK ([count] >= 1),
	FOREIGN KEY (book_id) REFERENCES books(id) ON DELETE CASCADE,
	FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE
)